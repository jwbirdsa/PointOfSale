using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace CashRegister
{
    abstract class ItemButton
    {
        private ItemButton()
        {
            // forbidden
        }

        public ItemButton(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
        {
            this.parent = parent;
            this.updater = updater;

            this.listDiscountGroups = item.Discounts;

            this.button = new UniformButton();
            this.button.Parent = this.parent;
            this.button.Text = item.Name;
            this.button.Location = location;
            this.button.Size = size;
            this.button.BackColor = item.Color;
            this.button.Enabled = false;
            this.button.Font = new Font(this.button.Font.FontFamily, this.button.Font.Size * 1.2f, FontStyle.Bold);
            this.button.Click += new EventHandler(ButtonOnClick);

            this.button.Show();
        }

        public void EnableButton(bool enable)
        {
            this.button.Enabled = enable;
        }

        abstract internal void ButtonOnClick(object sender, EventArgs ea);

        protected Form parent;
        protected UIHandler.UpdateDelegate updater;
        protected UniformButton button;

        protected Int16 quantity = 1;
        protected decimal priceEach = 0.00m;
        protected LineItem.TaxStatus taxable = LineItem.TaxStatus.TaxYes;
        protected string revenueGroup = "";
        protected string extraDetails = "";
        protected List<string> listDiscountGroups;
    }

    class FixedItem : ItemButton
    {
        public FixedItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            // quantity is fixed at 1
            this.priceEach = item.Price;
            this.taxable = item.Taxable;
            this.revenueGroup = item.RevenueGroup;
            // no details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            // This is a fixed item, there is no need to prompt for anything
            LineItem li = new LineItem(this.button.Text, 1, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
            Purchase.AddMerchandise(li);
            this.updater();
        }
    }

    class QtyItem : ItemButton
    {
        public QtyItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            // prompt for quantity
            this.priceEach = item.Price;
            this.taxable = item.Taxable;
            this.revenueGroup = item.RevenueGroup;
            // no details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            QtyItemDialog dlg = new QtyItemDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog(this.parent);
            if (dr == DialogResult.OK)
            {
                this.quantity = dlg.Count;

                LineItem li = new LineItem(this.button.Text, this.quantity, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
                Purchase.AddMerchandise(li);
                this.updater();
            }
        }
    }

    class VarBoxItem : ItemButton
    {
        public VarBoxItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            // quantity is fixed at 1
            // prompt for price
            this.taxable = item.Taxable;
            // prompt for revenue group to override passed in
            this.revenueGroup = item.RevenueGroup;
            // prompt for details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            VarBoxDialog dlg = new VarBoxDialog(this.revenueGroup);
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.priceEach = dlg.Price;
                this.revenueGroup = dlg.RevenueGroup;
                this.extraDetails = dlg.Details;

                LineItem li = new LineItem(this.button.Text, 1, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
                Purchase.AddMerchandise(li);
                this.updater();
            }
        }
    }

    class FixedBoxItem : ItemButton
    {
        public FixedBoxItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            // prompt for quantity
            this.priceEach = item.Price;
            this.taxable = item.Taxable;
            // prompt for revenue group
            // prompt for details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            FixedBoxDialog dlg = new FixedBoxDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.quantity = dlg.Quantity;
                this.revenueGroup = dlg.RevenueGroup;
                this.extraDetails = dlg.Details;

                LineItem li = new LineItem(this.button.Text, this.quantity, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
                Purchase.AddMerchandise(li);
                this.updater();
            }
        }
    }

    class MiscItem : ItemButton
    {
        public MiscItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            //quantity is fixed at 1
            // prompt for price
            // prompt for taxable
            // prompt for revenue group
            // prompt for details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            MiscItemDialog dlg = new MiscItemDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.priceEach = dlg.Price;
                this.taxable = dlg.Taxable;
                this.revenueGroup = dlg.RevenueGroup;
                this.extraDetails = dlg.Details;

                LineItem li = new LineItem(this.button.Text + " " + this.extraDetails, 1, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
                Purchase.AddMerchandise(li);
                this.updater();
            }
        }
    }

    class FixedDetailsItem : ItemButton
    {
        public FixedDetailsItem(Form parent, UIHandler.UpdateDelegate updater, Item item, Point location, Size size)
            : base(parent, updater, item, location, size)
        {
            // quantity is fixed at 1
            this.priceEach = item.Price;
            this.taxable = item.Taxable;
            this.revenueGroup = item.RevenueGroup;
            // prompt for details
        }

        internal override void ButtonOnClick(object sender, EventArgs ea)
        {
            FixedDetailsDialog dlg = new FixedDetailsDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.extraDetails = dlg.Details;

                LineItem li = new LineItem(this.button.Text + " " + this.extraDetails, 1, this.priceEach, this.taxable, this.revenueGroup, this.extraDetails, this.listDiscountGroups);
                Purchase.AddMerchandise(li);
                this.updater();
            }
        }
    }
}
