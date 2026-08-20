using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;
using ParcInformatiqueApp.Security;

namespace ParcInformatiqueApp.Services
{
    public class AuthService
    {
        public (bool Success, string Message, User User) Authenticate(string login, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users
                    .Include(u => u.Employe)
                    .FirstOrDefault(u => u.Login == login);

                if (user == null)
                    return (false, "Identifiant ou mot de passe incorrect.", null);

                if (!PasswordHasher.VerifyPassword(password, user.MotDePasse))
                    return (false, "Identifiant ou mot de passe incorrect.", null);

                if (user.StatutCompte == "Désactivé")
                    return (false, "Votre compte est désactivé.", null);

                return (true, "Connexion réussie.", user);
            }
        }
    }
}