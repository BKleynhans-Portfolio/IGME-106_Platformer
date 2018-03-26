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
    public partial class LoadLevel : Form
    {
        Form1 editorForm;

        public LoadLevel(Form1 editorForm)
        {
            this.editorForm = editorForm;

            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            editorForm.Open(txtFileName.Text);
            this.Close();
        }
    }
}
