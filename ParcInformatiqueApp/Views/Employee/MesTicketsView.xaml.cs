using System.Collections.Generic;
using System.Windows.Controls;

namespace ParcInformatiqueApp.Views.Employee
{
    public partial class MesTicketsView : UserControl
    {
        public MesTicketsView()
        {
            InitializeComponent();

            ChargerTicketsTest();
        }

        private void ChargerTicketsTest()
        {
            var tickets = new List<TicketTest>
            {
                new TicketTest
                {
                    IdTicket = 1,
                    Titre = "PC ne démarre pas",
                    Equipement = "PC-001",
                    DateCreation = "26/08/2026",
                    Priorite = "Élevée",
                    Statut = "En attente"
                },

                new TicketTest
                {
                    IdTicket = 2,
                    Titre = "Problème imprimante",
                    Equipement = "IMP-001",
                    DateCreation = "25/08/2026",
                    Priorite = "Normale",
                    Statut = "En cours"
                },

                new TicketTest
                {
                    IdTicket = 3,
                    Titre = "Installation logiciel",
                    Equipement = "PC-002",
                    DateCreation = "24/08/2026",
                    Priorite = "Faible",
                    Statut = "Terminé"
                }
            };

            DgMesTickets.ItemsSource = tickets;

            TxtTotal.Text = tickets.Count.ToString();

            TxtEnAttente.Text =
                tickets.FindAll(t => t.Statut == "En attente").Count.ToString();

            TxtEnCours.Text =
                tickets.FindAll(t => t.Statut == "En cours").Count.ToString();
        }
    }


    public class TicketTest
    {
        public int IdTicket { get; set; }

        public string Titre { get; set; }

        public string Equipement { get; set; }

        public string DateCreation { get; set; }

        public string Priorite { get; set; }

        public string Statut { get; set; }
    }
}