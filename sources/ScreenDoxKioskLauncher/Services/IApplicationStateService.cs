namespace ScreenDoxKioskLauncher.Services
{
    public interface IApplicationStateService
    {
        KioskApplicationState GetState();
        void SetState(KioskApplicationState state);

        bool IsInNormalState();
    }
}