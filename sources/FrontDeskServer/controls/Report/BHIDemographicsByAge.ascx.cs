using System;
using System.Linq;
using System.Web.UI.WebControls;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Formatters;
using FrontDeskServer.Controls;
using FrontDesk;

public partial class BHIDemographicsByAgeControl : BHIReportByAgeBase
{
    public BhsBHIReportSectionByAgeViewModel Model;


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
        Model = Model ?? new BhsBHIReportSectionByAgeViewModel();

        var items = Model.Items;

        rptReportSection.DataSource = Model.Items;
        
        _incrementalLineNumber = 0;
        DataBind();
    }

    void ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            var lblHeader = e.Item.FindControl("lblHeader") as Literal;
            if (lblHeader != null)
                lblHeader.Text = Model.Header;
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RenderPositiveScreeningsByAge(e.Item.DataItem as BhsIndicatorReportByAgeItemViewModel, e.Item);
        }

    }

    protected void RenderPositiveScreeningsByAge(BhsIndicatorReportByAgeItemViewModel dataItem, RepeaterItem container)
    {
        if (dataItem == null) return;

        var ltrCategory = container.FindControl("ltrCategory") as Label;

        var placeHolder = container.FindControl("plhAgeValues") as PlaceHolder;

        if (ltrCategory != null)
        {
            ltrCategory.Text = dataItem.Indicator;

            if (ApplyPaddingForItemsStartingFromLine.HasValue &&
            _incrementalLineNumber >= ApplyPaddingForItemsStartingFromLine.Value)
            {
                ltrCategory.CssClass += " lpad";
            }

        }
        
        if (placeHolder != null && dataItem.TotalByAge != null)
        {
            foreach (var age in dataItem.TotalByAge)
            {
                var ltrHtml = new Literal
                {
                    Mode = LiteralMode.PassThrough,
                    Text = "<td>{0}</td>".FormatWith(age.Value),
                };

                placeHolder.Controls.Add(ltrHtml);
            }

            //render total
            placeHolder.Controls.Add(new Literal
            {
                Mode = LiteralMode.PassThrough,
                Text = "<td>{0}</td>".FormatWith(dataItem.Total),
            });

        }

        _incrementalLineNumber++;
    }

}