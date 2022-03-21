using Common.Logging;
using ScreenDoxKioskLauncher.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Commands
{
    public abstract class BaseCommand<T> : ICommand<T>
    {
        protected readonly IEnvironmentProvider _environmentProvider;
        protected readonly ILog _logger = LogManager.GetLogger("LaunchCommans");

        public BaseCommand(IEnvironmentProvider environmentProvider)
        {
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
        }

        public abstract T Run();
        

        protected string GetKioskRuntimeDirectory()
        {
            return _environmentProvider.KioskApplicationDirectoryPath;
        }

        protected string GetKioskExePath()
        {
            return Path.Combine(_environmentProvider.KioskApplicationDirectoryPath, _environmentProvider.KioskExeName);
        }

        protected string GetKioskProcessName()
        {
            return Path.GetFileNameWithoutExtension(_environmentProvider.KioskExeName);
        }

    }
}
