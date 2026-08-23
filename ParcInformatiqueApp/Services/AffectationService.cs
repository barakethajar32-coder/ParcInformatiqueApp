using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class AffectationService
    {
        // 1. Affecter un équipement à un employé
        public (bool Success, string Message) AffecterMateriel(int idEquipement, int idEmploye, string? commentaire = null)
        {
            using var context = new AppDbContext();

            var eq = context.Equipements.Find(idEquipement);
            if (eq == null) return (false, "Équipement introuvable.");
            if (eq.Etat != "En service" && eq.Etat != "Disponible")
                return (false, $"L'équipement n'est pas disponible (État actuel: {eq.Etat}).");

            var emp = context.Employes.Find(idEmploye);
            if (emp == null) return (false, "Employé introuvable.");

            var affectation = new Affectation
            {
                IdEquipement = idEquipement,
                IdEmploye = idEmploye,
                DateAffectation = DateTime.Now,
            };

            eq.Etat = "En service";

            context.Affectations.Add(affectation);
            context.SaveChanges();

            return (true, "Équipement affecté avec succès.");
        }

        // 2. Traiter le retour d'un équipement
        public (bool Success, string Message) RetournerMateriel(int idAffectation, string nouvelEtatMateriel = "En service")
        {
            using var context = new AppDbContext();

            var affectation = context.Affectations
                .Include(a => a.Equipement)
                .FirstOrDefault(a => a.IdAffectation == idAffectation);

            if (affectation == null) return (false, "Affectation introuvable.");
            if (affectation.DateRetour.HasValue) return (false, "Cet équipement a déjà été retourné.");

            affectation.DateRetour = DateTime.Now;

            if (affectation.Equipement != null)
            {
                affectation.Equipement.Etat = nouvelEtatMateriel;
            }

            context.SaveChanges();
            return (true, "Matériel retourné et état mis à jour.");
        }

        // 3. Obtenir l'historique complet des affectations pour un équipement
        public List<Affectation> GetHistoriqueEquipement(int idEquipement)
        {
            using var context = new AppDbContext();
            return context.Affectations
                .Include(a => a.Employe)
                .Where(a => a.IdEquipement == idEquipement)
                .OrderByDescending(a => a.DateAffectation)
                .AsNoTracking()
                .ToList();
        }
    }
}