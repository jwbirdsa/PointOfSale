using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.IO;

namespace CashRegister
{
    class Printer
    {
        public bool selectPrinter(Form parentForDialog)
        {
            // Allow the user to select a printer.
            PrintDialog pd  = new PrintDialog();
            pd.PrinterSettings = new PrinterSettings();
            if (DialogResult.OK == pd.ShowDialog(parentForDialog))
            {
                this.printerName = pd.PrinterSettings.PrinterName;
                //MessageBox.Show(this.printerName, "PrinterName set to");
                return true;
            }
            return false;
        }

        internal void PrintReceipt(DateTime timeOfPurchase, string tenderType, string tenderAdditional, decimal tenderAmount)
        {
            this.cachedJob = new PrintJob(this.printerName);

            // Print header
            Center("Jarlidium Press");
            Center("www.jarlidium.com");
            SkipLine();

            // Print items
            foreach (LineItem li in Purchase.GetForDisplay())
            {
                Left(li.Format());
            }

            // Print footer
            Right("TAX  " + Utilities.PercentageString(Utilities.GetTaxRate(), 1) + "%     " + Utilities.DollarsAndCentsString(Utilities.ComputeTax(Purchase.Taxable()), 4));
            Right("TOTAL          " + Utilities.DollarsAndCentsString(Purchase.TotalDue(), 4));
            Right("Tender " + tenderType.PadRight(6) + "  " + Utilities.DollarsAndCentsString(tenderAmount, 4));
            if (tenderType.Equals("check"))
            {
                Left("Check # " + tenderAdditional);
            }
            else if (tenderType.Equals("credit"))
            {
                Left("Auth " + tenderAdditional);
            }
            Right("Change due     " + Utilities.DollarsAndCentsString(tenderAmount - Purchase.TotalDue(), 4));
            SkipLine();
            Center("Thank You!");
            Center(timeOfPurchase.ToString());
            SkipLine();
            Left("ID " + Purchase.purchaseId.ToString());

            this.cachedJob.SendJob();
            Cut();
        }

        private void SkipLine()
        {
            this.cachedJob.AddText("\n");
        }

        private void Left(string s)
        {
            this.cachedJob.AddText(s + "\n");
        }

        private void Center(string s)
        {
            this.cachedJob.AddText(s.PadLeft(((PrintParams.lineChars - s.Length) / 2) + s.Length) + "\n");
        }

        private void Right(string s)
        {
            this.cachedJob.AddText(s.PadLeft(PrintParams.lineChars - 1) + "\n");
        }

        public void Cut()
        {
            RawPrinterHelper.SendStringToPrinter(this.printerName, PrintParams.ClearCutterHead);

            Byte[] sequence = new Byte[3];
            sequence[0] = 0x1B;
            sequence[1] = 0x64;
            sequence[2] = 0x30;
            RawPrinterHelper.SendByteArrayToPrinter(this.printerName, sequence);
        }

        public void OpenDrawer()
        {
            Byte[] sequence = new Byte[1];
            sequence[0] = 0x1C;
            RawPrinterHelper.SendByteArrayToPrinter(this.printerName, sequence);
        }

        public void Reprint()
        {
            if (this.cachedJob == null)
            {
                MessageBox.Show("There is no previous receipt to reprint");
            }
            else
            {
                this.cachedJob.SendJob();
                Cut();
            }
        }

        private string printerName;
        private PrintJob cachedJob = null;
    }


    public class PrintParams
    {
        public const Int16 lineChars = 40;
        public const Int16 dollarPositions = 3;
        public const Int16 qtyPositions = 3;
        public const Int16 priceChars = dollarPositions + 1 + 2; // dollars plus . plus cents
        public const Int16 extPriceChars = 2 + priceChars + 1; // two guard spaces, price, tax marker
        public const Int16 qtyAtPriceChars = 2 + qtyPositions + 1 + priceChars; // two guard spaces, three qty, @, price
        public const Int16 itemChars = lineChars - extPriceChars - qtyAtPriceChars;

        public const string taxincMarker = "i";
        public const string taxnonMarker = "n";

        public const string printTest = "1234567890123456789012345678901234567890";
        public static string ClearCutterHead = "\n\n\n\n\n\n";
    }


    internal class PrintJob
    {
        public PrintJob(string printerName)
        {
            this.accumulatedText = "";
            this.printerName = printerName;
        }

        public void AddText(string text)
        {
            this.accumulatedText += text;
        }

        public void SendJob()
        {
            RawPrinterHelper.SendStringToPrinter(this.printerName, this.accumulatedText);
        }

        private string accumulatedText;
        private string printerName;
    }


    internal class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        private static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
                MessageBox.Show("Error " + dwError.ToString());
            }

            return bSuccess;
        }

        public static bool SendByteArrayToPrinter(string szPrinterName, Byte[] bytes)
        {
            int size = Marshal.SizeOf(bytes[0]) * bytes.Length;
            IntPtr pBytes = Marshal.AllocHGlobal(size);

            Marshal.Copy(bytes, 0, pBytes, bytes.Length);

            RawPrinterHelper.SendBytesToPrinter(szPrinterName, pBytes, size);

            Marshal.FreeHGlobal(pBytes);

            return true;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }

}