﻿namespace CashRegister
{
    partial class MiscItemDialog
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
            this.tPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lTaxable = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(151, 323);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(70, 323);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // tDetails
            // 
            this.tDetails.Location = new System.Drawing.Point(12, 249);
            this.tDetails.Multiline = true;
            this.tDetails.Name = "tDetails";
            this.tDetails.Size = new System.Drawing.Size(268, 67);
            this.tDetails.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 233);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "details";
            // 
            // lRevenueGroup
            // 
            this.lRevenueGroup.FormattingEnabled = true;
            this.lRevenueGroup.Location = new System.Drawing.Point(12, 130);
            this.lRevenueGroup.Name = "lRevenueGroup";
            this.lRevenueGroup.Size = new System.Drawing.Size(268, 95);
            this.lRevenueGroup.TabIndex = 4;
            this.lRevenueGroup.SelectedIndexChanged += new System.EventHandler(this.lRevenueGroup_SelectedIndexChanged);
            // 
            // tPrice
            // 
            this.tPrice.Location = new System.Drawing.Point(95, 8);
            this.tPrice.Name = "tPrice";
            this.tPrice.Size = new System.Drawing.Size(100, 20);
            this.tPrice.TabIndex = 1;
            this.tPrice.TextChanged += new System.EventHandler(this.tPrice_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "price/discount";
            // 
            // lTaxable
            // 
            this.lTaxable.FormattingEnabled = true;
            this.lTaxable.Location = new System.Drawing.Point(12, 50);
            this.lTaxable.Name = "lTaxable";
            this.lTaxable.Size = new System.Drawing.Size(268, 69);
            this.lTaxable.TabIndex = 3;
            this.lTaxable.SelectedIndexChanged += new System.EventHandler(this.lTaxable_SelectedIndexChanged);
            // 
            // MiscItemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 357);
            this.Controls.Add(this.lTaxable);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tDetails);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lRevenueGroup);
            this.Controls.Add(this.tPrice);
            this.Controls.Add(this.label1);
            this.Name = "MiscItemDialog";
            this.Text = "MiscItemDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.TextBox tDetails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lRevenueGroup;
        private System.Windows.Forms.TextBox tPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lTaxable;
    }
}