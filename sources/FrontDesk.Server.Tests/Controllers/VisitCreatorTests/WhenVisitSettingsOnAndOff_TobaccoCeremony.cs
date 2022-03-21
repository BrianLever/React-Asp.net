using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Tests.Controllers.VisitCreatorTests
{
    [TestClass]
    public class WhenVisitSettingsOnAndOff_TobaccoCeremony : WhenVisitSettingsOnAndOff_TobaccoBase
    {
        protected override string TobaccoQuestionSettingId => VisitSettingsDescriptor.TobaccoUseCeremony;
        protected override int TobaccoQuestionId => TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID;

        [TestMethod]
        public void When_TobaccoCeremony_isOn_AndPositive_VisitCreated()
        {
            base.When_Tobacco_isOn_AndPositive_VisitCreated();
        }
        [TestMethod]
        public void When_TobaccoCeremony_isOn_AndNegative_VisitNotCreated()
        {
            base.When_Tobacco_isOn_AndNegative_VisitNotCreated();
        }
        [TestMethod]
        public void When_TobaccoCeremony_isOff_AndPositive_VisitNotCreated()
        {
            base.When_Tobacco_isOff_AndPositive_VisitNotCreated();
        }
    }
}
