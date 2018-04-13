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

            cbbMovementAppliedTo.DataSource = Enum.GetValues(typeof(MovementAppliedTo));
            cbbMovementAppliedTo.SelectedIndex = 0;

            cbbGravityOnProximityFrom.DataSource = Enum.GetValues(typeof(GravityOnProximityFrom));
            cbbGravityOnProximityFrom.SelectedIndex = 0;

            cbbObjectMovement.DataSource = Enum.GetValues(typeof(ObjectMovement));
            cbbObjectMovement.SelectedIndex = 0;
        }

        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {            
            graphics.FillRectangle(new SolidBrush(boxColor), pointX, pointY, pixelSize, pixelSize);            
        }

        private void pnlLevel_MouseClick(object sender, MouseEventArgs e)
        {
            string name = null;

            if (rbnRemove.Checked != true)
            {
                //tiles
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

                gameTiles.Add(
                    new GameTile(
                        gridPointX,
                        gridPointY,
                        name,
                        (MovementAppliedTo)cbbMovementAppliedTo.SelectedValue,
                        (GravityOnProximityFrom)cbbGravityOnProximityFrom.SelectedValue,
                        (ObjectMovement)cbbObjectMovement.SelectedValue
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

            //switch to check which radio button is on


        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            //redraws the grid every tick, to keep it in place


            DrawGrid();
            //point = new Point(MousePosition.X, MousePosition.Y);
        }

        //draws the grid for each block in the level
        private void DrawGrid()
        {
            //holds the point within the box in the panel
            Point relativePoint = pnlLevel.PointToClient(Cursor.Position);

            pointX = relativePoint.X - (Math.Abs(relativePoint.X % pixelSize));
            pointY = relativePoint.Y - (Math.Abs(relativePoint.Y % pixelSize));

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


        //everything in the "Placing" group box
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

        private void pnlLevel_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pnlLevel_DragOver(object sender, DragEventArgs e)
        {

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rbnRemove_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}
