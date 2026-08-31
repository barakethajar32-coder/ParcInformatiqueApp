using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views.Employee
{
    public partial class CreateTicketView : UserControl
    {
        // Liste temporaire des tickets créés par les employés
        public static List<Ticket> TicketsCrees { get; set; }
            = new List<Ticket>();

        public CreateTicketView()
        {
            InitializeComponent();

            ChargerEquipementsTest();

            // Priorité par défaut : Normale
            CmbPriorite.SelectedIndex = 1;
        }

        // ==========================================
        // CHARGER LES ÉQUIPEMENTS DE TEST
        // ==========================================
        private void ChargerEquipementsTest()
        {
            var equipements = new List<EquipementTicketTest>
            {
                new EquipementTicketTest
                {
                    Id = 1,
                    Nom = "PC-001 - HP ProBook 450"
                },

                new EquipementTicketTest
                {
                    Id = 2,
                    Nom = "IMP-001 - Canon i-SENSYS"
                },

                new EquipementTicketTest
                {
                    Id = 3,
                    Nom = "PC-002 - Dell Latitude 5420"
                }
            };

            CmbEquipement.ItemsSource = equipements;
            CmbEquipement.DisplayMemberPath = "Nom";
            CmbEquipement.SelectedValuePath = "Id";
        }

        // ==========================================
        // CRÉER LE TICKET
        // ==========================================
        private void BtnCreerTicket_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier l'équipement
            if (CmbEquipement.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un équipement.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Vérifier le titre
            if (string.IsNullOrWhiteSpace(TxtTitre.Text))
            {
                MessageBox.Show(
                    "Veuillez saisir le titre du problème.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtTitre.Focus();
                return;
            }

            // Vérifier la description
            if (string.IsNullOrWhiteSpace(TxtDescription.Text))
            {
                MessageBox.Show(
                    "Veuillez décrire le problème.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtDescription.Focus();
                return;
            }

            // Vérifier la priorité
            if (CmbPriorite.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner une priorité.",
                    "Création du ticket",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Récupérer l'équipement sélectionné
            EquipementTicketTest equipement =
                (EquipementTicketTest)CmbEquipement.SelectedItem;

            // Récupérer la priorité
            string priorite =
                ((ComboBoxItem)CmbPriorite.SelectedItem)
                .Content
                .ToString();

            // ==========================================
            // CRÉATION DU TICKET
            // ==========================================
            Ticket nouveauTicket = new Ticket
            {
                IdTicket = TicketsCrees.Count + 1,

                DateCreation = DateTime.Now,

                NomTicket = TxtTitre.Text.Trim(),

                Description = TxtDescription.Text.Trim(),

                Priorite = priorite,

                Statut = "En attente",

                IdEquipement = equipement.Id,

                Equipement = new Equipement
                {
                    IdEquipement = equipement.Id,
                    NomEquipement = equipement.Nom
                }
            };

            // ==========================================
            // AJOUTER LE TICKET À LA LISTE
            // ==========================================
            TicketsCrees.Add(nouveauTicket);

            // ==========================================
            // MESSAGE DE CONFIRMATION
            // ==========================================
            MessageBox.Show(
                "Ticket créé avec succès !",
                "Confirmation",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            // ==========================================
            // RÉINITIALISER LE FORMULAIRE
            // ==========================================
            TxtTitre.Clear();

            TxtDescription.Clear();

            CmbEquipement.SelectedIndex = -1;

            CmbPriorite.SelectedIndex = 1;
        }

        // ==========================================
        // ANNULER
        // ==========================================
        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            TxtTitre.Clear();

            TxtDescription.Clear();

            CmbEquipement.SelectedIndex = -1;

            CmbPriorite.SelectedIndex = 1;
        }
    }

    // ==========================================
    // CLASSE DE TEST POUR LES ÉQUIPEMENTS
    // ==========================================
    public class EquipementTicketTest
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;
    }
}