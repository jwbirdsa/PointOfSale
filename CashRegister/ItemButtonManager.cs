using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;
using System.Xml;

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
#if false
            int itemCount = 0;
            int[] itemNumbers = null;
            int[] itemOrders = null;
            int validItemCount = 0;

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
            }
#endif

            List<Item> itemList = new List<Item>();

            XmlDocument itemFile = new XmlDocument();
            itemFile.Load(".\\items.xml");
            XmlNode itemsNode = itemFile.DocumentElement.SelectSingleNode("/items");
            foreach (XmlNode itemGroupNode in itemsNode)
            {
                ItemGroup group = new ItemGroup(itemGroupNode);
                foreach (XmlNode itemNode in itemGroupNode.ChildNodes)
                {
                    Item item = new Item(itemNode, group);
                    itemList.Add(item);
                }
            }

            int locationCounter = 0;
            foreach (Item item in itemList)
            {
                ItemButton buttonToAdd = null;
                int x = this.upperLeft.X + ((locationCounter / this.buttonsPerColumn) * this.buttonSpacing.X);
                int y = this.upperLeft.Y + ((locationCounter % this.buttonsPerColumn) * this.buttonSpacing.Y);
                Point location = new Point(x, y);

                switch (item.Type.ToLower())
                {
                    case "fixed":
                        buttonToAdd = new FixedItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    case "qtyitem":
                        buttonToAdd = new QtyItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    case "varboxitem":
                        buttonToAdd = new VarBoxItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    case "fixedboxitem":
                        buttonToAdd = new FixedBoxItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    case "miscitem":
                        buttonToAdd = new MiscItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    case "fixeddetailsitem":
                        buttonToAdd = new FixedDetailsItem(this.parent, this.updater, item, location, this.buttonSize);
                        break;

                    default:
                        MessageBox.Show("Item " + item.Name + " has unknown item type", "LOADING ITEMS", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                }
                if (buttonToAdd != null)
                {
                    this.buttonList.Add(buttonToAdd);
                    locationCounter++;
                }
            }
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