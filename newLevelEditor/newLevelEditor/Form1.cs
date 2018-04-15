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
              
        List<GameTile> gameTiles;

        private bool PlayerCreated { get; set; }
        private bool BirdHouseCreated { get; set; }


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

        private void pnlLevel_Paint(object sender, PaintEventArgs e)
        {            
            graphics.FillRectangle(new SolidBrush(boxColor), pointX, pointY, pixelSize, pixelSize);
        }

        private void pnlLevel_MouseClick(object sender, MouseEventArgs e)
        {
            string name = null;
            string operation = null;

            if (rbnRemove.Checked != true)
            {
                operation = "Add";

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
                else if (rbnPlayer.Checked)
                {
                    name = "Player";
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

                gameTiles.Add(                                                              // Add a tile to the list
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
                operation = "Remove";

                GameTile tileToRemove = null;                                               // You cannot modify a list while accessing it
                                                                                            // the temp object is required
                foreach (GameTile gameTile in gameTiles)
                {
                    if (gameTile.X == (gridPointX * 50) && gameTile.Y == (gridPointY * 50))
                    {
                        tileToRemove = gameTile;
                    }
                }

                if (tileToRemove != null)
                {
                    name = tileToRemove.Name;
                    gameTiles.Remove(tileToRemove);                                             // Remove the tile from the list
                }

                boxColor = SystemColors.Control;                                            // Reset grid point to system color
            }

            pnlLevel_Paint(this, null);
            DrawGrid();
            lastPlacedX = gridPointX;
            lastPlacedY = gridPointY;

            if (name != null)
            {
                if (name.Equals("Player") || name.Equals("BirdHouse"))
                {
                    UpdateControls(operation, name);
                }
            }

            //switch to check which radio button is on

        }

        /// <summary>
        /// Set the properties of the radio button controls of both player and bird feeder
        /// </summary>
        /// <param name="operation">Add or Remove an Object</param>
        /// <param name="objectName">Object to be removed</param>
        private void UpdateControls(string operation, string objectName)
        {
            switch (operation)
            {
                case "Add":                                                                 // Set these properties if we're adding an object
                    if (objectName.Equals("Player"))
                    {
                        PlayerCreated = true;
                        rbnPlayer.Enabled = false;
                        rbnEnemy.Checked = true;
                    }
                    else if (objectName.Equals("BirdHouse"))
                    {
                        BirdHouseCreated = true;
                        rbnBirdHouse.Enabled = false;
                        rbnEnemy.Checked = true;
                    }
                    break;
                case "Remove":                                                              // Set these properties if we're removing an object
                    if (objectName.Equals("Player"))
                    {
                        PlayerCreated = false;
                        rbnPlayer.Enabled = true;
                    }
                    else if (objectName.Equals("BirdHouse"))
                    {
                        BirdHouseCreated = false;
                        rbnBirdHouse.Enabled = true;
                    }

                    break;
            }
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

        private void rbnPlayer_CheckedChanged(object sender, EventArgs e)
        {
            boxColor = Color.Black;
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

        /// <summary>
        /// Saves the grid information to a text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PlayerCreated || !BirdHouseCreated)                                        // Check that both player and bird house are created
            {
                string errorObject = null;

                if (!PlayerCreated)                                                         // If not, determine which one is missing
                {
                    errorObject = "Player";
                }
                else if (!BirdHouseCreated)
                {
                    errorObject = "BirdHouse";
                }

                MessageBox.Show(                                                            // Error message to display which item is missing
                    "The level cannot be saved, you have not yet created a " + errorObject + " object",
                    "Save error " + errorObject + " missing",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            else
            {                                                                               // If no items are missing, save the file
                sfdSaveLevel.InitialDirectory = "Levels";                                   // Define directory for save dialog box to open in
                int lastSavedLevel = GetLastLevel();                                        // Determine what the last saved level was
                sfdSaveLevel.FileName = ("Level" + lastSavedLevel + ".txt");                // Suggest the next file name to use
                sfdSaveLevel.Filter = "txt files (*.txt)|*.txt";
                sfdSaveLevel.FilterIndex = 2;
                sfdSaveLevel.RestoreDirectory = true;

                if (sfdSaveLevel.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        StreamWriter swSaveLevel = new StreamWriter(sfdSaveLevel.FileName);

                        foreach (GameTile tiles in gameTiles)                               // Write each tile to the txt file
                        {
                            swSaveLevel.WriteLine(tiles.ToString());
                        }

                        swSaveLevel.Close();
                    }
                    catch (Exception fileWriteException)
                    {
                        MessageBox.Show(                                                    // Throw exception if error at save
                            fileWriteException.Message + "\n\n" +
                            "The file cannot be saved",
                            "Error in Form1.cs : lines 242 - 251",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Determines what level was last to be saved.  This is only accurate if launched from the game
        /// and not when launched from newLevelEditor because it references the relative path
        /// </summary>
        /// <returns>Integer of the next level that should be saved</returns>
        private int GetLastLevel()
        {
            int returnValue = 0;
            int startLevel = 1;
            bool foundLastLevel = false;
            string fullFilePath = Path.GetFullPath(sfdSaveLevel.InitialDirectory);          // Get full path

            do
            {
                if (File.Exists(fullFilePath + "\\Level" + startLevel + ".txt"))            // Checks if the file exists
                {
                    startLevel++;                                                           // Determine number of next level to save
                }
                else
                {
                    returnValue = startLevel;
                    foundLastLevel = true;
                }

            } while (!foundLastLevel);

            return returnValue;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rbnRemove_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Set controls active or inactive depending on status of other controls on the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbObjectMovement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ObjectMovement)cbbObjectMovement.SelectedValue == ObjectMovement.OneDirection)
            {
                lblGravityAppliedTo.Location = new Point(573, 476);
                lblGravityAppliedTo.Visible = true;
                cbbGravityAppliedTo.Location = new Point(691, 472);
                cbbGravityAppliedTo.Visible = true;

                lblObjectMoveDistance.Visible = false;
                txtbObjectMoveDistance.Visible = false;
            }
            else if ((ObjectMovement)cbbObjectMovement.SelectedValue != ObjectMovement.None)
            {
                lblObjectMoveDistance.Location = new Point(573, 476);
                lblObjectMoveDistance.Visible = true;
                txtbObjectMoveDistance.Location = new Point(691, 472);
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

            cbbGravityAppliedTo.Text = string.Empty;
            txtbObjectMoveDistance.Text = "0";
        }
    }
}
