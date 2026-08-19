using ParcInformatiqueApp.Data;
using System.Windows;

namespace ParcInformatiqueApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TxtLogin.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Veuillez remplir tous les champs.";
                return;
            }

            using (var context = new AppDbContext())
            {
                // Recherche de l'utilisateur en base
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.MotDePasse == password);

                if (user != null)
                {
                    if (user.StatutCompte == "Désactivé")
                    {
                        TxtError.Text = "Votre compte est en attente de validation.";
                        return;
                    }

                    MessageBox.Show($"Connexion réussie ! Rôle : {user.Role}", "Bienvenue",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    // TODO : Rediriger vers le Dashboard correspondant selon user.Role
                }
                else
                {
                    TxtError.Text = "Identifiant ou mot de passe incorrect.";
                }
            }
        }
    }
}