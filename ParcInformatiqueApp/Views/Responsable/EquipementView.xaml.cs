using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ParcInformatiqueApp.Views
{
    public partial class EquipementView : UserControl
    {
        public EquipementView()
        {
            InitializeComponent();

            ChargerEquipementsTest();
        }

        private void ChargerEquipementsTest()
        {
            var equipements = new List<EquipementTest>
            {
                new EquipementTest
                {
                    NomEquipement = "PC Dell",
                    NumeroSerie = "DELL-001",
                    Marque = "Dell",
                    Modele = "OptiPlex",
                    Etat = "En service"
                },

                new EquipementTest
                {
                    NomEquipement = "Imprimante HP",
                    NumeroSerie = "HP-002",
                    Marque = "HP",
                    Modele = "LaserJet",
                    Etat = "En maintenance"
                },

                new EquipementTest
                {
                    NomEquipement = "Imprimante Canon",
                    NumeroSerie = "CAN-004",
                    Marque = "Canon",
                    Modele = "1-SENSYS",
                    Etat = "En maintenance"
                }
            };

            DgEquipements.ItemsSource = equipements;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CboEtat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void BtnAddEquipement_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Formulaire d'ajout d'équipement.");
        }

        private void BtnAffecter_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
                return;

            var equipement = button.DataContext as EquipementTest;

            if (equipement == null)
                return;

            AffectationWindow window =
                new AffectationWindow(equipement.NomEquipement);

            window.ShowDialog();
        }
    }

    public class EquipementTest
    {
        public string NomEquipement { get; set; }
        public string NumeroSerie { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Etat { get; set; }
    }
}
