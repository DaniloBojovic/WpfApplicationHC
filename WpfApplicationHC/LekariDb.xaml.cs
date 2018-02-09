using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for LekariDb.xaml
    /// </summary>
    public partial class LekariDb : Page
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        DataSet ds;
        DataTable dt;
        public static int Id;
        string strName, imageName;
        byte[] imgByte;
        int n; //ExecuteReader

        public LekariDb()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myDataGrid.CanUserAddRows = false;
            txt0.Visibility = Visibility.Hidden;
            txtID.Visibility = Visibility.Hidden;
            updateDataGrid();
            btnUpload.Visibility = Visibility.Hidden;
            btnPregledi.IsEnabled = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void myDataGried_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpload.Visibility = Visibility.Visible;
            btnPregledi.IsEnabled = true;
            DataGrid dg = sender as DataGrid; //kastujem ga kao datagrid object
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtID.Text = dr["Rbr"].ToString(); //Rbr stavljen umesto ID
                txtIme.Text = dr["Ime"].ToString();
                txtPrz.Text = dr["Prezime"].ToString();
                txtEmail.Text = dr["Email"].ToString();
                dtpZaposlenja.SelectedDate = DateTime.Parse(dr["DatumZaposlenja"].ToString());
                Form.idMain = Convert.ToInt32(dr["Rbr"]);//dodao // Rbr stavljen umesto ID
                frmKarton.Content = new PageSlikaLekar();
                btnAdd.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDel.IsEnabled = true;
            }
        }

        private void updateDataGrid()
        {
            conn = new SqlConnection(constr);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT ID Rbr,Ime,Prezime,Email,DatumZaposlenja FROM Lekar", conn);
                SqlCommand cmd1 = new SqlCommand("declare @max int; " +
                    "select @max = max(ID) from Lekar; " +
                    "dbcc checkident(Lekar, reseed, @max)", conn);
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter("Select Max(Cast(ID as int)) + 1 from Lekar", conn);
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIme.Text) && !string.IsNullOrWhiteSpace(txtPrz.Text) &&
                    !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string sql = "Insert into Lekar(Ime, Prezime, DatumZaposlenja, Email) " +
                "values(@Ime, @Prezime, @DatumZaposlenja, @Email)";
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
            if (!string.IsNullOrWhiteSpace(txtIme.Text) && !string.IsNullOrWhiteSpace(txtPrz.Text) &&
                    !string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da napravite izmene?", "Potvrda izmena", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    string sql = "Update Lekar set Ime=@Ime, Prezime=@Prezime, DatumZaposlenja=@DatumZaposlenja, Email=@Email WHERE ID=@ID";
                    this.AddUpdDel(sql, 2);
                    this.Reset();
                }
            }
            else
            {
                MessageBox.Show("Polja ne smeju biti prazna.");
                if (string.IsNullOrWhiteSpace(txtIme.Text))
                {
                    txtIme.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtPrz.Text))
                {
                    txtPrz.Focus();
                }
                else if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Focus();
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete lekara?", "Potvrda brisanja", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string sql = "Delete from Lekar where ID=@ID";
                this.AddUpdDel(sql, 3);
                this.Reset();
            }
        }

        private void Reset()
        {
            txtID.Text = Id.ToString();
            txtIme.Text = "";
            txtPrz.Text = "";
            dtpZaposlenja.SelectedDate = null;
            txtEmail.Text = "";
            frmKarton.Content = null;
            btnAdd.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDel.IsEnabled = false;
            btnPregledi.IsEnabled = false;
        }

        private void AddUpdDel(string sqlString, int state)
        {
            string msg = "";
            conn = new SqlConnection(constr);// Dodao zbog inic. konekcije
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlString, conn);

                switch (state)
                {
                    case 1:
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = txtIme.Text;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = txtPrz.Text;
                        cmd.Parameters.Add("@DatumZaposlenja", SqlDbType.DateTime).Value = dtpZaposlenja.SelectedDate;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtEmail.Text;
                        msg = "Lekar je uspesno dodat.";
                        break;
                    case 2:
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = txtIme.Text;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = txtPrz.Text;
                        cmd.Parameters.Add("@DatumZaposlenja", SqlDbType.DateTime).Value = dtpZaposlenja.SelectedDate;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = txtEmail.Text;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Informacije uspesno izmenjene.";
                        break;
                    case 3:
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Lekar uspesno obrisan.";
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

        private void btnPregledi_Click(object sender, RoutedEventArgs e)
        {
            WindowLkrPlg wlp = new WindowLkrPlg();
            wlp.Show();
        }

        private void lblCreateClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblCreateLekarClick(object sender, MouseButtonEventArgs e)
        {
            btnUpload.Visibility = Visibility.Hidden;
            txtID.Text = Id.ToString();
            Reset();
            btnPregledi.IsEnabled = false;
            frmKarton.Content = null;
        }

        private void txtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            conn = new SqlConnection(constr);
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName; //ime slike
                    imageName = fldlg.FileName; //path
                    ImageSourceConverter isc = new ImageSourceConverter();
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            if (!string.IsNullOrEmpty(imageName)) //Dodao ako korisnik ne odabere sliku
            {
                try
                {
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
                    byte[] imgByteArr = new byte[fs.Length];
                    imgByte = imgByteArr;
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    using (conn)
                    {
                        conn.Open();
                        string sql = "Update Lekar set Slika = @img where ID=@ID";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                            cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
                            int result = cmd.ExecuteNonQuery();
                            if (result == 1)
                            {
                                MessageBox.Show("Slika je promenjena.");
                                frmKarton.Content = new PageSlikaLekar();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
