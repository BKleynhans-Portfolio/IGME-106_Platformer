using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace newLevelEditor
{
    public partial class Form1 : Form
    {
        int pixelSize;
        Graphics graphics;
        int pointX;
        int pointY;
        //SolidBrush sb;
        Color boxColor;
       // Point point;
        public Form1()
        {
            InitializeComponent();

            pixelSize = 20;

            DrawGrid();

            rbnGrass.Select();

        }

        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {
            graphics.FillRectangle(new SolidBrush(boxColor), pointX, pointY, pixelSize, pixelSize);
        }

        private void pnlLevel_MouseClick(object sender, MouseEventArgs e)
        {
            pnlLevel_Paint(this, null);
            DrawGrid();
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
    }
}
