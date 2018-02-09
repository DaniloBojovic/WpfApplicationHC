using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for PageSlikaLekar.xaml
    /// </summary>
    public partial class PageSlikaLekar : Page
    {
        DataSet ds;
        MainWindow Form = Application.Current.Windows[0] as MainWindow;
        SqlConnection conn = new SqlConnection(@"Data Source=.;Initial Catalog=aspnet-HealthCare;Integrated Security=True");

        public PageSlikaLekar()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            imgLekar.Source = null;
            try
            {
                conn.Open();
                ds = new DataSet();
                //SqlCommand cmd = new SqlCommand("SELECT Slika from Pacijent where ID=@ID", conn);
                //cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Form.idMain;

                SqlDataAdapter sqa = new SqlDataAdapter("SELECT Slika from Lekar where ID=" + Form.idMain.ToString(), conn);
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
                    imgLekar.Source = bi;
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
    }
}
