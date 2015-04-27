using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Xml;

namespace CashRegister
{
    class ItemGroup
    {
        public string Type { get; private set; }
        public System.Drawing.Color Color { get; private set; }
        public List<string> Discounts { get; private set; }
        public string RevenueGroup { get; private set; }
        public LineItem.TaxStatus Taxable { get; private set; }

        public ItemGroup(XmlNode source)
        {
            XmlNode dummyNode = source.Attributes["type"];
            this.Type = null;
            if (dummyNode != null)
            {
                this.Type = dummyNode.InnerText;
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

            dummyNode = source.Attributes["revenuegroup"];
            this.RevenueGroup = null;
            if (dummyNode != null)
            {
                this.RevenueGroup = dummyNode.InnerText;
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
                }
            }
        }
    }
}
