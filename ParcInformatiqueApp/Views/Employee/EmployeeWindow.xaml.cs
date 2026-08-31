using System.Windows;

namespace ParcInformatiqueApp.Views.Employee
{
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow()
        {
            InitializeComponent();

            // Page affichée au démarrage
            EmployeeContentArea.Content = new EmployeeDashboardView();
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            EmployeeContentArea.Content = new EmployeeDashboardView();
        }

        private void BtnEquipements_Click(object sender, RoutedEventArgs e)
        {
            EmployeeContentArea.Content = new MesEquipementsView();
        }

        private void BtnCreateTicket_Click(object sender, RoutedEventArgs e)
        {
            EmployeeContentArea.Content = new CreateTicketView();
        }

        private void BtnMesTickets_Click(object sender, RoutedEventArgs e)
        {
            EmployeeContentArea.Content = new MesTicketsView();
        }

        // ==========================================
        // DÉCONNEXION
        // ==========================================
        private void BtnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Voulez-vous vraiment vous déconnecter ?",
                "Déconnexion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();

                loginWindow.Show();

                this.Close();
            }
        }
    }
}