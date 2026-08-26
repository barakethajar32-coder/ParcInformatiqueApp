using System.Windows;

namespace ParcInformatiqueApp.Views
{
    public partial class EmployeView : System.Windows.Controls.UserControl
    {
        public EmployeView()
        {
            InitializeComponent();
        }

        private void BtnAddEmploye_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Formulaire nouvel employé.");
        }
    }
}