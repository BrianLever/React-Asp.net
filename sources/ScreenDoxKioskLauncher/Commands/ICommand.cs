namespace ScreenDoxKioskLauncher.Commands
{
    public interface ICommand<TResult>
    {
        TResult Run();
    }
}
