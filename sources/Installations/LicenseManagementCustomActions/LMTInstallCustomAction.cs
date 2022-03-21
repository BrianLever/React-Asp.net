using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace LicenseManagementCustomActions
{
    [RunInstaller(true)]
    public partial class LMTInstallCustomAction : Installer
    {
        public LMTInstallCustomAction()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }
    }
}
