using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filmovi
{
    public partial class Form1 : Form
    {
        private List<String> povrat;
        private NaiveBayes textClassification;
        private DataModel filmovi;
        private List<String> odgledani = new List<string>();
        public Form1()
        {
            InitializeComponent();
            textClassification = new NaiveBayes();
            filmovi = TextUtil.LoadData();
            //odgledani = DataModel.odgledani;
            povrat = textClassification.fit(filmovi);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var source = new AutoCompleteStringCollection();
            
            foreach(String str in povrat)
            {
                source.Add(str);
            }

            textBox2.Text = "20";

            textBox1.AutoCompleteCustomSource = source;
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

            linkLabel1.Text = "www.imdb.com";
            linkLabel1.Links.Add(0,linkLabel1.Text.Length , "www.imdb.com");

            linkLabel1.MaximumSize = new Size(180, 0);
           // linkLabel1.AutoSize = true;


        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
                return;
            if (!povrat.Contains(textBox1.Text))
            {
                return;
            }
            if (odgledani.Contains(textBox1.Text))
            {
                return;
            }

            odgledani.Add(textBox1.Text);
            textBox1.Text = "";
            listBox1.Items.Clear();
            int i = 0;
            foreach (String str in odgledani)
            {
                i++;
                listBox1.Items.Add(i + ". " + str);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        { 
            listBox2.Items.Clear();
            if (textBox2.Text.Equals(""))
                return;
            NaiveBayes.BROJ = Convert.ToInt32(textBox2.Text);
            DataModel.odgledani = odgledani;
            List<String> povrat = textClassification.predict();
            int i = 0;
            foreach (String str in povrat)
            {
                i++;
                listBox2.Items.Add(i + ". " + str);
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            string curItem = listBox1.SelectedItem.ToString();
            string naziv = curItem.Substring(curItem.IndexOf(' ')+1);
            linkLabel1.Text = naziv;
            linkLabel1.Links.Clear();
            linkLabel1.Links.Add(0, linkLabel1.Text.Length, "www.imdb.com/title/tt" + NaiveBayes.name_link[naziv] + "/");
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null)
                return;
            string curItem = listBox2.SelectedItem.ToString();
            string naziv = curItem.Substring(curItem.IndexOf(' ') + 1);
            linkLabel1.Text = naziv;
            linkLabel1.Links.Clear();
            linkLabel1.Links.Add(0, linkLabel1.Text.Length, "www.imdb.com/title/tt" + NaiveBayes.name_link[naziv] + "/");
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            if (e.KeyCode == Keys.Delete)
            {
                string curItem = listBox1.SelectedItem.ToString();
                odgledani.Remove(curItem.Substring(curItem.IndexOf(' ') + 1));

                listBox1.Items.Clear();
                int i = 0;
                foreach (String str in odgledani)
                {
                    i++;
                    listBox1.Items.Add(i + ". " + str);
                }
                linkLabel1.Links.Clear();
                linkLabel1.Text = "www.imdb.com";
                linkLabel1.Links.Add(0, linkLabel1.Text.Length, "www.imdb.com");
                e.Handled = true;
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                string curItem = listBox1.SelectedItem.ToString();
                odgledani.Remove(curItem.Substring(curItem.IndexOf(' ') + 1));

                listBox1.Items.Clear();
                int i = 0;
                foreach (String str in odgledani)
                {
                    i++;
                    listBox1.Items.Add(i + ". " + str);
                }
                linkLabel1.Links.Clear();
                linkLabel1.Text = "www.imdb.com";
                linkLabel1.Links.Add(0, linkLabel1.Text.Length, "www.imdb.com");

            }
        }
    }
}
