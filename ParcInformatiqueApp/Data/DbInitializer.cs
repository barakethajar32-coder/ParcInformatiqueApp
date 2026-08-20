using System;
using System.Linq;
using ParcInformatiqueApp.Models;
using ParcInformatiqueApp.Security;

namespace ParcInformatiqueApp.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any()) return;

            // 1. Service IT
            var serviceIT = new Service { NomService = "Informatique" };
            context.Services.Add(serviceIT);

            // 2. Localisation
            var loc = new Localisation { Batiment = "A", Salle = "101", NbBureau = "B1" };
            context.Localisations.Add(loc);

            context.SaveChanges();

            // 3. Employé Administrateur
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

            // 4. Utilisateur Admin avec mot de passe haché par BCrypt
            var userAdmin = new User
            {
                Login = "admin",
                MotDePasse = PasswordHasher.HashPassword("Admin1234!"),
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