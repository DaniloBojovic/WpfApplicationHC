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
    /// Interaction logic for PacijentiDb.xaml
    /// </summary>
    public partial class PacijentiDb : Page
    {
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        public static int Id;
        int n; //ExecuteReader
        string strName, imageName;
        byte[] imgByte;
        string constr = @"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True;MultipleActiveResultSets=true";
        SqlConnection conn;
        DataSet ds;
        DataTable dt;

        public PacijentiDb()
        {
            InitializeComponent();
            frmKarton.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        private void updateDataGrid()
        {
            conn = new SqlConnection(constr);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT ID Rbr,Ime,Prezime,Firma,DatumRegistracije FROM Pacijent", conn);
                SqlCommand cmd1 = new SqlCommand("declare @max int; " +
                    "select @max = max(ID) from Pacijent; " +
                    "dbcc checkident(Pacijent, reseed, @max)", conn); //Dodao zbog auto inc. kljuca 
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter("Select Max(Cast(ID as int)) + 1 from Pacijent", conn);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            myDataGrid.CanUserAddRows = false;
            txtID.Visibility = Visibility.Hidden;
            label4.Visibility = Visibility.Hidden;
            updateDataGrid();
            btnUpload.Visibility = Visibility.Hidden;
            btnPregledi.IsEnabled = false;
        }
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Column2")
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void myDataGried_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpload.Visibility = Visibility.Visible;
            btnPregledi.IsEnabled = true;
            DataGrid dg = sender as DataGrid; //kastujem ga kao datagrid object
            DataRowView dr = dg.SelectedItem as DataRowView;
            if (dr != null)
            {
                txtID.Text = dr["Rbr"].ToString(); // Rbr stavljen umesto ID
                txtIme.Text = dr["Ime"].ToString();
                txtPrz.Text = dr["Prezime"].ToString();
                txtFirma.Text = dr["Firma"].ToString();
                dtpRegistracija.SelectedDate = DateTime.Parse(dr["DatumRegistracije"].ToString());
                Form.idMain = Convert.ToInt32(dr["Rbr"]);//dodao // Rbr stavljen umesto ID
                frmKarton.Content = new PageKarton();
                btnAdd.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDel.IsEnabled = true;
            }
        }

        private void AddUpdDel(string sqlString, int state)
        {
            string msg = "";
            //conn = null;//
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
                        cmd.Parameters.Add("@Firma", SqlDbType.VarChar).Value = txtFirma.Text;
                        cmd.Parameters.Add("@DatumRegistracije", SqlDbType.DateTime).Value = dtpRegistracija.SelectedDate;
                        msg = "Pacijent je uspesno dodat.";
                        break;
                    case 2:
                        cmd.Parameters.Add("@Ime", SqlDbType.VarChar).Value = txtIme.Text;
                        cmd.Parameters.Add("@Prezime", SqlDbType.VarChar).Value = txtPrz.Text;
                        cmd.Parameters.Add("@Firma", SqlDbType.VarChar).Value = txtFirma.Text;
                        cmd.Parameters.Add("@DatumRegistracije", SqlDbType.DateTime).Value = dtpRegistracija.SelectedDate;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Informacije uspesno izmenjene.";
                        break;
                    case 3:
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                        msg = "Pacijent uspesno obrisan.";
                        break;
                }
                try
                {
                    n = cmd.ExecuteNonQuery(); //int n prebacen gore 
                    if (n > 0)
                    {
                        MessageBox.Show(msg);
                        updateDataGrid();
                    }
                    else
                    {
                        MessageBox.Show("Unesite ispravne podatke.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unesite ispravne podatke.");
                }
            }
        }

        private void AddBtnClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(txtIme.Text) && !string.IsNullOrWhiteSpace(txtPrz.Text) &&
                    !string.IsNullOrWhiteSpace(txtFirma.Text))
            {
                string sql = "Insert into Pacijent(Ime, Prezime, Firma, DatumRegistracije) " +
                "values(@Ime, @Prezime, @Firma, @DatumRegistracije)";
                AddUpdDel(sql, 1); //2 param. sql naredba i broj za operaciju (add-1)
            }
            else
            {
                MessageBox.Show("Unesite ispravne podatke.");
            }
            if(n != 0)
            {
                btnAdd.IsEnabled = false;
            }
        }

        private void EditBtnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtIme.Text) && !string.IsNullOrWhiteSpace(txtPrz.Text) &&
                    !string.IsNullOrWhiteSpace(txtFirma.Text))
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da na pravite izmene?", 
                    "Potvrda izmena", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    string sql = "UPDATE PACIJENT SET IME=@IME, PREZIME=@PREZIME, FIRMA=@FIRMA, DATUMREGISTRACIJE=@DATUMREGISTRACIJE" + 
                        " WHERE ID=@ID";
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
                else if (string.IsNullOrWhiteSpace(txtFirma.Text))
                {
                    txtFirma.Focus();
                }
            }
        }

        private void DelBtnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li ste sigurni da zelite da obrisete pacijenta?", "Potvrda brisanja", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string sql = "DELETE FROM PACIJENT WHERE ID=@ID";
                this.AddUpdDel(sql, 3);
                this.Reset();
            }
        }
        private void Reset()
        {
            txtID.Text = Id.ToString();
            txtIme.Text = "";
            txtPrz.Text = "";
            txtFirma.Text = "";
            dtpRegistracija.SelectedDate = null;
            frmKarton.Content = null;
            btnAdd.IsEnabled = true;
            btnEdit.IsEnabled = false;
            btnDel.IsEnabled = false;
            btnPregledi.IsEnabled = false;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            conn.Close();
        }

        private void btnPreglediClick(object sender, RoutedEventArgs e)
        {
            //frmKarton.Content = new PageKarton();
            WindowKarton wk = new WindowKarton();
            wk.Show();
        }

        private void lblCreatePacClick(object sender, MouseButtonEventArgs e)
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

        private void btnUploadClick(object sender, RoutedEventArgs e)
        {
            //conn = null;
            conn = new SqlConnection(constr);
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    //image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
            if(!string.IsNullOrEmpty(imageName)) //Dodao ako korisnik ne odabere sliku
            {
                try
                {
                    //if (imageName != "")
                    //{
                    //Initialize a file stream to read the image file
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];
                    imgByte = imgByteArr;
                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    //Close a file stream
                    fs.Close();

                    using (conn)
                    {
                        conn.Open();
                        if (int.Parse(txtID.Text) == Id)
                        {
                            //MessageBox.Show("NOVI NOVI NOVI!!!");
                        }
                        else
                        {
                            //MessageBox.Show("STARI!!!");
                            string sql = "Update Pacijent set Slika = @img where ID=@ID";
                            using (SqlCommand cmd = new SqlCommand(sql, conn))
                            {
                                //Pass byte array into database
                                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = txtID.Text;
                                cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
                                int result = cmd.ExecuteNonQuery();
                                if (result == 1)
                                {
                                    //MessageBox.Show("Slika je dodata.");
                                    frmKarton.Content = new PageKarton();
                                }
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
