using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplicationHC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int idMain = 0;
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True");
        public MainWindow()
        {
            InitializeComponent();
            frmMain.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Main.Content = new PacijentiDb();
            frmMain.Content = new HomePage();
        }

        private void PacijentiBtnClick(object sender, RoutedEventArgs e)
        {
            frmMain.Content = new PacijentiDb();
        }

        private void LekariBtnClick(object sender, RoutedEventArgs e)
        {
            frmMain.Content = new LekariDb();
        }

        private void PreglediBtnClick(object sender, RoutedEventArgs e)
        {
            frmMain.Content = new PreglediDb();
        }
    }
}
