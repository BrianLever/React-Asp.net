namespace FrontDeskKioskUILocker
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbDisableAutoLogin = new System.Windows.Forms.RadioButton();
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbEnableAutoLogin = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbForAll = new System.Windows.Forms.RadioButton();
            this.rbForCurrentOnly = new System.Windows.Forms.RadioButton();
            this.rbStansard = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCtrlAltDel = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.pnlLogin.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbDisableAutoLogin);
            this.groupBox1.Controls.Add(this.pnlLogin);
            this.groupBox1.Controls.Add(this.rbEnableAutoLogin);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(520, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto-login settings on windows start-up";
            // 
            // rbDisableAutoLogin
            // 
            this.rbDisableAutoLogin.AutoSize = true;
            this.rbDisableAutoLogin.Location = new System.Drawing.Point(20, 175);
            this.rbDisableAutoLogin.Name = "rbDisableAutoLogin";
            this.rbDisableAutoLogin.Size = new System.Drawing.Size(109, 17);
            this.rbDisableAutoLogin.TabIndex = 2;
            this.rbDisableAutoLogin.Text = "Disable auto-login";
            this.rbDisableAutoLogin.UseVisualStyleBackColor = true;
            // 
            // pnlLogin
            // 
            this.pnlLogin.Controls.Add(this.label4);
            this.pnlLogin.Controls.Add(this.txtConfirmPassword);
            this.pnlLogin.Controls.Add(this.label3);
            this.pnlLogin.Controls.Add(this.txtPassword);
            this.pnlLogin.Controls.Add(this.label2);
            this.pnlLogin.Controls.Add(this.txtUserName);
            this.pnlLogin.Controls.Add(this.label1);
            this.pnlLogin.Location = new System.Drawing.Point(11, 44);
            this.pnlLogin.Name = "pnlLogin";
            this.pnlLogin.Size = new System.Drawing.Size(499, 125);
            this.pnlLogin.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(129, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(355, 37);
            this.label4.TabIndex = 6;
            this.label4.Text = "Enter the user name specifying the domain (ie, DomainName\\UserName) to automatica" +
    "lly logon under domain user account";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Location = new System.Drawing.Point(129, 96);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(355, 20);
            this.txtConfirmPassword.TabIndex = 5;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Confirm Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(129, 70);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(355, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(129, 7);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(355, 20);
            this.txtUserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "User name:";
            // 
            // rbEnableAutoLogin
            // 
            this.rbEnableAutoLogin.AutoSize = true;
            this.rbEnableAutoLogin.Checked = true;
            this.rbEnableAutoLogin.Location = new System.Drawing.Point(22, 20);
            this.rbEnableAutoLogin.Name = "rbEnableAutoLogin";
            this.rbEnableAutoLogin.Size = new System.Drawing.Size(107, 17);
            this.rbEnableAutoLogin.TabIndex = 0;
            this.rbEnableAutoLogin.TabStop = true;
            this.rbEnableAutoLogin.Text = "Enable auto-login";
            this.rbEnableAutoLogin.UseVisualStyleBackColor = true;
            this.rbEnableAutoLogin.CheckedChanged += new System.EventHandler(this.rbEnableAutoLogin_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbForAll);
            this.groupBox2.Controls.Add(this.rbForCurrentOnly);
            this.groupBox2.Controls.Add(this.rbStansard);
            this.groupBox2.Location = new System.Drawing.Point(8, 209);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(520, 94);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Windows shell replacement";
            // 
            // rbForAll
            // 
            this.rbForAll.AutoSize = true;
            this.rbForAll.Location = new System.Drawing.Point(22, 66);
            this.rbForAll.Name = "rbForAll";
            this.rbForAll.Size = new System.Drawing.Size(303, 17);
            this.rbForAll.TabIndex = 2;
            this.rbForAll.TabStop = true;
            this.rbForAll.Text = "Make ScreenDox Kiosk a shell only for ALL Users Accounts";
            this.rbForAll.UseVisualStyleBackColor = true;
            // 
            // rbForCurrentOnly
            // 
            this.rbForCurrentOnly.AutoSize = true;
            this.rbForCurrentOnly.Location = new System.Drawing.Point(22, 43);
            this.rbForCurrentOnly.Name = "rbForCurrentOnly";
            this.rbForCurrentOnly.Size = new System.Drawing.Size(386, 17);
            this.rbForCurrentOnly.TabIndex = 1;
            this.rbForCurrentOnly.TabStop = true;
            this.rbForCurrentOnly.Text = "Make Screendox Kiosk a shell only for CURRENTLY logged-in User Account";
            this.rbForCurrentOnly.UseVisualStyleBackColor = true;
            // 
            // rbStansard
            // 
            this.rbStansard.AutoSize = true;
            this.rbStansard.Checked = true;
            this.rbStansard.Location = new System.Drawing.Point(22, 20);
            this.rbStansard.Name = "rbStansard";
            this.rbStansard.Size = new System.Drawing.Size(225, 17);
            this.rbStansard.TabIndex = 0;
            this.rbStansard.TabStop = true;
            this.rbStansard.Text = "Use standard Windows shell - explorer.exe";
            this.rbStansard.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(453, 409);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(372, 409);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkCtrlAltDel);
            this.groupBox3.Location = new System.Drawing.Point(8, 309);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(520, 94);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Windows Hotkey management";
            // 
            // chkCtrlAltDel
            // 
            this.chkCtrlAltDel.AutoSize = true;
            this.chkCtrlAltDel.Checked = true;
            this.chkCtrlAltDel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCtrlAltDel.Location = new System.Drawing.Point(22, 20);
            this.chkCtrlAltDel.Name = "chkCtrlAltDel";
            this.chkCtrlAltDel.Size = new System.Drawing.Size(81, 17);
            this.chkCtrlAltDel.TabIndex = 3;
            this.chkCtrlAltDel.Text = "Ctrl+Alt+Del";
            this.chkCtrlAltDel.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 441);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenDox Kiosk Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlLogin.ResumeLayout(false);
            this.pnlLogin.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbEnableAutoLogin;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbDisableAutoLogin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbStansard;
        private System.Windows.Forms.RadioButton rbForCurrentOnly;
        private System.Windows.Forms.RadioButton rbForAll;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCtrlAltDel;
    }
}

