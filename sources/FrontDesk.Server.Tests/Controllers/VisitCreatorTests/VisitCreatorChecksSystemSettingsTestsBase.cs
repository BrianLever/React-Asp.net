using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace FrontDesk.Server.Tests.Controllers.VisitCreatorTests
{
    [TestClass]
    public abstract class VisitCreatorChecksSystemSettingsTestsBase
    {
        protected Mock<IVisitSettingsService> _visitSettingsMock = new Mock<IVisitSettingsService>();
        protected Mock<IBhsVisitFactory> _bhsVisitFactoryMock = new Mock<IBhsVisitFactory>();

        protected FrontDesk.Screening _screeningInfo = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();

        public VisitCreatorChecksSystemSettingsTestsBase()
        {
            _bhsVisitFactoryMock.Setup(x => x.Create(It.IsAny<ScreeningResult>(), It.IsAny<FrontDesk.Screening>())).Returns(new BhsVisit());
        }

        protected virtual VisitCreator Sut()
        {
            return new VisitCreator(_visitSettingsMock.Object, _bhsVisitFactoryMock.Object);
        }



    }
}
