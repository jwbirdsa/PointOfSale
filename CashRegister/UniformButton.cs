using System;
using System.Windows.Forms;

namespace CashRegister
{
    class UniformButton : Button
    {
        static System.Media.SoundPlayer beeper = new System.Media.SoundPlayer(".\\beep.wav");

        public UniformButton()
            : base()
        {
            this.Font = new System.Drawing.Font(this.Font.FontFamily, this.Font.Size * 1.4f, System.Drawing.FontStyle.Bold);
            this.Click += new EventHandler(MakeSound);
        }

        private void MakeSound(object sender, EventArgs e)
        {
            UniformButton.beeper.Play();
        }
    }
}
