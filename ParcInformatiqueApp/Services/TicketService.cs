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
        // JOUR 6 : CRÉATION ET RESTRICTIONS DE VUE
        // 1. Consultation des tickets selon les règles de restriction par rôle
        public List<Ticket> GetTicketsForUser(User user)
        {
            using var context = new AppDbContext();
            var query = context.Tickets
                .Include(t => t.Equipement)
                .Include(t => t.UserCreateur)
                .Include(t => t.UserTraiteur)
                .AsNoTracking()
                .AsQueryable();

            // Règle 1 : Un Employé ne voit que les tickets qu'il a créés
            if (user.Role == "Employé")
            {
                query = query.Where(t => t.IdUserCreateur == user.IdUser);
            }
            // Règle 2 : Un Technicien ne voit que ses tickets attribués ou non attribués (En attente)
            else if (user.Role == "Technicien")
            {
                query = query.Where(t => t.IdUserTraiteur == user.IdUser || t.IdUserTraiteur == null);
            }
            // Note : Le Responsable voit l'ensemble des tickets du parc

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
            return (true, "Détails du ticket mis à jour avec succès.");
        }

        // JOUR 7 : AFFECTATION, DIAGNOSTIC ET CLÔTURE

        // 4. Affectation d'un ticket à un technicien par le Responsable
        public (bool Success, string Message) AffecterTechnicien(int idTicket, int idTechnicien, int idUserAction)
        {
            using var context = new AppDbContext();

            var userAction = context.Users.Find(idUserAction);
            if (userAction == null || userAction.Role != "Responsable")
                return (false, "Seul un Responsable Informatique peut assigner un ticket.");

            var ticket = context.Tickets.Find(idTicket);
            if (ticket == null) return (false, "Ticket introuvable.");

            var technicien = context.Users.Find(idTechnicien);
            if (technicien == null || technicien.Role != "Technicien")
                return (false, "L'utilisateur sélectionné n'est pas un technicien valide.");

            ticket.IdUserTraiteur = idTechnicien;
            if (ticket.Statut == "En attente")
            {
                ticket.Statut = "En cours";
            }

            context.SaveChanges();
            return (true, $"Ticket assigné avec succès au technicien ID {technicien.IdUser}.");
        }
        // 5. Traitement d'un ticket par le Technicien (Prise en charge, Diagnostic, Actions, Clôture)
        public (bool Success, string Message) TraiterTicket(int idTicket, int idTechnicien, string diagnostic, string typeIntervention, string actionRealisee, bool cloturer = false)
        {
            using var context = new AppDbContext();

            var ticket = context.Tickets
                .Include(t => t.Equipement)
                .FirstOrDefault(t => t.IdTicket == idTicket);

            if (ticket == null) return (false, "Ticket introuvable.");

            var tech = context.Users.Find(idTechnicien);
            if (tech == null || (tech.Role != "Technicien" && tech.Role != "Responsable"))
                return (false, "Vous n'avez pas les droits requis pour traiter ce ticket.");

            if (ticket.IdUserTraiteur == null)
            {
                ticket.IdUserTraiteur = idTechnicien;
            }

            ticket.Diagnostic = string.IsNullOrWhiteSpace(diagnostic) ? ticket.Diagnostic : diagnostic.Trim();
            ticket.TypeIntervention = string.IsNullOrWhiteSpace(typeIntervention) ? ticket.TypeIntervention : typeIntervention.Trim();
            ticket.ActionRealisee = string.IsNullOrWhiteSpace(actionRealisee) ? ticket.ActionRealisee : actionRealisee.Trim();

            if (cloturer)
            {
                ticket.Statut = "Terminé";
                ticket.DateCloture = DateTime.Now;

                if (ticket.Equipement != null && ticket.Equipement.Etat == "En maintenance")
                {
                    ticket.Equipement.Etat = "En service";
                }
            }
            else
            {
                ticket.Statut = "En cours";
            }

            context.SaveChanges();

            string message = cloturer ? "Ticket clôturé et marqué comme 'Terminé'." : "Avancement du traitement enregistré avec succès.";
            return (true, message);
        }
    }
}