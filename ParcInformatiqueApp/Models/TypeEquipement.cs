using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class TypeEquipement
    {
        public int IdType { get; set; } // PK[cite: 1]
        public string Libelle { get; set; } = string.Empty; 

        public ICollection<Equipement> Equipements { get; set; } = new List<Equipement>(); 
    }
}