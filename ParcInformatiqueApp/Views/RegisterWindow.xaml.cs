using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ParcInformatiqueApp.Data;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Views
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            CboRole.SelectedIndex = 0;

            ChargerServices();
        }

        // ==========================================
        // CHARGER LES SERVICES
        // ==========================================
        private void ChargerServices()
        {
            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    // Si aucun service n'existe,
                    // on crée quelques services de base.
                    if (!db.Services.Any())
                    {
                        db.Services.AddRange(
                            new Service
                            {
                                NomService = "Informatique"
                            },

                            new Service
                            {
                                NomService = "Ressources Humaines"
                            },

                            new Service
                            {
                                NomService = "Finance"
                            },

                            new Service
                            {
                                NomService = "Direction"
                            },

                            new Service
                            {
                                NomService = "Comptabilité"
                            }
                        );

                        db.SaveChanges();
                    }

                    var services = db.Services
                        .OrderBy(s => s.NomService)
                        .ToList();

                    CboService.ItemsSource = services;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Impossible de charger les services.\n\n" +
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // ==========================================
        // INSCRIPTION
        // ==========================================
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Vérifier le nom
            if (string.IsNullOrWhiteSpace(TxtNom.Text))
            {
                MessageBox.Show(
                    "Veuillez saisir le nom.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtNom.Focus();
                return;
            }

            // Vérifier le prénom
            if (string.IsNullOrWhiteSpace(TxtPrenom.Text))
            {
                MessageBox.Show(
                    "Veuillez saisir le prénom.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtPrenom.Focus();
                return;
            }

            // Vérifier le mot de passe
            if (string.IsNullOrWhiteSpace(TxtPassword.Password))
            {
                MessageBox.Show(
                    "Veuillez saisir un mot de passe.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtPassword.Focus();
                return;
            }

            // Vérifier le service
            if (CboService.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un service.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Vérifier le poste
            if (string.IsNullOrWhiteSpace(TxtPoste.Text))
            {
                MessageBox.Show(
                    "Veuillez saisir le poste.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                TxtPoste.Focus();
                return;
            }

            // Vérifier le rôle
            if (CboRole.SelectedItem == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un rôle.",
                    "Inscription",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            try
            {
                using (AppDbContext db = new AppDbContext())
                {
                    // ==========================================
                    // RÉCUPÉRER LE SERVICE
                    // ==========================================

                    Service service =
                        (Service)CboService.SelectedItem;

                    // ==========================================
                    // RÉCUPÉRER LE RÔLE
                    // ==========================================

                    ComboBoxItem selectedRole =
                        CboRole.SelectedItem as ComboBoxItem;

                    string role =
                        selectedRole?.Content?.ToString()
                        ?? "Employé";

                    // ==========================================
                    // GÉNÉRER LE LOGIN
                    // ==========================================

                    string login =
                        TxtPrenom.Text.Trim().ToLower()
                        + "."
                        + TxtNom.Text.Trim().ToLower();

                    login = login.Replace(" ", "");

                    // ==========================================
                    // VÉRIFIER SI LE LOGIN EXISTE
                    // ==========================================

                    bool loginExiste =
                        db.Users.Any(u => u.Login == login);

                    if (loginExiste)
                    {
                        MessageBox.Show(
                            "Un compte avec ce nom existe déjà.\n\n" +
                            "Login : " + login,
                            "Inscription",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }

                    // ==========================================
                    // CRÉER L'EMPLOYÉ
                    // ==========================================

                    Employe employe = new Employe
                    {
                        Nom = TxtNom.Text.Trim(),

                        Prenom = TxtPrenom.Text.Trim(),

                        Poste = TxtPoste.Text.Trim(),

                        IdService = service.IdService
                    };

                    db.Employes.Add(employe);

                    db.SaveChanges();

                    // ==========================================
                    // CRÉER LE USER
                    // ==========================================

                    User user = new User
                    {
                        Login = login,

                        MotDePasse = TxtPassword.Password,

                        Role = role,

                        StatutCompte = "En attente",

                        DateCreation = DateTime.Now,

                        IdEmploye = employe.IdEmploye
                    };

                    db.Users.Add(user);

                    db.SaveChanges();

                    // ==========================================
                    // MESSAGE DE CONFIRMATION
                    // ==========================================

                    MessageBox.Show(
                        "Compte créé avec succès !\n\n" +
                        "Login : " + login + "\n\n" +
                        "Votre compte est en attente de validation " +
                        "par le responsable informatique.",
                        "Inscription",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Fermer la fenêtre
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Une erreur est survenue lors de l'inscription.\n\n" +
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}