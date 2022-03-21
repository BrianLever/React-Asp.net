using FrontDesk.Server;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Services;
using ScreenDox.Server.Common.Services;
using System;

public partial class SystemStatusSummaryCtrl : System.Web.UI.UserControl
{
    private readonly IBranchLocationService _branchLocationService = new BranchLocationService();
    private readonly IKioskService _kioskService = new KioskService();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTotalSummaryLabels();
        }
    }

    protected void BindTotalSummaryLabels()
    {
        var checkInCount = ScreeningResultHelper.GetTotalRecordCount();
        var kioskCount = _kioskService.GetNotDisabledCount();
        var locCount = _branchLocationService.GetNotDisabledCount();

        lblCheckInCount.Text = checkInCount.ToString();
        lblKioskCout.Text = kioskCount.ToString();
        lblLocationCount.Text = locCount.ToString();
    }
}
