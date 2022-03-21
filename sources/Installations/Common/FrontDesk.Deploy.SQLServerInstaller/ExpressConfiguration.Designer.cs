namespace FrontDesk.Deploy.Server.Actions
{
    partial class ExpressConfiguration
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
            this.components = new System.ComponentModel.Container();
            this.txtConfirmPass = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSrvName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtConfirmPass
            // 
            this.txtConfirmPass.Location = new System.Drawing.Point(137, 155);
            this.txtConfirmPass.Name = "txtConfirmPass";
            this.txtConfirmPass.PasswordChar = '*';
            this.txtConfirmPass.Size = new System.Drawing.Size(284, 20);
            this.txtConfirmPass.TabIndex = 18;
            this.txtConfirmPass.Validating += new System.ComponentModel.CancelEventHandler(this.txtConfirmPass_Validating);
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(137, 123);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(284, 20);
            this.txtPass.TabIndex = 17;
            this.txtPass.Validating += new System.ComponentModel.CancelEventHandler(this.txtPass_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Password:";
            // 
            // txtSrvName
            // 
            this.txtSrvName.Location = new System.Drawing.Point(137, 14);
            this.txtSrvName.Name = "txtSrvName";
            this.txtSrvName.Size = new System.Drawing.Size(284, 20);
            this.txtSrvName.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Server Instance Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Confirm Password:";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(404, 26);
            this.label4.TabIndex = 20;
            this.label4.Text = "FrontDesk Screener Server application configured to use SQL mode authentication. " +
                "\r\nPlease input password for the built-in superadmin user account (\'sa\').";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(472, 29);
            this.label5.TabIndex = 21;
            this.label5.Text = "Password must contain one lowercase character, one uppercase character, one numer" +
                "ic character and should be at least 6 characters";
            // 
            // ExpressConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConfirmPass);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSrvName);
            this.Controls.Add(this.label1);
            this.Name = "ExpressConfiguration";
            this.Size = new System.Drawing.Size(495, 186);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.ExpressConfiguration_Validating);
            this.Enter += new System.EventHandler(this.ExpressConfiguration_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConfirmPass;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSrvName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}
