using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;

namespace CashRegister
{
    class ItemButtonManager
    {
        public ItemButtonManager(Form parent, UIHandler.UpdateDelegate updater, Size buttonSize, Point buttonSpacing, Point upperLeft)
        {
            this.parent = parent;
            this.updater = updater;

            this.buttonSize = buttonSize;
            this.buttonSpacing = buttonSpacing;
            this.upperLeft = upperLeft;

            this.buttonsPerColumn = (parent.Height - (2 * upperLeft.Y)) / buttonSpacing.Y;
        }

        public void Setup()
        {
            int itemCount = 0;
            int[] itemNumbers = null;
            int[] itemOrders = null;
            int validItemCount = 0;

#if false
            itemCount = int.Parse(ConfigurationManager.AppSettings["ItemCount"]);

            itemNumbers = new int[itemCount + 1];
            itemOrders = new int[itemCount + 1];
            validItemCount = itemCount;
            for (int i = 1; i <= itemCount; i++)
            {
                itemNumbers[i] = i;
                if (ConfigurationManager.AppSettings["Item" + i.ToString() + "Type"].ToLower().CompareTo("deleted") == 0)
                {
                    itemOrders[i] = 999999;
                    validItemCount--;
                }
                else
                {
                    itemOrders[i] = int.Parse(ConfigurationManager.AppSettings["Item" + i.ToString() + "Sort"]);
                }
            }
#else
            int itemMax = int.Parse(ConfigurationManager.AppSettings["ItemMax"]);
            itemNumbers = new int[itemMax + 1];
            itemOrders = new int[itemMax + 1];
            for (int i = 1; i <= itemMax; i++)
            {
                try
                {
                    string dummy = ConfigurationManager.AppSettings["Item" + i.ToString() + "Type"];
                    if (dummy != null)
                    {
                        // if we get here, then the item exists
                        itemCount++;
                        validItemCount++;
                        itemNumbers[validItemCount] = i;
                        itemOrders[validItemCount] = int.Parse(ConfigurationManager.AppSettings["Item" + i.ToString() + "Sort"]);
                    }
                }
                catch (ConfigurationErrorsException)
                {
                    // No such item, keep going
                }
            }
#endif
            Array.Sort(itemOrders, itemNumbers, 1, validItemCount);

            for (int j = 1; j <= validItemCount; j++)
            {
                int i = itemNumbers[j];

                string itemType = ConfigurationManager.AppSettings["Item" + i.ToString() + "Type"];
                if (itemType.ToLower().CompareTo("spacer") == 0)
                {
                    continue;
                }

                int hexColor = int.Parse(ConfigurationManager.AppSettings["Item" + i.ToString() + "Color"], System.Globalization.NumberStyles.AllowHexSpecifier);
                System.Drawing.Color backColor = System.Drawing.Color.FromArgb(hexColor);

                string itemName = ConfigurationManager.AppSettings["Item" + i.ToString() + "Name"];

                int x = this.upperLeft.X + (((j - 1) / this.buttonsPerColumn) * this.buttonSpacing.X);
                int y = this.upperLeft.Y + (((j - 1) % this.buttonsPerColumn) * this.buttonSpacing.Y);
                Point location = new Point(x, y);

                string itemDiscountGroups = ConfigurationManager.AppSettings["Item" + i.ToString() + "Discounts"];
                List<string> listDiscountGroups = new List<string>();
                if (itemDiscountGroups != null)
                {
                    char[] delimiters = { ',' };
                    string[] discountArray = itemDiscountGroups.Split(delimiters);
                    foreach (string s in discountArray)
                    {
                        listDiscountGroups.Add(s);
                    }
                }

                switch (itemType.ToLower())
                {
                    case "fixeditem":
                        CreateFixedItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    case "qtyitem":
                        CreateQtyItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    case "varboxitem":
                        CreateVarBoxItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    case "fixedboxitem":
                        CreateFixedBoxItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    case "miscitem":
                        CreateMiscItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    case "fixeddetailsitem":
                        CreateFixedDetailsItem(i, backColor, itemName, location, listDiscountGroups);
                        break;

                    default:
                        MessageBox.Show(String.Format("{0} in item {1}", itemType, i), "Unknown item type", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                }
            }
        }

        private void CreateFixedItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            decimal price = decimal.Parse(ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Price"]);
            string revenueGroup = ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Revgrp"];

            this.buttonList.Add(new FixedItem(this.parent, this.updater, itemName, price, GetTaxable(itemNumber), revenueGroup, backColor, location, this.buttonSize, listDiscountGroups));
        }

        private void CreateQtyItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            decimal price = decimal.Parse(ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Price"]);
            string revenueGroup = ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Revgrp"];

            this.buttonList.Add(new QtyItem(this.parent, this.updater, itemName, price, GetTaxable(itemNumber), revenueGroup, backColor, location, this.buttonSize, listDiscountGroups));
        }

        private void CreateVarBoxItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            string revenueGroup = ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Revgrp"];

            this.buttonList.Add(new VarBoxItem(this.parent, this.updater, itemName, GetTaxable(itemNumber), revenueGroup, backColor, location, this.buttonSize, listDiscountGroups));
        }

        private void CreateFixedBoxItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            decimal price = decimal.Parse(ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Price"]);

            this.buttonList.Add(new FixedBoxItem(this.parent, this.updater, itemName, price, GetTaxable(itemNumber), backColor, location, this.buttonSize, listDiscountGroups));
        }

        private void CreateMiscItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            this.buttonList.Add(new MiscItem(this.parent, this.updater, itemName, backColor, location, this.buttonSize, listDiscountGroups));
        }

        private void CreateFixedDetailsItem(int itemNumber, System.Drawing.Color backColor, string itemName, Point location, List<string> listDiscountGroups)
        {
            decimal price = decimal.Parse(ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Price"]);
            string revenueGroup = ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Revgrp"];

            this.buttonList.Add(new FixedDetailsItem(this.parent, this.updater, itemName, price, GetTaxable(itemNumber), revenueGroup, backColor, location, this.buttonSize, listDiscountGroups));
        }

        private LineItem.TaxStatus GetTaxable(int itemNumber)
        {
            LineItem.TaxStatus retval = LineItem.TaxStatus.TaxYes;

            string taxable = ConfigurationManager.AppSettings["Item" + itemNumber.ToString() + "Taxable"];
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
                    MessageBox.Show(String.Format("{0} in item {1}, defaulting to taxable", taxable, itemNumber), "Unknown taxable state", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }

            return retval;
        }

        public void EnableMerchButtons(bool enable)
        {
            foreach (ItemButton ib in this.buttonList)
            {
                ib.EnableButton(enable);
            }
        }

        private Size buttonSize;
        private Point buttonSpacing, upperLeft;
        private int buttonsPerColumn;

        private List<ItemButton> buttonList = new List<ItemButton>();

        private Form parent;
        private UIHandler.UpdateDelegate updater;
    }
}