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
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWood
            // 
            this.btnWood.Image = global::LevelEditor.Properties.Resources.woodtexture;
            this.btnWood.Location = new System.Drawing.Point(12, 12);
            this.btnWood.Name = "btnWood";
            this.btnWood.Size = new System.Drawing.Size(86, 64);
            this.btnWood.TabIndex = 0;
            this.btnWood.UseVisualStyleBackColor = true;
            this.btnWood.Click += new System.EventHandler(this.btnWood_Click);
            // 
            // btnStone
            // 
            this.btnStone.Image = global::LevelEditor.Properties.Resources.stonetexture;
            this.btnStone.Location = new System.Drawing.Point(104, 12);
            this.btnStone.Name = "btnStone";
            this.btnStone.Size = new System.Drawing.Size(86, 64);
            this.btnStone.TabIndex = 1;
            this.btnStone.UseVisualStyleBackColor = true;
            this.btnStone.Click += new System.EventHandler(this.btnStone_Click);
            // 
            // btnWater
            // 
            this.btnWater.Image = global::LevelEditor.Properties.Resources.watertexture;
            this.btnWater.Location = new System.Drawing.Point(196, 12);
            this.btnWater.Name = "btnWater";
            this.btnWater.Size = new System.Drawing.Size(86, 64);
            this.btnWater.TabIndex = 2;
            this.btnWater.UseVisualStyleBackColor = true;
            this.btnWater.Click += new System.EventHandler(this.btnWater_Click);
            // 
            // btnGrass
            // 
            this.btnGrass.Image = global::LevelEditor.Properties.Resources.grasstexture;
            this.btnGrass.Location = new System.Drawing.Point(288, 12);
            this.btnGrass.Name = "btnGrass";
            this.btnGrass.Size = new System.Drawing.Size(86, 64);
            this.btnGrass.TabIndex = 3;
            this.btnGrass.UseVisualStyleBackColor = true;
            this.btnGrass.Click += new System.EventHandler(this.btnGrass_Click);
            // 
            // lblPlacing
            // 
            this.lblPlacing.AutoSize = true;
            this.lblPlacing.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlacing.Location = new System.Drawing.Point(523, 29);
            this.lblPlacing.Name = "lblPlacing";
            this.lblPlacing.Size = new System.Drawing.Size(151, 25);
            this.lblPlacing.TabIndex = 4;
            this.lblPlacing.Text = "Placing: Wood";
            // 
            // btnNothing
            // 
            this.btnNothing.Location = new System.Drawing.Point(380, 12);
            this.btnNothing.Name = "btnNothing";
            this.btnNothing.Size = new System.Drawing.Size(86, 64);
            this.btnNothing.TabIndex = 5;
            this.btnNothing.Text = "nothing";
            this.btnNothing.UseVisualStyleBackColor = true;
            this.btnNothing.Click += new System.EventHandler(this.btnNothing_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(945, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(109, 26);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save Level";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 565);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNothing);
            this.Controls.Add(this.lblPlacing);
            this.Controls.Add(this.btnWood);
            this.Controls.Add(this.btnStone);
            this.Controls.Add(this.btnGrass);
            this.Controls.Add(this.btnWater);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.Button btnSave;
    }
}

