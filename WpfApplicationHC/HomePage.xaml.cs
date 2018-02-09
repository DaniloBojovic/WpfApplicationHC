using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Xml;
using Microsoft.CognitiveServices.SpeechRecognition;
using System.Configuration;
using System.Reflection;

namespace WpfApplicationHC
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        AutoResetEvent are;
        MicrophoneRecognitionClient client;
        string firstWord;
        string myKey = ConfigurationManager.AppSettings["CitiesApiKey"];

        public HomePage()
        {
            InitializeComponent();
            btnPlay.Content = "Start recording";
            are = new AutoResetEvent(false);
            txtVoice.Background = Brushes.White;
            txtVoice.Foreground = Brushes.Black;
        }

        private void Convert()
        {
            var speechRec = SpeechRecognitionMode.ShortPhrase;
            string lan = "en-us";
            string subKey = ConfigurationManager.AppSettings["MicrosoftSpeechApiKey"].ToString();
            client = SpeechRecognitionServiceFactory.CreateMicrophoneClient(speechRec, lan, subKey); //3 parametra
            client.OnPartialResponseReceived += ResponseReceived;
            client.StartMicAndRecognition();
        }

        private void ResponseReceived(object sender, PartialSpeechResponseEventArgs e)
        {
            string res = e.PartialResult;
            Dispatcher.Invoke(() =>
            {
                txtVoice.Text = e.PartialResult;
            });
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            btnPlay.Content = "LISTENING . . .";
            btnPlay.IsEnabled = false;
            txtVoice.Background = Brushes.White;
            txtVoice.Foreground = Brushes.Blue;
            Convert();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                are.Set();
                client.EndMicAndRecognition();
                client.Dispose();
                client = null;
                btnPlay.Content = "Start recording";
                btnPlay.IsEnabled = true;
                txtVoice.Background = Brushes.White;
                txtVoice.Foreground = Brushes.Black;
            });
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetDictionary();
        }

        private void GetDictionary()
        {
            txtDictionary.Clear();
            firstWord = txtVoice.Text.Split(' ')[0];
            string query = String.Format("https://www.dictionaryapi.com/api/references/medical/v2/xml/" + firstWord + 
                "?key=9e0b1560-e91c-466a-8824-cc6f9136f1a7");
            XmlDocument doc = new XmlDocument();
            doc.Load(query);
            //XmlNode node = doc.SelectSingleNode("sens").SelectSingleNode("dt");
            //string s = node.SelectSingleNode("dt").InnerText;
            //txtRez.Text = s;
            int i = 0;
            foreach (XmlNode xn in doc.SelectNodes("//sens"))
            {
                if(i < 3)
                {
                    string s1 = xn["dt"].InnerText;
                    txtDictionary.Text += i+1 + ". " + s1;
                    txtDictionary.Text += "\n";
                }
                else
                {
                    break;
                }
                i++;
            }
            
            //XmlNode node = doc.SelectSingleNode("entry_list").SelectSingleNode("entry");
            //string definition = node.SelectSingleNode("def").SelectSingleNode("sensb").SelectSingleNode("sens").SelectSingleNode("dt").InnerText;
            //richTextBox.Document.Blocks.Clear();
            //richTextBox.AppendText(definition);
        }

        private void btnPodaci_Click(object sender, RoutedEventArgs e)
        {
            txtRez.Clear();
            string grad = txtGrad.Text;
            XmlDocument document = new XmlDocument();
            //string s = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=health+in+Beograd&key=" + myKey;
            //string s = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + "DomZdravlja+in+" + "NoviSad&key=" + myKey;
            string s = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=" + "DomZdravlja+in+" + txtGrad.Text + "&key=" + myKey;
            document.Load(s);
            foreach (XmlNode xn in document.SelectNodes("//result"))
            {
                string s1 = xn["name"].InnerText;
                string s2 = xn["formatted_address"].InnerText;
                txtRez.Text += s1;
                txtRez.Text += " - ";
                txtRez.Text += s2;
                txtRez.Text += "\n";
            }
            if (string.IsNullOrEmpty(txtRez.Text))
            {
                txtRez.Text = "Nema podataka za uneseni grad.";
            }
            txtGrad.Clear();
        }
    }
}
