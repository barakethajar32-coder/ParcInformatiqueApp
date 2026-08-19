using System;
using System.Linq;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Vérifie si la base contient déjà des données
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // La BDD a déjà été initialisée
            }

            // 1. Ajouter un Service
            var serviceIT = new Service { NomService = "Informatique" };
            context.Services.Add(serviceIT);

            // 2. Ajouter une Localisation
            var loc = new Localisation { Batiment = "A", Salle = "101", NbBureau = "B1" };
            context.Localisations.Add(loc);

            context.SaveChanges();

            // 3. Ajouter un Employé
            var employeAdmin = new Employe
            {
                Nom = "Admin",
                Prenom = "Système",
                Poste = "Responsable IT",
                IdService = serviceIT.IdService,
                IdLocalisation = loc.IdLocalisation
            };
            context.Employes.Add(employeAdmin);
            context.SaveChanges();

            // 4. Ajouter un Utilisateur Admin (Mot de passe à hacher ultérieurement)
            var userAdmin = new User
            {
                Login = "admin",
                MotDePasse = "Admin1234!", // À remplacer par un hash BCrypt plus tard
                Role = "Responsable",
                StatutCompte = "Activé",
                DateCreation = DateTime.Now,
                IdEmploye = employeAdmin.IdEmploye
            };
            context.Users.Add(userAdmin);

            context.SaveChanges();
        }
    }
}