namespace CashRegister
{
    partial class FixedBoxDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tDetails = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lRevenueGroup = new System.Windows.Forms.ListBox();
            this.tQuantity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(153, 242);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 14;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(72, 242);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 13;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // tDetails
            // 
            this.tDetails.Location = new System.Drawing.Point(14, 168);
            this.tDetails.Multiline = true;
            this.tDetails.Name = "tDetails";
            this.tDetails.Size = new System.Drawing.Size(268, 67);
            this.tDetails.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "details";
            // 
            // lRevenueGroup
            // 
            this.lRevenueGroup.FormattingEnabled = true;
            this.lRevenueGroup.Location = new System.Drawing.Point(14, 54);
            this.lRevenueGroup.Name = "lRevenueGroup";
            this.lRevenueGroup.Size = new System.Drawing.Size(268, 95);
            this.lRevenueGroup.TabIndex = 10;
            this.lRevenueGroup.SelectedIndexChanged += new System.EventHandler(this.lRevenueGroup_SelectedIndexChanged);
            // 
            // tQuantity
            // 
            this.tQuantity.Location = new System.Drawing.Point(75, 8);
            this.tQuantity.Name = "tQuantity";
            this.tQuantity.Size = new System.Drawing.Size(100, 20);
            this.tQuantity.TabIndex = 9;
            this.tQuantity.TextChanged += new System.EventHandler(this.tQuantity_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "quantity";
            // 
            // FixedBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tDetails);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lRevenueGroup);
            this.Controls.Add(this.tQuantity);
            this.Controls.Add(this.label1);
            this.Name = "FixedBoxDialog";
            this.Text = "Fixed Price Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.TextBox tDetails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lRevenueGroup;
        private System.Windows.Forms.TextBox tQuantity;
        private System.Windows.Forms.Label label1;
    }
}