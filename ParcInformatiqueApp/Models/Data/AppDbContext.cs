using Microsoft.EntityFrameworkCore;
using ParcInformatiqueApp.Models;

namespace ParcInformatiqueApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Localisation> Localisations { get; set; }
        public DbSet<TypeEquipement> TypesEquipements { get; set; }
        public DbSet<Equipement> Equipements { get; set; }
        public DbSet<Affectation> Affectations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Logiciel> Logiciels { get; set; }
        public DbSet<InstallationLogiciel> InstallationsLogiciels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Chaîne de connexion SQL Server Express (à adapter si le nom de l'instance change)
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=ParcInformatiqueDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des clés primaires
            modelBuilder.Entity<Service>().HasKey(s => s.IdService); 
            modelBuilder.Entity<Localisation>().HasKey(l => l.IdLocalisation); 
            modelBuilder.Entity<Employe>().HasKey(e => e.IdEmploye); 
            modelBuilder.Entity<User>().HasKey(u => u.IdUser); 
            modelBuilder.Entity<TypeEquipement>().HasKey(t => t.IdType); 
            modelBuilder.Entity<Equipement>().HasKey(eq => eq.IdEquipement); 
            modelBuilder.Entity<Affectation>().HasKey(a => a.IdAffectation);
            modelBuilder.Entity<Ticket>().HasKey(t => t.IdTicket); 
            modelBuilder.Entity<Logiciel>().HasKey(l => l.IdLogiciel); 
            modelBuilder.Entity<InstallationLogiciel>().HasKey(i => i.IdInstallation);

            // Configuration des relations pour les tickets (UserCreateur et UserTraiteur)[cite: 1]
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UserCreateur)
                .WithMany(u => u.TicketsCrees)
                .HasForeignKey(t => t.IdUserCreateur)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UserTraiteur)
                .WithMany(u => u.TicketsTraites)
                .HasForeignKey(t => t.IdUserTraiteur)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}