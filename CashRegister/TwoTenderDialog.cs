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
    public partial class TwoTenderDialog : Form
    {
        public TwoTenderDialog()
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;
        }

        private void tAmount_TextChanged(object sender, EventArgs e)
        {
            if (AmountValidator != 0m)
            {
                decimal tempCash = 0;
                try
                {
                    tempCash = decimal.Parse(this.tAmount.Text);
                }
                catch (FormatException)
                {
                    this.tAmount.ForeColor = Color.Red;
                    this.bOK.Enabled = false;
                }

                if (tempCash >= AmountValidator)
                {
                    this.tAmount.ForeColor = Color.Green;
                    this.bOK.Enabled = true;
                    TenderedAmount = tempCash;
                }
                else
                {
                    this.tAmount.ForeColor = Color.Red;
                    this.bOK.Enabled = false;
                }
            }
            else
            {
                this.bOK.Enabled = false;
            }
        }

        public decimal AmountValidator { get; set; }
        public decimal TenderedAmount { get; private set; }
        public string OtherData { get { return this.tCheckNumber.Text; } }
    }
}
