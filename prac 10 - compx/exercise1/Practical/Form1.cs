using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Practical
{
    public partial class Form1 : Form
    {

        //The width of a bar in the bar graph
        const int BAR_WIDTH = 25;

        //the gap between bars in the bar graph
        const int GAP = 5;

        //the factor to scale the graph by to make it fit nicely in the the picturebox
        const int SCALE_FACTOR = 15;

        //set filter
        const string FILTER = "Text Files|*.txt|All Files|*.*";

        //creating lists

        //List<string> csvList = new List<string>();
        List<string> dateList = new List<string>();
        List<int> tcalList = new List<int>();
        List<int> stepsList = new List<int>();
        List<double> distList = new List<double>();
        List<int> sedenList = new List<int>();
        List<int> lightList = new List<int>();
        List<int> fairList = new List<int>();
        List<int> veryList = new List<int>();
        List<int> acalList = new List<int>();
        List <double> steps_per_metreList = new List<double>();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Draws a vertical bar that is part of a bar graph.
        /// i.e. It fills a rectangle at position (x,y) with the specified colour.
        /// Then draws a black outline for the rectangle.
        /// Uses the BAR_WIDTH constant for the size of the rectangle.
        /// </summary>
        /// <param name="paper">The Graphics object to draw on.</param>
        /// <param name="x">The x position of the top left corner of the rectangle.</param>
        /// <param name="y">The y position of the top left corner of the rectangle.</param>
        /// <param name="colour">The colour to fill the background of the rectangle with.</param>
        private void DrawABar(Graphics paper, int x, int y, int length, Color colour)
        {
            //create a brush of specified colour and fill background with this colour 
            SolidBrush brush = new SolidBrush(colour);
            paper.FillRectangle(brush, x, y, BAR_WIDTH, length);

            //draw outline in black
            paper.DrawRectangle(Pens.Black, x, y, BAR_WIDTH, length);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBoxTop.Refresh();
            listBoxOutput.Refresh();
        }

        private double CalculateStepsPerMetre(int steps, double dist)
        {
            double distance_in_metres = dist * 1000;
            double steps_per_metre = steps / distance_in_metres;
            return steps_per_metre;
        }

        private void openFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Create OpenFileDialog to select a file
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Check if the file dialog result is OK (i.e., the user selected a file)
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader reader;

                // Initialize variables
                string line = "";
                string[] csvArray;
                string date;
                int tcal;
                int steps;
                double dist;
                int seden;
                int light;
                int fair;
                int very;
                int acal;
                double steps_per_metre;
                int steps_total = 0;
                int y = 0;
                int x = 0;

                // Open the selected file for reading
                reader = File.OpenText(openFileDialog1.FileName);

                // Add column headers to the list box
                listBoxOutput.Items.Add("Date".PadRight(15) + "Tcal".PadRight(7) + "Steps".PadRight(7) +
                    "Dist".PadRight(7) + "Seden".PadRight(7) + "Light".PadRight(7) + "Fair".PadRight(7) + "Very".PadRight(7) + "ACal".PadRight(7) + "Steps/metre");

                // Read the file line by line until the end of the stream is reached
                while (!reader.EndOfStream)
                {
                    try
                    {
                        // Read a line from the file
                        line = reader.ReadLine();

                        // Split the line into an array of values
                        csvArray = line.Split(',');

                        // Check if the line contains the expected number of values
                        if (csvArray.Length == 9)
                        {
                            // Extract values from the CSV array
                            date = csvArray[0];
                            tcal = int.Parse(csvArray[1]);
                            steps = int.Parse(csvArray[2]);
                            dist = double.Parse(csvArray[3]);
                            seden = int.Parse(csvArray[4]);
                            light = int.Parse(csvArray[5]);
                            fair = int.Parse(csvArray[6]);
                            very = int.Parse(csvArray[7]);
                            acal = int.Parse(csvArray[8]);

                            // Calculate steps per meter
                            steps_per_metre = CalculateStepsPerMetre(steps, dist);

                            // Add the formatted line to the list box
                            listBoxOutput.Items.Add(date.PadRight(15) + tcal.ToString().PadRight(7) + steps.ToString().PadRight(7) +
                                dist.ToString().PadRight(7) + seden.ToString().PadRight(7) + light.ToString().PadRight(7) + fair.ToString().PadRight(7)
                                + very.ToString().PadRight(7) + acal.ToString().PadRight(7) + steps_per_metre.ToString("F3"));

                            // Update the total number of steps
                            steps_total += steps;

                            // Create a Graphics object for drawing on the pictureBoxTop control
                            Graphics paper = pictureBoxTop.CreateGraphics();

                            // Calculate the length of the bar based on the distance
                            int bar_length = (int)(dist * SCALE_FACTOR);

                            // Draw a bar on the pictureBoxTop control
                            DrawABar(paper, x, pictureBoxTop.Height - bar_length, bar_length, Color.LightPink);

                            // Update the x-coordinate for the next bar
                            x += BAR_WIDTH + GAP;

                            // Storing information into the appropriate lists
                            dateList.Add(date);
                            tcalList.Add(tcal);
                            stepsList.Add(steps);
                            distList.Add(dist);
                            sedenList.Add(seden);
                            lightList.Add(light);
                            fairList.Add(fair);
                            veryList.Add(very);
                            acalList.Add(acal);

                            
                        }
                        else
                        {
                            // Display an error message if the line does not contain the expected number of values
                            MessageBox.Show("Error: " + line);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                MessageBox.Show("Number of steps taken this month: " + steps_total.ToString());
                
                reader.Close();
            }
            
        }

        private void graphDaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            int x = 0;
            

            //Create graphics and graphics objects to draw on pictureBoxBottom
            Graphics paper = pictureBoxBottom.CreateGraphics();
            
            Pen pen1 = new Pen(Color.Black);

            //Create array of colours for each day of the week
            string[] colorArray = new string[] { "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet" };


            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                for (int i = dayOfWeek; i < distList.Count; i+=7)
                {
                    int bar_length = (int)(distList[i] * SCALE_FACTOR);

                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[dayOfWeek]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }
            }

            /*for(int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);
                
                if (i % 7 ==0) // Monday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[0]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1 , x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }
                
            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==1) // Tuesday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[1]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==2) // Wednesday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[2]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==3) // Thursday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[3]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==4) // Friday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[4]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==5) // Saturday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[5]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }
            for (int i = 0; i < distList.Count; i++)
            {
                int bar_length = (int)(distList[i] * SCALE_FACTOR);

                if (i % 7 ==6) // Sunday
                {
                    SolidBrush br1 = new SolidBrush(Color.FromName(colorArray[6]));
                    paper.FillRectangle(br1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    paper.DrawRectangle(pen1, x, pictureBoxBottom.Height - bar_length, BAR_WIDTH, bar_length);
                    x+=BAR_WIDTH + GAP;
                }

            }*/
        }
    }
}
