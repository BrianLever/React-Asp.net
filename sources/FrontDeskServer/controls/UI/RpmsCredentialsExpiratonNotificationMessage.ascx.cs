using System;

using FrontDesk.Server.Controllers;
using FrontDesk.Server.Web.Controls;

using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;

public partial class RpmsCredentialsExpiratonNotificationMessageCtrl : BaseUserControl
{

    private readonly RpmsExportController _exportController;

    public RpmsCredentialsExpiratonNotificationMessageCtrl()
    {
        _exportController = new RpmsExportController(
            new DefaultDateService(),
            new RpmsCredentialsService(new RpmsCredentialsRepository(), new CryptographyService()),
            new GlobalSettingsService()
            );
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        pnlNotification.Visible = _exportController.ShouldDisplayAlertMessage();
        lblRpmsCredentialsExpirationAlert.Text = _exportController.GetAlertMessageText();
        if (!_exportController.HasConnectionToExternalService)
        {
            pnlNotification.Visible = true;
            lblRpmsCredentialsExpirationAlert.Text = Resources.TextMessages.RPMS_NotConnectedMessage;

        }
    }

    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {

    }
}