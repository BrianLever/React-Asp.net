using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.Controllers.VisitFactoryTests
{
    [TestClass]
    public abstract class VisitFactoryTestsBase
    {
        protected FrontDesk.Screening _screeningInfo = MotherObjects.ScreeningInfoMotherObject.GetFullScreening();

        public VisitFactoryTestsBase()
        {
           
        }

        public virtual BhsVisitFactory Sut()
        {
            return new BhsVisitFactory();
        }



    }
}
