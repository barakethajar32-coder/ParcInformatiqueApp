using System.Text.RegularExpressions;
using BCrypt.Net;

namespace ParcInformatiqueApp.Security
{
    public static class PasswordHasher
    {
        // Hachage du mot de passe avec sel automatique (BCrypt)
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        // Vérification du mot de passe haché lors de la connexion
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword)) return false;
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Validation de sécurité : 8 caractères min, 1 majuscule, 1 chiffre, 1 caractère spécial
        public static (bool IsValid, string ErrorMessage) ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return (false, "Le mot de passe doit contenir au moins 8 caractères.");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return (false, "Le mot de passe doit contenir au moins une lettre majuscule.");

            if (!Regex.IsMatch(password, @"[0-9]"))
                return (false, "Le mot de passe doit contenir au moins un chiffre.");

            if (!Regex.IsMatch(password, @"[\W_]"))
                return (false, "Le mot de passe doit contenir au moins un caractère spécial.");

            return (true, string.Empty);
        }
    }
}
