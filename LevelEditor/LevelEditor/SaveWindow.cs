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
    public partial class SaveWindow : Form
    {
        Form1 editorForm;
        public SaveWindow(Form1 editorForm)
        {
            this.editorForm = editorForm;

            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            editorForm.Save(txtFileName.Text);
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
