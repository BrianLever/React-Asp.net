using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Deployment
{
    public enum SqlInstallWizardSteps
    {
        InstallExpressOption,
        ExpressConfiguration,
        InstallationProgress,
        InstallationCompleted,
        SetConnectionString,
        CreatingDatabase
    }
}
