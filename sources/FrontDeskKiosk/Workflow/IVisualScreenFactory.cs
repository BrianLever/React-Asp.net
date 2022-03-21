namespace FrontDesk.Kiosk.Workflow
{
	public interface IVisualScreenFactory
    {
        FrontDesk.Kiosk.Screens.IVisualScreen CreateScreenForStep(ScreeningStep step, ScreeningSection section, IScreeningResultState resultState);
    }
}
