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

namespace LevelEditor
{
    public partial class Form1 : Form
    {            
        //list of lists of buttons, functionally like 2d arrays.
        //this is being done so that the buttons can be read from to save the map into a text file
        List<List<Button>> listofBtnRows = new List<List<Button>>() ;

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

            listofBtnRows = new List<List<Button>>();
            tpState = new TilePlacingState();
            levelWidth = 48;
            levelHeight = 9;

            //create buttons for map and add to list in a loop
            //i am using x and y here instead of i and j because i will be using these attributes 
            //  for button placement as well, and it makes it a little bit clearer for both math and list placement
            for (int y = 0; y < levelHeight; y++)
            {
                //add a new "row" of buttons to the list
                listofBtnRows.Add(new List<Button>());





                for (int x = 0; x < levelWidth; x++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(25, 25);
                    btn.Location = new Point(25 * x, 100 + 25 * y);
                    btn.Name = "n"; //initialize to n, meaning nothing in the cell
                    //btn.Text = "smol button";

                    //need to tell the form that the buttons EXIST so it can actually put them in
                    this.Controls.Add(btn);

                    //this hooks up the button click to the event handler
                    btn.MouseClick += ButtonClickHandler;

                    //add the button to the "row" in the list
                    listofBtnRows[y].Add(btn);
                }
            }

            void ButtonClickHandler(object sender, EventArgs e)//event handler for button click 
            {
                Button b = (Button)sender;//cast sender to button object

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
                        b.Image = null;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            string row = "";
            StreamWriter level = new StreamWriter("level.txt");

            foreach (List<Button> rows in listofBtnRows)
            {
                row = "";
                foreach ( Button btn in rows)
                {
                    row += btn.Name;
                }
                level.WriteLine(row);
            }
            level.Close();
        }
    }
}
