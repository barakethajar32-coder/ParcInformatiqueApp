using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;
using ParcInformatiqueApp.Security;

namespace ParcInformatiqueApp.Services
{
    public class UserService
    {
        public List<User> GetAllUsers()
        {
            using var context = new AppDbContext();
            return context.Users.Include(u => u.Employe).AsNoTracking().ToList();
        }

        // Activation / Désactivation de compte
        public (bool Success, string Message) ToggleUserStatus(int userId)
        {
            using var context = new AppDbContext();
            var user = context.Users.Find(userId);
            if (user == null) return (false, "Utilisateur introuvable.");

            // Règle métier : Basculer entre 'Activé' et 'Désactivé'
            user.StatutCompte = (user.StatutCompte == "Activé") ? "Désactivé" : "Activé";
            context.SaveChanges();

            return (true, $"Le compte de {user.Login} est maintenant {user.StatutCompte}.");
        }

        public (bool Success, string Message) UpdateRole(int userId, string newRole)
        {
            string[] validRoles = { "Employé", "Responsable", "Technicien" };
            if (!validRoles.Contains(newRole))
                return (false, "Rôle invalide.");

            using var context = new AppDbContext();
            var user = context.Users.Find(userId);
            if (user == null) return (false, "Utilisateur introuvable.");

            user.Role = newRole;
            context.SaveChanges();
            return (true, "Rôle mis à jour avec succès.");
        }
    }
}
