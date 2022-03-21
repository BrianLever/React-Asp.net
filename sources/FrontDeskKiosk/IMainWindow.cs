namespace FrontDesk.Kiosk
{
    public interface IMainWindow
    {
        /// <summary>
        /// True if this is Main flow window. False if this is error message window.
        /// </summary>
        bool IsSucceedFlow { get; }

        void Show();
    }
}