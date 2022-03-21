namespace FrontDesk.Kiosk.Screens
{
	public interface IVisualThankYouScreen
    {
        bool IsPositiveResult { get; set; }
        void BeginDataBinding();
        void EndDataBinding();
    }
}
