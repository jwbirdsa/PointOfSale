using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;


namespace CashRegister
{
    class TapeDisplay
    {
        public TapeDisplay()
        {
        }

        public void Setup(Form mainWindow, Form posWindow)
        {
            SetupMain(mainWindow);
            SetupPos(posWindow);
        }

        public void SetupMain(Form mainWindow)
        {
            // Create the tape itself first so we can get the width
            this.tape = new Label();
            this.tape.Parent = mainWindow;
            this.tape.BackColor = Color.White;
            this.tape.Font = new Font(FontFamily.GenericMonospace, 10f);

            Graphics g = this.tape.CreateGraphics();
            Int32 neededWidth = (Int32)g.MeasureString(PrintParams.printTest + "X", this.tape.Font).Width;
            this.MainWidth = neededWidth;
            g.Dispose();

            Int32 leftSide = mainWindow.ClientRectangle.Right - neededWidth - 10;
            this.tape.Location = new Point(leftSide, 10);
            this.tape.Width = neededWidth;
            this.tape.Height = mainWindow.ClientRectangle.Bottom - 10 - 140;
            this.tape.Text = PrintParams.printTest;

            // Create the total display at the bottom
            this.totalShow = new Label();
            this.totalShow.Parent = mainWindow;
            this.totalShow.ForeColor = Color.Indigo;
            this.totalShow.Font = new Font(FontFamily.GenericMonospace, 27f, FontStyle.Bold);

            g = this.totalShow.CreateGraphics();
            Int32 dollarsHeight = (Int32)g.MeasureString("00000.00", this.totalShow.Font).Height;
            Int32 dollarsWidth = (Int32)g.MeasureString("00000.00", this.totalShow.Font).Width;
            g.Dispose();
            this.totalShow.Location = new Point(leftSide + neededWidth - dollarsWidth, mainWindow.ClientRectangle.Bottom - 10 - dollarsHeight);
            this.totalShow.Width = dollarsWidth;
            this.totalShow.Height = dollarsHeight;
            this.totalShow.Text = "0.00";
            this.totalShow.TextAlign = ContentAlignment.TopRight;

            Label totalLabel = new Label();
            totalLabel.Parent = mainWindow;
            totalLabel.ForeColor = this.totalShow.ForeColor;
            totalLabel.Font = new Font(FontFamily.GenericMonospace, 12f, FontStyle.Bold);
            totalLabel.Location = new Point(leftSide, this.totalShow.Top);
            totalLabel.Width = neededWidth - dollarsWidth;
            totalLabel.Height = dollarsHeight;
            totalLabel.Text = "total\n    $";

            // Create the tax display above the total display
            Label taxLabel = new Label();
            taxLabel.Parent = mainWindow;
            taxLabel.ForeColor = this.totalShow.ForeColor;
            taxLabel.Font = new Font(FontFamily.GenericMonospace, 12f);
            taxLabel.Location = new Point(leftSide, totalLabel.Top - dollarsHeight - 10);
            taxLabel.Width = neededWidth - dollarsWidth;
            taxLabel.Height = dollarsHeight;
            taxLabel.Text = "tax @\n" + Utilities.PercentageString(Utilities.GetTaxRate(), 1) + "%";

            this.taxShow = new Label();
            this.taxShow.Parent = mainWindow;
            this.taxShow.ForeColor = this.totalShow.ForeColor;
            this.taxShow.Font = new Font(FontFamily.GenericMonospace, 27f);
            this.taxShow.Location = new Point(leftSide + neededWidth - dollarsWidth, taxLabel.Top);
            this.taxShow.Width = dollarsWidth;
            this.taxShow.Height = dollarsHeight;
            this.taxShow.Text = "0.00";
            this.taxShow.TextAlign = ContentAlignment.TopRight;

            // Create the subtotal display above the tax display
            Label subLabel = new Label();
            subLabel.Parent = mainWindow;
            subLabel.ForeColor = this.totalShow.ForeColor;
            subLabel.Font = new Font(FontFamily.GenericMonospace, 12f);
            subLabel.Location = new Point(leftSide, taxLabel.Top - dollarsHeight - 10);
            subLabel.Width = neededWidth - dollarsWidth;
            subLabel.Height = dollarsHeight;
            subLabel.Text = "subtotal\n    $";

            this.subShow = new Label();
            this.subShow.Parent = mainWindow;
            this.subShow.ForeColor = this.totalShow.ForeColor;
            this.subShow.Font = new Font(FontFamily.GenericMonospace, 27f);
            this.subShow.Location = new Point(leftSide + neededWidth - dollarsWidth, subLabel.Top);
            this.subShow.Width = dollarsWidth;
            this.subShow.Height = dollarsHeight;
            this.subShow.Text = "0.00";
            this.subShow.TextAlign = ContentAlignment.TopRight;

            // Finally, fix up the height of the tape
            this.tape.Height = mainWindow.ClientRectangle.Bottom - 10 - dollarsHeight - 10 - dollarsHeight - 10 - dollarsHeight - 10;
        }

