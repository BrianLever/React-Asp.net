using System;
namespace FrontDesk.Data
{
    public interface IScreeningAnswersDb
    {
        System.Collections.Generic.List<FrontDesk.AnswerScaleOption> GetAnswerOptions(int answerScaleID);
        System.Collections.Generic.Dictionary<int, FrontDesk.AnswerScale> GetAnswerOptions();
    }
}
