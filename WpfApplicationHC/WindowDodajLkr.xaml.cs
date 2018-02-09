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
using System.Windows.Shapes;

namespace WpfApplicationHC
{
    /// <summary>
    /// Interaction logic for WindowDodajLkr.xaml
    /// </summary>
    public partial class WindowDodajLkr : Window
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        DataSet ds;
        DataTable dt;
        int idLekar, n;
        string strImeLkr, strPrzLkr;

        public WindowDodajLkr()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridLkr.CanUserAddRows = false;
            updateDataGrid();
        }

        private void updateDataGrid()
        {
            conn = new SqlConnection(constr);
            using (conn)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select 'dr ' + Prezime + ', ' + Ime as 'Lekar', Email, Naziv from Lekar l, LekarPregled lp, Pregled p " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID " +
                        "and p.PregledID = @ID", conn);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    ds = new DataSet();
                    dt = new DataTable();
                    da.Fill(ds);
                    DataGridLkr.HeadersVisibility = DataGridHeadersVisibility.All;
                    DataGridLkr.ItemsSource = ds.Tables[0].DefaultView;
                    if (DataGridLkr.Items.IsEmpty)
                    {
                        cmbOdaberi.Visibility = Visibility.Visible;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void cmbOdaberi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            btnDodaj.Visibility = Visibility.Visible;
            using (conn)
            {
                conn.Open();
                if (cmbOdaberi.SelectedIndex == 0)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if (idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (cmbOdaberi.SelectedIndex == 1)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if (idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (cmbOdaberi.SelectedIndex == 2)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if (idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (cmbOdaberi.SelectedIndex == 3)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if (idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (cmbOdaberi.SelectedIndex == 4)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if (idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (cmbOdaberi.SelectedIndex == 5)
                {
                    try
                    {
                        //Uzimam ID Lekara
                        strImeLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[3];
                        //strImeLkr = strImeLkr.Substring(0);
                        strPrzLkr = cmbOdaberi.SelectedValue.ToString().Split(' ')[2];
                        SqlCommand cmd = new SqlCommand("Select ID from Lekar where Ime=@Ime and Prezime=@Prezime");
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = strImeLkr;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = strPrzLkr;
                        cmd.Connection = conn; //Resilo problem inic. sa executescalar
                        idLekar = int.Parse(cmd.ExecuteScalar().ToString());
                        if(idLekar != 0)
                        {
                            updateDataGrid();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }                       
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert into LekarPregled (LekarID,PregledID,Participacija) "+
                    "values (@LekarID,@PregledID,450)", conn);
                cmd.Parameters.Add("@PregledID", SqlDbType.Int).Value = Form.idMain;
                cmd.Parameters.Add("@LekarID", SqlDbType.Int).Value = idLekar;
                
                cmbOdaberi.Text = "Odaberite pregled";
                btnDodaj.Visibility = Visibility.Hidden;
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                n = cmd.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show("Lekar uspesno dodat.");
                    updateDataGrid();
                }
            }
        }
    }
}
