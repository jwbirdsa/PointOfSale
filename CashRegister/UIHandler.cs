using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;


namespace CashRegister
{
    class UIHandler
    {
        public UIHandler()
        {
            this.mainWindow = new Form();
            mainWindow.Text = "Jarlidium Cash Register";
            mainWindow.Width = 1400;
            mainWindow.Height = 900;
            mainWindow.FormBorderStyle = FormBorderStyle.FixedSingle;
            mainWindow.MaximizeBox = false;
            mainWindow.StartPosition = FormStartPosition.CenterScreen;

            this.pos = new Form();
            pos.Text = "Jarlidium Press";
            pos.Width = 740;
            pos.Height = 580;
            pos.FormBorderStyle = FormBorderStyle.FixedSingle;
            pos.MaximizeBox = false;
            pos.Show();

            this.tapeDisplay = new TapeDisplay();
            this.tapeDisplay.Setup(this.mainWindow, this.pos);

            this.printer = new Printer();

            this.buttonManager = new ItemButtonManager(this.mainWindow, new UpdateDelegate(MerchandiseAdded), new Size(buttonWidth, buttonHeight),
                new Point(buttonHSpace, buttonVSpace), new Point(10, 10));
        }

        public void Run()
        {
            Application.Run(this.mainWindow);
        }

        public void Setup()
        {
            Int32 systemButtonsLeft = this.mainWindow.Width - 10 - tapeDisplay.MainWidth - 20 - buttonHSpace;
            //systemButtonsLeft /= buttonHSpace;
            //systemButtonsLeft *= buttonHSpace;

            Int16 b = 0;
            Int32 systemButtonsTop = 10;

            Color systemButtonsColor = Color.YellowGreen;

            this.startPurchase = new UniformButton();
            this.startPurchase.Parent = this.mainWindow;
            this.startPurchase.Text = "Start";
            this.startPurchase.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.startPurchase.Width = buttonWidth;
            this.startPurchase.Height = buttonHeight;
            this.startPurchase.BackColor = systemButtonsColor;
            this.startPurchase.Click += new EventHandler(StartPurchase);

            b++;

            this.editPurchase = new UniformButton();
            this.editPurchase.Parent = this.mainWindow;
            this.editPurchase.Text = "Remove Items";
            this.editPurchase.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.editPurchase.Width = buttonWidth;
            this.editPurchase.Height = buttonHeight;
            this.editPurchase.BackColor = systemButtonsColor;
            this.editPurchase.Click += new EventHandler(EditPurchase);

            b++;

            this.tenderCash = new UniformButton();
            this.tenderCash.Parent = this.mainWindow;
            this.tenderCash.Text = "Cash";
            this.tenderCash.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.tenderCash.Width = buttonWidth;
            this.tenderCash.Height = buttonHeight;
            this.tenderCash.BackColor = systemButtonsColor;
            this.tenderCash.Click += new EventHandler(TenderCash);

            this.tenderCheck = new UniformButton();
            this.tenderCheck.Parent = this.mainWindow;
            this.tenderCheck.Text = "Check";
            this.tenderCheck.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.tenderCheck.Width = buttonWidth;
            this.tenderCheck.Height = buttonHeight;
            this.tenderCheck.BackColor = systemButtonsColor;
            this.tenderCheck.Click += new EventHandler(TenderCheck);

            this.tenderCredit = new UniformButton();
            this.tenderCredit.Parent = this.mainWindow;
            this.tenderCredit.Text = "Credit";
            this.tenderCredit.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.tenderCredit.Width = buttonWidth;
            this.tenderCredit.Height = buttonHeight;
            this.tenderCredit.BackColor = systemButtonsColor;
            this.tenderCredit.Click += new EventHandler(TenderCredit);

            this.cut = new UniformButton();
            this.cut.Parent = this.mainWindow;
            this.cut.Text = "Cut";
            this.cut.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.cut.Width = buttonWidth;
            this.cut.Height = buttonHeight;
            this.cut.BackColor = systemButtonsColor;
            this.cut.Click += new EventHandler(Cut);

            this.noSale = new UniformButton();
            this.noSale.Parent = this.mainWindow;
            this.noSale.Text = "No Sale";
            this.noSale.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.noSale.Width = buttonWidth;
            this.noSale.Height = buttonHeight;
            this.noSale.BackColor = Color.YellowGreen;
            this.noSale.Click += new EventHandler(NoSale);

            b++;

            this.reprintLastReceipt = new UniformButton();
            this.reprintLastReceipt.Parent = this.mainWindow;
            this.reprintLastReceipt.Text = "Reprint Receipt";
            this.reprintLastReceipt.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.reprintLastReceipt.Width = buttonWidth;
            this.reprintLastReceipt.Height = buttonHeight;
            this.reprintLastReceipt.BackColor = systemButtonsColor;
            this.reprintLastReceipt.Click += new EventHandler(ReprintLastReceipt);

            this.openDrawer = new UniformButton();
            this.openDrawer.Parent = this.mainWindow;
            this.openDrawer.Text = "Open Drawer";
            this.openDrawer.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            this.openDrawer.Width = buttonWidth;
            this.openDrawer.Height = buttonHeight;
            this.openDrawer.BackColor = systemButtonsColor;
            this.openDrawer.Click += new EventHandler(OpenDrawer);

            b++;

            UniformButton selPrinter = new UniformButton();
            selPrinter.Parent = this.mainWindow;
            selPrinter.Text = "Select Printer";
            selPrinter.Location = new Point(systemButtonsLeft, systemButtonsTop + (buttonVSpace * b++));
            selPrinter.Width = buttonWidth;
            selPrinter.Height = buttonHeight;
            selPrinter.BackColor = systemButtonsColor;
            selPrinter.Click += new EventHandler(SelPrinter);

            this.buttonManager.Setup();
            DiscountManager.Setup();

            SetButtonState(ButtonStates.INITIALIZING);
        }

