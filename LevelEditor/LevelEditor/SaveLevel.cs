using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class SaveLevel : Form
    {
        Form1 editorForm;

        public SaveLevel(Form1 editorForm)
        {
            this.editorForm = editorForm;
            MessageBox.Show("There is currently no way of checking if a file already exists. Be careful not to overwite a file!!!");
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            editorForm.Save(txtFileName.Text);
            MessageBox.Show("Saved to bin>Debug");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
