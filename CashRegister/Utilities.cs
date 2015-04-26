using System;
using System.Collections.Generic;
using System.Configuration;

namespace CashRegister
{
    static class Utilities
    {
        public static string DollarsAndCentsString(decimal d, Int16 wholeDigits)
        {
            return DecimalString(d, wholeDigits, 2);
        }

        public static string PercentageString(decimal d, Int16 wholeDigits)
        {
            return DecimalString(d * 100, wholeDigits, 2);
        }

        public static string DecimalString(decimal d, Int16 wholeDigits, Int16 fracDigits)
        {
            string s = Math.Truncate(d).ToString();
            if ((d > -1.00m) && (d < 0.00m))
            {
                s = "-" + s;
            }
            s = s.PadLeft(wholeDigits) + ".";
            decimal factor = (decimal)Math.Pow(10.0, (double)fracDigits);
            decimal frac = Math.Abs(Math.Truncate((d - Math.Truncate(d)) * factor));
            s += frac.ToString().PadLeft(fracDigits, '0');
            return s;
        }

        public static string PadOrTruncateLeft(string victim, int hardLen)
        {
            if (victim.Length < hardLen)
            {
                return victim.PadRight(hardLen, ' ');
            }
            else
            {
                return victim.Substring(0, hardLen);
            }
        }

        public static string PadOrTruncateRight(string victim, int hardLen)
        {
            if (victim.Length < hardLen)
            {
                return victim.PadLeft(hardLen, ' ');
            }
            else
            {
                return victim.Substring(0, hardLen);
            }
        }

        public static decimal GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings["TaxRate"];
            return decimal.Parse(taxRate);
        }

        public static decimal ComputeTax(decimal taxable)
        {
            return Math.Round(taxable * Utilities.GetTaxRate(), 2, MidpointRounding.AwayFromZero);
        }

        public static List<string> GetRevenueGroups()
        {
            List<string> retval = new List<string>();
            int revenueGroupCount = int.Parse(ConfigurationManager.AppSettings["RevenueGroupCount"]);
            for (int i = 1; i <= revenueGroupCount; i++)
            {
                retval.Add(ConfigurationManager.AppSettings["RevenueGroup" + i.ToString()]);
            }
            return retval;
        }

        public static string GetInventoryLogFilename()
        {
            return ConfigurationManager.AppSettings["InventoryLog"];
        }

        public static string GetTransactionLogFilename()
        {
            return ConfigurationManager.AppSettings["TransactionLog"];
        }

        public static decimal GetMasterDiscountThreshhold()
        {
            return decimal.Parse(ConfigurationManager.AppSettings["MasterDiscountThresh"]);
        }

        public static decimal GetMasterDiscountPercent()
        {
            return decimal.Parse(ConfigurationManager.AppSettings["MasterDiscountPercent"]);
        }
    }
}