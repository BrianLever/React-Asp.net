using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using FrontDesk.Deployment.Extensions;
using System.Reflection;


namespace FrontDesk.RPMS.InstallerCustomActions
{
    [RunInstaller(true)]
    public partial class RpmsServiceCustomActionsInstaller : System.Configuration.Install.Installer
    {
        private string TargetDirectory;

        public RpmsServiceCustomActionsInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            try
            {
                this.TargetDirectory = Context.Parameters["targetdir"];

                //log parameters
                stateSaver.AddOrUpdate("targetdir", this.TargetDirectory);

                foreach (string key in Context.Parameters.Keys)
                {
                    stateSaver.AddOrUpdate(key, Context.Parameters[key]);
                }
                Assembly.Load(new AssemblyName("RPMS.Common"));
                //end log
                var customActions = new RpmsInterfaceServiceInstallerActions(Context.Parameters);

                customActions.UpdateWebConfigFile(this.TargetDirectory);
            }
            catch (Exception ex)
            {
                this.Context.LogMessage(ex.ToString());
                throw new InstallException(ex.ToString(), ex);
            }
        }
    }
}
