using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class Service
    {
        public int IdService { get; set; } // Clé primaire (PK)
        public string NomService { get; set; } = string.Empty; 

        // Relation : Un service contient plusieurs employés
        public ICollection<Employe> Employes { get; set; } = new List<Employe>();
    }
}