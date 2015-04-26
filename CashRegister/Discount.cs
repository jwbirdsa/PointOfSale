using System;
using System.Collections.Generic;
using System.Configuration;

namespace CashRegister
{
    public class DiscountEntry
    {
        public decimal GreaterOrEqual { get; set; } // may be count or $$$
        public decimal LessThan { get; set; }       // may be count or $$$
        public decimal Discount { get; set; }       // may be flat $$$ or %
    }

    public abstract class DiscountBase
    {
        public DiscountBase(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
        {
            this.Name = name;
            this.discounts = discounts;
            this.taxable = taxable;
            this.revenueGroup = revenueGroup;
        }

        public abstract void Inspect();

        protected void Phase1Inspect(out decimal count, out decimal dollars)
        {
            count = 0.00m;
            dollars = 0.00m;

            foreach (LineItem li in Purchase.GetMerchandise())
            {
                if ((li.discountGroups.Contains(this.Name)) && (li.revenueGroup.CompareTo(this.revenueGroup) == 0))
                {
                    count += li.quantity;
                    dollars += (li.quantity * li.priceEach);
                }
            }
        }

        public string Name { get; private set; }
        protected List<DiscountEntry> discounts;
        protected LineItem.TaxStatus taxable;
        protected string revenueGroup;
    }

    public abstract class ByCountDiscount : DiscountBase
    {
        public ByCountDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        protected DiscountEntry Phase2Inspect(out decimal dollars)
        {
            decimal count;
            Phase1Inspect(out count, out dollars);

            DiscountEntry retval = null;
            foreach (DiscountEntry de in this.discounts)
            {
                if ((count >= de.GreaterOrEqual) && (count < de.LessThan))
                {
                    retval = de;
                    break;
                }
            }

            return retval;
        }
    }

    public abstract class ByDollarsDiscount : DiscountBase
    {
        public ByDollarsDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        protected DiscountEntry Phase2Inspect(out decimal dollars)
        {
            decimal count;
            Phase1Inspect(out count, out dollars);

            DiscountEntry retval = null;
            foreach (DiscountEntry de in this.discounts)
            {
                if ((dollars >= de.GreaterOrEqual) && (dollars < de.LessThan))
                {
                    retval = de;
                    break;
                }
            }

            return retval;
        }
    }

    public class FixedByCountDiscount : ByCountDiscount
    {
        public FixedByCountDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        public override void Inspect()
        {
            decimal dummy2;
            DiscountEntry de = Phase2Inspect(out dummy2);
            LineItem discountItem = null;
            if (de != null)
            {
                discountItem = new LineItem(this.Name, 1, -de.Discount, this.taxable, this.revenueGroup, "", null);
            }
            Purchase.SetDiscount(this.Name, discountItem);
        }
    }

    public class PercentByCountDiscount : ByCountDiscount
    {
        public PercentByCountDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        public override void Inspect()
        {
            decimal dollars;
            DiscountEntry de = Phase2Inspect(out dollars);
            LineItem discountItem = null;
            if (de != null)
            {
                discountItem = new LineItem(this.Name + " " + Utilities.PercentageString(de.Discount, 0) + "%", 1,
                    -Math.Round(de.Discount * dollars, 2, MidpointRounding.AwayFromZero), this.taxable, this.revenueGroup, "", null);
            }
            Purchase.SetDiscount(this.Name, discountItem);
        }
    }

    public class FixedByDollarsDiscount : ByDollarsDiscount
    {
        public FixedByDollarsDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        public override void Inspect()
        {
            decimal dummy2;
            DiscountEntry de = Phase2Inspect(out dummy2);
            LineItem discountItem = null;
            if (de != null)
            {
                discountItem = new LineItem(this.Name, 1, -de.Discount, this.taxable, this.revenueGroup, "", null);
            }
            Purchase.SetDiscount(this.Name, discountItem);
        }
    }

    public class PercentByDollarsDiscount : ByDollarsDiscount
    {
        public PercentByDollarsDiscount(string name, List<DiscountEntry> discounts, LineItem.TaxStatus taxable, string revenueGroup)
            : base(name, discounts, taxable, revenueGroup)
        {
        }

        public override void Inspect()
        {
            decimal dollars;
            DiscountEntry de = Phase2Inspect(out dollars);
            LineItem discountItem = null;
            if (de != null)
            {
                discountItem = new LineItem(this.Name + " " + Utilities.PercentageString(de.Discount, 0) + "%", 1,
                    -Math.Round(de.Discount * dollars, 2, MidpointRounding.AwayFromZero), this.taxable, this.revenueGroup, "", null);
            }
            Purchase.SetDiscount(this.Name, discountItem);
        }
    }
}