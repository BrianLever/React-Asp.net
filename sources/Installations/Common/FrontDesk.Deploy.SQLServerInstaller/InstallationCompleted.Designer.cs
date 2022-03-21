namespace FrontDesk.Deployment.SQLServerInstaller
{
    partial class InstallationCompleted
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
            this.lblSuccessText = new System.Windows.Forms.Label();
            this.lblErrorText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblSuccessText
            // 
            this.lblSuccessText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSuccessText.Location = new System.Drawing.Point(0, 0);
            this.lblSuccessText.Name = "lblSuccessText";
            this.lblSuccessText.Size = new System.Drawing.Size(500, 150);
            this.lblSuccessText.TabIndex = 0;
            this.lblSuccessText.Text = "MS SQL Server 2008 Express has been installed successfully.";
            this.lblSuccessText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblErrorText
            // 
            this.lblErrorText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrorText.ForeColor = System.Drawing.Color.MediumVioletRed;
            this.lblErrorText.Location = new System.Drawing.Point(0, 0);
            this.lblErrorText.Name = "lblErrorText";
            this.lblErrorText.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.lblErrorText.Size = new System.Drawing.Size(500, 150);
            this.lblErrorText.TabIndex = 1;
            this.lblErrorText.Text = "Installation failed. MS SQL Server did not install properly.\r\nPlease run MS SQL S" +
                "erver 2008 Express Installation manually to see the detailed error message.\r\nCon" +
                "nection error: {0}";
            this.lblErrorText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InstallationCompleted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.lblErrorText);
            this.Controls.Add(this.lblSuccessText);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "InstallationCompleted";
            this.Size = new System.Drawing.Size(500, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblSuccessText;
        private System.Windows.Forms.Label lblErrorText;
    }
}