        private void StartPurchase(object sender, EventArgs e)
        {
            Purchase.Reset();
            this.tapeDisplay.Clear();
            SetButtonState(ButtonStates.STARTED);
        }

        private void EditPurchase(object sender, EventArgs e)
        {
            EditItemsDialog dlg = new EditItemsDialog(Purchase.GetMerchandise());
            dlg.StartPosition = FormStartPosition.CenterParent;
            DialogResult dr = dlg.ShowDialog();
            if (dr == DialogResult.OK)
            {
                foreach (string item in dlg.DeleteTheseItems)
                {
                    Purchase.DeleteMerchandise(item);
                }
            }

            if (Purchase.IsPurchaseEmpty())
            {
                Purchase.Reset();
                this.tapeDisplay.Clear();
                SetButtonState(ButtonStates.IDLE);
            }
            else
            {
                this.tapeDisplay.PurchaseUpdated();
            }
        }

        private void TenderCash(object sender, EventArgs e)
        {
            SetButtonState(ButtonStates.TENDERING);

            TenderDialog dlg = new TenderDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.CashValidator = Purchase.TotalDue();
            DialogResult dr = dlg.ShowDialog(this.mainWindow);
            if (dr == DialogResult.OK)
            {
                this.printer.OpenDrawer();
                DateTime timeOfPurchase = DateTime.Now;
                this.printer.PrintReceipt(timeOfPurchase, "cash", "", dlg.TenderedCash);

                Purchase.WriteLogs(timeOfPurchase);

                ChangeDue change = new ChangeDue(dlg.TenderedCash - Purchase.TotalDue());
                change.StartPosition = FormStartPosition.CenterParent;
                dr = change.ShowDialog(this.mainWindow);

                this.tapeDisplay.Clear();
                SetButtonState(ButtonStates.IDLE);
            }
            else
            {
                SetButtonState(ButtonStates.INPROGRESS);
            }
        }

        private void TenderCheck(object sender, EventArgs e)
        {
            SetButtonState(ButtonStates.TENDERING);

            TwoTenderDialog dlg = new TwoTenderDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.AmountValidator = Purchase.TotalDue();
            DialogResult dr = dlg.ShowDialog(this.mainWindow);
            if (dr == DialogResult.OK)
            {
                this.printer.OpenDrawer();
                DateTime timeOfPurchase = DateTime.Now;
                this.printer.PrintReceipt(timeOfPurchase, "check", dlg.OtherData, dlg.TenderedAmount);

                Purchase.WriteLogs(timeOfPurchase);

                ChangeDue change = new ChangeDue(dlg.TenderedAmount - Purchase.TotalDue());
                change.StartPosition = FormStartPosition.CenterParent;
                dr = change.ShowDialog(this.mainWindow);

                this.tapeDisplay.Clear();
                SetButtonState(ButtonStates.IDLE);
            }
            else
            {
                SetButtonState(ButtonStates.INPROGRESS);
            }
        }

        private void TenderCredit(object sender, EventArgs e)
        {
            SetButtonState(ButtonStates.TENDERING);

            TenderDialog dlg = new TenderDialog();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.CashValidator = 0.00m;
            dlg.Label = "Auth #";
            DialogResult dr = dlg.ShowDialog(this.mainWindow);
            if (dr == DialogResult.OK)
            {
                this.printer.OpenDrawer();
                DateTime timeOfPurchase = DateTime.Now;
                this.printer.PrintReceipt(timeOfPurchase, "credit", dlg.OtherData, Purchase.TotalDue());

                Purchase.WriteLogs(timeOfPurchase);

                this.tapeDisplay.Clear();
                SetButtonState(ButtonStates.IDLE);
            }
            else
            {
                SetButtonState(ButtonStates.INPROGRESS);
            }
        }

