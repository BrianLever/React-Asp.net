using System;

using FrontDesk.Server.Extensions;
using FrontDesk.Server.Logging;

namespace FrontDesk.Server.Web.Controls
{

    public partial class BHSReportControl : BaseUserControl
    {
        #region Property: ScreeningResult

        private ScreeningResult _screeningResult = null;
        /// <summary>
        /// Screening result data
        /// </summary>
        public ScreeningResult ScreeningResult
        {
            get { return _screeningResult; }
            set { _screeningResult = value; }
        }

        public bool _isStaff = false;
        public bool isStaff
        {
            get { return _isStaff; }
            set { _isStaff = value; }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            SetFormData();

        }

        protected int _briefRowNumber = 0;

        protected void SetFormData()
        {
            try
            {
                if (_screeningResult != null)
                {
                    #region Patient Info

                    lblFirstname.Text = _screeningResult.FirstName;
                    lblLastname.Text = _screeningResult.LastName;
                    lblMiddlename.Text = _screeningResult.MiddleName;
                    lblBirthday.Text = _screeningResult.Birthday.ToString("MM/dd/yyyy");
                    lblRecordNo.Text = string.Empty;

                    lblStreetAddress.Text = _screeningResult.StreetAddress.FormatAsNullableString();
                    lblCity.Text = _screeningResult.City.FormatAsNullableString();
                    lblState.Text = _screeningResult.StateName.FormatAsNullableString().ToUpper();
                    lblZipCode.Text = _screeningResult.ZipCode.FormatAsNullableString();
                    lblPhone.Text = _screeningResult.Phone.FormatAsNullableString();

                    #endregion

                    #region Brief section answers
                    _briefRowNumber = 1;
                    //TODO: Delete
                    //rptSectionAnswers.DataSource = _screeningResult.SectionAnswers;
                    //rptSectionAnswers.DataBind();

                    #endregion

                    /* Requirement Remark
                     * KSA: July 11, 2012, Version 4.0
                     * 10.		Display “No” answer for “Smoker at Home” question for children.
                     * If a parent is completing the check-in for a child who is under the minimum age to complete the screenings 
                     * and ONLY answers the question: “Does anyone in the home smoke tobacco (such as cigarettes, cigars, pipes, electronic nicotine delivery devices, etc.)? 
                     * the “Yes” or “No” check box should appear and show the answer on the Behavioral Health Screening Report. 
                     * Currently, if someone answers “No” only the demographic information appears and the question and answer does NOT appear on the report.
                     */


                    

                    var screeningInfo = ServerScreening.GetByID(ScreeningResult.ScreeningID);

                    ucTobacco.ScreeningResult = ScreeningResult;
                    ucTobacco.ScreeningInfo = screeningInfo;

                    ucAlcohol.ScreeningResult = ScreeningResult;
                    ucAlcohol.ScreeningInfo = screeningInfo;


                    ucDrugs.ScreeningResult = ScreeningResult;
                    ucDrugs.ScreeningInfo = screeningInfo;

                    ucDrugsOfChoice.ScreeningResult = ScreeningResult;
                    ucDrugsOfChoice.ScreeningInfo = screeningInfo;

                    ucDepression.ScreeningResult = ScreeningResult;
                    ucDepression.ScreeningInfo = screeningInfo;

                    ucAnxiety.ScreeningResult = ScreeningResult;
                    ucAnxiety.ScreeningInfo = screeningInfo;

                    ucViolence.ScreeningResult = ScreeningResult;
                    ucViolence.ScreeningInfo = screeningInfo;

                    ucGambling.ScreeningResult = ScreeningResult;
                    ucGambling.ScreeningInfo = screeningInfo;

                    lblCreateDate.Text = String.Format("{0:MM/dd/yyyy HH:mm:ss zzz}", ScreeningResult.CreatedDate);
                    /* Legacy requirements: Brief section not visible if persone under fifteen years old */
                    //phBrief.Visible = ScreeningResult.IsPassedAnySection;

                    phSecuritySectrion.Visible = !isStaff;

                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException(null, ex);
                Page.RedirectToErrorPage();
            }
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            //do nothing

        }
    }
}
