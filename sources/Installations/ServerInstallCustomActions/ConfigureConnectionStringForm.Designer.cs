namespace FrontDesk.Deploy.Server.Actions
{
    partial class ConfigureConnectionStringForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigureConnectionStringForm));
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ctrlDataCreationProgress = new FrontDesk.Deploy.Server.Actions.DatabaseInstallProgress();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.ctrlConfigureConnection = new FrontDesk.Deploy.Server.Actions.ConfigureConnection();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(407, 341);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 341);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Image = global::FrontDesk.Deploy.Server.Actions.Properties.Resources.fd_install_banner;
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10, 10, 0, 10);
            this.lblTitle.Size = new System.Drawing.Size(497, 68);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "Database connection settings";
            // 
            // ctrlDataCreationProgress
            // 
            this.ctrlDataCreationProgress.Location = new System.Drawing.Point(18, 146);
            this.ctrlDataCreationProgress.Name = "ctrlDataCreationProgress";
            this.ctrlDataCreationProgress.Size = new System.Drawing.Size(457, 117);
            this.ctrlDataCreationProgress.TabIndex = 13;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(326, 341);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnPrevious.TabIndex = 14;
            this.btnPrevious.Text = "< Previous";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // ctrlConfigureConnection
            // 
            this.ctrlConfigureConnection.EmbeddedExpressMode = false;
            this.ctrlConfigureConnection.Location = new System.Drawing.Point(12, 84);
            this.ctrlConfigureConnection.Name = "ctrlConfigureConnection";
            this.ctrlConfigureConnection.Password = "";
            this.ctrlConfigureConnection.ServerName = "SIROCCO\\SQL2008";
            this.ctrlConfigureConnection.Size = new System.Drawing.Size(469, 261);
            this.ctrlConfigureConnection.TabIndex = 12;
            this.ctrlConfigureConnection.UserName = "";
            // 
            // ConfigureConnectionStringForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(493, 380);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.ctrlConfigureConnection);
            this.Controls.Add(this.ctrlDataCreationProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ConfigureConnectionStringForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installing FrontDesk Database";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private DatabaseInstallProgress ctrlDataCreationProgress;
        private System.Windows.Forms.Button btnPrevious;
        private ConfigureConnection ctrlConfigureConnection;
       
    }
}