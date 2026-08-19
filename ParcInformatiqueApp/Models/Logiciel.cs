using System;
using System.Collections.Generic;

namespace ParcInformatiqueApp.Models
{
    public class Logiciel
    {
        public int IdLogiciel { get; set; } // Clé primaire (PK)[cite: 1]
        public string NomLogiciel { get; set; } = string.Empty; 
        public string Version { get; set; } = string.Empty; 
        public string Licence { get; set; } = string.Empty; 
        public DateTime DateExpiration { get; set; }

        // Relation : Un logiciel peut être installé sur plusieurs équipements[cite: 1]
        public ICollection<InstallationLogiciel> Installations { get; set; } = new List<InstallationLogiciel>(); 
    }
}
