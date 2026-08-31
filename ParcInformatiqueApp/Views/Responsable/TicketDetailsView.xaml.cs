using System.Windows.Controls;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views
{
    public partial class TicketDetailsView : UserControl
    {
        private Ticket ticket;

        public TicketDetailsView(Ticket ticket)
        {
            InitializeComponent();

            this.ticket = ticket;

            AfficherDetails();
        }

        private void AfficherDetails()
        {
            TxtNomTicket.Text = ticket.NomTicket;
            TxtDescription.Text = ticket.Description;
            TxtPriorite.Text = ticket.Priorite;
            TxtStatut.Text = ticket.Statut;

            TxtDiagnostic.Text =
                ticket.Diagnostic ?? "Non renseigné";

            TxtAction.Text =
                ticket.ActionRealisee ?? "Non renseignée";
        }
    }
}