        public void SetupPos(Form posWindow)
        {
            // Create the tape itself first so we can get the width
            this.tape2 = new Label();
            this.tape2.Parent = posWindow;
            this.tape2.BackColor = Color.White;
            this.tape2.Font = new Font(FontFamily.GenericMonospace, 10f);

            Graphics g = this.tape2.CreateGraphics();
            Int32 neededWidth = (Int32)g.MeasureString(PrintParams.printTest + "X", this.tape2.Font).Width;
            g.Dispose();

            this.tape2.Location = new Point(posWindow.ClientRectangle.Right - neededWidth - 10, 10);
            this.tape2.Width = neededWidth;
            this.tape2.Height = posWindow.ClientRectangle.Bottom - 10 - 140;
            this.tape2.Text = PrintParams.printTest;

            // Create the total display at the bottom
            Int32 leftSide = 10;

            this.totalShow2 = new Label();
            this.totalShow2.Parent = posWindow;
            this.totalShow2.ForeColor = Color.Indigo;
            this.totalShow2.Font = new Font(FontFamily.GenericMonospace, 27f, FontStyle.Bold);

            g = this.totalShow2.CreateGraphics();
            Int32 dollarsHeight = (Int32)g.MeasureString("00000.00", this.totalShow2.Font).Height;
            Int32 dollarsWidth = (Int32)g.MeasureString("00000.00", this.totalShow2.Font).Width;
            g.Dispose();
            this.totalShow2.Location = new Point(leftSide, posWindow.ClientRectangle.Bottom - 10 - dollarsHeight);
            this.totalShow2.Width = dollarsWidth;
            this.totalShow2.Height = dollarsHeight;
            this.totalShow2.Text = "0.00";
            this.totalShow2.TextAlign = ContentAlignment.TopRight;

            Label totalLabel2 = new Label();
            totalLabel2.Parent = posWindow;
            totalLabel2.ForeColor = this.totalShow2.ForeColor;
            totalLabel2.Font = new Font(FontFamily.GenericMonospace, 12f, FontStyle.Bold);
            totalLabel2.Location = new Point(leftSide, this.totalShow2.Top - dollarsHeight);
            totalLabel2.Width = dollarsWidth;
            totalLabel2.Height = dollarsHeight;
            totalLabel2.Text = "\ntotal $";

            // Create the tax display above the total display
            this.taxShow2 = new Label();
            this.taxShow2.Parent = posWindow;
            this.taxShow2.ForeColor = this.totalShow2.ForeColor;
            this.taxShow2.Font = new Font(FontFamily.GenericMonospace, 27f);
            this.taxShow2.Location = new Point(leftSide, totalLabel2.Top - dollarsHeight);
            this.taxShow2.Width = dollarsWidth;
            this.taxShow2.Height = dollarsHeight;
            this.taxShow2.Text = "0.00";
            this.taxShow2.TextAlign = ContentAlignment.TopRight;

            Label taxLabel2 = new Label();
            taxLabel2.Parent = posWindow;
            taxLabel2.ForeColor = this.totalShow2.ForeColor;
            taxLabel2.Font = new Font(FontFamily.GenericMonospace, 12f);
            taxLabel2.Location = new Point(leftSide, this.taxShow2.Top - dollarsHeight);
            taxLabel2.Width = dollarsWidth;
            taxLabel2.Height = dollarsHeight;
            taxLabel2.Text = "\ntax @ " + Utilities.PercentageString(Utilities.GetTaxRate(), 1) + "%";

            // Create the subtotal display above the tax display
            this.subShow2 = new Label();
            this.subShow2.Parent = posWindow;
            this.subShow2.ForeColor = this.totalShow2.ForeColor;
            this.subShow2.Font = new Font(FontFamily.GenericMonospace, 27f);
            this.subShow2.Location = new Point(leftSide, taxLabel2.Top - dollarsHeight);
            this.subShow2.Width = dollarsWidth;
            this.subShow2.Height = dollarsHeight;
            this.subShow2.Text = "0.00";
            this.subShow2.TextAlign = ContentAlignment.TopRight;

            Label subLabel2 = new Label();
            subLabel2.Parent = posWindow;
            subLabel2.ForeColor = this.totalShow2.ForeColor;
            subLabel2.Font = new Font(FontFamily.GenericMonospace, 12f);
            subLabel2.Location = new Point(leftSide, this.subShow2.Top - dollarsHeight);
            subLabel2.Width = dollarsWidth;
            subLabel2.Height = dollarsHeight;
            subLabel2.Text = "\nsubtotal $";

            // Put up the logo
            PictureBox logoBox = new PictureBox();
            logoBox.Parent = posWindow;
            logoBox.BorderStyle = BorderStyle.FixedSingle;
            logoBox.Height = 140; // max seems to be about 140
            logoBox.Width = (int)(logoBox.Height * 0.627);
            logoBox.Location = new Point((posWindow.ClientRectangle.Width - this.tape2.Width - 88 - 10 - 10) / 2, 10);
            logoBox.Image = new Bitmap(".\\jarlogo.tif");
            logoBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoBox.Show();

            // Finally, fix up the height of the tape
            this.tape2.Height = posWindow.ClientRectangle.Bottom - 20;
        }

