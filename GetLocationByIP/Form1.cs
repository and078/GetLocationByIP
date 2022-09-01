using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GetLocationByIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        //comment

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckInternetConnection();
            splitContainer1.Panel2.Hide();
            webBrowser1.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckInternetConnection();
            string line = "";
            WebClient wc = new WebClient();
            line = wc.DownloadString($"http://ipwhois.app/json/{textBox1.Text}");
            Match match = Regex.Match(line, "\"country\":\"(.*?)\",(.*?)\"city\":\"(.*?)\",(.*?)\"latitude\":(.*?),\"longitude\":(.*?),(.*?)\"timezone_gmt\":\"(.*?)\"");
            label1.Text = match.Groups[1].Value + "\n" +
                match.Groups[3].Value + "\n" +
                match.Groups[8].Value + "\nLatitude: " +
                match.Groups[5].Value + "\nLongitude: " +
                match.Groups[6].Value;

            string lineToNavigate = "https://www.google.ru/maps/place/" + match.Groups[5].Value + "," + match.Groups[6].Value;
            splitContainer1.Panel2.Show();
            webBrowser1.Show();
            webBrowser1.Navigate(lineToNavigate);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IpEntryValidation();
        }

        private void IpEntryValidation()
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9-.]"))
            {
                DeleteWrongChar();
                MessageBox.Show("Only digits and dots!");
            }

            string[] nums = textBox1.Text.Split('.');


            if (nums.Length > 4)
            {
                DeleteWrongChar();
                MessageBox.Show("More than 4 ip numbers!");
            }


            int ipPart;

            foreach (var num in nums)
            {
                if (int.TryParse(num, out ipPart) && ipPart > 255)
                {
                    DeleteWrongChar();
                    MessageBox.Show("Number more than 255!");
                }
            }
        }

        private void DeleteWrongChar()
        {
            textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void CheckInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                string host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            }
            catch (Exception)
            {
                MessageBox.Show("Internet connection needed!");
                Form1_Load(this, EventArgs.Empty);
            }
        }
    }
}

