using System;

namespace ParcInformatiqueApp.Models
{
    public class InstallationLogiciel
    {
        public int IdInstallation { get; set; } // Clé primaire (PK)

        public DateTime DateInstallation { get; set; } = DateTime.Now; 
        public string VersionInstallee { get; set; } = string.Empty; 

        // Clé étrangère vers Logiciel[cite: 1]
        public int IdLogiciel { get; set; }
        public Logiciel? Logiciel { get; set; }
       

        // Clé étrangère vers Equipement[cite: 1]
        public int IdEquipement { get; set; }
        public Equipement? Equipement { get; set; }
    }
}