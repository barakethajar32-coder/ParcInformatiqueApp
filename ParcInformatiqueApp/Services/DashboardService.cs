using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;

namespace ParcInformatiqueApp.Services
{
    public class DashboardStatsDto
    {
        public int TotalEquipements { get; set; }
        public int EquipementsEnPanne { get; set; }
        public int EquipementsEnService { get; set; }
        public int EquipementsEnMaintenance { get; set; }

        public int TotalTickets { get; set; }
        public int TicketsEnAttente { get; set; }
        public int TicketsEnCours { get; set; }
        public int TicketsResolus { get; set; }

        public int LicencesExpirees { get; set; }
        public int LicencesProchesExpiration { get; set; }
    }

    public class DashboardService
    {
        /// <summary>
        /// Exécute une opération BDD avec la stratégie de reconnexion automatique EF Core en cas d'interruption réseau.
        /// </summary>
        public static T ExecuteWithRetry<T>(Func<AppDbContext, T> operation)
        {
            using var context = new AppDbContext();
            var strategy = context.Database.CreateExecutionStrategy();

            return strategy.Execute(() =>
            {
                return operation(context);
            });
        }

        /// <summary>
        /// Récupère toutes les données statistiques agrégées pour le tableau de bord du Responsable.
        /// </summary>
        public DashboardStatsDto GetDashboardStatistics()
        {
            return ExecuteWithRetry(context =>
            {
                var stats = new DashboardStatsDto();

                // Statistiques Équipements
                stats.TotalEquipements = context.Equipements.Count();
                stats.EquipementsEnPanne = context.Equipements.Count(e => e.Etat == "En panne");
                stats.EquipementsEnService = context.Equipements.Count(e => e.Etat == "En service");
                stats.EquipementsEnMaintenance = context.Equipements.Count(e => e.Etat == "En maintenance");

                // Statistiques Tickets
                stats.TotalTickets = context.Tickets.Count();
                stats.TicketsEnAttente = context.Tickets.Count(t => t.Statut == "En attente");
                stats.TicketsEnCours = context.Tickets.Count(t => t.Statut == "En cours");
                stats.TicketsResolus = context.Tickets.Count(t => t.Statut == "Terminé");

                // Statistiques Licences Logiciels
                DateTime auj = DateTime.Now;
                DateTime limiteAlerte = auj.AddDays(30);

                stats.LicencesExpirees = context.Logiciels.Count(l => l.DateExpiration < auj);
                stats.LicencesProchesExpiration = context.Logiciels.Count(l => l.DateExpiration >= auj && l.DateExpiration <= limiteAlerte);

                return stats;
            });
        }
    }
}
