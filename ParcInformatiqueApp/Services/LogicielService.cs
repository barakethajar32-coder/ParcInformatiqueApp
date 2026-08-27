using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class LogicielService
    {
        // ==========================================
        // 1. GESTION DU RÉFÉRENTIEL LOGICIELS
        // ==========================================

        /// <summary>
        /// Récupère la liste complète des logiciels avec les installations associées.
        /// </summary>
        public List<Logiciel> GetAllLogiciels()
        {
            using var context = new AppDbContext();
            return context.Logiciels
                .Include(l => l.Installations)
                    .ThenInclude(il => il.Equipement)
                .AsNoTracking()
                .OrderBy(l => l.NomLogiciel)
                .ToList();
        }

        /// <summary>
        /// Récupère un logiciel spécifique par son identifiant ID.
        /// </summary>
        public Logiciel? GetLogicielById(int idLogiciel)
        {
            using var context = new AppDbContext();
            return context.Logiciels
                .Include(l => l.Installations)
                    .ThenInclude(il => il.Equipement)
                .AsNoTracking()
                .FirstOrDefault(l => l.IdLogiciel == idLogiciel);
        }

        /// <summary>
        /// Ajoute un nouveau logiciel au référentiel.
        /// </summary>
        public (bool Success, string Message, int IdLogiciel) AddLogiciel(string nomLogiciel, string version, string licence, DateTime dateExpiration)
        {
            if (string.IsNullOrWhiteSpace(nomLogiciel))
                return (false, "Le nom du logiciel est obligatoire.", 0);

            using var context = new AppDbContext();

            bool existe = context.Logiciels.Any(l => l.NomLogiciel.ToLower() == nomLogiciel.Trim().ToLower()
                                                  && l.Version.ToLower() == (version ?? "").Trim().ToLower());
            if (existe)
                return (false, "Ce logiciel existe déjà dans cette même version.", 0);

            var logiciel = new Logiciel
            {
                NomLogiciel = nomLogiciel.Trim(),
                Version = (version ?? "").Trim(),
                Licence = (licence ?? "").Trim(),
                DateExpiration = dateExpiration
            };

            context.Logiciels.Add(logiciel);
            context.SaveChanges();

            return (true, "Logiciel ajouté au référentiel avec succès.", logiciel.IdLogiciel);
        }

        /// <summary>
        /// Met à jour un logiciel existant.
        /// </summary>
        public (bool Success, string Message) UpdateLogiciel(int idLogiciel, string nomLogiciel, string version, string licence, DateTime dateExpiration)
        {
            if (string.IsNullOrWhiteSpace(nomLogiciel))
                return (false, "Le nom du logiciel est obligatoire.");

            using var context = new AppDbContext();

            var logiciel = context.Logiciels.Find(idLogiciel);
            if (logiciel == null)
                return (false, "Logiciel introuvable.");

            logiciel.NomLogiciel = nomLogiciel.Trim();
            logiciel.Version = (version ?? "").Trim();
            logiciel.Licence = (licence ?? "").Trim();
            logiciel.DateExpiration = dateExpiration;

            context.SaveChanges();
            return (true, "Informations du logiciel mises à jour avec succès.");
        }

        /// <summary>
        /// Supprime un logiciel si celui-ci n'est installé sur aucun équipement.
        /// </summary>
        public (bool Success, string Message) DeleteLogiciel(int idLogiciel)
        {
            using var context = new AppDbContext();

            var logiciel = context.Logiciels
                .Include(l => l.Installations)
                .FirstOrDefault(l => l.IdLogiciel == idLogiciel);

            if (logiciel == null)
                return (false, "Logiciel introuvable.");

            if (logiciel.Installations != null && logiciel.Installations.Any())
            {
                return (false, "Impossible de supprimer ce logiciel car il est actuellement installé sur un ou plusieurs équipements. Désinstallez-le d'abord.");
            }

            context.Logiciels.Remove(logiciel);
            context.SaveChanges();

            return (true, "Logiciel supprimé avec succès.");
        }

        // ==========================================
        // 2. GESTION DES INSTALLATIONS (LOGICIEL <-> ÉQUIPEMENT)
        // ==========================================

        /// <summary>
        /// Installe/Associe un logiciel à un équipement donné avec prise en compte du champ VersionInstallee.
        /// </summary>
        public (bool Success, string Message) InstallerLogiciel(int idLogiciel, int idEquipement, string? versionInstallee = null, DateTime? dateInstallation = null)
        {
            using var context = new AppDbContext();

            var logiciel = context.Logiciels.Find(idLogiciel);
            if (logiciel == null)
                return (false, "Logiciel introuvable.");

            var equipement = context.Equipements.Find(idEquipement);
            if (equipement == null)
                return (false, "Équipement introuvable.");

            bool dejaInstalle = context.InstallationsLogiciels.Any(il => il.IdLogiciel == idLogiciel && il.IdEquipement == idEquipement);
            if (dejaInstalle)
                return (false, "Ce logiciel est déjà installé sur cet équipement.");

            if (logiciel.DateExpiration < DateTime.Now)
            {
                return (false, $"La licence de ce logiciel a expiré le {logiciel.DateExpiration:dd/MM/yyyy}. Impossible d'effectuer l'installation.");
            }

            var installation = new InstallationLogiciel
            {
                IdLogiciel = idLogiciel,
                IdEquipement = idEquipement,
                VersionInstallee = string.IsNullOrWhiteSpace(versionInstallee) ? logiciel.Version : versionInstallee.Trim(),
                DateInstallation = dateInstallation ?? DateTime.Now
            };

            context.InstallationsLogiciels.Add(installation);
            context.SaveChanges();

            return (true, $"Le logiciel '{logiciel.NomLogiciel}' a été installé avec succès sur l'équipement.");
        }

        /// <summary>
        /// Supprime l'installation d'un logiciel sur un équipement (Désinstallation).
        /// </summary>
        public (bool Success, string Message) DesinstallerLogiciel(int idLogiciel, int idEquipement)
        {
            using var context = new AppDbContext();

            var installation = context.InstallationsLogiciels
                .FirstOrDefault(il => il.IdLogiciel == idLogiciel && il.IdEquipement == idEquipement);

            if (installation == null)
                return (false, "L'installation spécifiée n'existe pas.");

            context.InstallationsLogiciels.Remove(installation);
            context.SaveChanges();

            return (true, "Logiciel désinstallé de l'équipement avec succès.");
        }

        /// <summary>
        /// Récupère toutes les installations enregistrées sur un équipement spécifique.
        /// </summary>
        public List<InstallationLogiciel> GetInstallationsParEquipement(int idEquipement)
        {
            using var context = new AppDbContext();
            return context.InstallationsLogiciels
                .Include(il => il.Logiciel)
                .Where(il => il.IdEquipement == idEquipement)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Alerte de gestion des licences : liste les logiciels dont l'expiration survient sous un nombre de jours donné.
        /// </summary>
        public List<Logiciel> GetLicencesProchesExpiration(int joursAlerte = 30)
        {
            using var context = new AppDbContext();
            DateTime dateLimite = DateTime.Now.AddDays(joursAlerte);

            return context.Logiciels
                .Where(l => l.DateExpiration <= dateLimite)
                .OrderBy(l => l.DateExpiration)
                .AsNoTracking()
                .ToList();
        }
    }
}
