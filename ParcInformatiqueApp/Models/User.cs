using System;
using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string MotDePasse { get; set; }
        public string Role { get; set; }

        // Propriété manquante responsable des erreurs
        public string StatutCompte { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;

        // Clé étrangère et navigation vers Employe
        public int IdEmploye { get; set; }
        public Employe Employe { get; set; }

        // Navigation vers les tickets
        public ICollection<Ticket> TicketsCrees { get; set; }
        public ICollection<Ticket> TicketsTraites { get; set; }
    }
}