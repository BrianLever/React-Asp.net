namespace FrontDesk.Deploy.Server.Actions.WizardSteps
{
    partial class EnableKiosksAuthentificationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rbAnonymousAccess = new System.Windows.Forms.RadioButton();
            this.rbEnableAuthentification = new System.Windows.Forms.RadioButton();
            this.pnlCertificatePath = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txbCertificatePath = new System.Windows.Forms.TextBox();
            this.folderDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlCertificatePath.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbAnonymousAccess
            // 
            this.rbAnonymousAccess.AutoSize = true;
            this.rbAnonymousAccess.Location = new System.Drawing.Point(50, 172);
            this.rbAnonymousAccess.Name = "rbAnonymousAccess";
            this.rbAnonymousAccess.Size = new System.Drawing.Size(163, 17);
            this.rbAnonymousAccess.TabIndex = 20;
            this.rbAnonymousAccess.TabStop = true;
            this.rbAnonymousAccess.Text = "No, allow anonymous access";
            this.rbAnonymousAccess.UseVisualStyleBackColor = true;
            // 
            // rbEnableAuthentification
            // 
            this.rbEnableAuthentification.AutoSize = true;
            this.rbEnableAuthentification.Checked = true;
            this.rbEnableAuthentification.Location = new System.Drawing.Point(50, 50);
            this.rbEnableAuthentification.Name = "rbEnableAuthentification";
            this.rbEnableAuthentification.Size = new System.Drawing.Size(325, 17);
            this.rbEnableAuthentification.TabIndex = 19;
            this.rbEnableAuthentification.TabStop = true;
            this.rbEnableAuthentification.Text = "Yes, enable kiosk authentification on the server (recommended)";
            this.rbEnableAuthentification.UseVisualStyleBackColor = true;
            this.rbEnableAuthentification.CheckedChanged += new System.EventHandler(this.rbEnableAuthentification_CheckedChanged);
            // 
            // pnlCertificatePath
            // 
            this.pnlCertificatePath.AutoSize = true;
            this.pnlCertificatePath.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlCertificatePath.Controls.Add(this.label2);
            this.pnlCertificatePath.Controls.Add(this.label1);
            this.pnlCertificatePath.Controls.Add(this.btnBrowse);
            this.pnlCertificatePath.Controls.Add(this.txbCertificatePath);
            this.pnlCertificatePath.Location = new System.Drawing.Point(89, 73);
            this.pnlCertificatePath.Name = "pnlCertificatePath";
            this.pnlCertificatePath.Size = new System.Drawing.Size(373, 93);
            this.pnlCertificatePath.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 44);
            this.label2.TabIndex = 3;
            this.label2.Text = "Setup application will generate \"Screendox Root Authority.pfx\" certificate in the" +
                " selected folder. You will be required this file when installing Screendox kiosk" +
                " application.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Certificate authority folder:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(295, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txbCertificatePath
            // 
            this.txbCertificatePath.Location = new System.Drawing.Point(6, 26);
            this.txbCertificatePath.Name = "txbCertificatePath";
            this.txbCertificatePath.Size = new System.Drawing.Size(280, 20);
            this.txbCertificatePath.TabIndex = 0;
            // 
            // folderDlg
            // 
            this.folderDlg.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(361, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Specify whether Screendox Screener Server requires kiosk authentification:";
            // 
            // EnableKiosksAuthentificationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbAnonymousAccess);
            this.Controls.Add(this.rbEnableAuthentification);
            this.Controls.Add(this.pnlCertificatePath);
            this.Name = "EnableKiosksAuthentificationControl";
            this.Padding = new System.Windows.Forms.Padding(20, 20, 0, 0);
            this.Size = new System.Drawing.Size(465, 192);
            this.pnlCertificatePath.ResumeLayout(false);
            this.pnlCertificatePath.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbAnonymousAccess;
        private System.Windows.Forms.RadioButton rbEnableAuthentification;
        private System.Windows.Forms.Panel pnlCertificatePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txbCertificatePath;
        private System.Windows.Forms.FolderBrowserDialog folderDlg;
        private System.Windows.Forms.Label label3;
    }
}
