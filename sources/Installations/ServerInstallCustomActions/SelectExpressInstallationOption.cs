using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class SelectExpressInstallationOption : UserControl
    {
        public SelectExpressInstallationOption()
        {
            InitializeComponent();
        }

        public bool InstallSqlExpress
        {
            get { return rbInstallExpress.Checked; }
            set { rbInstallExpress.Checked = value; }
        }
    }
}
