using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;
using ParcInformatiqueApp.Security;

namespace ParcInformatiqueApp.Services
{
    public class ReferentielService
    {
        // CRUD SERVICES 
        public List<Service> GetAllServices()
        {
            using var context = new AppDbContext();
            return context.Services.AsNoTracking().ToList();
        }

        public (bool Success, string Message) AddService(string nomService)
        {
            if (string.IsNullOrWhiteSpace(nomService))
                return (false, "Le nom du service est obligatoire.");

            using var context = new AppDbContext();
            if (context.Services.Any(s => s.NomService.ToLower() == nomService.Trim().ToLower()))
                return (false, "Ce service existe déjà.");

            context.Services.Add(new Service { NomService = nomService.Trim() });
            context.SaveChanges();
            return (true, "Service ajouté avec succès.");
        }

        public (bool Success, string Message) UpdateService(int idService, string nouveauNom)
        {
            if (string.IsNullOrWhiteSpace(nouveauNom))
                return (false, "Le nom du service ne peut pas être vide.");

            using var context = new AppDbContext();
            var service = context.Services.Find(idService);
            if (service == null) return (false, "Service introuvable.");

            service.NomService = nouveauNom.Trim();
            context.SaveChanges();
            return (true, "Service mis à jour avec succès.");
        }

        public (bool Success, string Message) DeleteService(int idService)
        {
            using var context = new AppDbContext();
            var service = context.Services.Find(idService);
            if (service == null) return (false, "Service introuvable.");

            if (context.Employes.Any(e => e.IdService == idService))
                return (false, "Impossible de supprimer ce service car des employés y sont rattachés.");

            context.Services.Remove(service);
            context.SaveChanges();
            return (true, "Service supprimé avec succès.");
        }

        //  CRUD LOCALISATIONS 
        public List<Localisation> GetAllLocalisations()
        {
            using var context = new AppDbContext();
            return context.Localisations.AsNoTracking().ToList();
        }

        public (bool Success, string Message) AddLocalisation(string batiment, string salle, string nbBureau)
        {
            if (string.IsNullOrWhiteSpace(batiment) || string.IsNullOrWhiteSpace(salle))
                return (false, "Le bâtiment et la salle sont obligatoires.");

            using var context = new AppDbContext();
            context.Localisations.Add(new Localisation
            {
                Batiment = batiment.Trim(),
                Salle = salle.Trim(),
                NbBureau = nbBureau?.Trim()
            });
            context.SaveChanges();
            return (true, "Localisation ajoutée avec succès.");
        }
    }
}