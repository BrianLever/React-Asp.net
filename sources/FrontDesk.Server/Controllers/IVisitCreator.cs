using FrontDesk.Common.Bhservice;

namespace FrontDesk.Server.Controllers
{
    public interface IVisitCreator
    {
        BhsVisit Create(ScreeningResult result, FrontDesk.Screening screeningInfo);
    }
}