using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;


namespace CashRegister
{
    static class Startup
    {
        static void Main()
        {
            UIHandler main = new UIHandler();
            main.Setup();

            Purchase.Reset();

            main.Run();
        }
    }
}
