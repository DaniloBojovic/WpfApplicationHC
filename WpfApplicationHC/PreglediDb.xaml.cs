using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PreglediDb.xaml
    /// </summary>
    public partial class PreglediDb : Page
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        //SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true");
        //string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True";
        DataSet ds;
        DataTable dt;
        public static int Id;
        int n;

        public PreglediDb()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myDataGrid.CanUserAddRows = false;
            updateDataGrid();
            btnLekari.IsEnabled = false;
        }

        private void updateDataGrid()
        {
            //conn.Close();
            conn = new SqlConnection(constr); //DODODO
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT PregledID, Naziv, Opis From Pregled", conn);
                SqlDataAdapter sda = new SqlDataAdapter("Select Max(Cast(PregledID as int)) + 1 from Pregled", conn);
                ds = new DataSet();
                dt = new DataTable();
                sda.Fill(dt);
                Id = int.Parse(dt.Rows[0][0].ToString());
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(ds);
                myDataGrid.HeadersVisibility = DataGridHeadersVisibility.All;
                myDataGrid.ItemsSource = ds.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnLekari.IsEnabled = true;
            DataGrid dg = sender as DataGrid; //kastujem ga kao datagrid object
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtID.Text = dr["PregledID"].ToString();
                txtNaziv.Text = dr["Naziv"].ToString();
                txtOpis.Text = dr["Opis"].ToString();
                Form.idMain = Convert.ToInt32(dr["PregledID"]);//dodao
                btnAdd.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDel.IsEnabled = true;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNaziv.Text) && !string.IsNullOrWhiteSpace(txtOpis.Text))
            {
                string sql = "Insert into Pregled(PregledID, Naziv, Opis) " +
                "values(@PregledID, @Naziv, @Opis)";
                AddUpdDel(sql, 1); //2 param. sql naredba i broj za operaciju (add-1)
            }
            else
            {
                MessageBox.Show("Unesite ispravne podatke.");
            }
            if (n != 0)
            {
                btnAdd.IsEnabled = false;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNaziv.Text) && !string.IsNullOrWhiteSpace(txtOpis.Text))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da napravite izmene?", "Potvrda izmena", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    string sql = "Update Pregled set Naziv=@Naziv, Opis=@Opis";
                    this.AddUpdDel(sql, 2);
                    this.Reset();
                }
            }
            else
            {
                MessageBox.Show("Polja ne smeju biti prazna.");
                if (string.IsNullOrWhiteSpace(txtNaziv.Text))
                {
                    txtNaziv.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtOpis.Text))
                {
                    txtOpis.Focus();
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete pregled?", "Potvrda brisanja", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string sql = "Delete from Pregled where PregledID=@PregledID";
                this.AddUpdDel(sql, 3);
                this.Reset();
            }
        }

        private void AddUpdDel(string sqlString, int state)
        {
            string msg = "";
            //conn = null;// zatvaram konekciju otvaram i inic. ponovo
            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                switch (state)
                {
                    case 1:
                        cmd.Parameters.Add("@PregledID", SqlDbType.Int).Value = txtID.Text;
                        cmd.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = txtNaziv.Text;
                        cmd.Parameters.Add("@Opis", SqlDbType.VarChar).Value = txtOpis.Text;
                        msg = "Pregled je uspesno dodat.";
                        break;
                    case 2:
                        cmd.Parameters.Add("@PregledID", SqlDbType.VarChar).Value = txtNaziv.Text;
                        cmd.Parameters.Add("@Opis", SqlDbType.VarChar).Value = txtOpis.Text;
                        cmd.Parameters.Add("@PregledID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Informacije uspesno izmenjene.";
                        break;
                    case 3:
                        cmd.Parameters.Add("@PregledID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Pregled uspesno obrisan.";
                        break;
                }
                try
                {
                    n = cmd.ExecuteNonQuery();
                    if (n > 0)
                    {
                        MessageBox.Show(msg);
                        updateDataGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unesite ispravne podatke.");
                }
            }
        }

        private void Reset()
        {
            txtID.Text = Id.ToString();
            txtNaziv.Text = "";
            txtOpis.Text = "";
            btnAdd.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDel.IsEnabled = false;
            btnLekari.IsEnabled = false;
        }

        private void lblCreatePrgldClick(object sender, MouseButtonEventArgs e)
        {
            txtID.Text = Id.ToString();
            Reset();
            btnLekari.IsEnabled = false;
            //frmKarton.Content = null;
        }

        private void btnLekari_Click(object sender, RoutedEventArgs e)
        {
            WindowDodajLkr wdl = new WindowDodajLkr();
            wdl.Show();
        }

        private void txtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
