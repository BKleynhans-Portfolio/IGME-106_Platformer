﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor
{
    public partial class Form1 : Form
    {            
        //list of lists of PictureBoxs, functionally like 2d arrays.
        //this is being done so that the PictureBoxs can be read from to save the map into a text file
        List<List<PictureBox>> listofBtnRows = new List<List<PictureBox>>() ;

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

            listofBtnRows = new List<List<PictureBox>>();
            tpState = new TilePlacingState();
            levelWidth = 48;
            levelHeight = 9;

            //create PictureBoxs for map and add to list in a loop
            //i am using x and y here instead of i and j because i will be using these attributes 
            //  for PictureBox placement as well, and it makes it a little bit clearer for both math and list placement
            for (int y = 0; y < levelHeight; y++)
            {
                //add a new "row" of PictureBoxs to the list
                listofBtnRows.Add(new List<PictureBox>());
                
                for (int x = 0; x < levelWidth; x++)
                {
                    PictureBox pcb = new PictureBox();
                    pcb.Size = new Size(25, 25);
                    pcb.Location = new Point(25 * x, 200 + 25 * y);
                    pcb.Name = "n"; //initialize to n, meaning nothing in the cell
                    pcb.BackColor = Color.Gray;
                    pcb.Image = LevelEditor.Properties.Resources.blank;
                    pcb.Padding = new Padding(3, 3, 3, 3);
                    //pcb.Padding = 1;

                    //btn.Text = "smol PictureBox";

                    //need to tell the form that the PictureBoxs EXIST so it can actually put them in
                    this.Controls.Add(pcb);

                    //this hooks up the PictureBox click to the event handler
                    //pcb.MouseDown += PictureBoxHandler;
                    pcb.MouseDown += PictureBoxMouseDownHandler;
                    pcb.MouseEnter += PictureBoxMouseDownHandler;

                    //add the PictureBox to the "row" in the list
                    listofBtnRows[y].Add(pcb);
                }
            }

            void PictureBoxMouseDownHandler(object sender, EventArgs e)//event handler for PictureBox click 
            {
                
                PictureBox b = (PictureBox)sender;//cast sender to PictureBox object
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

        }

        private void tsiSave_Click(object sender, EventArgs e)
        {
            SaveWindow saveWindow = new SaveWindow(this);
            saveWindow.ShowDialog();
        }

        private void tsiLoad_Click(object sender, EventArgs e)
        {
            
        }

        public void Save(string fileName)
        {
            string row;
            StreamWriter level = new StreamWriter("level.txt");

            foreach (List<PictureBox> rows in listofBtnRows)
            {
                row = "";
                foreach (PictureBox btn in rows)
                {
                    row += btn.Name;
                }
                level.WriteLine(row);
            }
            level.Close();
        }
    }
}
