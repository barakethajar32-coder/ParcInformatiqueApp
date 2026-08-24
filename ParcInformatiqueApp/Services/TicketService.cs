using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class TicketService
    {
        // 1. Consultation des tickets selon le rôle de l'utilisateur
        public List<Ticket> GetTicketsForUser(User user)
        {
            using var context = new AppDbContext();
            var query = context.Tickets
                .Include(t => t.Equipement)
                .Include(t => t.UserCreateur)
                .Include(t => t.UserTraiteur)
                .AsNoTracking()
                .AsQueryable();

            // Règle 1 : Un Employé ne voit que ses propres tickets
            if (user.Role == "Employé")
            {
                query = query.Where(t => t.IdUserCreateur == user.IdUser);
            }
            // Règle 2 : Un Technicien ne voit que ses tickets attribués ou en attente
            else if (user.Role == "Technicien")
            {
                query = query.Where(t => t.IdUserTraiteur == user.IdUser || t.IdUserTraiteur == null);
            }

            return query.OrderByDescending(t => t.DateCreation).ToList();
        }

        // 2. Création d'un ticket par un employé
        public (bool Success, string Message, int IdTicket) CreateTicket(int idUserCreateur, int idEquipement, string nomTicket, string description, string priorite = "Moyenne")
        {
            if (string.IsNullOrWhiteSpace(nomTicket) || string.IsNullOrWhiteSpace(description))
                return (false, "Le nom du ticket et la description sont obligatoires.", 0);

            using var context = new AppDbContext();

            var user = context.Users.Find(idUserCreateur);
            if (user == null) return (false, "Utilisateur créateur introuvable.", 0);

            var equipement = context.Equipements.Find(idEquipement);
            if (equipement == null) return (false, "Équipement introuvable.", 0);

            var ticket = new Ticket
            {
                NomTicket = nomTicket.Trim(),
                Description = description.Trim(),
                Priorite = priorite,
                Statut = "En attente",
                DateCreation = DateTime.Now,
                IdUserCreateur = idUserCreateur,
                IdEquipement = idEquipement
            };

            context.Tickets.Add(ticket);
            context.SaveChanges();

            return (true, "Ticket créé avec succès.", ticket.IdTicket);
        }

        // 3. Mise à jour des détails du ticket
        public (bool Success, string Message) UpdateTicketDetails(int idTicket, int idUser, string nouveauNom, string nouvelleDescription, string nouvellePriorite)
        {
            using var context = new AppDbContext();
            var ticket = context.Tickets.Find(idTicket);
            if (ticket == null) return (false, "Ticket introuvable.");

            var user = context.Users.Find(idUser);
            if (user == null) return (false, "Utilisateur introuvable.");

            if (user.Role == "Employé" && (ticket.IdUserCreateur != idUser || ticket.Statut != "En attente"))
            {
                return (false, "Vous ne pouvez plus modifier ce ticket une fois son traitement démarré.");
            }

            ticket.NomTicket = nouveauNom.Trim();
            ticket.Description = nouvelleDescription.Trim();
            ticket.Priorite = nouvellePriorite;

            context.SaveChanges();
            return (true, "Ticket mis à jour avec succès.");
        }
    }
}