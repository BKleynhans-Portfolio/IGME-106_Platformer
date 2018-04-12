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


        //2d array to hold the level info in text form
        //string[,] level= {
        // // y, x  0,  1,  2,  3,  4,  5,  6,  7,  8,  9,  10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31
        //   /*0*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*1*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*2*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*3*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*4*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*5*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*6*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*7*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*8*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //   /*9*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*10*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*11*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*12*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*13*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*14*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*15*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*16*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},
        //  /*17*/{ "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",},


        //    };

        List<GameTile> gameTiles;


        public Form1()
        {
            InitializeComponent();

            pixelSize = 20;

            DrawGrid();

            rbnGrass.Select();

            gameTiles = new List<GameTile>();
            //pState = new PlacingState();
        }

        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {
            
            graphics.FillRectangle(new SolidBrush(boxColor), pointX, pointY, pixelSize, pixelSize);

            
        }

        private void pnlLevel_MouseClick(object sender, MouseEventArgs e)
        {
            //tiles
            if (rbnGrass.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnWater.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnStone.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnWood.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnEnemy.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnWorm.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnSeed.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnBirdHouse.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));
            }
            else if (rbnRemove.Checked)
            {
                gameTiles.Add(new GameTile(gridPointX, gridPointY));

            }

            //properties
            else if (rbnNone.Checked)
            {
                
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
            rbnNone.Select();

        }

        private void rbnWater_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Blue;
            rbnNone.Select();

        }

        private void rbnStone_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Gray;
            rbnNone.Select();

        }

        private void rbnWood_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Brown;
            rbnNone.Select();

        }

        private void rbnEnemy_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Red;
            rbnNone.Select();

        }

        private void rbnWorm_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Pink;
            rbnNone.Select();
        }

        private void rbnSeed_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.LightGray;
            rbnNone.Select();
        }

        private void rbnBirdHouse_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.DarkGreen;
            rbnNone.Select();
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
    }
}
