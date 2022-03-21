using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Tests.MotherObjects;
using Moq;

namespace FrontDesk.Server.Tests.Controllers.VisitCreatorTests
{
    [TestClass]
    public class WhenVisitSettingsOnAndOff_TobaccoSmokeless : WhenVisitSettingsOnAndOff_TobaccoBase
    {
        protected override string TobaccoQuestionSettingId => VisitSettingsDescriptor.TobaccoUseSmokeless;
        protected override int TobaccoQuestionId => TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID;

        [TestMethod]
        public void When_TobaccoSmokeless_isOn_AndPositive_VisitCreated()
        {
            base.When_Tobacco_isOn_AndPositive_VisitCreated();
        }
        [TestMethod]
        public void When_TobaccoSmokeless_isOn_AndNegative_VisitNotCreated()
        {
            base.When_Tobacco_isOn_AndNegative_VisitNotCreated();
        }
        [TestMethod]
        public void When_TobaccoSmokeless_isOff_AndPositive_VisitNotCreated()
        {
            base.When_Tobacco_isOff_AndPositive_VisitNotCreated();
        }
    }
}
