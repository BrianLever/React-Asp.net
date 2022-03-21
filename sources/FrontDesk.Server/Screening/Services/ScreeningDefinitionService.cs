using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Services
{
    public interface IScreeningDefinitionService
    {
       FrontDesk.Screening Get();
       List<ScreeningSection> GetSections();
    }


    public class ScreeningDefinitionService : IScreeningDefinitionService
    {
        public FrontDesk.Screening Get()
        {
            var result = ServerScreening.Get();

            replaceDrugOfChoiceQuestion(result);

            return result;
        }


        public List<ScreeningSection> GetSections()
        {
            var result = ServerScreening.GetSections();

            replaceDrugOfChoiceQuestion(result);

            return result;
        }

        private void replaceDrugOfChoiceQuestion(FrontDesk.Screening info)
        {
            replaceDrugOfChoiceQuestion(info.Sections);
        }

        private void replaceDrugOfChoiceQuestion(List<ScreeningSection> sections)
        {
            var section = sections.FirstOrDefault(x => x.ScreeningSectionID == ScreeningSectionDescriptor.DrugOfChoice);

            if (section == null) return;

            section.Questions.ForEach(x =>
            {
                var questionText = string.Empty;

                switch (x.QuestionID)
                {
                    case 1:
                        questionText = "Primary";
                        break;
                    case 2:
                        questionText = "Secondary";
                        break;
                    case 3:
                        questionText = "Tertiary";
                        break;
                }
                x.QuestionText = questionText;
            });
        }
    }

}
