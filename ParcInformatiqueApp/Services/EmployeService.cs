using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class EmployeService
    {
        // Récupérer la liste complète des employés avec leurs relations
        public List<Employe> GetAllEmployes()
        {
            using var context = new AppDbContext();
            return context.Employes
                .Include(e => e.Service)
                .Include(e => e.Localisation)
                .Include(e => e.User)
                .AsNoTracking()
                .ToList();
        }

        // Ajouter un nouvel employé
        public (bool Success, string Message, int EmployeId) AddEmploye(string nom, string prenom, string poste, int idService, int idLocalisation)
        {
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom))
                return (false, "Le nom et le prénom sont obligatoires.", 0);

            using var context = new AppDbContext();

            var employe = new Employe
            {
                Nom = nom.Trim(),
                Prenom = prenom.Trim(),
                Poste = poste?.Trim(),
                IdService = idService,
                IdLocalisation = idLocalisation
            };

            context.Employes.Add(employe);
            context.SaveChanges();

            return (true, "Employé créé avec succès.", employe.IdEmploye);
        }

        // Mettre à jour les informations d'un employé
        public (bool Success, string Message) UpdateEmploye(int idEmploye, string nom, string prenom, string poste, int idService, int idLocalisation)
        {
            using var context = new AppDbContext();
            var employe = context.Employes.Find(idEmploye);
            if (employe == null) return (false, "Employé introuvable.");

            employe.Nom = nom.Trim();
            employe.Prenom = prenom.Trim();
            employe.Poste = poste?.Trim();
            employe.IdService = idService;
            employe.IdLocalisation = idLocalisation;

            context.SaveChanges();
            return (true, "Employé mis à jour avec succès.");
        }

        // Association ou création d'un compte User pour un Employé
        public (bool Success, string Message) LinkUserToEmploye(int idEmploye, string login, string role)
        {
            using var context = new AppDbContext();

            var employe = context.Employes.Include(e => e.User).FirstOrDefault(e => e.IdEmploye == idEmploye);
            if (employe == null) return (false, "Employé introuvable.");

            if (employe.User != null)
                return (false, "Cet employé possède déjà un compte utilisateur associé.");

            if (context.Users.Any(u => u.Login.ToLower() == login.Trim().ToLower()))
                return (false, "Ce nom d'utilisateur (login) est déjà utilisé.");

            var newUser = new User
            {
                Login = login.Trim(),
                MotDePasse = Security.PasswordHasher.HashPassword("DefaultPass123!"), // Mot de passe par défaut
                Role = role,
                StatutCompte = "Activé",
                DateCreation = DateTime.Now,
                IdEmploye = idEmploye
            };

            context.Users.Add(newUser);
            context.SaveChanges();

            return (true, "Compte utilisateur créé et associé à l'employé avec succès.");
        }

        // Supprimer un employé (s'il n'a pas de contraintes actives)
        public (bool Success, string Message) DeleteEmploye(int idEmploye)
        {
            using var context = new AppDbContext();
            var employe = context.Employes.Include(e => e.User).FirstOrDefault(e => e.IdEmploye == idEmploye);
            if (employe == null) return (false, "Employé introuvable.");

            if (context.Affectations.Any(a => a.IdEmploye == idEmploye && a.DateRetour == null))
                return (false, "Impossible de supprimer cet employé : des équipements lui sont actuellement affectés.");

            context.Employes.Remove(employe);
            context.SaveChanges();
            return (true, "Employé supprimé avec succès.");
        }
    }
}