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
    /// Interaction logic for WindowLkrPlg.xaml
    /// </summary>
    public partial class WindowLkrPlg : Window
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        DataSet ds;
        DataTable dt;
        int n;
        string strIzvestaj;

        public WindowLkrPlg()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
                
        private void Update()
        { 
            DataGridIzvrsenPgld.CanUserAddRows = false;
            DataGridZakazanPgld.CanUserAddRows = false;
            grpInfo.Visibility = Visibility.Hidden;
            conn = new SqlConnection(constr);
            using (conn)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select p.Naziv, k.PacijentID, pa.Prezime + ', ' + pa.Ime as 'Pacijent', "+
                        "lp.Participacija as 'Participacija u RSD', k.Izvestaj " +
                        "from Lekar l, LekarPregled lp, Pregled p, Karton k, Pacijent pa "+
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID and p.PregledID = k.PregledID and k.PacijentID = pa.ID "+
                        "and k.Izvestaj is NOT NULL and l.ID = @ID", conn);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
                    ds = new DataSet();
                    dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataGridIzvrsenPgld.HeadersVisibility = DataGridHeadersVisibility.All;
                    DataGridIzvrsenPgld.ItemsSource = ds.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            using (conn)
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select p.Naziv, k.PacijentID, pa.Prezime + ', ' + pa.Ime as 'Pacijent', " +
                        "lp.Participacija as 'Participacija u RSD', k.Izvestaj, k.KartonID " +
                        "from Lekar l, LekarPregled lp, Pregled p, Karton k, Pacijent pa " +
                        "where l.ID = lp.LekarID and lp.PregledID = p.PregledID and p.PregledID = k.PregledID and k.PacijentID = pa.ID " +
                        "and k.Izvestaj is NULL and l.ID = @ID", conn);
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
                    ds = new DataSet();
                    dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataGridZakazanPgld.HeadersVisibility = DataGridHeadersVisibility.All;
                    DataGridZakazanPgld.ItemsSource = ds.Tables[0].DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void DataGridZakazanPgld_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid; 
            DataRowView dr = dg.SelectedItem as DataRowView;
            grpInfo.Visibility = Visibility.Visible;
            txtKartonId.IsReadOnly = true;
            if(dr != null)
            {
                txtKartonId.Text = dr["KartonId"].ToString();
            }
        }

        private void btnIzvrsi_Click(object sender, RoutedEventArgs e)
        {
            if (cmbIzvestaj.SelectedIndex > -1)
            {
                //conn = null;
                conn = new SqlConnection(constr);
                using (conn)
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("Update Karton set Izvestaj = @Izvestaj where KartonID = @TxtBoxKartonID", conn);
                        cmd.Parameters.Add("@TxtBoxKartonID", SqlDbType.Int).Value = Convert.ToInt32(txtKartonId.Text);
                        //cmd.Parameters.Add("@Izvestaj", SqlDbType.Int).Value = Convert.ToInt32(txtIzvestaj.Text);
                        cmd.Parameters.Add("@Izvestaj", SqlDbType.Int).Value = Convert.ToInt32(strIzvestaj);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        n = cmd.ExecuteNonQuery();
                        if(n > 0)
                        {
                            MessageBox.Show("Izvestaj uspesno kreiran.");
                            Update();
                        }
                        grpInfo.Visibility = Visibility.Hidden;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Unesite izvestaj!");
            }
        }

        private void cmbIzvestaj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbIzvestaj.SelectedIndex == 0)
            {
                strIzvestaj = cmbIzvestaj.SelectedValue.ToString().Split('-')[1];
                strIzvestaj = strIzvestaj.Substring(1);
            }
            else if (cmbIzvestaj.SelectedIndex == 1)
            {
                strIzvestaj = cmbIzvestaj.SelectedValue.ToString().Split('-')[1];
                strIzvestaj = strIzvestaj.Substring(1);
            }
        }
    }
}
