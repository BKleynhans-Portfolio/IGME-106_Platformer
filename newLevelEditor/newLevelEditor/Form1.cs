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

//Project: Level Editor
//Programmers: Miranda Auriemma & Benjamin Kleynhans
//Last Modified By: Miranda Auriemma (added functionality to add the player into the level)


namespace newLevelEditor
{
    public partial class Form1 : Form
    {
        int pixelSize;
        Graphics graphics;
        int pointX;
        int pointY;
        int gridPointX;
        int gridPointY;

        int lastPlacedX;
        int lastPlacedY;

        Color boxColor;

        //enum PlacingState
        //{
        //    grass,
        //    water,
        //    stone,
        //    wood,
        //    enemy,
        //    worm,
        //    seed,
        //    birdhouse
        //}
        //PlacingState pState;


       

        List<GameTile> gameTiles;


        public Form1()
        {
            InitializeComponent();

            pixelSize = 20;

            DrawGrid();

            rbnGrass.Select();

            gameTiles = new List<GameTile>();

            cbbGravityOnProximityFrom.DataSource = Enum.GetValues(typeof(GravityOnProximityFrom));
            cbbGravityOnProximityFrom.SelectedIndex = 0;

            cbbObjectMovement.DataSource = Enum.GetValues(typeof(ObjectMovement));
            cbbObjectMovement.SelectedIndex = 0;

            cbbGravityAppliedTo.DataSource = Enum.GetValues(typeof(GravityAppliedTo));
            cbbGravityAppliedTo.SelectedIndex = 0;
        }

        //draw the tile in the grid cell in the panel
        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {            
            graphics.FillRectangle(new SolidBrush(boxColor), pointX, pointY, pixelSize, pixelSize);            
        }

        private void pnlLevel_MouseClick(object sender, MouseEventArgs e)
        {
            string name = null;

            if (rbnRemove.Checked != true)
            {
                //check which tile type is selected
                if (rbnGrass.Checked)
                {
                    name = "Grass";
                }
                else if (rbnWater.Checked)
                {
                    name = "Water";
                }
                else if (rbnStone.Checked)
                {
                    name = "Stone";
                }
                else if (rbnWood.Checked)
                {
                    name = "Wood";
                }
                else if (rbnEnemy.Checked)
                {
                    name = "Enemy";
                }
                else if (rbnWorm.Checked)
                {
                    name = "Worm";
                }
                else if (rbnSeed.Checked)
                {
                    name = "Seed";
                }
                else if (rbnBirdHouse.Checked)
                {
                    name = "BirdHouse";
                }
                else if (rbnPlayer.Checked)
                {
                    name = "Player";
                    
                }
                
                gameTiles.Add(
                    new GameTile(
                        x: gridPointX,
                        y: gridPointY,
                        name: name,
                        gravityOnProximityFrom: (GravityOnProximityFrom)cbbGravityOnProximityFrom.SelectedValue,
                        objectMovement: (ObjectMovement)cbbObjectMovement.SelectedValue,
                        gravityDirection: (GravityAppliedTo)cbbGravityAppliedTo.SelectedValue,
                        objectMoveDistance: txtbObjectMoveDistance.Text
                    )
                );
            }
            else
            {
                foreach (GameTile gameTile in gameTiles)
                {
                    if (gameTile.X == gridPointX && gameTile.Y == gridPointY)
                    {
                        gameTiles.Remove(gameTile);
                    }
                }
            }

            pnlLevel_Paint(this, null);
            DrawGrid();
            lastPlacedX = gridPointX;
            lastPlacedY = gridPointY;

        }
        
        //redraws the grid every tick, to keep it in place
        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            DrawGrid();
        }

        //draws the grid for each block in the level
        private void DrawGrid()
        {
            //holds the point within the box in the panel
            Point relativePoint = pnlLevel.PointToClient(Cursor.Position);

            //pointX and pointY give the location for the panel paint method to draw the rectangle
            pointX = relativePoint.X - (Math.Abs(relativePoint.X % pixelSize));
            pointY = relativePoint.Y - (Math.Abs(relativePoint.Y % pixelSize));

            //gridPointX and gridPointY give the visual cell that the mouse is in
            gridPointX = pointX / pixelSize;
            gridPointY = pointY / pixelSize;
            
            graphics = pnlLevel.CreateGraphics();
            
            for (int x = 0; x < pixelSize * (pnlLevel.Height - 1 / pixelSize); x += pixelSize)
            {
                for (int y = 0; y < pixelSize * (pnlLevel.Width - 1 / pixelSize); y += pixelSize)
                {
                    graphics.DrawRectangle(Pens.Purple, y, x, pixelSize, pixelSize);
                }
            }
        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfdSaveLevel.Filter = "txt files (*.txt)|*.txt";
            sfdSaveLevel.FilterIndex = 2;
            sfdSaveLevel.RestoreDirectory = true;
            if (sfdSaveLevel.ShowDialog() == DialogResult.OK)
            {
                StreamWriter swSaveLevel = new StreamWriter(sfdSaveLevel.FileName);

                // Code to write the stream goes here.
                //for (int y = 0; y < 18; y++)
                //{
                //    //reset row
                //    for (int x = 0; x < 32; x++)
                //    {
                //        //swSaveLevel.WriteLine(level[y, x]);
                //    }

                //    //swSaveLevel.WriteLine();
                //}
                foreach (GameTile tiles in gameTiles)
                {
                    swSaveLevel.WriteLine(tiles.ToString());
                }

                swSaveLevel.Close();
            }
        }
        
        private void cbbObjectMovement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ObjectMovement)cbbObjectMovement.SelectedValue == ObjectMovement.OneDirection)
            {
                lblGravityAppliedTo.Location = new Point(548, 479);
                lblGravityAppliedTo.Visible = true;
                cbbGravityAppliedTo.Location = new Point(678, 474);
                cbbGravityAppliedTo.Visible = true;

                lblObjectMoveDistance.Visible = false;
                txtbObjectMoveDistance.Visible = false;
            }
            else if ((ObjectMovement)cbbObjectMovement.SelectedValue != ObjectMovement.None)
            {
                lblObjectMoveDistance.Location = new Point(548, 479);
                lblObjectMoveDistance.Visible = true;
                txtbObjectMoveDistance.Location = new Point(678, 474);
                txtbObjectMoveDistance.Visible = true;

                lblGravityAppliedTo.Visible = false;
                cbbGravityAppliedTo.Visible = false;
            }
            else
            {
                lblGravityAppliedTo.Visible = false;
                cbbGravityAppliedTo.Visible = false;
                lblObjectMoveDistance.Visible = false;
                txtbObjectMoveDistance.Visible = false;
            }
        }

        //radio buttons for the game tiles
        private void rbnGrass_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Green;

        }

        private void rbnWater_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Blue;

        }

        private void rbnStone_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Gray;

        }

        private void rbnWood_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Brown;

        }

        private void rbnEnemy_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Red;

        }

        private void rbnWorm_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Pink;
        }

        private void rbnSeed_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.LightGray;
        }

        private void rbnBirdHouse_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.DarkGreen;
        }
        private void rbnPlayer_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Yellow;
        }
        private void rbnRemove_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.White;
        }

        private void pnlLevel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pnlLevel_DragOver(object sender, DragEventArgs e)
        {

        }
    }
}
