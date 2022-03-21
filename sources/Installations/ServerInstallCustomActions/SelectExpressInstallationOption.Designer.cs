namespace FrontDesk.Deploy.Server.Actions
{
    partial class SelectExpressInstallationOption
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
            this.rbUseAnother = new System.Windows.Forms.RadioButton();
            this.rbInstallExpress = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rbUseAnother
            // 
            this.rbUseAnother.AutoSize = true;
            this.rbUseAnother.Location = new System.Drawing.Point(25, 113);
            this.rbUseAnother.Name = "rbUseAnother";
            this.rbUseAnother.Size = new System.Drawing.Size(279, 17);
            this.rbUseAnother.TabIndex = 11;
            this.rbUseAnother.Text = "No, I have SQL Server 2008 server and want to use it";
            this.rbUseAnother.UseVisualStyleBackColor = true;
            // 
            // rbInstallExpress
            // 
            this.rbInstallExpress.AutoSize = true;
            this.rbInstallExpress.Checked = true;
            this.rbInstallExpress.Location = new System.Drawing.Point(25, 73);
            this.rbInstallExpress.Name = "rbInstallExpress";
            this.rbInstallExpress.Size = new System.Drawing.Size(253, 17);
            this.rbInstallExpress.TabIndex = 10;
            this.rbInstallExpress.TabStop = true;
            this.rbInstallExpress.Text = "Yes, install free SQL SQL Server Express  server";
            this.rbInstallExpress.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(433, 36);
            this.label1.TabIndex = 9;
            this.label1.Text = "Do you want to install new MS SQL Server 2008 Express database server or you want" +
                " to use your existing SQL Server 2008 Server?";
            // 
            // SelectExpressInstallationOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbUseAnother);
            this.Controls.Add(this.rbInstallExpress);
            this.Controls.Add(this.label1);
            this.Name = "SelectExpressInstallationOption";
            this.Size = new System.Drawing.Size(450, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbUseAnother;
        private System.Windows.Forms.RadioButton rbInstallExpress;
        private System.Windows.Forms.Label label1;

    }
}
