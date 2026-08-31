using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views
{
    public partial class TicketsView : UserControl
    {
        private List<Ticket> tousLesTickets = new List<Ticket>();

        public TicketsView()
        {
            InitializeComponent();

            ChargerTicketsTest();
        }

        // ==============================
        // CHARGER LES TICKETS DE TEST
        // ==============================
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
        }
    };

            // Ajouter les tickets créés par Employee
            foreach (Ticket ticket in
                     ParcInformatiqueApp.Views.Employee.CreateTicketView.TicketsCrees)
            {
                tousLesTickets.Add(ticket);
            }

            DgTickets.ItemsSource = tousLesTickets;
        }

        // ==============================
        // RECHERCHE
        // ==============================
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            AppliquerFiltres();
        }

        // ==============================
        // FILTRE STATUT
        // ==============================
        private void CboStatut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppliquerFiltres();
        }

        // ==============================
        // APPLIQUER LES FILTRES
        // ==============================
        private void AppliquerFiltres()
        {
            if (DgTickets == null)
                return;

            string recherche = TxtSearch?.Text?.ToLower() ?? "";

            string statut = "Tous les statuts";

            if (CboStatut?.SelectedItem is ComboBoxItem item)
            {
                statut = item.Content?.ToString() ?? "Tous les statuts";
            }

            var resultat = tousLesTickets
                .Where(t =>
                    string.IsNullOrEmpty(recherche)
                    ||
                    t.NomTicket.ToLower().Contains(recherche)
                    ||
                    t.Description.ToLower().Contains(recherche)
                )
                .Where(t =>
                    statut == "Tous les statuts"
                    ||
                    t.Statut == statut
                )
                .ToList();

            DgTickets.ItemsSource = resultat;
        }

        // ==============================
        // BOUTON DETAILS
        // ==============================
        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is Ticket ticket)
            {
                TicketDetailsView detailsView =
                    new TicketDetailsView(ticket);

                Window window = new Window
                {
                    Title = "Détails du ticket",
                    Content = detailsView,
                    Width = 600,
                    Height = 500,
                    WindowStartupLocation =
                        WindowStartupLocation.CenterScreen
                };

                window.ShowDialog();
            }
        }

        // ==============================
        // BOUTON AFFECTER
        // ==============================
        private void BtnAffecter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is Ticket ticket)
            {
                MessageBox.Show(
                    $"Ticket sélectionné : {ticket.NomTicket}",
                    "Affectation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}