using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Screening.Models
{
    public class DrugOfChoiceModel
    {

        private readonly ScreeningSectionResult _section;

        private ScreeningSectionResult InitSection()
        {
            return new ScreeningSectionResult(ScreeningSectionDescriptor.DrugOfChoice, 0);
        }

        public DrugOfChoiceModel()
        {
            InitSection();
        }

        public DrugOfChoiceModel(ScreeningSectionResult section)
        {

            _section = section ?? InitSection();
        }

        public DrugOfChoiceModel(ScreeningResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            _section = result.FindSectionByID(ScreeningSectionDescriptor.DrugOfChoice) ?? InitSection();

        }


        private int GetQuestionValue(int questionId)
        {
            return _section.FindQuestionByID(questionId)?.AnswerValue ?? 0;
        }

        private void SetQuestionValue(int questionId, int value)
        {
            var answer = _section.FindQuestionByID(questionId);
            if (answer == null)
            {
                answer = new ScreeningSectionQuestionResult(questionId, value);
                _section.AppendQuestionAnswer(answer);
            }
            else
            {
                answer.AnswerValue = value;
            }
        }

        public int Primary
        {
            get
            {
                return GetQuestionValue(DrugOfChoiceDescriptor.PrimaryQuestionId);
            }
            set
            {
                SetQuestionValue(DrugOfChoiceDescriptor.PrimaryQuestionId, value);
            }
        }

        public int Secondary
        {
            get
            {
                return GetQuestionValue(DrugOfChoiceDescriptor.SecondaryQuestionId);
            }
            set
            {
                SetQuestionValue(DrugOfChoiceDescriptor.SecondaryQuestionId, value);
            }
        }

        public int Tertiary
        {
            get
            {
                return GetQuestionValue(DrugOfChoiceDescriptor.TertiaryQuestionId);
            }
            set
            {
                SetQuestionValue(DrugOfChoiceDescriptor.TertiaryQuestionId, value);
            }
        }

        public ScreeningSectionResult GetSection()
        {
            var result = _section;


            var tmpPrimary = Primary;
            var tmpSecondary = Secondary;
            var tmpTertiary = Tertiary;

            //reset values to make sure we write correct answers

            if (tmpPrimary > 0)
            {

                Secondary = tmpSecondary;

                Tertiary = tmpSecondary > 0 ? tmpTertiary : 0;

                result.AnswerValue = 1;
            }
            else
            {

                Secondary = 0;
                Tertiary = 0;

                result.AnswerValue = 0;
            }

            return result;
        }
    }

}
