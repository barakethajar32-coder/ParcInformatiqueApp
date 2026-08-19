using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class Localisation
    {
        public int IdLocalisation { get; set; } // PK
        public string Batiment { get; set; } = string.Empty; 
        public string Salle { get; set; } = string.Empty; 
        public string NbBureau { get; set; } = string.Empty; 

        // Relations
        public ICollection<Employe> Employes { get; set; } = new List<Employe>(); 
        public ICollection<Equipement> Equipements { get; set; } = new List<Equipement>(); 
    }
}