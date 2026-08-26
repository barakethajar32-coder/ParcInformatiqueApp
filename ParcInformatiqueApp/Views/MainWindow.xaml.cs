using System.Windows;

namespace ParcInformatiqueApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Page affichée au démarrage
            MainContentArea.Content = new EquipementView();
        }

        private void NavEquipements_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new EquipementView();
        }

        private void NavEmployes_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new EmployeView();
        }

        private void NavReferentiels_Click(object sender, RoutedEventArgs e)
        {
            MainContentArea.Content = new ReferentielView();
        }

        private void NavValidation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Validation des comptes utilisateurs.",
                "Validation",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}