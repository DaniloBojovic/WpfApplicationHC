using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApplicationHC
{
    /// <summary>
    /// Interaction logic for PageKarton.xaml
    /// </summary>
    public partial class PageKarton : Page
    {
        DataSet ds;
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True");

        public PageKarton()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Implementirati sliku! :)
            imagebox.Source = null;
            try
            {
                conn.Open();
                ds = new DataSet();
                //SqlCommand cmd = new SqlCommand("SELECT Slika from Pacijent where ID=@ID", conn);
                //cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;

                SqlDataAdapter sqa = new SqlDataAdapter("SELECT Slika from Pacijent where ID=" + Form.idMain.ToString(), conn);
                sqa.Fill(ds);
                if (!ds.Tables[0].Rows[0].IsNull(0)) //Proveravam da li ima sliku bazu
                {
                    byte[] data = (byte[])ds.Tables[0].Rows[0][0];
                    MemoryStream strm = new MemoryStream();
                    strm.Write(data, 0, data.Length);
                    strm.Position = 0;
                    System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms;
                    bi.EndInit();
                    imagebox.Source = bi;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show("Pacijent nema sliku");
            }
            finally
            {
                conn.Close();
            }
        }
        //try
        //{
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("SELECT Slika from Pacijent where ID=@ID", conn);
        //    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    DataGridKarton.ItemsSource = dt.DefaultView;
        //    //SqlCommand cmd = new SqlCommand("Select PacijentID, KartonID, Izvestaj, Naziv from Karton k, Pregled p" +
        //    //    " where k.PregledID = p.PregledID and PacijentID = @ID", conn);
        //    //SqlCommand cmd = new SqlCommand("SELECT * from Karton where PacijentID=@ID", conn);
        //    //cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;
        //    //DataSet ds = new DataSet();
        //    ////
        //    //DataTable dt = new DataTable();
        //    ////
        //    //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    //da.Fill(ds);
        //    //DataGridKarton.HeadersVisibility = DataGridHeadersVisibility.All;
        //    //DataGridKarton.ItemsSource = ds.Tables[0].DefaultView;
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show(ex.Message.ToString());
        //}
        //finally
        //{
        //    conn.Close();
        //}
    }
}
