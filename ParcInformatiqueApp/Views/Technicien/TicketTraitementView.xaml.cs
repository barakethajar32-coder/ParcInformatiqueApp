using System;
using System.Windows;
using System.Windows.Controls;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views.Technicien
{
    public partial class TicketTraitementView : UserControl
    {
        private Ticket ticket;

        public TicketTraitementView(Ticket ticket)
        {
            InitializeComponent();

            this.ticket = ticket;

            AfficherTicket();
        }

        // ==========================================
        // AFFICHER LE TICKET
        // ==========================================

        private void AfficherTicket()
        {
            TxtNomTicket.Text =
                ticket.NomTicket;

            TxtDescription.Text =
                ticket.Description;

            TxtPriorite.Text =
                ticket.Priorite;

            TxtDiagnostic.Text =
                ticket.Diagnostic ?? "";

            TxtAction.Text =
                ticket.ActionRealisee ?? "";

            // Sélectionner le statut actuel
            if (ticket.Statut == "En attente")
            {
                CboStatut.SelectedIndex = 0;
            }
            else if (ticket.Statut == "En cours")
            {
                CboStatut.SelectedIndex = 1;
            }
            else if (ticket.Statut == "Terminé")
            {
                CboStatut.SelectedIndex = 2;
            }
        }

        // ==========================================
        // ENREGISTRER
        // ==========================================

        private void BtnEnregistrer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (CboStatut.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un statut.",
                    "Traitement",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            ComboBoxItem selectedItem =
                CboStatut.SelectedItem as ComboBoxItem;

            string nouveauStatut =
                selectedItem?.Content?.ToString()
                ?? "En attente";

            // Modifier le ticket
            ticket.Statut = nouveauStatut;

            ticket.Diagnostic =
                string.IsNullOrWhiteSpace(TxtDiagnostic.Text)
                ? null
                : TxtDiagnostic.Text.Trim();

            ticket.ActionRealisee =
                string.IsNullOrWhiteSpace(TxtAction.Text)
                ? null
                : TxtAction.Text.Trim();

            // Date de clôture
            if (nouveauStatut == "Terminé")
            {
                ticket.DateCloture = DateTime.Now;
            }
            else
            {
                ticket.DateCloture = null;
            }

            MessageBox.Show(
                "Les modifications du ticket ont été enregistrées.",
                "Traitement",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}