        public void PurchaseUpdated()
        {
            List<LineItem> items = Purchase.GetForDisplay();
            string tapeText = "";

            foreach (LineItem li in items)
            {
                tapeText += li.Format();
                tapeText += "\n";
            }
            this.tape.Text = tapeText;
            this.tape2.Text = tapeText;

            decimal subtotal = Purchase.Subtotal();
            this.subShow.Text = Utilities.DollarsAndCentsString(subtotal, 3);
            this.subShow2.Text = this.subShow.Text;

            decimal tax = Utilities.ComputeTax(Purchase.Taxable());
            this.taxShow.Text = Utilities.DollarsAndCentsString(tax, 3);
            this.taxShow2.Text = this.taxShow.Text;

            this.totalShow.Text = Utilities.DollarsAndCentsString(Purchase.TotalDue(), 4);
            this.totalShow2.Text = this.totalShow.Text;
        }

        public void Clear()
        {
            this.tape.Text = "";
            this.tape2.Text = "";
            this.subShow.Text = "0.00";
            this.subShow2.Text = "0.00";
            this.taxShow.Text = "0.00";
            this.taxShow2.Text = "0.00";
            this.totalShow.Text = "0.00";
            this.totalShow2.Text = "0.00";
        }

        public Int32 MainWidth { get; private set; }

        private Label tape;
        private Label subShow;
        private Label taxShow;
        private Label totalShow;
        private Label tape2;
        private Label subShow2;
        private Label taxShow2;
        private Label totalShow2;
    }
}