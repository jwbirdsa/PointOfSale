using System;
using System.Collections.Generic;
using System.IO;

namespace CashRegister
{
    static class Purchase
    {
        private static List<LineItem> merchandise = new List<LineItem>();
        private static Dictionary<string, LineItem> discounts = new Dictionary<string,LineItem>();
        private static bool masterDiscountApply = false;

        public static long purchaseId { get; private set; }

        public static void Reset()
        {
            Purchase.merchandise.Clear();
            Purchase.discounts.Clear();
            Purchase.purchaseId = DateTime.Now.Ticks;
            Purchase.masterDiscountApply = false;
        }

        public static void AddMerchandise(LineItem li)
        {
            Purchase.merchandise.Add(li);
            DiscountManager.Inspect();
            Purchase.masterDiscountApply = false;
            Purchase.masterDiscountApply = (Purchase.Subtotal() >= Utilities.GetMasterDiscountThreshhold());
        }

        public static void DeleteMerchandise(string matchThis)
        {
            foreach (LineItem li in Purchase.merchandise)
            {
                if (li.Format().CompareTo(matchThis) == 0)
                {
                    Purchase.merchandise.Remove(li);
                    break;
                }
            }
            DiscountManager.Inspect();
            Purchase.masterDiscountApply = false;
            Purchase.masterDiscountApply = (Purchase.Subtotal() >= Utilities.GetMasterDiscountThreshhold());
        }

        public static List<LineItem> GetForDisplay()
        {
            List<LineItem> retval = new List<LineItem>();
            retval.AddRange(Purchase.merchandise);
            foreach (KeyValuePair<string, LineItem> kvp in Purchase.discounts)
            {
                if (kvp.Value.quantity == 1)
                {
                    // Return only discounts which have been applied
                    retval.Add(kvp.Value);
                }
            }
            if (Purchase.masterDiscountApply)
            {
                retval.AddRange(Purchase.MakeMasterDiscountLineItems());
            }

            return retval;
        }

        public static bool IsPurchaseEmpty()
        {
            return (Purchase.merchandise.Count == 0);
        }

        public static List<LineItem> GetMerchandise()
        {
            return Purchase.merchandise;
        }

        public static decimal Taxable()
        {
            decimal taxable, dummy1, dummy2;
            Purchase.TaxTotals(out taxable, out dummy1, out dummy2);
            return taxable;
        }

        public static void TaxTotals(out decimal taxable, out decimal nontaxable, out decimal taxIncluded)
        {
            RawTaxTotals(out taxable, out nontaxable, out taxIncluded);
            if (Purchase.masterDiscountApply)
            {
                decimal taxableDiscount, nontaxableDiscount, taxIncludedDiscount;
                ComputeMasterDiscounts(out taxableDiscount, out nontaxableDiscount, out taxIncludedDiscount);
                taxable -= taxableDiscount;
                nontaxable -= nontaxableDiscount;
                taxIncluded -= taxIncludedDiscount;
            }
        }

        private static void RawTaxTotals(out decimal taxable, out decimal nontaxable, out decimal taxIncluded)
        {
            taxable = 0.00m;
            nontaxable = 0.00m;
            taxIncluded = 0.00m;
            foreach (LineItem li in Purchase.merchandise)
            {
                decimal extended = li.quantity * li.priceEach;
                switch (li.taxable)
                {
                    case LineItem.TaxStatus.TaxYes:
                        taxable += extended;
                        break;

                    case LineItem.TaxStatus.TaxNo:
                        nontaxable += extended;
                        break;

                    case LineItem.TaxStatus.TaxIncluded:
                        taxIncluded += extended;
                        break;
                }
            }
            foreach (KeyValuePair<string, LineItem> kvp in Purchase.discounts)
            {
                if (kvp.Value.quantity == 1)
                {
                    decimal extended = kvp.Value.quantity * kvp.Value.priceEach;
                    switch (kvp.Value.taxable)
                    {
                        case LineItem.TaxStatus.TaxYes:
                            taxable += extended;
                            break;

                        case LineItem.TaxStatus.TaxNo:
                            nontaxable += extended;
                            break;

                        case LineItem.TaxStatus.TaxIncluded:
                            taxIncluded += extended;
                            break;
                    }
                }
            }
        }

        public static decimal Subtotal()
        {
            decimal taxable, nontaxable, taxincluded;
            Purchase.TaxTotals(out taxable, out nontaxable, out taxincluded);
            return taxable + nontaxable + taxincluded;
        }

        public static decimal TotalDue()
        {
            decimal subtotal = Purchase.Subtotal();
            return subtotal + Utilities.ComputeTax(Purchase.Taxable());
        }

        public static void SetDiscount(string discountKey, LineItem discount)
        {
            if (discount == null)
            {
                Purchase.discounts.Remove(discountKey);
            }
            else
            {
                Purchase.discounts[discountKey] = discount;
            }
        }

