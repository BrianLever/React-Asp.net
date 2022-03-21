using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Formatters;

namespace FrontDeskServer.Controls
{
    public class BhiBehavioralReportByAge : BHIReportByAgeBase
    {
        protected void RenderPositiveScreeningsByAge(IndicatorReportByAgeItemViewModel dataItem, RepeaterItem container)
        {
            if (dataItem == null) return;

            var ltrQuestion = container.FindControl("ltrQuestion") as Label;
            var ltrScoreIndicates = container.FindControl("ltrScoreIndicates") as Literal;

            var placeHolder = container.FindControl("plhAgeValues") as PlaceHolder;

            if (ltrQuestion != null)
            {
                ltrQuestion.Text = dataItem.ScreeningSectionQuestion.FormatAsQuestionWithPreamble();

                if (ApplyPaddingForItemsStartingFromLine.HasValue &&
                _incrementalLineNumber >= ApplyPaddingForItemsStartingFromLine.Value)
                {
                    ltrQuestion.CssClass += " lpad";
                }

            }
            if (ltrScoreIndicates != null)
            {
                ltrScoreIndicates.Text = dataItem.ScreeningSectionIndicates;
            }

            if (placeHolder != null && dataItem.PositiveScreensByAge != null)
            {
                foreach (var age in dataItem.PositiveScreensByAge)
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
}