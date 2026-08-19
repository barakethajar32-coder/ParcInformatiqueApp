using System;
using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class Equipement
    {
        public int IdEquipement { get; set; } // PK[cite: 1]
        public string NomEquipement { get; set; } = string.Empty; 
        public string Marque { get; set; } = string.Empty; 
        public string Modele { get; set; } = string.Empty; 
        public string NumeroSerie { get; set; } = string.Empty; 
        public DateTime DateAchat { get; set; }
        public string Fournisseur { get; set; } = string.Empty; 
        public DateTime FinGarantie { get; set; }
        public string Etat { get; set; } = "En service"; // En service, En maintenance, Hors service[cite: 1]

        // FKs[cite: 1]
        public int IdType { get; set; }
        public TypeEquipement? TypeEquipement { get; set; }

        public int IdLocalisation { get; set; }
        public Localisation? Localisation { get; set; }

        // Relations[cite: 1]
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>(); 
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); 
        public ICollection<InstallationLogiciel> Installations { get; set; } = new List<InstallationLogiciel>(); 
    }
}
