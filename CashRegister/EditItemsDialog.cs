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
    public partial class EditItemsDialog : Form
    {
        public EditItemsDialog(List<LineItem> merchandise)
        {
            InitializeComponent();

            AcceptButton = bOK;
            CancelButton = bCancel;

            foreach (LineItem item in merchandise)
            {
                this.clbItems.Items.Add(item.Format(), false);
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            this.DeleteTheseItems = new List<string>();
            foreach (object item in this.clbItems.CheckedItems)
            {
                this.DeleteTheseItems.Add(item.ToString());
            }
        }

        public List<string> DeleteTheseItems { get; private set; }
    }
}
