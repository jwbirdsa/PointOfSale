using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CashRegister
{
    class Item
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public System.Drawing.Color Color { get; private set; }
        public List<string> Discounts { get; private set; }
        public decimal Price { get; private set; }
        public string RevenueGroup { get; private set; }
        public LineItem.TaxStatus Taxable { get; private set; }

        public Item(XmlNode source)
        {
            this.Name = source.Attributes["name"].InnerText;
            if (this.Name == null)
            {
                MessageBox.Show("Nameless item!", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            this.Type = source.Attributes["type"].InnerText;
            if (this.Type == null)
            {
                MessageBox.Show("Item " + this.Name + " does not have a type", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            string dummy = source.Attributes["color"].InnerText;
            int hexColor = 0x00FFFFFF; // default color
            if (dummy != null)
            {
                hexColor = int.Parse(dummy, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            this.Color = System.Drawing.Color.FromArgb(hexColor);

            this.Discounts = new List<string>();
            dummy = source.Attributes["discounts"].InnerText;
            if (dummy != null)
            {
                if (dummy != null)
                {
                    char[] delimiters = { ',' };
                    string[] discountArray = dummy.Split(delimiters);
                    foreach (string s in discountArray)
                    {
                        this.Discounts.Add(s);
                    }
                }
            }
            else
            {
                // Default to an empty list
            }

            dummy = source.Attributes["price"].InnerText;
            if (dummy != null)
            {
                this.Price = decimal.Parse(dummy);
            }
            else
            {
                MessageBox.Show("Item " + this.Name + " does not have a price", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            this.RevenueGroup = source.Attributes["revenuegroup"].InnerText;
            if (this.RevenueGroup == null)
            {
                MessageBox.Show("Item " + this.Name + " does not have a revenue group", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            this.Taxable = LineItem.TaxStatus.TaxYes;
            dummy = source.Attributes["taxable"].InnerText;
            switch (dummy.ToLower())
            {
                case "taxable":
                    // nothing to do here
                    break;

                case "nontaxable":
                    this.Taxable = LineItem.TaxStatus.TaxNo;
                    break;

                case "taxincluded":
                    this.Taxable = LineItem.TaxStatus.TaxIncluded;
                    break;

                default:
                    MessageBox.Show("Item " + this.Name + " defaulting to taxable", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }

        }
    }
}
