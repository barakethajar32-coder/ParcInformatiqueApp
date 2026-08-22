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

            if (context.Users.Any()) return; // Déjà initialisé

            // 1. Services
            var serviceIT = new Service { NomService = "Informatique" };
            var serviceRH = new Service { NomService = "Ressources Humaines" };
            var serviceCompta = new Service { NomService = "Comptabilité" };
            context.Services.AddRange(serviceIT, serviceRH, serviceCompta);

            // 2. Localisations
            var locA1 = new Localisation { Batiment = "Bâtiment A", Salle = "Salle 101", NbBureau = "B01" };
            var locA2 = new Localisation { Batiment = "Bâtiment A", Salle = "Salle 102", NbBureau = "B05" };
            var locB1 = new Localisation { Batiment = "Bâtiment B", Salle = "Salle 201", NbBureau = "B12" };
            context.Localisations.AddRange(locA1, locA2, locB1);

            context.SaveChanges();

            // 3. Types d'Équipements
            var typePC = new TypeEquipement { Libelle = "PC Portable" };
            var typeEcran = new TypeEquipement { Libelle = "Écran" };
            var typeImprimante = new TypeEquipement { Libelle = "Imprimante" };
            context.TypeEquipements.AddRange(typePC, typeEcran, typeImprimante);

            // 4. Employés
            var empAdmin = new Employe { Nom = "Admin", Prenom = "Système", Poste = "Responsable IT", IdService = serviceIT.IdService, IdLocalisation = locA1.IdLocalisation };
            var empTech = new Employe { Nom = "Dupont", Prenom = "Jean", Poste = "Technicien Support", IdService = serviceIT.IdService, IdLocalisation = locA1.IdLocalisation };
            var empUser = new Employe { Nom = "Martin", Prenom = "Sophie", Poste = "Gestionnaire RH", IdService = serviceRH.IdService, IdLocalisation = locA2.IdLocalisation };
            context.Employes.AddRange(empAdmin, empTech, empUser);

            context.SaveChanges();

            // 5. Utilisateurs
            var hashPassword = PasswordHasher.HashPassword("Pass1234!");
            context.Users.AddRange(
                new User { Login = "admin", MotDePasse = hashPassword, Role = "Responsable", StatutCompte = "Activé", DateCreation = DateTime.Now, IdEmploye = empAdmin.IdEmploye },
                new User { Login = "tech", MotDePasse = hashPassword, Role = "Technicien", StatutCompte = "Activé", DateCreation = DateTime.Now, IdEmploye = empTech.IdEmploye },
                new User { Login = "user", MotDePasse = hashPassword, Role = "Employé", StatutCompte = "Activé", DateCreation = DateTime.Now, IdEmploye = empUser.IdEmploye }
            );

            context.SaveChanges();
        }
    }
}