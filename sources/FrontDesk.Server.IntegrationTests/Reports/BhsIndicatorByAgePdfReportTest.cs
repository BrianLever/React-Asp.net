using System;
using FrontDesk.Server.Reports;
using System.IO;
using FluentAssertions;
using Moq;
using FrontDesk.Server.Screening.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Server.Screening.Models;

namespace FrontDesk.Server.Tests.Reports
{
    [DeploymentItem(@"Reports\checked.gif", "images")]
    [DeploymentItem(@"Reports\unchecked.gif", "images")]
    [DeploymentItem(@"Reports\logo_dark_pdf.png", "images")]
    [DeploymentItem(@"Reports\fd_report_logo_small.png", "images")]
    [DeploymentItem(@"Reports\TTFFonts\arial.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\arialbd.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\arialbi.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\ariali.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\times.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\timesbd.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\timesbi.ttf", @"App_Data\TTFFonts")]
    [DeploymentItem(@"Reports\TTFFonts\timesi.ttf", @"App_Data\TTFFonts")]
    [TestClass]
    public class BhsIndicatorByAgePdfReportTest
    {
        protected Mock<IScreeningResultService> screeningResultServiceMock = new Mock<IScreeningResultService>();
        protected Mock<IScreeningDefinitionService> screeningDefinitionServiceMock = new Mock<IScreeningDefinitionService>();


        public BhsIndicatorByAgePdfReportTest()
        {
            screeningResultServiceMock.Setup(x => x.GetMinDate()).Returns(new DateTimeOffset(2015, 01, 08, 9, 10, 20, TimeSpan.FromHours(2)));
            screeningDefinitionServiceMock.Setup<FrontDesk.Screening>(x => x.Get()).Returns(new FrontDesk.ServerScreening
            {

            });
        }


        protected BhsIndicatorByAgePdfReport Sut()
        {
            var filter = new SimpleFilterModel
            {
                Location = null,
                StartDate = new DateTime(2015, 07, 01),
                EndDate = new DateTime(2016, 06, 30),
            };

            return new BhsIndicatorByAgePdfReport(filter, false);
        }

        [TestMethod]
        public void BhsIndicatorByAge_PrintGeneratesReportWithAtLeast1Kilobyte()
        {
            using (var stream = new MemoryStream())
            {
                Sut().CreatePDF(stream);

                var data = stream.ToArray();
                data.Length.Should().BeGreaterThan(1024);
            }
        }
    }
}
