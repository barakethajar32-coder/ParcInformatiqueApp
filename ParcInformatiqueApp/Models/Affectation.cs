using System;

namespace ParcInformatiqueApp.Models
{
    public class Affectation
    {
        public int IdAffectation { get; set; } // PK
        public DateTime DateAffectation { get; set; } = DateTime.Now; 
        public DateTime? DateRetour { get; set; }
        public string Observation { get; set; } = string.Empty; 

        public int IdEmploye { get; set; }
        public Employe? Employe { get; set; }
        public int IdEquipement { get; set; }
        public Equipement? Equipement { get; set; }
    }
}