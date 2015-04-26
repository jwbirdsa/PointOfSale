using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CashRegister
{
    public partial class FixedDetailsDialog : Form
    {
        public FixedDetailsDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void tDetails_TextChanged(object sender, EventArgs e)
        {
            this.bOK.Enabled = true;
        }

        public string Details { get { return this.tDetails.Text; } }
    }
}
