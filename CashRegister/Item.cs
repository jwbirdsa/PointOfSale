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
            XmlNode dummyNode = source.Attributes["name"];
            if (dummyNode != null)
            {
                this.Name = dummyNode.InnerText;
            }
            else
            {
                MessageBox.Show("Nameless item!", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dummyNode = source.Attributes["type"];
            if (dummyNode != null)
            {
                this.Type = dummyNode.InnerText;
            }
            else
            {
                MessageBox.Show("Item " + this.Name + " does not have a type", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dummyNode = source.Attributes["color"];
            int hexColor = 0x00FFFFFF; // default color
            if (dummyNode != null)
            {
                hexColor = int.Parse(dummyNode.InnerText, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            this.Color = System.Drawing.Color.FromArgb(hexColor);

            this.Discounts = new List<string>();
            dummyNode = source.Attributes["discounts"];
            if (dummyNode != null)
            {
                char[] delimiters = { ',' };
                string[] discountArray = dummyNode.InnerText.Split(delimiters);
                foreach (string s in discountArray)
                {
                    this.Discounts.Add(s);
                }
            }
            else
            {
                // Default to an empty list
            }

            dummyNode = source.Attributes["price"];
            if (dummyNode != null)
            {
                this.Price = decimal.Parse(dummyNode.InnerText);
            }
            else
            {
                MessageBox.Show("Item " + this.Name + " does not have a price", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            dummyNode = source.Attributes["revenuegroup"];
            if (dummyNode != null)
            {
                this.RevenueGroup = dummyNode.InnerText;
            }
            else
            {
                MessageBox.Show("Item " + this.Name + " does not have a revenue group", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            this.Taxable = LineItem.TaxStatus.TaxYes;
            dummyNode = source.Attributes["taxable"];
            if (dummyNode != null)
            {
                switch (dummyNode.InnerText.ToLower())
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
}
