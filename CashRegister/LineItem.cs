using System;
using System.Collections.Generic;

namespace CashRegister
{
    public class LineItem
    {
        public LineItem(string itemName, Int16 qty, decimal each, TaxStatus taxable, string revenueGroup, string extraDetails, List<string> discountGroups)
        {
            this.itemName = itemName;

            this.quantity = qty;
            this.priceEach = each;

            this.taxable = taxable;

            this.revenueGroup = revenueGroup;

            this.extraDetails = extraDetails;

            if (discountGroups != null)
            {
                this.discountGroups = discountGroups;
            }
            else
            {
                this.discountGroups = new List<string>();
            }
        }

        public string Format()
        {
            string line;

            // Trim the item name to the available size
            line = Utilities.PadOrTruncateLeft(this.itemName, ((this.quantity > 1) ? PrintParams.itemChars : (PrintParams.itemChars + PrintParams.qtyAtPriceChars)));

            // Add qty@price if needed
            if (this.quantity > 1)
            {
                line += "  ";
                line += Utilities.PadOrTruncateRight(this.quantity.ToString(), PrintParams.qtyPositions);
                line += "@";
                line += Utilities.DollarsAndCentsString(this.priceEach, PrintParams.dollarPositions);
            }

            // Finally, put the total on
            line += "  ";
            line += Utilities.DollarsAndCentsString(this.quantity * this.priceEach, PrintParams.dollarPositions);
            if (this.taxable == TaxStatus.TaxIncluded)
            {
                line += PrintParams.taxincMarker;
            }
            else if (this.taxable == TaxStatus.TaxNo)
            {
                line += PrintParams.taxnonMarker;
            }
            else
            {
                line += " ";
            }

            return line;
        }

        public readonly string itemName;

        public readonly Int16 quantity;
        public readonly decimal priceEach;

        public enum TaxStatus { TaxYes, TaxNo, TaxIncluded };
        public readonly TaxStatus taxable;

        public readonly string revenueGroup;

        public readonly string extraDetails;
        public readonly List<string> discountGroups;
    }
}