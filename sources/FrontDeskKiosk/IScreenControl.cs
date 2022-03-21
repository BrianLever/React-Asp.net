namespace FrontDesk.Kiosk
{
	public interface IScreenControl
    {
        void Show();
        void Hide();
        bool IsVisible { get; }
    }
}
