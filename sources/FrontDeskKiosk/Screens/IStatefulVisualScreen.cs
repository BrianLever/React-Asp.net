using FrontDesk.Kiosk.Workflow;

namespace FrontDesk.Kiosk.Screens
{
    public interface IStatefulVisualScreen : IVisualScreen
    {
        IScreeningResultState ResultState { get;  set; }
    }
}
