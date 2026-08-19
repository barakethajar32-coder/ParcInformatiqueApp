using System;
using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class User
    {
        public int IdUser { get; set; } // PK[cite: 1]
        public string Login { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty; // Sera haché en BCrypt[cite: 1]
        public string Role { get; set; } = "Employé"; // Employé, Technicien, Responsable[cite: 1]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        // FK vers Employe (Relation 1-1)[cite: 1]
        public int IdEmploye { get; set; }
        public Employe? Employe { get; set; }

        // Tickets créés et traités[cite: 1]
        public ICollection<Ticket> TicketsCrees { get; set; } = new List<Ticket>(); 
        public ICollection<Ticket> TicketsTraites { get; set; } = new List<Ticket>(); 
    }
}