using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Utils;
using FluentAssertions;

namespace FrontDesk.Server.Tests.Utils
{
    [TestClass]
    public class FilePathResolverTest
    {
        [TestMethod]
        public void CanResolveWebPathInConsole()
        {
            FilePathResolver.ResolveFilePath("~/images/fd_report_logo_small.png")
                .Should()
                .EndWith(@"\images\fd_report_logo_small.png");
        }
    }
}
