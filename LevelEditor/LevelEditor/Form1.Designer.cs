namespace LevelEditor
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
            this.btnWood = new System.Windows.Forms.Button();
            this.btnStone = new System.Windows.Forms.Button();
            this.btnWater = new System.Windows.Forms.Button();
            this.btnGrass = new System.Windows.Forms.Button();
            this.lblPlacing = new System.Windows.Forms.Label();
            this.btnNothing = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnWood
            // 
            this.btnWood.ForeColor = System.Drawing.SystemColors.Control;
            this.btnWood.Image = global::LevelEditor.Properties.Resources.woodtexture;
            this.btnWood.Location = new System.Drawing.Point(12, 75);
            this.btnWood.Name = "btnWood";
            this.btnWood.Size = new System.Drawing.Size(86, 64);
            this.btnWood.TabIndex = 0;
            this.btnWood.Text = "Wood";
            this.btnWood.UseVisualStyleBackColor = true;
            this.btnWood.Click += new System.EventHandler(this.btnWood_Click);
            // 
            // btnStone
            // 
            this.btnStone.ForeColor = System.Drawing.SystemColors.Control;
            this.btnStone.Image = global::LevelEditor.Properties.Resources.stonetexture;
            this.btnStone.Location = new System.Drawing.Point(104, 75);
            this.btnStone.Name = "btnStone";
            this.btnStone.Size = new System.Drawing.Size(86, 64);
            this.btnStone.TabIndex = 1;
            this.btnStone.Text = "Stone";
            this.btnStone.UseVisualStyleBackColor = true;
            this.btnStone.Click += new System.EventHandler(this.btnStone_Click);
            // 
            // btnWater
            // 
            this.btnWater.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnWater.Image = global::LevelEditor.Properties.Resources.watertexture;
            this.btnWater.Location = new System.Drawing.Point(196, 75);
            this.btnWater.Name = "btnWater";
            this.btnWater.Size = new System.Drawing.Size(86, 64);
            this.btnWater.TabIndex = 2;
            this.btnWater.Text = "Water";
            this.btnWater.UseVisualStyleBackColor = true;
            this.btnWater.Click += new System.EventHandler(this.btnWater_Click);
            // 
            // btnGrass
            // 
            this.btnGrass.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGrass.Image = global::LevelEditor.Properties.Resources.grasstexture;
            this.btnGrass.Location = new System.Drawing.Point(288, 75);
            this.btnGrass.Name = "btnGrass";
            this.btnGrass.Size = new System.Drawing.Size(86, 64);
            this.btnGrass.TabIndex = 3;
            this.btnGrass.Text = "Grass";
            this.btnGrass.UseVisualStyleBackColor = true;
            this.btnGrass.Click += new System.EventHandler(this.btnGrass_Click);
            // 
            // lblPlacing
            // 
            this.lblPlacing.AutoSize = true;
            this.lblPlacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlacing.Location = new System.Drawing.Point(608, 114);
            this.lblPlacing.Name = "lblPlacing";
            this.lblPlacing.Size = new System.Drawing.Size(151, 25);
            this.lblPlacing.TabIndex = 4;
            this.lblPlacing.Text = "Placing: Wood";
            // 
            // btnNothing
            // 
            this.btnNothing.BackgroundImage = global::LevelEditor.Properties.Resources.blank;
            this.btnNothing.Location = new System.Drawing.Point(380, 75);
            this.btnNothing.Name = "btnNothing";
            this.btnNothing.Size = new System.Drawing.Size(86, 64);
            this.btnNothing.TabIndex = 5;
            this.btnNothing.Text = "nothing";
            this.btnNothing.UseVisualStyleBackColor = true;
            this.btnNothing.Click += new System.EventHandler(this.btnNothing_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiNew,
            this.tsiSave,
            this.tsiLoad});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1209, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsiNew
            // 
            this.tsiNew.Name = "tsiNew";
            this.tsiNew.Size = new System.Drawing.Size(43, 20);
            this.tsiNew.Text = "New";
            this.tsiNew.Click += new System.EventHandler(this.tsiNew_Click);
            // 
            // tsiSave
            // 
            this.tsiSave.Name = "tsiSave";
            this.tsiSave.Size = new System.Drawing.Size(43, 20);
            this.tsiSave.Text = "Save";
            this.tsiSave.Click += new System.EventHandler(this.tsiSave_Click);
            // 
            // tsiLoad
            // 
            this.tsiLoad.Name = "tsiLoad";
            this.tsiLoad.Size = new System.Drawing.Size(45, 20);
            this.tsiLoad.Text = "Load";
            this.tsiLoad.Click += new System.EventHandler(this.tsiLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Click on a tile to begin placing that tile in the level";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 565);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNothing);
            this.Controls.Add(this.lblPlacing);
            this.Controls.Add(this.btnWood);
            this.Controls.Add(this.btnStone);
            this.Controls.Add(this.btnGrass);
            this.Controls.Add(this.btnWater);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Level Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnWood;
        private System.Windows.Forms.Button btnStone;
        private System.Windows.Forms.Button btnWater;
        private System.Windows.Forms.Button btnGrass;
        private System.Windows.Forms.Label lblPlacing;
        private System.Windows.Forms.Button btnNothing;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsiNew;
        private System.Windows.Forms.ToolStripMenuItem tsiSave;
        private System.Windows.Forms.ToolStripMenuItem tsiLoad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

