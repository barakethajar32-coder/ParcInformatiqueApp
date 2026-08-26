using System;
using System.Windows;

namespace ParcInformatiqueApp.Views
{
    public partial class AffectationWindow : Window
    {
        public AffectationWindow(string equipement)
        {
            InitializeComponent();

            TxtEquipement.Text = equipement;
            DateAffectation.SelectedDate = DateTime.Now;
        }

        private void BtnAffecter_Click(object sender, RoutedEventArgs e)
        {
            if (CboEmploye.SelectedIndex <= 0)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un employé.",
                    "Affectation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Équipement affecté avec succès.",
                "Affectation",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            Close();
        }
    }
}