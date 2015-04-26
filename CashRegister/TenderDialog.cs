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
    public partial class TenderDialog : Form
    {
        public TenderDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void tData_TextChanged(object sender, EventArgs e)
        {
            if (CashValidator != 0m)
            {
                decimal tempCash = 0;
                try
                {
                    tempCash = decimal.Parse(this.tData.Text);
                }
                catch (FormatException)
                {
                    this.tData.ForeColor = Color.Red;
                    this.bOK.Enabled = false;
                }

                if (tempCash >= CashValidator)
                {
                    this.tData.ForeColor = Color.Green;
                    this.bOK.Enabled = true;
                    TenderedCash = tempCash;
                }
                else
                {
                    this.tData.ForeColor = Color.Red;
                    this.bOK.Enabled = false;
                }
            }
            else if (this.tData.TextLength > 0)
            {
                OtherData = this.tData.Text;
                this.bOK.Enabled = true;
            }
            else
            {
                this.bOK.Enabled = false;
            }
        }

        public decimal CashValidator { get; set; }
        public decimal TenderedCash { get; private set; }
        public string OtherData { get; private set; }
        public string Label { get { return this.lLabel.Text; } set { this.lLabel.Text = value; } }
    }
}
