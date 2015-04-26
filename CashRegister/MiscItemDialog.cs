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
    public partial class MiscItemDialog : Form
    {
        public MiscItemDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            this.priceValid = false;
            this.Price = 0.00m;

            foreach (string s in Utilities.GetRevenueGroups())
            {
                this.lRevenueGroup.Items.Add(s);
            }

            this.lTaxable.Items.Add("taxable");
            this.lTaxable.Items.Add("nontaxable");
            this.lTaxable.Items.Add("taxincluded");
        }

        private bool priceValid;
        public decimal Price { get; private set; }
        public LineItem.TaxStatus Taxable { get; private set; }
        public string RevenueGroup { get; private set; }
        public string Details { get { return this.tDetails.Text; } }

        private void tPrice_TextChanged(object sender, EventArgs e)
        {
            decimal tempPrice = 0.00m;
            this.priceValid = false;

            try
            {
                tempPrice = decimal.Parse(this.tPrice.Text);
            }
            catch (FormatException)
            {
                this.tPrice.ForeColor = Color.Red;
                this.bOK.Enabled = false;
            }

            this.tPrice.ForeColor = Color.Green;
            this.Price = tempPrice;
            this.priceValid = true;
            if ((this.lRevenueGroup.SelectedItem != null) && (this.lTaxable.SelectedItem != null))
            {
                this.bOK.Enabled = true;
            }
        }

        private void lRevenueGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lRevenueGroup.SelectedItem != null)
            {
                this.RevenueGroup = (string)this.lRevenueGroup.SelectedItem;
                if (this.priceValid && (this.lTaxable.SelectedItem != null))
                {
                    this.bOK.Enabled = true;
                }
            }
        }

        private void lTaxable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lTaxable.SelectedItem != null)
            {
                switch (((string)this.lTaxable.SelectedItem).ToLower())
                {
                    case "taxable":
                        this.Taxable = LineItem.TaxStatus.TaxYes;
                        break;

                    case "nontaxable":
                        this.Taxable = LineItem.TaxStatus.TaxNo;
                        break;

                    case "taxincluded":
                        this.Taxable = LineItem.TaxStatus.TaxIncluded;
                        break;
                }
                if (this.priceValid && (this.lRevenueGroup.SelectedItem != null))
                {
                    this.bOK.Enabled = true;
                }
            }
        }
    }
}
