using System.Collections.Generic;
using System.Windows.Controls;

namespace ParcInformatiqueApp.Views.Employee
{
    public partial class MesEquipementsView : UserControl
    {
        public MesEquipementsView()
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
                    NomEquipement = "PC-001",
                    Type = "Ordinateur",
                    Marque = "HP",
                    Modele = "ProBook 450",
                    Etat = "En service"
                },

                new EquipementTest
                {
                    NomEquipement = "IMP-001",
                    Type = "Imprimante",
                    Marque = "Canon",
                    Modele = "i-SENSYS",
                    Etat = "En service"
                },

                new EquipementTest
                {
                    NomEquipement = "PC-002",
                    Type = "Ordinateur",
                    Marque = "Dell",
                    Modele = "Latitude 5420",
                    Etat = "En maintenance"
                }
            };

            DgMesEquipements.ItemsSource = equipements;
        }
    }

    public class EquipementTest
    {
        public string NomEquipement { get; set; }
        public string Type { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Etat { get; set; }
    }
}