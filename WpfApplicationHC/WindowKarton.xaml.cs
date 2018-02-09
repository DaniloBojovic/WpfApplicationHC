using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplicationHC
{
    /// <summary>
    /// Interaction logic for WindowKarton.xaml
    /// </summary>
    ///
    public partial class WindowKarton : Window
    {
        int num;
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        DataSet ds/* = new DataSet()*/;
        DataTable dt/* = new DataTable()*/;
        int n;

        public WindowKarton()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
        private void Update()
        { 
            DataGridKarton.CanUserAddRows = false;
            conn = new SqlConnection(constr);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select PacijentID, KartonID as 'Sifra pregleda'," +
                    " Izvestaj, Naziv, 'Dr ' + Prezime + ', ' + Ime As Lekar" +
                    " from Karton k, Pregled p, LekarPregled lp, Lekar l" +
                    " where k.PregledID = p.PregledID and p.PregledID = lp.PregledID" +
                    " and lp.LekarID = l.ID and PacijentID = @ID", conn);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
                ds = new DataSet();
                dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                DataGridKarton.HeadersVisibility = DataGridHeadersVisibility.All;
                DataGridKarton.ItemsSource = ds.Tables[0].DefaultView;
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

        private void cmbClick(object sender, SelectionChangedEventArgs e)
        {
            string str;
            txtLekar.Visibility = Visibility.Visible;
            txtOpis.Visibility = Visibility.Visible;
            groupBox.Visibility = Visibility.Visible;
            btnZakazi.Visibility = Visibility.Visible;

            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije

            using (conn)
            {
                conn.Open();
                if (cmbOdaberi.SelectedIndex == 0)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if(cmbOdaberi.SelectedIndex == 1)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if (cmbOdaberi.SelectedIndex == 2)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if (cmbOdaberi.SelectedIndex == 3)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if (cmbOdaberi.SelectedIndex == 4)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if (cmbOdaberi.SelectedIndex == 5)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
                else if (cmbOdaberi.SelectedIndex == 6)
                {
                    str = cmbOdaberi.SelectedValue.ToString().Split(':')[1];
                    str = str.Substring(1);
                    SqlCommand cmd1 = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as lekar, Opis " +
                        "from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.Naziv = @Naziv");
                    cmd1.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    cmd1.Connection = conn;
                    SqlDataReader rdr = cmd1.ExecuteReader();
                    while (rdr.Read())
                    {
                        txtLekar.Text = rdr[0].ToString();
                        txtOpis.Text = rdr[1].ToString();
                    }
                    SqlCommand cmd2 = new SqlCommand("Select PregledID from Pregled where naziv = @Naziv", conn);
                    cmd2.Parameters.Add("@Naziv", SqlDbType.VarChar).Value = str;
                    num = int.Parse(cmd2.ExecuteScalar().ToString());
                }
            }
        }

        private void btnZakaziClick(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            using (conn)
            {
                try
                {
                    conn.Open();
                    //SqlCommand cmd = new SqlCommand("Insert into Karton values(1000,18,null)", conn);
                    SqlCommand cmd = new SqlCommand("Insert into Karton values(@PregledID,@PacijentID,null)", conn);
                    cmd.Parameters.Add("@PregledID", SqlDbType.Int).Value = num;
                    cmd.Parameters.Add("@PacijentID", SqlDbType.Int).Value = Form.idMain;
                    //MessageBox.Show("Pregled uspesno zakazan.");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    n = cmd.ExecuteNonQuery();
                    if (n > 0)
                    {
                        MessageBox.Show("Pregled uspesno zakazan.");
                        Update();
                    }
                    cmbOdaberi.Text = "Odaberite pregled";
                    groupBox.Visibility = Visibility.Hidden;
                    btnZakazi.Visibility = Visibility.Hidden;
                    txtLekar.Visibility = Visibility.Hidden;
                    txtOpis.Visibility = Visibility.Hidden;
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
