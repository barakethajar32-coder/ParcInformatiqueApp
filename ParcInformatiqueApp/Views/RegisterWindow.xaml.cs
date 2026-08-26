using System.Windows;

namespace ParcInformatiqueApp.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNom.Text) ||
                string.IsNullOrWhiteSpace(TxtPrenom.Text) ||
                string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                MessageBox.Show(
                    "Veuillez remplir les champs obligatoires.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Compte créé avec succès.\nEn attente de validation par le responsable informatique.",
                "Inscription",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            this.Close();
        }
    }
}