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
    public partial class VarBoxDialog : Form
    {
        public VarBoxDialog(string revenueGroupIfAny)
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            this.Price = 0.00m;

            foreach (string s in Utilities.GetRevenueGroups())
            {
                this.lRevenueGroup.Items.Add(s);
            }
            if (revenueGroupIfAny != null)
            {
                int match = this.lRevenueGroup.FindString(revenueGroupIfAny);
                if (match != ListBox.NoMatches)
                {
                    this.lRevenueGroup.SetSelected(match, true);
                }
            }
        }

        private void tPrice_TextChanged(object sender, EventArgs e)
        {
            decimal tempPrice = 0.00m;
            try
            {
                tempPrice = decimal.Parse(this.tPrice.Text);
            }
            catch (FormatException)
            {
                this.tPrice.ForeColor = Color.Red;
                this.bOK.Enabled = false;
            }

            if (tempPrice > 0)
            {
                this.tPrice.ForeColor = Color.Green;
                this.Price = tempPrice;
                if (this.lRevenueGroup.SelectedItem != null)
                {
                    this.bOK.Enabled = true;
                }
            }
            else
            {
                this.tPrice.ForeColor = Color.Red;
                this.bOK.Enabled = false;
            }
        }

        private void lRevenueGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lRevenueGroup.SelectedItem != null)
            {
                this.RevenueGroup = (string)this.lRevenueGroup.SelectedItem;
                if (this.Price > 0.00m)
                {
                    this.bOK.Enabled = true;
                }
            }
        }

        public decimal Price { get; private set; }
        public string RevenueGroup { get; private set; }
        public string Details { get { return this.tDetails.Text; } }
    }
}
