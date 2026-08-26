using System.Windows;
using System.Windows.Controls;

namespace ParcInformatiqueApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            // Sélection par défaut
            CmbRole.SelectedIndex = 0;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtLogin.Text) ||
                string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                MessageBox.Show(
                    "Veuillez remplir tous les champs.",
                    "Connexion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Récupération du rôle choisi
            ComboBoxItem selectedRole =
                CmbRole.SelectedItem as ComboBoxItem;

            if (selectedRole == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un rôle.",
                    "Connexion",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            string role = selectedRole.Content.ToString();

            // Employé
            if (role == "Employé")
            {
                Employee.EmployeeWindow employeeWindow =
                    new Employee.EmployeeWindow();

                employeeWindow.Show();
                this.Close();
            }

            // Responsable
            else if (role == "Responsable")
            {
                MainWindow mainWindow = new MainWindow();

                mainWindow.Show();
                this.Close();
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();

            registerWindow.ShowDialog();
        }
    }
}
