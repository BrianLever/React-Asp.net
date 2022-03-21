using System;
using System.Linq;
using System.Web.UI.WebControls;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Formatters;
using FrontDeskServer.Controls;

public partial class BHIReportSectionByAge : BhiBehavioralReportByAge
{
    public BHIReportSectionByAgeViewModel Model;


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
            RenderPositiveScreeningsByAge(e.Item.DataItem as IndicatorReportByAgeItemViewModel, e.Item);
        }

    }

    private void QuestionOnFocusOnDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Header)
        {
            var lblPreamble = e.Item.FindControl("lblPreamble") as Label;
            var lblQuestion = e.Item.FindControl("lblQuestion") as Label;

            if (lblPreamble != null)
            { lblPreamble.Text = Model.QuestionOnFocus.Question.PreambleText.FormatAsQuestionWithPreamble(); }

            if (lblQuestion != null)
            {
                lblQuestion.Text = Model.QuestionOnFocus.Question.QuestionText;

                if (!string.IsNullOrEmpty(Model.QuestionOnFocus.Question.PreambleText))
                {
                    lblQuestion.CssClass += "lpad";
                }
            }

        }
        else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RenderPositiveScreeningsByAge(e.Item.DataItem as IndicatorReportByAgeItemViewModel, e.Item);
        }
    }
}