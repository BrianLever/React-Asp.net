using System.Web.UI.WebControls;
using System.Web.UI;


namespace FrontDesk.Server.Extensions
{
    public enum WizardNavigationTempContainer
    {
        StartNavigationTemplateContainerID = 1,
        StepNavigationTemplateContainerID = 2,
        FinishNavigationTemplateContainerID = 3
    }

    public static class WizardExtensions
    {
        public static Control FindControlInTemplate(this Wizard wizard,
            WizardNavigationTempContainer wzdTemplate,
            string controlID)
        {
            System.Text.StringBuilder strCtrl = new System.Text.StringBuilder();
            strCtrl.Append(wzdTemplate);
            strCtrl.Append("$");
            strCtrl.Append(controlID);

            return wizard.FindControl(strCtrl.ToString());
        }
    }
}
