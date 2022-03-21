using System;
using System.Web.UI.WebControls;

using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;

using FrontDeskServer.Controls;

public partial class ScreenTimeLogReportSectionControl : BHIReportByAgeBase
{
    public ScreeningTimeLogReportViewModel Model;


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rptReportSection.ItemDataBound += ItemDataBound;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindReportData();
    }

    protected void BindReportData()
    {
        Model = Model ?? new ScreeningTimeLogReportViewModel();

        rptReportSection.DataSource = Model.SectionMeasures;

        _incrementalLineNumber = 0;
        DataBind();
    }

    void ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Footer)
        {
            RenderItem(Model.EntireScreeningsMeasures, e.Item);
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RenderItem(e.Item.DataItem as ScreeningTimeLogReportItem, e.Item);
        }

    }

    protected void RenderItem(ScreeningTimeLogReportItem dataItem, RepeaterItem container)
    {
        if (dataItem == null) return;

        var ltrCategory = container.FindControl("ltrCategory") as Label;

        var lblCount = container.FindControl("lblCount") as Label;
        var lblTotalTime = container.FindControl("lblTotalTime") as Label;
        var lblAvgTime = container.FindControl("lblAvgTime") as Label;


        if (ltrCategory != null)
        {
            ltrCategory.Text = dataItem.ScreeningSectionName;
        }
        if (lblCount != null)
        {
            lblCount.Text = dataItem.NumberOfReports.ToString();
        }

        if (lblTotalTime != null)
        {
           lblTotalTime.Text = dataItem.TotalTime.AsFormattedString();
        }

        if (lblAvgTime != null)
        {
            lblAvgTime.Text = dataItem.AverageTime.AsFormattedAverageString();
        }

        
    }

}