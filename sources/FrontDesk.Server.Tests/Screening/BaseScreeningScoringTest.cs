using FrontDesk.Server.Screening;
using FrontDesk.Server.Tests.Helpers;
using FrontDesk.Server.Tests.MotherObjects;
using Moq;

namespace FrontDesk.Server.Tests.Screening
{
    public abstract class BaseScreeningScoringTest
    {
        protected Mock<IScreeningScoreLevelRepository> repositoryMock = new Mock<IScreeningScoreLevelRepository>();

        protected FrontDesk.Screening ScreeningInfo;

        public BaseScreeningScoringTest()
        {
            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("CAGE")).Returns(
                    ScreeningScoreLevelRepositoryFactory.CreateCageItems());

            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("DAST")).Returns(
                ScreeningScoreLevelRepositoryFactory.CreateDastItems());

            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("HITS")).Returns(
                ScreeningScoreLevelRepositoryFactory.CreateHitsItems());

            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("PHQ-9")).Returns(
                ScreeningScoreLevelRepositoryFactory.CreatePhq9Items());

            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("SIH ")).Returns(
                ScreeningScoreLevelRepositoryFactory.CreateSihItems());


            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID("TCC")).Returns(
                ScreeningScoreLevelRepositoryFactory.CreateTccItems());

            repositoryMock.Setup(x => x.GetScoreLevelsBySectionID(ScreeningSectionDescriptor.Anxiety)).Returns(
               ScreeningScoreLevelRepositoryFactory.CreateAnxietyItems());


            ScreeningInfo = ScreeningInfoMotherObject.GetFullScreening();
        }
    }
}