        private void Cut(object sender, EventArgs e)
        {
            this.printer.Cut();
            // Do not change button state
        }

        private void NoSale(object sender, EventArgs e)
        {
            this.tapeDisplay.Clear();
            Purchase.Reset();
            SetButtonState(ButtonStates.IDLE);
        }

        private void ReprintLastReceipt(object sender, EventArgs e)
        {
            this.printer.Reprint();
            // Do not change button state
        }

        private void OpenDrawer(object sender, EventArgs e)
        {
            this.printer.OpenDrawer();
            // Do not change button state
        }

        private void SelPrinter(object sender, EventArgs e)
        {
            this.printerSelected = this.printer.selectPrinter(this.mainWindow);
            if (this.printerSelected)
            {
                SetButtonState(ButtonStates.IDLE);
            }
        }

        public delegate void UpdateDelegate();

        public void MerchandiseAdded()
        {
            this.tapeDisplay.PurchaseUpdated();
            SetButtonState(ButtonStates.INPROGRESS);
        }

        private Form mainWindow;
        private Form pos;
        private TapeDisplay tapeDisplay;
        private Printer printer;

        public const Int32 buttonWidth = 120;
        public const Int32 buttonHSpace = buttonWidth + 20;
        public const Int32 buttonHeight = 50;
        public const Int32 buttonVSpace = buttonHeight + 10;

        private bool printerSelected = false;

        private UniformButton startPurchase, editPurchase, tenderCash, tenderCheck, tenderCredit, cut, noSale, openDrawer, reprintLastReceipt;

        private enum ButtonStates { INITIALIZING, IDLE, STARTED, INPROGRESS, TENDERING };
        private ButtonStates currentState;
        private ItemButtonManager buttonManager;

        private void SetButtonState(ButtonStates newState)
        {
            this.currentState = newState;

            switch (this.currentState)
            {
                case ButtonStates.INITIALIZING:
                    this.startPurchase.Enabled = false;
                    this.editPurchase.Enabled = false;
                    this.tenderCash.Enabled = false;
                    this.tenderCheck.Enabled = false;
                    this.tenderCredit.Enabled = false;
                    this.cut.Enabled = false;
                    this.noSale.Enabled = false;
                    this.openDrawer.Enabled = false;
                    this.reprintLastReceipt.Enabled = false;
                    this.buttonManager.EnableMerchButtons(false);
                    break;

                case ButtonStates.IDLE:
                    this.startPurchase.Enabled = true;
                    this.editPurchase.Enabled = false;
                    this.tenderCash.Enabled = false;
                    this.tenderCheck.Enabled = false;
                    this.tenderCredit.Enabled = false;
                    this.cut.Enabled = true;
                    this.noSale.Enabled = false;
                    this.openDrawer.Enabled = true;
                    this.reprintLastReceipt.Enabled = true;
                    this.buttonManager.EnableMerchButtons(false);
                    break;

                case ButtonStates.STARTED:
                    this.startPurchase.Enabled = false;
                    this.editPurchase.Enabled = false;
                    this.tenderCash.Enabled = false;
                    this.tenderCheck.Enabled = false;
                    this.tenderCredit.Enabled = false;
                    this.cut.Enabled = true;
                    this.noSale.Enabled = true;
                    this.openDrawer.Enabled = true;
                    this.reprintLastReceipt.Enabled = false;
                    this.buttonManager.EnableMerchButtons(true);
                    break;

                case ButtonStates.INPROGRESS:
                    this.startPurchase.Enabled = false;
                    this.editPurchase.Enabled = true;
                    this.tenderCash.Enabled = true;
                    this.tenderCheck.Enabled = true;
                    this.tenderCredit.Enabled = true;
                    this.cut.Enabled = true;
                    this.noSale.Enabled = true;
                    this.openDrawer.Enabled = true;
                    this.reprintLastReceipt.Enabled = false;
                    this.buttonManager.EnableMerchButtons(true);
                    break;

                case ButtonStates.TENDERING:
                    this.startPurchase.Enabled = false;
                    this.editPurchase.Enabled = false;
                    this.tenderCash.Enabled = false;
                    this.tenderCheck.Enabled = false;
                    this.tenderCredit.Enabled = false;
                    this.cut.Enabled = true;
                    this.noSale.Enabled = false;
                    this.openDrawer.Enabled = true;
                    this.reprintLastReceipt.Enabled = false;
                    this.buttonManager.EnableMerchButtons(false);
                    break;
            }
        }
    }
}
