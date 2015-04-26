namespace CashRegister
{
    partial class VarBoxDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.tPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lRevenueGroup = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tDetails = new System.Windows.Forms.TextBox();
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "total price";
            // 
            // tPrice
            // 
            this.tPrice.Location = new System.Drawing.Point(73, 13);
            this.tPrice.Name = "tPrice";
            this.tPrice.Size = new System.Drawing.Size(100, 20);
            this.tPrice.TabIndex = 1;
            this.tPrice.TextChanged += new System.EventHandler(this.tPrice_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "revenue group";
            // 
            // lRevenueGroup
            // 
            this.lRevenueGroup.FormattingEnabled = true;
            this.lRevenueGroup.Location = new System.Drawing.Point(12, 59);
            this.lRevenueGroup.Name = "lRevenueGroup";
            this.lRevenueGroup.Size = new System.Drawing.Size(268, 95);
            this.lRevenueGroup.TabIndex = 3;
            this.lRevenueGroup.SelectedIndexChanged += new System.EventHandler(this.lRevenueGroup_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "details";
            // 
            // tDetails
            // 
            this.tDetails.Location = new System.Drawing.Point(12, 173);
            this.tDetails.Multiline = true;
            this.tDetails.Name = "tDetails";
            this.tDetails.Size = new System.Drawing.Size(268, 67);
            this.tDetails.TabIndex = 5;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(70, 247);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 6;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(151, 247);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 7;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // VarBoxDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 281);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tDetails);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lRevenueGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tPrice);
            this.Controls.Add(this.label1);
            this.Name = "VarBoxDialog";
            this.Text = "Variable Price Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lRevenueGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tDetails;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
    }
}