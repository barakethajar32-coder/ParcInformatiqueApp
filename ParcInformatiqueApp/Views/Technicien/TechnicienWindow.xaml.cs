using System.Windows;

namespace ParcInformatiqueApp.Views.Technicien
{
    public partial class TechnicienWindow : Window
    {
        public TechnicienWindow()
        {
            InitializeComponent();

            // Page affichée au démarrage
            TechnicienContentArea.Content =
                new TechnicienDashboardView();
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            TechnicienContentArea.Content =
                new TechnicienDashboardView();
        }

        private void BtnTickets_Click(object sender, RoutedEventArgs e)
        {
            TechnicienContentArea.Content =
                new TicketsTechnicienView();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow =
                new LoginWindow();

            loginWindow.Show();

            this.Close();
        }
    }
}