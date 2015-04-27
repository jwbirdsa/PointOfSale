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
    public partial class ChangeDue : Form
    {
        public ChangeDue(decimal amount)
        {
            InitializeComponent();
            label2.Text = String.Format("${0}", amount);
        }
    }
}
