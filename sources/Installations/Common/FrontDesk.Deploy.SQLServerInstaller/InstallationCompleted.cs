using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrontDesk.Deployment.SQLServerInstaller
{
    public partial class InstallationCompleted : UserControl
    {
        public InstallationCompleted()
        {
            InitializeComponent();
            lblSuccessText.Visible = false;
            lblErrorText.Visible = false;
        }

        public string SuccessText
        {
            set
            {
                lblSuccessText.Text = value;
                lblSuccessText.Visible = true;
                lblErrorText.Visible = false;
            }
        }

        public string ErrorText
        {
            set
            {
                lblErrorText.Text = value;
                lblErrorText.Visible = true;
                lblSuccessText.Visible = false;
            }
        }

    }
}
