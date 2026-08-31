using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views.Technicien
{
    public partial class TicketsTechnicienView : UserControl
    {
        private List<Ticket> tousLesTickets =
            new List<Ticket>();

        public TicketsTechnicienView()
        {
            InitializeComponent();

            ChargerTicketsTest();
        }

        // ==========================================
        // TICKETS DE TEST
        // ==========================================

        private void ChargerTicketsTest()
        {
            tousLesTickets = new List<Ticket>
            {
                new Ticket
                {
                    IdTicket = 1,
                    NomTicket = "PC ne démarre pas",
                    Description = "L'ordinateur ne démarre plus.",
                    Priorite = "Élevée",
                    Statut = "En attente",
                    DateCreation = DateTime.Now.AddDays(-2),

                    Equipement = new Equipement
                    {
                        NomEquipement = "PC Dell Latitude"
                    }
                },

                new Ticket
                {
                    IdTicket = 2,
                    NomTicket = "Imprimante bloquée",
                    Description = "L'imprimante ne fonctionne plus.",
                    Priorite = "Normale",
                    Statut = "En cours",
                    DateCreation = DateTime.Now.AddDays(-1),

                    Equipement = new Equipement
                    {
                        NomEquipement = "HP LaserJet"
                    }
                },

                new Ticket
                {
                    IdTicket = 3,
                    NomTicket = "Connexion Internet",
                    Description = "Impossible de se connecter à Internet.",
                    Priorite = "Faible",
                    Statut = "Terminé",
                    DateCreation = DateTime.Now.AddDays(-3),

                    Equipement = new Equipement
                    {
                        NomEquipement = "PC Lenovo"
                    }
                }
            };

            DgTickets.ItemsSource = tousLesTickets;
        }

        // ==========================================
        // FILTRE
        // ==========================================

        private void CboStatut_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DgTickets == null)
                return;

            string statut = "Tous";

            if (CboStatut.SelectedItem is ComboBoxItem item)
            {
                statut = item.Content?.ToString() ?? "Tous";
            }

            if (statut == "Tous")
            {
                DgTickets.ItemsSource = tousLesTickets;
            }
            else
            {
                DgTickets.ItemsSource =
                    tousLesTickets
                        .Where(t => t.Statut == statut)
                        .ToList();
            }
        }

        // ==========================================
        // TRAITER
        // ==========================================

        private void BtnTraiter_Click(
     object sender,
     RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is Ticket ticket)
            {
                TicketTraitementView traitementView =
                    new TicketTraitementView(ticket);

                Window window = new Window
                {
                    Title = "Traitement du ticket",
                    Content = traitementView,
                    Width = 700,
                    Height = 700,
                    WindowStartupLocation =
                        WindowStartupLocation.CenterScreen
                };

                window.ShowDialog();

                // Actualiser le tableau après modification
                DgTickets.ItemsSource = null;
                DgTickets.ItemsSource = tousLesTickets;
            }
        }
    }
}