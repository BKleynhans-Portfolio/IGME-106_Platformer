namespace newLevelEditor
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlLevel = new System.Windows.Forms.Panel();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sfdSaveLevel = new System.Windows.Forms.SaveFileDialog();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbnGrass = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbnPlayer = new System.Windows.Forms.RadioButton();
            this.rbnRemove = new System.Windows.Forms.RadioButton();
            this.rbnBirdHouse = new System.Windows.Forms.RadioButton();
            this.rbnSeed = new System.Windows.Forms.RadioButton();
            this.rbnWorm = new System.Windows.Forms.RadioButton();
            this.rbnEnemy = new System.Windows.Forms.RadioButton();
            this.rbnWood = new System.Windows.Forms.RadioButton();
            this.rbnStone = new System.Windows.Forms.RadioButton();
            this.rbnWater = new System.Windows.Forms.RadioButton();
            this.cbbGravityAppliedTo = new System.Windows.Forms.ComboBox();
            this.cbbObjectMovement = new System.Windows.Forms.ComboBox();
            this.cbbGravityOnProximityFrom = new System.Windows.Forms.ComboBox();
            this.lblGravityAppliedTo = new System.Windows.Forms.Label();
            this.lblGravityOnProximityFrom = new System.Windows.Forms.Label();
            this.lblObjectMovement = new System.Windows.Forms.Label();
            this.txtbObjectMoveDistance = new System.Windows.Forms.TextBox();
            this.lblObjectMoveDistance = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLevel
            // 
            this.pnlLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLevel.Location = new System.Drawing.Point(104, 27);
            this.pnlLevel.Name = "pnlLevel";
            this.pnlLevel.Size = new System.Drawing.Size(640, 360);
            this.pnlLevel.TabIndex = 0;
            this.pnlLevel.DragOver += new System.Windows.Forms.DragEventHandler(this.pnlLevel_DragOver);
            this.pnlLevel.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlLevel_Paint);
            this.pnlLevel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlLevel_MouseClick);
            this.pnlLevel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlLevel_MouseMove);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(858, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.newToolStripMenuItem.Text = "New";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "ofdOpenLevel";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(-30, -62);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // rbnGrass
            // 
            this.rbnGrass.AutoSize = true;
            this.rbnGrass.Location = new System.Drawing.Point(6, 19);
            this.rbnGrass.Name = "rbnGrass";
            this.rbnGrass.Size = new System.Drawing.Size(52, 17);
            this.rbnGrass.TabIndex = 4;
            this.rbnGrass.TabStop = true;
            this.rbnGrass.Text = "Grass";
            this.rbnGrass.UseVisualStyleBackColor = true;
            this.rbnGrass.CheckedChanged += new System.EventHandler(this.rbnGrass_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbnPlayer);
            this.groupBox1.Controls.Add(this.rbnRemove);
            this.groupBox1.Controls.Add(this.rbnBirdHouse);
            this.groupBox1.Controls.Add(this.rbnSeed);
            this.groupBox1.Controls.Add(this.rbnWorm);
            this.groupBox1.Controls.Add(this.rbnEnemy);
            this.groupBox1.Controls.Add(this.rbnWood);
            this.groupBox1.Controls.Add(this.rbnStone);
            this.groupBox1.Controls.Add(this.rbnWater);
            this.groupBox1.Controls.Add(this.rbnGrass);
            this.groupBox1.Location = new System.Drawing.Point(12, 413);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 137);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Placing:";
            // 
            // rbnPlayer
            // 
            this.rbnPlayer.AutoSize = true;
            this.rbnPlayer.Location = new System.Drawing.Point(65, 111);
            this.rbnPlayer.Name = "rbnPlayer";
            this.rbnPlayer.Size = new System.Drawing.Size(54, 17);
            this.rbnPlayer.TabIndex = 12;
            this.rbnPlayer.TabStop = true;
            this.rbnPlayer.Text = "Player";
            this.rbnPlayer.UseVisualStyleBackColor = true;
            this.rbnPlayer.CheckedChanged += new System.EventHandler(this.rbnPlayer_CheckedChanged);
            // 
            // rbnRemove
            // 
            this.rbnRemove.AutoSize = true;
            this.rbnRemove.Location = new System.Drawing.Point(65, 88);
            this.rbnRemove.Name = "rbnRemove";
            this.rbnRemove.Size = new System.Drawing.Size(85, 17);
            this.rbnRemove.TabIndex = 7;
            this.rbnRemove.TabStop = true;
            this.rbnRemove.Text = "Remove Tile";
            this.rbnRemove.UseVisualStyleBackColor = true;
            this.rbnRemove.CheckedChanged += new System.EventHandler(this.rbnRemove_CheckedChanged);
            // 
            // rbnBirdHouse
            // 
            this.rbnBirdHouse.AutoSize = true;
            this.rbnBirdHouse.Location = new System.Drawing.Point(65, 65);
            this.rbnBirdHouse.Name = "rbnBirdHouse";
            this.rbnBirdHouse.Size = new System.Drawing.Size(74, 17);
            this.rbnBirdHouse.TabIndex = 11;
            this.rbnBirdHouse.TabStop = true;
            this.rbnBirdHouse.Text = "BirdHouse";
            this.rbnBirdHouse.UseVisualStyleBackColor = true;
            this.rbnBirdHouse.CheckedChanged += new System.EventHandler(this.rbnBirdHouse_CheckedChanged);
            // 
            // rbnSeed
            // 
            this.rbnSeed.AutoSize = true;
            this.rbnSeed.Location = new System.Drawing.Point(65, 42);
            this.rbnSeed.Name = "rbnSeed";
            this.rbnSeed.Size = new System.Drawing.Size(50, 17);
            this.rbnSeed.TabIndex = 10;
            this.rbnSeed.TabStop = true;
            this.rbnSeed.Text = "Seed";
            this.rbnSeed.UseVisualStyleBackColor = true;
            this.rbnSeed.CheckedChanged += new System.EventHandler(this.rbnSeed_CheckedChanged);
            // 
            // rbnWorm
            // 
            this.rbnWorm.AutoSize = true;
            this.rbnWorm.Location = new System.Drawing.Point(65, 19);
            this.rbnWorm.Name = "rbnWorm";
            this.rbnWorm.Size = new System.Drawing.Size(53, 17);
            this.rbnWorm.TabIndex = 9;
            this.rbnWorm.TabStop = true;
            this.rbnWorm.Text = "Worm";
            this.rbnWorm.UseVisualStyleBackColor = true;
            this.rbnWorm.CheckedChanged += new System.EventHandler(this.rbnWorm_CheckedChanged);
            // 
            // rbnEnemy
            // 
            this.rbnEnemy.AutoSize = true;
            this.rbnEnemy.Location = new System.Drawing.Point(6, 111);
            this.rbnEnemy.Name = "rbnEnemy";
            this.rbnEnemy.Size = new System.Drawing.Size(57, 17);
            this.rbnEnemy.TabIndex = 8;
            this.rbnEnemy.TabStop = true;
            this.rbnEnemy.Text = "Enemy";
            this.rbnEnemy.UseVisualStyleBackColor = true;
            this.rbnEnemy.CheckedChanged += new System.EventHandler(this.rbnEnemy_CheckedChanged);
            // 
            // rbnWood
            // 
            this.rbnWood.AutoSize = true;
            this.rbnWood.Location = new System.Drawing.Point(6, 88);
            this.rbnWood.Name = "rbnWood";
            this.rbnWood.Size = new System.Drawing.Size(54, 17);
            this.rbnWood.TabIndex = 7;
            this.rbnWood.TabStop = true;
            this.rbnWood.Text = "Wood";
            this.rbnWood.UseVisualStyleBackColor = true;
            this.rbnWood.CheckedChanged += new System.EventHandler(this.rbnWood_CheckedChanged);
            // 
            // rbnStone
            // 
            this.rbnStone.AutoSize = true;
            this.rbnStone.Location = new System.Drawing.Point(6, 65);
            this.rbnStone.Name = "rbnStone";
            this.rbnStone.Size = new System.Drawing.Size(53, 17);
            this.rbnStone.TabIndex = 6;
            this.rbnStone.TabStop = true;
            this.rbnStone.Text = "Stone";
            this.rbnStone.UseVisualStyleBackColor = true;
            this.rbnStone.CheckedChanged += new System.EventHandler(this.rbnStone_CheckedChanged);
            // 
            // rbnWater
            // 
            this.rbnWater.AutoSize = true;
            this.rbnWater.Location = new System.Drawing.Point(6, 42);
            this.rbnWater.Name = "rbnWater";
            this.rbnWater.Size = new System.Drawing.Size(54, 17);
            this.rbnWater.TabIndex = 5;
            this.rbnWater.TabStop = true;
            this.rbnWater.Text = "Water";
            this.rbnWater.UseVisualStyleBackColor = true;
            this.rbnWater.CheckedChanged += new System.EventHandler(this.rbnWater_CheckedChanged);
            // 
            // cbbGravityAppliedTo
            // 
            this.cbbGravityAppliedTo.FormattingEnabled = true;
            this.cbbGravityAppliedTo.Location = new System.Drawing.Point(686, 428);
            this.cbbGravityAppliedTo.Name = "cbbGravityAppliedTo";
            this.cbbGravityAppliedTo.Size = new System.Drawing.Size(121, 21);
            this.cbbGravityAppliedTo.TabIndex = 7;
            this.cbbGravityAppliedTo.Visible = false;
            // 
            // cbbObjectMovement
            // 
            this.cbbObjectMovement.FormattingEnabled = true;
            this.cbbObjectMovement.Location = new System.Drawing.Point(419, 474);
            this.cbbObjectMovement.Name = "cbbObjectMovement";
            this.cbbObjectMovement.Size = new System.Drawing.Size(121, 21);
            this.cbbObjectMovement.TabIndex = 8;
            this.cbbObjectMovement.SelectedIndexChanged += new System.EventHandler(this.cbbObjectMovement_SelectedIndexChanged);
            // 
            // cbbGravityOnProximityFrom
            // 
            this.cbbGravityOnProximityFrom.FormattingEnabled = true;
            this.cbbGravityOnProximityFrom.Location = new System.Drawing.Point(419, 428);
            this.cbbGravityOnProximityFrom.Name = "cbbGravityOnProximityFrom";
            this.cbbGravityOnProximityFrom.Size = new System.Drawing.Size(121, 21);
            this.cbbGravityOnProximityFrom.TabIndex = 9;
            // 
            // lblGravityAppliedTo
            // 
            this.lblGravityAppliedTo.AutoSize = true;
            this.lblGravityAppliedTo.Location = new System.Drawing.Point(556, 431);
            this.lblGravityAppliedTo.Name = "lblGravityAppliedTo";
            this.lblGravityAppliedTo.Size = new System.Drawing.Size(107, 13);
            this.lblGravityAppliedTo.TabIndex = 10;
            this.lblGravityAppliedTo.Text = "Gravity applied to the";
            this.lblGravityAppliedTo.Visible = false;
            // 
            // lblGravityOnProximityFrom
            // 
            this.lblGravityOnProximityFrom.AutoSize = true;
            this.lblGravityOnProximityFrom.Location = new System.Drawing.Point(274, 432);
            this.lblGravityOnProximityFrom.Name = "lblGravityOnProximityFrom";
            this.lblGravityOnProximityFrom.Size = new System.Drawing.Size(139, 13);
            this.lblGravityOnProximityFrom.TabIndex = 11;
            this.lblGravityOnProximityFrom.Text = "Gravity on proximity from the";
            // 
            // lblObjectMovement
            // 
            this.lblObjectMovement.AutoSize = true;
            this.lblObjectMovement.Location = new System.Drawing.Point(287, 479);
            this.lblObjectMovement.Name = "lblObjectMovement";
            this.lblObjectMovement.Size = new System.Drawing.Size(126, 13);
            this.lblObjectMovement.TabIndex = 12;
            this.lblObjectMovement.Text = "Object movement pattern";
            // 
            // txtbObjectMoveDistance
            // 
            this.txtbObjectMoveDistance.Location = new System.Drawing.Point(686, 474);
            this.txtbObjectMoveDistance.Name = "txtbObjectMoveDistance";
            this.txtbObjectMoveDistance.Size = new System.Drawing.Size(121, 20);
            this.txtbObjectMoveDistance.TabIndex = 13;
            this.txtbObjectMoveDistance.Visible = false;
            // 
            // lblObjectMoveDistance
            // 
            this.lblObjectMoveDistance.AutoSize = true;
            this.lblObjectMoveDistance.Location = new System.Drawing.Point(556, 479);
            this.lblObjectMoveDistance.Name = "lblObjectMoveDistance";
            this.lblObjectMoveDistance.Size = new System.Drawing.Size(112, 13);
            this.lblObjectMoveDistance.TabIndex = 14;
            this.lblObjectMoveDistance.Text = "Blocks to move object";
            this.lblObjectMoveDistance.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 562);
            this.Controls.Add(this.lblObjectMoveDistance);
            this.Controls.Add(this.txtbObjectMoveDistance);
            this.Controls.Add(this.lblObjectMovement);
            this.Controls.Add(this.lblGravityOnProximityFrom);
            this.Controls.Add(this.lblGravityAppliedTo);
            this.Controls.Add(this.cbbGravityOnProximityFrom);
            this.Controls.Add(this.cbbObjectMovement);
            this.Controls.Add(this.cbbGravityAppliedTo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.pnlLevel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Level Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLevel;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog sfdSaveLevel;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton rbnGrass;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbnWood;
        private System.Windows.Forms.RadioButton rbnStone;
        private System.Windows.Forms.RadioButton rbnWater;
        private System.Windows.Forms.RadioButton rbnBirdHouse;
        private System.Windows.Forms.RadioButton rbnSeed;
        private System.Windows.Forms.RadioButton rbnWorm;
        private System.Windows.Forms.RadioButton rbnEnemy;
        private System.Windows.Forms.RadioButton rbnRemove;
        private System.Windows.Forms.ComboBox cbbGravityAppliedTo;
        private System.Windows.Forms.ComboBox cbbObjectMovement;
        private System.Windows.Forms.ComboBox cbbGravityOnProximityFrom;
        private System.Windows.Forms.Label lblGravityAppliedTo;
        private System.Windows.Forms.Label lblGravityOnProximityFrom;
        private System.Windows.Forms.Label lblObjectMovement;
        private System.Windows.Forms.TextBox txtbObjectMoveDistance;
        private System.Windows.Forms.Label lblObjectMoveDistance;
        private System.Windows.Forms.RadioButton rbnPlayer;
    }
}

