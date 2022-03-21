namespace FrontDesk.Deploy.Server.Actions
{
    partial class EnableKiosksAuthenticationForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlCertificatePath = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txbCertificatePath = new System.Windows.Forms.TextBox();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbAnonymousAccess = new System.Windows.Forms.RadioButton();
            this.rbEnableAuthentification = new System.Windows.Forms.RadioButton();
            this.pnlCertificatePath.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Image = global::FrontDesk.Deploy.Server.Actions.Properties.Resources.fd_install_banner;
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTitle.Location = new System.Drawing.Point(-3, -1);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.lblTitle.Size = new System.Drawing.Size(497, 68);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Enable Kiosks Authentication";
            // 
            // pnlCertificatePath
            // 
            this.pnlCertificatePath.Controls.Add(this.label2);
            this.pnlCertificatePath.Controls.Add(this.label1);
            this.pnlCertificatePath.Controls.Add(this.btnBrowse);
            this.pnlCertificatePath.Controls.Add(this.txbCertificatePath);
            this.pnlCertificatePath.Location = new System.Drawing.Point(44, 52);
            this.pnlCertificatePath.Name = "pnlCertificatePath";
            this.pnlCertificatePath.Size = new System.Drawing.Size(399, 97);
            this.pnlCertificatePath.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 33);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selected folder will contain a security certificate that will be required during " +
                "kiosks installation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Certificate destination folder:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(289, 29);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txbCertificatePath
            // 
            this.txbCertificatePath.Location = new System.Drawing.Point(3, 32);
            this.txbCertificatePath.Name = "txbCertificatePath";
            this.txbCertificatePath.Size = new System.Drawing.Size(280, 20);
            this.txbCertificatePath.TabIndex = 0;
            // 
            // folderDlg
            // 
            this.folderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(315, 335);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(396, 335);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 15;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbAnonymousAccess);
            this.panel1.Controls.Add(this.rbEnableAuthentification);
            this.panel1.Controls.Add(this.pnlCertificatePath);
            this.panel1.Location = new System.Drawing.Point(1, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 259);
            this.panel1.TabIndex = 17;
            // 
            // rbAnonymousAccess
            // 
            this.rbAnonymousAccess.AutoSize = true;
            this.rbAnonymousAccess.Location = new System.Drawing.Point(25, 170);
            this.rbAnonymousAccess.Name = "rbAnonymousAccess";
            this.rbAnonymousAccess.Size = new System.Drawing.Size(144, 17);
            this.rbAnonymousAccess.TabIndex = 17;
            this.rbAnonymousAccess.TabStop = true;
            this.rbAnonymousAccess.Text = "Allow anonymous access";
            this.rbAnonymousAccess.UseVisualStyleBackColor = true;
            // 
            // rbEnableAuthentification
            // 
            this.rbEnableAuthentification.AutoSize = true;
            this.rbEnableAuthentification.Checked = true;
            this.rbEnableAuthentification.Location = new System.Drawing.Point(25, 20);
            this.rbEnableAuthentification.Name = "rbEnableAuthentification";
            this.rbEnableAuthentification.Size = new System.Drawing.Size(302, 17);
            this.rbEnableAuthentification.TabIndex = 16;
            this.rbEnableAuthentification.TabStop = true;
            this.rbEnableAuthentification.Text = "Enable kiosk authentification on the server (recommended)";
            this.rbEnableAuthentification.UseVisualStyleBackColor = true;
            this.rbEnableAuthentification.CheckedChanged += new System.EventHandler(this.rbEnableAuthentification_CheckedChanged);
            // 
            // EnableKiosksAuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 370);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EnableKiosksAuthenticationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installing FrontDesk Screener Server";
            this.pnlCertificatePath.ResumeLayout(false);
            this.pnlCertificatePath.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlCertificatePath;
        private System.Windows.Forms.TextBox txbCertificatePath;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbEnableAuthentification;
        private System.Windows.Forms.RadioButton rbAnonymousAccess;

    }
}