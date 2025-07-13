namespace DVLD_Project.Small_Forms
{
    partial class frm_UpdateAddNewUser
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_UpdateAddNewUser));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lbl_Update_Addnew_Subject = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tcPersonalInfo = new System.Windows.Forms.TabPage();
            this.btnNext = new System.Windows.Forms.Button();
            this.tcUserInfo = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.mbConfirmPassword = new System.Windows.Forms.MaskedTextBox();
            this.mbPassword = new System.Windows.Forms.MaskedTextBox();
            this.cbIsActive = new System.Windows.Forms.CheckBox();
            this.lblUserID = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ctrFindPerson1 = new DVLD_Project.User_Controls.ctrFindPerson();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tcPersonalInfo.SuspendLayout();
            this.tcUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lbl_Update_Addnew_Subject
            // 
            this.lbl_Update_Addnew_Subject.AutoSize = true;
            this.lbl_Update_Addnew_Subject.Font = new System.Drawing.Font("Mongolian Baiti", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Update_Addnew_Subject.ForeColor = System.Drawing.Color.Red;
            this.lbl_Update_Addnew_Subject.Location = new System.Drawing.Point(282, 28);
            this.lbl_Update_Addnew_Subject.Name = "lbl_Update_Addnew_Subject";
            this.lbl_Update_Addnew_Subject.Size = new System.Drawing.Size(208, 31);
            this.lbl_Update_Addnew_Subject.TabIndex = 35;
            this.lbl_Update_Addnew_Subject.Text = "Add New User";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.Location = new System.Drawing.Point(589, 599);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 39);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tcPersonalInfo);
            this.tabControl1.Controls.Add(this.tcUserInfo);
            this.tabControl1.Location = new System.Drawing.Point(8, 71);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(706, 503);
            this.tabControl1.TabIndex = 37;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tcPersonalInfo
            // 
            this.tcPersonalInfo.Controls.Add(this.ctrFindPerson1);
            this.tcPersonalInfo.Controls.Add(this.btnNext);
            this.tcPersonalInfo.Location = new System.Drawing.Point(4, 22);
            this.tcPersonalInfo.Name = "tcPersonalInfo";
            this.tcPersonalInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tcPersonalInfo.Size = new System.Drawing.Size(698, 477);
            this.tcPersonalInfo.TabIndex = 0;
            this.tcPersonalInfo.Text = "Personal Info";
            this.tcPersonalInfo.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNext.Location = new System.Drawing.Point(575, 431);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(114, 39);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // tcUserInfo
            // 
            this.tcUserInfo.Controls.Add(this.btnSave);
            this.tcUserInfo.Controls.Add(this.btnPrev);
            this.tcUserInfo.Controls.Add(this.mbConfirmPassword);
            this.tcUserInfo.Controls.Add(this.mbPassword);
            this.tcUserInfo.Controls.Add(this.cbIsActive);
            this.tcUserInfo.Controls.Add(this.lblUserID);
            this.tcUserInfo.Controls.Add(this.pictureBox3);
            this.tcUserInfo.Controls.Add(this.label3);
            this.tcUserInfo.Controls.Add(this.pictureBox2);
            this.tcUserInfo.Controls.Add(this.label2);
            this.tcUserInfo.Controls.Add(this.pictureBox1);
            this.tcUserInfo.Controls.Add(this.tbUserName);
            this.tcUserInfo.Controls.Add(this.label1);
            this.tcUserInfo.Controls.Add(this.pictureBox5);
            this.tcUserInfo.Controls.Add(this.label6);
            this.tcUserInfo.Location = new System.Drawing.Point(4, 22);
            this.tcUserInfo.Name = "tcUserInfo";
            this.tcUserInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tcUserInfo.Size = new System.Drawing.Size(698, 477);
            this.tcUserInfo.TabIndex = 1;
            this.tcUserInfo.Text = "User Info";
            this.tcUserInfo.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(562, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 39);
            this.btnSave.TabIndex = 89;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Mongolian Baiti", 11F);
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrev.Location = new System.Drawing.Point(429, 432);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(114, 39);
            this.btnPrev.TabIndex = 88;
            this.btnPrev.Text = "Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // mbConfirmPassword
            // 
            this.mbConfirmPassword.Location = new System.Drawing.Point(221, 173);
            this.mbConfirmPassword.Name = "mbConfirmPassword";
            this.mbConfirmPassword.PasswordChar = '*';
            this.mbConfirmPassword.Size = new System.Drawing.Size(97, 20);
            this.mbConfirmPassword.TabIndex = 87;
            this.mbConfirmPassword.Leave += new System.EventHandler(this.mbConfirmPassword_Leave);
            // 
            // mbPassword
            // 
            this.mbPassword.Location = new System.Drawing.Point(221, 132);
            this.mbPassword.Name = "mbPassword";
            this.mbPassword.PasswordChar = '*';
            this.mbPassword.Size = new System.Drawing.Size(97, 20);
            this.mbPassword.TabIndex = 86;
            // 
            // cbIsActive
            // 
            this.cbIsActive.AutoSize = true;
            this.cbIsActive.Font = new System.Drawing.Font("Mongolian Baiti", 9.5F, System.Drawing.FontStyle.Bold);
            this.cbIsActive.Location = new System.Drawing.Point(195, 216);
            this.cbIsActive.Name = "cbIsActive";
            this.cbIsActive.Size = new System.Drawing.Size(83, 18);
            this.cbIsActive.TabIndex = 85;
            this.cbIsActive.Text = "Is Active";
            this.cbIsActive.UseVisualStyleBackColor = true;
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserID.Location = new System.Drawing.Point(231, 51);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(43, 16);
            this.lblUserID.TabIndex = 84;
            this.lblUserID.Text = "[???]";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(195, 172);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(20, 20);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 83;
            this.pictureBox3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(35, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 16);
            this.label3.TabIndex = 81;
            this.label3.Text = "Confirm Password: ";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(195, 131);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 80;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(35, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 78;
            this.label2.Text = "Password: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(195, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 77;
            this.pictureBox1.TabStop = false;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(221, 90);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(97, 20);
            this.tbUserName.TabIndex = 76;
            this.tbUserName.TextChanged += new System.EventHandler(this.tbUserName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 16);
            this.label1.TabIndex = 75;
            this.label1.Text = "User PersonID: ";
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(195, 49);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(20, 20);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 74;
            this.pictureBox5.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(35, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 16);
            this.label6.TabIndex = 46;
            this.label6.Text = "User Name: ";
            // 
            // ctrFindPerson1
            // 
            this.ctrFindPerson1.FilterEnabled = true;
            this.ctrFindPerson1.Location = new System.Drawing.Point(1, 13);
            this.ctrFindPerson1.Name = "ctrFindPerson1";
            this.ctrFindPerson1.PersonID = 0;
            this.ctrFindPerson1.ShowAddPerson = true;
            this.ctrFindPerson1.Size = new System.Drawing.Size(692, 412);
            this.ctrFindPerson1.TabIndex = 1;
            this.ctrFindPerson1.OnPersonSelected += new System.Action<int>(this.ctrFindPerson1_OnPersonSelected);
            // 
            // frm_UpdateAddNewUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(723, 645);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbl_Update_Addnew_Subject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_UpdateAddNewUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmAddNewUser";
            this.Load += new System.EventHandler(this.frmAddNewUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tcPersonalInfo.ResumeLayout(false);
            this.tcUserInfo.ResumeLayout(false);
            this.tcUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lbl_Update_Addnew_Subject;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tcPersonalInfo;
        private User_Controls.ctrFindPerson ctrFindPerson1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TabPage tcUserInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.MaskedTextBox mbConfirmPassword;
        private System.Windows.Forms.MaskedTextBox mbPassword;
        private System.Windows.Forms.CheckBox cbIsActive;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label label6;
    }
}