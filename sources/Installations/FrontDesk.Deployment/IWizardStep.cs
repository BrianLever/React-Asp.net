using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Deployment
{
    public interface IWizardStep
    {
        string Title { get; set; }

        void ShowStep();
        void HideStep();
        bool Validate(out string errorMessage);

    }
}
