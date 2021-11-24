using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetLocationByIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitContainer1.Panel2.Hide();
            webBrowser1.Hide();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
            if (Regex.IsMatch(textBox1.Text, "[^0-9-.]"))
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
                textBox1.SelectionStart = textBox1.Text.Length;
                MessageBox.Show("Only digits and dots!");
            }
        }
    }
}

