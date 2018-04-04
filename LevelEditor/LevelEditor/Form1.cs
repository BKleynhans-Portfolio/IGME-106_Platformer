using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/* Project: Platformer Level Editor
 * Programmer: Miranda Auriemma
 * Last Edited: 3/26/18
 */

namespace LevelEditor
{
    public partial class Form1 : Form
    {
        //list of lists of Labels, functionally like 2d arrays.
        //this is being done so that the Labels can be read from to save the map into a text file
        List<List<Label>> listofLblbRows;

        //enum for the tile thats being placed  
        enum TilePlacingState
        {
            wood,
            stone,
            water,
            grass,
            nothing
        }
        TilePlacingState tpState;

        //width and height of the level in tiles
        int levelWidth;
        int levelHeight;

        public Form1()
        {
            InitializeComponent();

            listofLblbRows = new List<List<Label>>();
            tpState = new TilePlacingState();
            levelWidth = 48;
            levelHeight = 9;

            //create Labeles for map and add to list in a loop
            /*(i am using x and y here instead of i and j because i will be using these attributes 
              for Label placement as well, and it makes it a little bit clearer for both math and list placement)
            */
            for (int y = 0; y < levelHeight; y++)
            {
                //add a new "row" of Labels to the list
                listofLblbRows.Add(new List<Label>());

                for (int x = 0; x < levelWidth; x++)
                {
                    Label lbl = new Label();
                    lbl.Size = new Size(25, 25);
                    lbl.Location = new Point(25 * x, 200 + 25 * y);
                    lbl.Name = "n"; //initialize to n, meaning nothing in the cell
                    lbl.BackColor = Color.Gray;
                    lbl.Padding = new Padding(3, 3, 3, 3);
                    //there has to be a "placeholder" image that is just plain white, that combined with the padding makes the grid visible
                    lbl.Image = LevelEditor.Properties.Resources.blank;

                    //need to tell the form that the Labeles EXIST so it can actually put them in
                    this.Controls.Add(lbl);

                    //this hooks up the Label click to the event handler
                    //pcb.MouseDown += LabelHandler;
                    lbl.MouseDown += LabelMouseDownHandler;
                    lbl.MouseEnter += LabelMouseDownHandler;

                    //add the Label to the "row" in the list
                    listofLblbRows[y].Add(lbl);
                }
            }

            void LabelMouseDownHandler(object sender, EventArgs e)//event handler for Label click 
            {

                Label b = (Label)sender;//cast sender to Label object
                //if(mouse) { }


                switch (tpState)
                {
                    case TilePlacingState.wood:
                        b.Image = LevelEditor.Properties.Resources.woodtexture;
                        b.Name = "w";
                        break;

                    case TilePlacingState.stone:
                        b.Image = LevelEditor.Properties.Resources.stonetexture;
                        b.Name = "s";

                        break;

                    case TilePlacingState.water:
                        b.Image = LevelEditor.Properties.Resources.watertexture;
                        b.Name = "a"; //wood already has w

                        break;

                    case TilePlacingState.grass:
                        b.Image = LevelEditor.Properties.Resources.grasstexture;
                        b.Name = "g";

                        break;

                    case TilePlacingState.nothing:
                        b.Image = LevelEditor.Properties.Resources.blank;
                        b.Name = "n";


                        break;
                }

            }

        }


        private void btnWood_Click(object sender, EventArgs e)
        {
            tpState = TilePlacingState.wood;
            lblPlacing.Text = "Placing: Wood";
        }

        private void btnStone_Click(object sender, EventArgs e)
        {
            tpState = TilePlacingState.stone;
            lblPlacing.Text = "Placing: Stone";
        }

        private void btnWater_Click(object sender, EventArgs e)
        {
            tpState = TilePlacingState.water;
            lblPlacing.Text = "Placing: Water";
        }

        private void btnGrass_Click(object sender, EventArgs e)
        {
            tpState = TilePlacingState.grass;
            lblPlacing.Text = "Placing: Grass";
        }

        private void btnNothing_Click(object sender, EventArgs e)
        {
            tpState = TilePlacingState.nothing;
            lblPlacing.Text = "Placing: Nothing";
        }
        

        private void tsiNew_Click(object sender, EventArgs e)
        {
            foreach (List<Label> rows in listofLblbRows)
            {
                foreach (Label pcb in rows)
                {
                    //resets each image to the "placeholder"
                    pcb.Image = LevelEditor.Properties.Resources.blank;
                }
            }
        }

        private void tsiSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfdSaveLevel = new SaveFileDialog();

            sfdSaveLevel.Filter = "txt files (*.txt)|*.txt";
            sfdSaveLevel.FilterIndex = 2;
            sfdSaveLevel.RestoreDirectory = true;
            string row;
            if (sfdSaveLevel.ShowDialog() == DialogResult.OK)
            {
                StreamWriter swSaveLevel = new StreamWriter(sfdSaveLevel.FileName);

                // Code to write the stream goes here.
                foreach (List<Label> rows in listofLblbRows)
                {
                    //reset row
                    row = "";
                    foreach (Label pcb in rows)
                    {
                        row += pcb.Name;
                    }

                    swSaveLevel.WriteLine(row);
                }

                swSaveLevel.Close();
            }
        }

        private void tsiLoad_Click(object sender, EventArgs e)
        {
           
            int loopCounter;

            ofdOpenLevel.InitialDirectory = "Z:\\gdaps2\\_teamProject\\LevelEditor\\LevelEditor\\bin\\Debug";
            ofdOpenLevel.Filter = "txt files (*.txt)|*.txt";
            ofdOpenLevel.FilterIndex = 2;
            ofdOpenLevel.RestoreDirectory = true;

            if (ofdOpenLevel.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader srReadFile = new StreamReader(ofdOpenLevel.FileName);
                                        
                    using (srReadFile)
                    {
                        //Insert code to read the stream here.
                        
                        foreach (List<Label> rows in listofLblbRows)
                        {
                            string row = srReadFile.ReadLine();

                            loopCounter = 0;
                            foreach (Label lbl in rows)
                            {
                                //resets each image to the "placeholder"
                                char tile = row[loopCounter];
                                lbl.Name = tile.ToString();
                                InterperetText(lbl, tile);
                                loopCounter++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }


        //convert the text into the level tiles in the pic boxes
        public void InterperetText(Label pcb, char tile)
        {
            switch (tile)
            {
                case 'w':
                    pcb.Image = LevelEditor.Properties.Resources.woodtexture;
                    break;

                case 's':
                    pcb.Image = LevelEditor.Properties.Resources.stonetexture;

                    break;
                case 'a':
                    pcb.Image = LevelEditor.Properties.Resources.watertexture;

                    break;
                case 'g':
                    pcb.Image = LevelEditor.Properties.Resources.grasstexture;

                    break;
                case 'n':
                    pcb.Image = LevelEditor.Properties.Resources.blank;

                    break;
                default:
                    break;

            }
        }

        

    }
}
