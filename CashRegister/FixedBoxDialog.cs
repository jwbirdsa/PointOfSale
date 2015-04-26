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
    public partial class FixedBoxDialog : Form
    {
        public FixedBoxDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            this.Quantity = 0;

            foreach (string s in Utilities.GetRevenueGroups())
            {
                this.lRevenueGroup.Items.Add(s);
            }
        }

        private void tQuantity_TextChanged(object sender, EventArgs e)
        {
            Int16 tempQty = 0;
            try
            {
                tempQty = Int16.Parse(this.tQuantity.Text);
            }
            catch (FormatException)
            {
                this.tQuantity.ForeColor = Color.Red;
                this.bOK.Enabled = false;
            }

            if (tempQty > 0)
            {
                this.tQuantity.ForeColor = Color.Green;
                this.Quantity = tempQty;
                if (this.lRevenueGroup.SelectedItem != null)
                {
                    this.bOK.Enabled = true;
                }
            }
            else
            {
                this.tQuantity.ForeColor = Color.Red;
                this.bOK.Enabled = false;
            }
        }

        private void lRevenueGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lRevenueGroup.SelectedItem != null)
            {
                this.RevenueGroup = (string)this.lRevenueGroup.SelectedItem;
                if (this.Quantity > 0)
                {
                    this.bOK.Enabled = true;
                }
            }
        }

        public Int16 Quantity { get; private set; }
        public string RevenueGroup { get; private set; }
        public string Details { get { return this.tDetails.Text; } }
    }
}
