using System;
using System.Collections.Generic;
using System.Configuration;

namespace CashRegister
{
    static class DiscountManager
    {
        static public void Setup()
        {
            DiscountManager.discounts = new List<DiscountBase>();

            int discountCount = int.Parse(ConfigurationManager.AppSettings["DiscountCount"]);
            for (int i = 1; i <= discountCount; i++)
            {
                string discountType = ConfigurationManager.AppSettings["Discount" + i.ToString() + "Type"];
                string discountName = ConfigurationManager.AppSettings["Discount" + i.ToString() + "Name"];
                string discountRevenueGroup = ConfigurationManager.AppSettings["Discount" + i.ToString() + "Revgrp"];
                int discountEntries = int.Parse(ConfigurationManager.AppSettings["Discount" + i.ToString() + "Entries"]);

                List<DiscountEntry> entries = new List<DiscountEntry>();
                for (int j = 1; j <= discountEntries; j++)
                {
                    DiscountEntry de = new DiscountEntry();
                    de.GreaterOrEqual = decimal.Parse(ConfigurationManager.AppSettings["Discount" + i.ToString() + "Entry" + j.ToString() + "Low"]);
                    de.LessThan = decimal.Parse(ConfigurationManager.AppSettings["Discount" + i.ToString() + "Entry" + j.ToString() + "High"]);
                    de.Discount = decimal.Parse(ConfigurationManager.AppSettings["Discount" + i.ToString() + "Entry" + j.ToString() + "Discount"]);
                    entries.Add(de);
                }

                switch (discountType.ToLower())
                {
                    case "fixedbycount":
                        DiscountManager.discounts.Add(new FixedByCountDiscount(discountName, entries, GetTaxable(i), discountRevenueGroup));
                        break;

                    case "fixedbydollars":
                        DiscountManager.discounts.Add(new FixedByDollarsDiscount(discountName, entries, GetTaxable(i), discountRevenueGroup));
                        break;

                    case "percentbycount":
                        DiscountManager.discounts.Add(new PercentByCountDiscount(discountName, entries, GetTaxable(i), discountRevenueGroup));
                        break;

                    case "percentbydollars":
                        DiscountManager.discounts.Add(new PercentByDollarsDiscount(discountName, entries, GetTaxable(i), discountRevenueGroup));
                        break;

                    default:
                        System.Windows.Forms.MessageBox.Show(String.Format("{0} in discount {1}", discountType, i), "Unknown discount type",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                        break;
                }
            }
        }

        static private LineItem.TaxStatus GetTaxable(int discountNumber)
        {
            LineItem.TaxStatus retval = LineItem.TaxStatus.TaxYes;

            string taxable = ConfigurationManager.AppSettings["Discount" + discountNumber.ToString() + "Taxable"];
            switch (taxable.ToLower())
            {
                case "taxable":
                    // nothing to do here
                    break;

                case "nontaxable":
                    retval = LineItem.TaxStatus.TaxNo;
                    break;

                case "taxincluded":
                    retval = LineItem.TaxStatus.TaxIncluded;
                    break;

                default:
                    System.Windows.Forms.MessageBox.Show(String.Format("{0} in discount {1}, defaulting to taxable", taxable, discountNumber), "Unknown taxable state",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                    break;
            }

            return retval;
        }

        static public void Inspect()
        {
            foreach (DiscountBase db in DiscountManager.discounts)
            {
                db.Inspect();
            }
        }

        static private List<DiscountBase> discounts;
    }
}