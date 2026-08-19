using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class Employe
    {
        public int IdEmploye { get; set; } // PK
        public string Nom { get; set; } = string.Empty; 
        public string Prenom { get; set; } = string.Empty; 
        public string Poste { get; set; } = string.Empty;

        // Clés étrangères (FK)
        public int IdService { get; set; }

        public Service? Service { get; set; }


        public int? IdLocalisation { get; set; }
        public Localisation? Localisation { get; set; }

        // Compte utilisateur associé
        public User? User { get; set; }

        // Relations[cite: 1]
        public ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    }
}