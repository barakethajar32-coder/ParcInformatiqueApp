using System.Windows;

namespace ParcInformatiqueApp.Views
{
    public partial class ReferentielView : System.Windows.Controls.UserControl
    {
        public ReferentielView()
        {
            InitializeComponent();
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Service ajouté.");
        }

        private void BtnAddLocalisation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Localisation ajoutée.");
        }
    }
}