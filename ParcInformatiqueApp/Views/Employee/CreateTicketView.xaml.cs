using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ParcInformatiqueApp.Views.Employee
{
    public partial class CreateTicketView : UserControl
    {
        public CreateTicketView()
        {
            InitializeComponent();

            ChargerEquipementsTest();

            CmbPriorite.SelectedIndex = 1;
        }

        private void ChargerEquipementsTest()
        {
            var equipements = new List<EquipementTicketTest>
            {
                new EquipementTicketTest
                {
                    Id = 1,
                    Nom = "PC-001 - HP ProBook 450"
                },

                new EquipementTicketTest
                {
                    Id = 2,
                    Nom = "IMP-001 - Canon i-SENSYS"
                },

                new EquipementTicketTest
                {
                    Id = 3,
                    Nom = "PC-002 - Dell Latitude 5420"
                }
            };

            CmbEquipement.ItemsSource = equipements;
            CmbEquipement.DisplayMemberPath = "Nom";
            CmbEquipement.SelectedValuePath = "Id";
        }

        private void BtnCreerTicket_Click(object sender, RoutedEventArgs e)
        {
            if (CmbEquipement.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un équipement.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(TxtTitre.Text))
            {
                MessageBox.Show(
                    "Veuillez saisir le titre du problème.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtTitre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                MessageBox.Show(
                    "Veuillez décrire le problème.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtDescription.Focus();
                return;
            }

            if (CmbPriorite.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner une priorité.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            MessageBox.Show(
                "Ticket créé avec succès !",
                "Confirmation",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            TxtTitre.Clear();
            TxtDescription.Clear();

            CmbEquipement.SelectedIndex = -1;
            CmbPriorite.SelectedIndex = 1;
        }


        // AJOUTER ICI
        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            TxtTitre.Clear();
            TxtDescription.Clear();

            CmbEquipement.SelectedIndex = -1;
            CmbPriorite.SelectedIndex = 1;
        }
    }


    public class EquipementTicketTest
    {
        public int Id { get; set; }

        public string Nom { get; set; }
    }
}