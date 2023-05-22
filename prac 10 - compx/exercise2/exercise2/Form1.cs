using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exercise2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> bookmarkList = new List<string>();

        //Filter for csv files and all files
        const string FILTER = "CSV files|*.csv|All Files|*.*";

        private void UpdateListbox()
        {
            listBoxBookmarks.Items.Clear();
            for (int i = 0; i < bookmarkList.Count; i++)
            {
                listBoxBookmarks.Items.Add(bookmarkList[i]);
            }
        }

        private void Initialise()
        {
            listBoxBookmarks.Items.Clear();
            bookmarkList.Clear();
            textBoxURL.Clear();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(textBoxURL.Text);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            textBoxStatus.Text = "Loading...";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            textBoxStatus.Clear();
            textBoxURL.Text = webBrowser1.Document.Url.ToString();
            this.Text = webBrowser1.DocumentTitle;
            
        }

        private void newBookmarkFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialise();
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBoxURL.Text.Length > 0)
            {
                if (textBoxURL.Text.Contains("https://"))
                {
                    textBoxURL.Text = textBoxURL.Text.Replace("https://", "");
                    bookmarkList.Add(textBoxURL.Text);
                    UpdateListbox();
                }
            }

        }

        private void listBoxBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxBookmarks.SelectedIndex;
            if (index >= 0 && index < bookmarkList.Count)
            {
                textBoxURL.Text = bookmarkList[index];
                buttonGo.PerformClick();
            }
            
        }

        private void saveBookmarkFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter writer; 
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = FILTER;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                writer = File.CreateText(saveFileDialog1.FileName);

                for (int i = 0; i < bookmarkList.Count; i++)
                {
                    writer.WriteLine(bookmarkList[i]);
                }
                writer.Close();
            }
            
        }

        private void loadBookmarkFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamReader reader;
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = FILTER;


            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    reader = File.OpenText(openFileDialog1.FileName);

                    Initialise();

                    while (!reader.EndOfStream)
                    {
                        bookmarkList.Add(reader.ReadLine());


                    }
                    UpdateListbox();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxBookmarks.SelectedIndex;
            listBoxBookmarks.Items.RemoveAt(index);
            bookmarkList.RemoveAt(index);
            
            
        }
    }
}
