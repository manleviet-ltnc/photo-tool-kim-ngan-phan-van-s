using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manning.MyPhotoControls
{
    public partial class BaseExitDialog : Form
    {
        public BaseExitDialog()
        {
            InitializeComponent();
        }

        protected virtual void ReselDialog()
        {
            // Dose nothing in base class
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ReselDialog();
        }
    }
}
