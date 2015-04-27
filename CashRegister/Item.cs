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

        public Item(XmlNode source, ItemGroup group)
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

            // Default pretty much everything from the group.
            this.Type = group.Type;
            this.Color = group.Color;
            this.Discounts = new List<string>(group.Discounts);
            this.RevenueGroup = group.RevenueGroup;
            this.Taxable = group.Taxable;

            dummyNode = source.Attributes["type"];
            if (dummyNode != null)
            {
                this.Type = dummyNode.InnerText;
            }
            else if (this.Type == null)
            {
                MessageBox.Show("Item " + this.Name + " does not have a type", "ITEM LOADING", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            dummyNode = source.Attributes["color"];
            if (dummyNode != null)
            {
                int hexColor = 0x00FFFFFF; // default color
                hexColor = int.Parse(dummyNode.InnerText, System.Globalization.NumberStyles.AllowHexSpecifier);
                this.Color = System.Drawing.Color.FromArgb(hexColor);
            }

            dummyNode = source.Attributes["discounts"];
            if (dummyNode != null)
            {
                this.Discounts = new List<string>();
                char[] delimiters = { ',' };
                string[] discountArray = dummyNode.InnerText.Split(delimiters);
                foreach (string s in discountArray)
                {
                    this.Discounts.Add(s);
                }
            }

            dummyNode = source.Attributes["price"];
            if (dummyNode != null)
            {
                this.Price = decimal.Parse(dummyNode.InnerText);
            }
            else
            {
                // Normally means a variable-price item
                this.Price = 0.00M;
            }

            dummyNode = source.Attributes["revenuegroup"];
            if (dummyNode != null)
            {
                this.RevenueGroup = dummyNode.InnerText;
            }
            else if (this.RevenueGroup == null)
            {
                // One of the dialogbox items
                this.RevenueGroup = "Jarlidium";
            }

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
                }
            }
        }
    }
}
