using System;
using System.Diagnostics;
using FrontDesk.Kiosk.Screens;

namespace FrontDesk.Kiosk.Workflow
{
    public class SectionQuestionsVisualScreenController
    {
        public void ProcessNextScreenClicked(
                NextScreenClickedEventArg e, IScreeningResultState resultState,
                ref bool forceSkipSectionQuestion)
        {
            var answerOption = (AnswerScaleOption)e.Value;
            var section = e.Screen.ScreenSection;
            var question = e.Screen.ScreenSectionQuestion;

            //Check
            if (section != null)
            {
                if (question == null)
                {
                    throw new InvalidOperationException(
                            "We always ask question in the section. Fist comes Main Section Questions.");
                }

                // if we answering on section question determ 'Yes' or 'No' result
                if (question.IsMainQuestion)
                {

                    if (section.MainSectionQuestions.Count == 1)
                    {

                        if (answerOption.Text == "No")
                        {
                            forceSkipSectionQuestion = true;
                        }


                        resultState.Result.AppendSectionAnswer(new ScreeningSectionResult(section.ScreeningSectionID,
                                answerOption.Value));


                        //save answer result
                        AppendQuestionAnswer(resultState, question, answerOption);

                    }
                    else //when there are two main questions
                    {
                        var answerOnGenericQuestion = answerOption.Text == QuestionAnswersConstants.NotAtAllAnswer
                                ? QuestionAnswersConstants.NoAnswer
                                : QuestionAnswersConstants.YesAnswer;

                        //check if this is the first question in section or it was alread added
                        var answerSectionState = resultState.Result.FindSectionByID(section.ScreeningSectionID);

                        if (answerSectionState != null)
                        {
                            //To get positive answer on the section we need either of generic answers be positive
                            answerOnGenericQuestion = Math.Max(answerSectionState.AnswerValue, answerOnGenericQuestion);

                            if (answerOnGenericQuestion == QuestionAnswersConstants.NoAnswer)
                            {
                                // if this is the last main question - skip the section (PHQ9A has all 1- questions as mandatory)
                                if (section.MainSectionQuestions.Count == answerSectionState.Answers.Count + 1)
                                {
                                    forceSkipSectionQuestion = true;
                                }
                            }
                            AppendOrUpdateSectionAnswerIfNotExists(resultState, section, answerOnGenericQuestion);
                        }
                        else
                        {
                            //this is the first answer, just record it.
                            AppendOrUpdateSectionAnswerIfNotExists(resultState, section, answerOnGenericQuestion);
                        }

                        //append answer on question
                        AppendQuestionAnswer(resultState, question, answerOption);
                    }
                }
                else
                {
                    AppendQuestionAnswer(resultState, question, answerOption);
                }
            }


        }

        private void AppendQuestionAnswer(IScreeningResultState resultState, ScreeningSectionQuestion question, AnswerScaleOption answerOption)
        {
            //save answer result
            var sectionAnswer = resultState.Result.FindSectionByID(question.ScreeningSectionID);
            if (sectionAnswer == null)
            {
                Debug.Assert(true, "Question's section was not answered before answering on section's question");
            }
            else
            {
                sectionAnswer.AppendQuestionAnswer(new ScreeningSectionQuestionResult(question.QuestionID,
                        answerOption.Value));
            }

        }

        private void AppendOrUpdateSectionAnswerIfNotExists(IScreeningResultState resultState, ScreeningSection section,
                int answerOptionValue)
        {

            var sectionAnswer = resultState.Result.FindSectionByID(section.ScreeningSectionID);

            if (sectionAnswer == null)
            {

                resultState.Result.AppendSectionAnswer(new ScreeningSectionResult(section.ScreeningSectionID,
                        answerOptionValue));
            }
            else
            {
                sectionAnswer.AnswerValue = answerOptionValue;
            }

        }

    }
}
