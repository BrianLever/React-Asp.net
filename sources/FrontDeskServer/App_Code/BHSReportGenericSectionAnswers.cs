using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
	/// <summary>
	/// Summary description for BHSReportGenericSectionAnswers
	/// </summary>
	public abstract class BHSReportGenericSectionAnswers : BaseUserControl
	{
		public BHSReportGenericSectionAnswers()
		{

		}

		#region Abstract members
		/// <summary>
		/// Gets the id of the displaying screening section answers
		/// </summary>
		public abstract string ScreeningSectionID { get; }

		#endregion


		#region Property: ScreeningSectionResult

		protected ScreeningResult _screeningResult = null;
		/// <summary>
		/// Screening result data
		/// </summary>
		public virtual ScreeningResult ScreeningResult
		{
			get { return _screeningResult; }
			set
			{
				_screeningResult = value;

				if (value != null)
				{
					//get bound section result
					_screeningSectionResult = _screeningResult.FindSectionByID(ScreeningSectionID);
				}
			}
		}

		protected ScreeningSectionResult _screeningSectionResult = null;
		/// <summary>
		/// get bound section result
		/// </summary>
		public ScreeningSectionResult ScreeningSectionResult
		{
			get { return _screeningSectionResult; }

		}

		#endregion


		#region Property: Screening Info

		private FrontDesk.Screening _screeningInfo = null;
		/// <summary>
		/// Screening info
		/// </summary>
		public FrontDesk.Screening ScreeningInfo
		{
			get { return _screeningInfo; }
			set
			{
				_screeningInfo = value;

				if (value != null)
				{
					//get bound section info
					_screeningSectionInfo = _screeningInfo.FindSectionByID(ScreeningSectionID);
				}
			}
		}
		private ScreeningSection _screeningSectionInfo = null;

		/// <summary>
		/// Get bound section info
		/// </summary>
		public ScreeningSection ScreeningSectionInfo
		{
			get { return _screeningSectionInfo; }
		}

		#endregion

		#region Binding Control Data

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			//determ if section should be visible
			if (_screeningSectionResult != null && _screeningSectionInfo != null)
			{
				this.Visible = true;
				BindSectionData();

			}
			else
			{
				this.Visible = false;
			}
		}


		/// <summary>
		/// Get repeater which is used to show section questions answers
		/// </summary>
		/// <returns></returns>
		protected abstract Repeater GetSectionAnswerRepeater();

		protected virtual Repeater GetMainSectionAnswerRepeater()
		{
			return null;
		}

		/// <summary>
		/// Bind answers repeater and other form controls
		/// </summary>
		[Obfuscation(Feature = "renaming", Exclude = true)]
		protected virtual void BindSectionData()
		{
			//bind group of main questions which are not embedded into the other section
			var rpt = GetMainSectionAnswerRepeater();
			AnswerListIndex = 1;
			if (rpt != null)
			{
				rpt.ItemDataBound += new RepeaterItemEventHandler(rpt_ItemDataBound);

				rpt.DataSource = GetMainSectionQuestionsDataSource();
				rpt.DataBind();
			}

			rpt = GetSectionAnswerRepeater();
			AnswerListIndex = 1;
			if (rpt != null)
			{
				rpt.ItemDataBound += new RepeaterItemEventHandler(rpt_ItemDataBound);

				rpt.DataSource = GetQuestionsDataSource();
				rpt.DataBind();
			}



			SetFormData();
		}

		protected virtual IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
		{
			return _screeningSectionInfo.MainSectionQuestions;
		}

		protected virtual IList<ScreeningSectionQuestion> GetQuestionsDataSource()
		{
			return _screeningSectionInfo.NotMainSectionQuestions;
		}
		/// <summary>
		/// Bind data to control except answer repeater
		/// </summary>
		protected abstract void SetFormData();

		/// <summary>
		/// Answer list item index. AnswerListIndex starts from 1.
		/// </summary>
		protected int AnswerListIndex = 1;

		/// <summary>
		/// Bind Repeater item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{


			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var questionInfo = e.Item.DataItem as ScreeningSectionQuestion;

				if (questionInfo == null) return;

				//answer can be null
				var answer = _screeningSectionResult.FindQuestionByID(questionInfo.QuestionID);



				if (!IgnoreQuestion(answer, questionInfo)) //derived controls can filter answers
				{
					//dynamically bind all options
					PerformReaderItemBinding(e.Item, answer, questionInfo);
				}
				else
				{
					//remove ignored item
					e.Item.Visible = false;
				}
			}
			else if (e.Item.ItemType == ListItemType.Header)
			{
				var lblPreamble = e.Item.FindControl("lblPreamble") as Label;
				if (lblPreamble != null)
				{

					//set preambula text if first question has preambula
					if (_screeningSectionInfo.NotMainSectionQuestions.Count > 0 && !string.IsNullOrEmpty(_screeningSectionInfo.NotMainSectionQuestions[0].PreambleText))
					{
						lblPreamble.Text = _screeningSectionInfo.NotMainSectionQuestions[0].PreambleText;
					}
					else
					{
						lblPreamble.Visible = false;
					}
				}

				Repeater mainQuestionsRepeater = e.Item.FindControl("mainQuestionsRepeater") as Repeater;
				if (mainQuestionsRepeater != null)
				{
					mainQuestionsRepeater.ItemDataBound += new RepeaterItemEventHandler(mainQuestionsRepeater_ItemDataBound);
					mainQuestionsRepeater.DataSource = GetMainSectionQuestionsDataSource();
					mainQuestionsRepeater.DataBind();
				}
				;
			}

		}

		private void mainQuestionsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var questionInfo = e.Item.DataItem as ScreeningSectionQuestion;

				ScreeningSectionQuestionResult answer = null;
				Debug.Assert(questionInfo != null, "questionInfo != null");
				var section = _screeningResult.FindSectionByID(questionInfo.ScreeningSectionID);
				if (section != null)
				{
					answer = section.FindQuestionByID(questionInfo.QuestionID);
				}
				
				PerformReaderItemBinding(e.Item, answer, questionInfo);
			}
		}


		/// <summary>
		/// Bind section question answers and question text to repeater item
		/// </summary>
		/// <param name="answer"></param>
		/// <param name="questionInfo"></param>
		protected void PerformReaderItemBinding(Control item, ScreeningSectionQuestionResult answer, ScreeningSectionQuestion questionInfo)
		{
			YesNoLabel ynLabel = null;
			Label ltrAnswerText = null;
			Literal ltrQuestion = null;
			Literal ltrNo = null;


			ltrQuestion = item.FindControl("ltrQuestion") as Literal;
			if (ltrQuestion != null)
			{
				ltrQuestion.Text = questionInfo.QuestionText;
			}
			ltrNo = item.FindControl("ltrNo") as Literal;
			if (ltrNo != null)
			{
				ltrNo.Text = string.Format("{0}.", AnswerListIndex++);
			}

			for (int i = 0; i < questionInfo.AnswerOptions.Count; i++)
			{
				ynLabel = item.FindControl("ynl" + i) as YesNoLabel;
				ltrAnswerText = item.FindControl("ltr" + i) as Label;

				if (ynLabel != null && ltrAnswerText != null)
				{
					ltrAnswerText.Text = questionInfo.AnswerOptions[i].Text;
					if (answer != null) //answer can be null
					{
						ynLabel.Checked = answer.AnswerValue == questionInfo.AnswerOptions[i].Value;
					}
				}
			}


		}

		[Obfuscation(Feature = "renaming", Exclude = true)]
		protected virtual bool IgnoreQuestion(ScreeningSectionQuestionResult answer, ScreeningSectionQuestion questionInfo)
		{
			return false;
		}

		#endregion

		public override void ApplyTabIndexToControl(ref short startTabIndex)
		{

		}
	}
}
