using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Services
{
    public class TypeEquipementService
    {
        // Obtenir la liste de tous les types d'équipements
        public List<TypeEquipement> GetAllTypes()
        {
            using var context = new AppDbContext();
            return context.TypeEquipements.AsNoTracking().ToList();
        }

        // Ajouter un nouveau type d'équipement
        public (bool Success, string Message) AddType(string libelleType)
        {
            if (string.IsNullOrWhiteSpace(libelleType))
                return (false, "Le libellé du type d'équipement est obligatoire.");

            using var context = new AppDbContext();

            if (context.TypeEquipements.Any(t => t.Libelle.ToLower() == libelleType.Trim().ToLower()))
                return (false, "Ce type d'équipement existe déjà.");

            context.TypeEquipements.Add(new TypeEquipement { Libelle = libelleType.Trim() });
            context.SaveChanges();

            return (true, "Type d'équipement ajouté avec succès.");
        }

        // Modifier un type d'équipement
        public (bool Success, string Message) UpdateType(int idType, string nouveauLibelle)
        {
            if (string.IsNullOrWhiteSpace(nouveauLibelle))
                return (false, "Le libellé ne peut pas être vide.");

            using var context = new AppDbContext();
            var typeEq = context.TypeEquipements.Find(idType);
            if (typeEq == null) return (false, "Type d'équipement introuvable.");

            typeEq.Libelle = nouveauLibelle.Trim();
            context.SaveChanges();

            return (true, "Type d'équipement mis à jour avec succès.");
        }

        // Supprimer un type d'équipement s'il n'est relié à aucun matériel
        public (bool Success, string Message) DeleteType(int idType)
        {
            using var context = new AppDbContext();
            var typeEq = context.TypeEquipements.Find(idType);
            if (typeEq == null) return (false, "Type d'équipement introuvable.");

            if (context.Equipements.Any(eq => eq.IdType == idType))
                return (false, "Impossible de supprimer ce type : des équipements enregistrés y sont rattachés.");

            context.TypeEquipements.Remove(typeEq);
            context.SaveChanges();

            return (true, "Type d'équipement supprimé avec succès.");
        }
    }
}