        public static void WriteLogs(DateTime purchaseTime)
        {
            string inventoryLogFilename = Utilities.GetInventoryLogFilename();
            string transactionLogFilename = Utilities.GetTransactionLogFilename();

            // Make backup copies
            string newSuffix = ".bak." + DateTime.Now.Ticks.ToString();
            try
            {
                File.Copy(inventoryLogFilename, inventoryLogFilename + newSuffix);
                File.Copy(transactionLogFilename, transactionLogFilename + newSuffix);
            }
            catch (FileNotFoundException)
            {
                // If the source file doesn't exist, that's fine
            }

            decimal taxable, nontaxable, taxincluded;

            // Inventory log
            //  for each in merchandise and discounts
            //      purchaseId, item name, revenue group, qty, price each, tax status
            FileStream inventoryStream = File.Open(inventoryLogFilename, FileMode.Append, FileAccess.Write);
            StreamWriter inventoryWriter = new StreamWriter(inventoryStream);
            foreach (LineItem li in Purchase.merchandise)
            {
                inventoryWriter.Write(String.Format("{0},{1},{2},{3},{4},{5},{6}\n", Purchase.purchaseId, li.itemName, li.revenueGroup, li.quantity,
                    Utilities.DollarsAndCentsString(li.priceEach, 0), li.taxable.ToString(), li.extraDetails.Replace(',', '_')));
            }
            foreach (KeyValuePair<string, LineItem> kvp in Purchase.discounts)
            {
                if (kvp.Value.quantity == 1)
                {
                    inventoryWriter.Write(String.Format("{0},{1},{2},{3},{4},{5}\n", Purchase.purchaseId, kvp.Value.itemName, kvp.Value.revenueGroup, kvp.Value.quantity,
                        Utilities.DollarsAndCentsString(kvp.Value.priceEach, 0), kvp.Value.taxable.ToString()));
                }
            }
            if (Purchase.masterDiscountApply)
            {
                foreach (LineItem li in Purchase.MakeMasterDiscountLineItems())
                {
                    inventoryWriter.Write(String.Format("{0},{1},{2},{3},{4},{5}\n", Purchase.purchaseId, li.itemName, li.revenueGroup, li.quantity,
                        Utilities.DollarsAndCentsString(li.priceEach, 0), li.taxable.ToString()));
                }
            }
            inventoryWriter.Flush();
            inventoryStream.Close();

            // Transaction log
            //  timestamp, purchaseId, taxable total, nontaxable total, tax-included total, tax explicitly collected
            FileStream transactionStream = File.Open(transactionLogFilename, FileMode.Append, FileAccess.Write);
            StreamWriter transactionWriter = new StreamWriter(transactionStream);
            Purchase.TaxTotals(out taxable, out nontaxable, out taxincluded);
            transactionWriter.Write(String.Format("{0},{1},{2},{3},{4},{5}\n", purchaseTime, Purchase.purchaseId, Utilities.DollarsAndCentsString(taxable, 0),
                Utilities.DollarsAndCentsString(nontaxable, 0), Utilities.DollarsAndCentsString(taxincluded, 0),
                Utilities.DollarsAndCentsString(Utilities.ComputeTax(taxable), 0)));
            transactionWriter.Flush();
            transactionWriter.Close();
        }

        private static List<LineItem> MakeMasterDiscountLineItems()
        {
            List<LineItem> retval = new List<LineItem>();
            decimal taxable, nontaxable, taxIncluded;
            Purchase.RawTaxTotals(out taxable, out nontaxable, out taxIncluded);
            decimal taxableDiscount, nontaxableDiscount, taxIncludedDiscount;
            Purchase.ComputeMasterDiscounts(out taxableDiscount, out nontaxableDiscount, out taxIncludedDiscount);
            if (taxable > 0.00m)
            {
                LineItem li = new LineItem(Utilities.PercentageString(Utilities.GetMasterDiscountPercent(), 0) + "% off taxable", 1,
                    -taxableDiscount, LineItem.TaxStatus.TaxYes, "MasterDiscount", "", null);
                retval.Add(li);
            }
            if (nontaxable > 0.00m)
            {
                LineItem li = new LineItem(Utilities.PercentageString(Utilities.GetMasterDiscountPercent(), 0) + "% off nontaxable", 1,
                    -nontaxableDiscount, LineItem.TaxStatus.TaxNo, "MasterDiscount", "", null);
                retval.Add(li);
            }
            if (taxIncluded > 0.00m)
            {
                LineItem li = new LineItem(Utilities.PercentageString(Utilities.GetMasterDiscountPercent(), 0) + "% off taxinc", 1,
                    -taxIncludedDiscount, LineItem.TaxStatus.TaxIncluded, "MasterDiscount", "", null);
                retval.Add(li);
            }
            return retval;
        }

        private static void ComputeMasterDiscounts(out decimal taxableDiscount, out decimal nontaxableDiscount, out decimal taxIncludedDiscount)
        {
            decimal taxable, nontaxable, taxIncluded;
            Purchase.RawTaxTotals(out taxable, out nontaxable, out taxIncluded);
            taxableDiscount = Math.Round(taxable * Utilities.GetMasterDiscountPercent(), 2, MidpointRounding.AwayFromZero);
            nontaxableDiscount = Math.Round(nontaxable * Utilities.GetMasterDiscountPercent(), 2, MidpointRounding.AwayFromZero);
            taxIncludedDiscount = Math.Round(taxIncluded * Utilities.GetMasterDiscountPercent(), 2, MidpointRounding.AwayFromZero);
        }
    }
}