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
        SolidBrush sb;
       // Point point;
        public Form1()
        {
            InitializeComponent();

            pixelSize = 20;

            DrawGrid();

        }

        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {
            graphics.FillRectangle(new SolidBrush(Color.Red), pointX, pointY, pixelSize, pixelSize);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            pnlLevel_Paint(this, null);
            DrawGrid();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawGrid();
            //point = new Point(MousePosition.X, MousePosition.Y);
        }

        private void DrawGrid()
        {
            var relativePoint = panel1.PointToClient(Cursor.Position);
            graphics = panel1.CreateGraphics();
            pointX = relativePoint.X - (Math.Abs(relativePoint.X % pixelSize));
            pointY = relativePoint.Y - (Math.Abs(relativePoint.Y % pixelSize));




            for (int x = 0; x < pixelSize * (panel1.Height - 1 / pixelSize); x += pixelSize)
            {
                for (int y = 0; y < pixelSize * (panel1.Width - 1 / pixelSize); y += pixelSize)
                {
                    graphics.DrawRectangle(Pens.Purple, y, x, pixelSize, pixelSize);
                }
            }
        }


    }
}
