namespace FrontDesk.Deploy.Server.Actions.WizardSteps
{
    partial class ConfigureSSL
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
            this.lblText = new System.Windows.Forms.Label();
            this.rbUnsecureHttp = new System.Windows.Forms.RadioButton();
            this.rbUseSSL = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(53, 30);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(218, 13);
            this.lblText.TabIndex = 5;
            this.lblText.Text = "Specify whether to use a secure connection:";
            // 
            // rbUnsecureHttp
            // 
            this.rbUnsecureHttp.AutoSize = true;
            this.rbUnsecureHttp.Location = new System.Drawing.Point(95, 87);
            this.rbUnsecureHttp.Name = "rbUnsecureHttp";
            this.rbUnsecureHttp.Size = new System.Drawing.Size(211, 17);
            this.rbUnsecureHttp.TabIndex = 4;
            this.rbUnsecureHttp.TabStop = true;
            this.rbUnsecureHttp.Text = "No, use standard connection via HTTP";
            this.rbUnsecureHttp.UseVisualStyleBackColor = true;
            // 
            // rbUseSSL
            // 
            this.rbUseSSL.AutoSize = true;
            this.rbUseSSL.Checked = true;
            this.rbUseSSL.Location = new System.Drawing.Point(95, 64);
            this.rbUseSSL.Name = "rbUseSSL";
            this.rbUseSSL.Size = new System.Drawing.Size(237, 17);
            this.rbUseSSL.TabIndex = 3;
            this.rbUseSSL.TabStop = true;
            this.rbUseSSL.Text = "Yes, use secure HTTP (HTTPS)  connection";
            this.rbUseSSL.UseVisualStyleBackColor = true;
            // 
            // ConfigureSSL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.rbUnsecureHttp);
            this.Controls.Add(this.rbUseSSL);
            this.Name = "ConfigureSSL";
            this.Padding = new System.Windows.Forms.Padding(50, 30, 0, 0);
            this.Size = new System.Drawing.Size(335, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.RadioButton rbUnsecureHttp;
        private System.Windows.Forms.RadioButton rbUseSSL;
    }
}
