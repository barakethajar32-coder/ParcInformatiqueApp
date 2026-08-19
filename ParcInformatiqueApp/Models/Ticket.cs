using System;

namespace ParcInformatiqueApp.Models
{
    public class Ticket
    {
        public int IdTicket { get; set; } // PK[cite: 1]
        public DateTime DateCreation { get; set; } = DateTime.Now; 
        public string NomTicket { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public string? Diagnostic { get; set; }
        public string? TypeIntervention { get; set; }
        public string Priorite { get; set; } = "Moyenne"; // Basse, Moyenne, Haute[cite: 1]
        public string Statut { get; set; } = "En attente"; // En attente, En cours, Terminé[cite: 1]
        public string? ActionRealisee { get; set; }
        public DateTime? DateCloture { get; set; }

        // FK Équipement[cite: 1]
        public int IdEquipement { get; set; }
        public Equipement? Equipement { get; set; }

        // FK Créateur du ticket[cite: 1]
        public int IdUserCreateur { get; set; }
        public User? UserCreateur { get; set; }
        // FK Technicien/Traiteur du ticket[cite: 1]
        public int? IdUserTraiteur { get; set; }
        public User? UserTraiteur { get; set; }
    }
}
