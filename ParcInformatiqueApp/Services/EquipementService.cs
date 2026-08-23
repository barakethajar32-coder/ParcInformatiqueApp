using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class EquipementService
    {
        // 1. Recherche avec filtres multiples
        public List<Equipement> GetEquipements(string? search = null, int? idType = null, string? etat = null, bool? sousGarantie = null)
        {
            using var context = new AppDbContext();
            var query = context.Equipements
                .Include(e => e.TypeEquipement)
                .Include(e => e.Localisation)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim().ToLower();
                query = query.Where(e => e.NumeroSerie.ToLower().Contains(term) ||
                                         e.NomEquipement.ToLower().Contains(term) ||
                                         e.Marque.ToLower().Contains(term) ||
                                         e.Modele.ToLower().Contains(term));
            }

            if (idType.HasValue && idType.Value > 0)
                query = query.Where(e => e.IdType == idType.Value);

            if (!string.IsNullOrWhiteSpace(etat))
                query = query.Where(e => e.Etat == etat);

            if (sousGarantie.HasValue)
            {
                DateTime today = DateTime.Today;
                if (sousGarantie.Value)
                    query = query.Where(e => e.FinGarantie >= today);
                else
                    query = query.Where(e => e.FinGarantie < today);
            }

            return query.ToList();
        }

        // 2. Ajouter un équipement
        public (bool Success, string Message, int IdEquipement) AddEquipement(Equipement equipement)
        {
            if (string.IsNullOrWhiteSpace(equipement.NumeroSerie))
                return (false, "Le numéro de série est obligatoire.", 0);

            using var context = new AppDbContext();
            if (context.Equipements.Any(e => e.NumeroSerie.ToLower() == equipement.NumeroSerie.Trim().ToLower()))
                return (false, "Un équipement avec ce numéro de série existe déjà.", 0);

            equipement.Etat = string.IsNullOrWhiteSpace(equipement.Etat) ? "En service" : equipement.Etat;
            context.Equipements.Add(equipement);
            context.SaveChanges();

            return (true, "Équipement enregistré avec succès.", equipement.IdEquipement);
        }

        // 3. Mise à jour de l'état
        public (bool Success, string Message) UpdateEtat(int idEquipement, string nouvelEtat)
        {
            string[] etatsValides = { "En service", "En maintenance", "Hors service" };
            if (!etatsValides.Contains(nouvelEtat))
                return (false, "État invalide.");

            using var context = new AppDbContext();
            var eq = context.Equipements.Find(idEquipement);
            if (eq == null) return (false, "Équipement introuvable.");

            eq.Etat = nouvelEtat;
            context.SaveChanges();
            return (true, $"État mis à jour vers '{nouvelEtat}'.");
        }
    }
}
