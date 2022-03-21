namespace FrontDesk.Deployment.SQLServerInstaller
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.ctrlInstallationCompleted = new FrontDesk.Deployment.SQLServerInstaller.InstallationCompleted();
            this.ctrlExpressProgress = new FrontDesk.Deploy.Server.Actions.InstallingExpressProgress();
            this.ctrlExpressConfiguration = new FrontDesk.Deploy.Server.Actions.ExpressConfiguration();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTitle.Location = new System.Drawing.Point(0, -1);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.lblTitle.Size = new System.Drawing.Size(498, 68);
            this.lblTitle.TabIndex = 11;
            this.lblTitle.Text = "Database connection settings";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(314, 303);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(408, 303);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 12;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // ctrlInstallationCompleted
            // 
            this.ctrlInstallationCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ctrlInstallationCompleted.Location = new System.Drawing.Point(7, 76);
            this.ctrlInstallationCompleted.Name = "ctrlInstallationCompleted";
            this.ctrlInstallationCompleted.Size = new System.Drawing.Size(479, 217);
            this.ctrlInstallationCompleted.TabIndex = 16;
            // 
            // ctrlExpressProgress
            // 
            this.ctrlExpressProgress.Location = new System.Drawing.Point(18, 111);
            this.ctrlExpressProgress.Name = "ctrlExpressProgress";
            this.ctrlExpressProgress.Size = new System.Drawing.Size(457, 117);
            this.ctrlExpressProgress.TabIndex = 15;
            // 
            // ctrlExpressConfiguration
            // 
            this.ctrlExpressConfiguration.Location = new System.Drawing.Point(4, 70);
            this.ctrlExpressConfiguration.Name = "ctrlExpressConfiguration";
            this.ctrlExpressConfiguration.Size = new System.Drawing.Size(463, 186);
            this.ctrlExpressConfiguration.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(492, 338);
            this.Controls.Add(this.ctrlExpressConfiguration);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ctrlInstallationCompleted);
            this.Controls.Add(this.ctrlExpressProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install MS SQL Server 2008 Express";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private FrontDesk.Deploy.Server.Actions.ExpressConfiguration ctrlExpressConfiguration;
        private FrontDesk.Deploy.Server.Actions.InstallingExpressProgress ctrlExpressProgress;
        private InstallationCompleted ctrlInstallationCompleted;
    }
}

