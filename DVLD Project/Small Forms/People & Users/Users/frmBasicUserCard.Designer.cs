﻿namespace DVLD_Project.Small_Forms
{
    partial class frmBasicUserCard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBasicUserCard));
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrBaseUserCard1 = new DVLD_Project.User_Controls.ctrBaseUserCard();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.Location = new System.Drawing.Point(592, 413);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 39);
            this.btnClose.TabIndex = 37;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrBaseUserCard1
            // 
            this.ctrBaseUserCard1.Location = new System.Drawing.Point(0, -10);
            this.ctrBaseUserCard1.Name = "ctrBaseUserCard1";
            this.ctrBaseUserCard1.Size = new System.Drawing.Size(727, 435);
            this.ctrBaseUserCard1.TabIndex = 0;
            this.ctrBaseUserCard1.Load += new System.EventHandler(this.ctrBaseUserCard1_Load);
            // 
            // frmBasicUserCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 462);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrBaseUserCard1);
            this.Name = "frmBasicUserCard";
            this.Text = "frmBasicUserCard";
            this.ResumeLayout(false);

        }

        #endregion

        private User_Controls.ctrBaseUserCard ctrBaseUserCard1;
        private System.Windows.Forms.Button btnClose;
    }
}