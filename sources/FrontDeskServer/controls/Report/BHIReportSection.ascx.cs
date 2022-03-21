using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

using FrontDesk.Server.Extensions;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Formatters;


public partial class BHIReportSection : System.Web.UI.UserControl
{
    public BHIReportSectionViewModel Model;
    
    public int? ApplyPaddingForItemsStartingFromLine { get; set; }

    private int _incrementalLineNumber = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        rptReportSection.ItemDataBound += ItemDataBound;
        rptQuestionOnFocus.ItemDataBound += QuestionOnFocusOnDataBound;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        BindReportData();
        ltrCopyright.Text = Model.Copyrights;
    }

    protected void BindReportData()
    {
        var items = Model.Items;

        if (Model.MainQuestions != null && Model.MainQuestions.Any())
        {
            items.InsertRange(0, Model.MainQuestions);
        }

        rptReportSection.DataSource = Model.Items;
        if (Model.QuestionOnFocus != null)
        {
            rptQuestionOnFocus.DataSource = Model.QuestionOnFocus.Items;
        }
        _incrementalLineNumber = 0;
        DataBind();
    }

    private void QuestionOnFocusOnDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            var lblPreamble = e.Item.FindControl("lblPreamble") as Label;
            var lblQuestion = e.Item.FindControl("lblQuestion") as Label;

            if (lblPreamble != null)
            { lblPreamble.Text = Model.QuestionOnFocus.Question.PreambleText;}

            if (lblQuestion != null)
            {
                lblQuestion.Text = Model.QuestionOnFocus.Question.QuestionText;

                if (!string.IsNullOrEmpty(Model.QuestionOnFocus.Question.PreambleText))
                {
                    lblQuestion.CssClass += "lpad";
                }
            }   
                     
        }
        else
        {
            var dataItem = e.Item.DataItem as IndicatorReportItem;
            if (dataItem != null)
            {
                BindGridItemLine(e, dataItem);
            }
        }
    }

    void ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if(e.Item.ItemType == ListItemType.Header)
        {
            var lblHeader = e.Item.FindControl("lblHeader") as Literal;
            if (lblHeader != null)
                lblHeader.Text = Model.Header;
        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var dataItem = e.Item.DataItem as IndicatorReportItem;
            if (dataItem != null)
            {
                BindGridItemLine(e, dataItem);

                _incrementalLineNumber++;
            }
        }
        
    }

    private void BindGridItemLine(RepeaterItemEventArgs e, IndicatorReportItem dataItem)
    {
        var ltrQuestion = e.Item.FindControl("ltrQuestion") as Label;
        var ltrPositive = e.Item.FindControl("ltrPositive") as Literal;
        var ltrNegative = e.Item.FindControl("ltrNegative") as Literal;
        var ltrPositivePercent = e.Item.FindControl("ltrPositivePercent") as Literal;
        var ltrNegativePerent = e.Item.FindControl("ltrNegativePerent") as Literal;
        var ltrTotal = e.Item.FindControl("ltrTotal") as Literal;
        var ltrScoreIndicates = e.Item.FindControl("ltrScoreIndicates") as Literal;

        if (ltrQuestion != null && ltrPositive != null && ltrNegative != null
            && ltrPositivePercent != null && ltrNegativePerent != null && ltrTotal != null &&
            ltrScoreIndicates != null)
        {
            //get text
            ltrQuestion.Text = dataItem.ScreeningSectionQuestion.FormatAsQuestionWithPreamble();
            ltrPositive.Text = dataItem.PositiveCount.ToString(CultureInfo.InvariantCulture);
            ltrNegative.Text = dataItem.NegativeCount.ToString(CultureInfo.InvariantCulture);
            ltrPositivePercent.Text = dataItem.PositivePercent.FormatAsPercentage();
            ltrNegativePerent.Text = dataItem.NegativePercent.FormatAsPercentage();
            ltrTotal.Text = dataItem.TotalCount.ToString(CultureInfo.InvariantCulture);
            ltrScoreIndicates.Text = dataItem.ScreeningSectionIndicates;

            if (ApplyPaddingForItemsStartingFromLine.HasValue &&
                _incrementalLineNumber >= ApplyPaddingForItemsStartingFromLine.Value)
            {
                ltrQuestion.CssClass += " lpad";
            }
        }
    }
}