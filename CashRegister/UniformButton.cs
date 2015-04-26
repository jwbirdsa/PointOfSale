using System;
using System.Windows.Forms;

namespace CashRegister
{
    class UniformButton : Button
    {
        public UniformButton()
            : base()
        {
            this.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size * 1.4f, System.Drawing.FontStyle.Bold);
        }
    }
}
