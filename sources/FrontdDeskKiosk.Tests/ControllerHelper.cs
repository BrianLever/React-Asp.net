using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Kiosk.Workflow;
using Moq;
using FrontDesk.Kiosk;

namespace FrontdDeskKiosk.Tests
{
    internal static class ControllerHelper
    {
       internal static Mock<IUserSessionTimeoutController> CreateUserSessionTimeoutController()
        {
            var moq = new Mock<IUserSessionTimeoutController>();
            moq.Setup(x => x.SessionTimeoutPeriod).Returns(TimeSpan.FromMinutes(100));
            moq.Setup(x => x.SessionExpiringNotificationTimeout).Returns(TimeSpan.FromMinutes(100));
            moq.Setup(x => x.LastUsedTime).Returns(DateTime.Now);
            moq.Setup(x => x.StartMonitoring()).Verifiable();
            moq.Setup(x => x.StopMonitoring()).Verifiable();


            return moq;
        }
    }
}
