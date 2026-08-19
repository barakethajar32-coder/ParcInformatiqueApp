using System.Windows;
using ParcInformatiqueApp.Data;

namespace ParcInformatiqueApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Exécution du Seeding au démarrage
            using (var context = new AppDbContext())
            {
                DbInitializer.Seed(context);
            }
        }
    }
}
