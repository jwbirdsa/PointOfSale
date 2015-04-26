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
    public partial class QtyItemDialog : Form
    {
        public QtyItemDialog()
        {
            InitializeComponent();

            AcceptButton = button1;
            CancelButton = button2;

            textBox1.TextChanged += new EventHandler(CountChanged);

            this.Count = 0;
        }

        void CountChanged(object sender, EventArgs e)
        {
            Int16 tempCount = 0;
            try
            {
                tempCount = Int16.Parse(textBox1.Text);
            }
            catch (FormatException)
            {
                textBox1.ForeColor = Color.Red;
                button1.Enabled = false;
            }

            if ((tempCount > 0) && (tempCount < 1000)) // we don't have 1000 of anything to sell!
            {
                textBox1.ForeColor = Color.Green;
                button1.Enabled = true;
                this.Count = tempCount;
            }
            else
            {
                textBox1.ForeColor = Color.Red;
                button1.Enabled = false;
            }
        }

        public Int16 Count { get; private set; }
    }
}
