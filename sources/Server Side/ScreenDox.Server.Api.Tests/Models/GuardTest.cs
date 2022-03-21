using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Models;

using System;
using System.Web.Http;

namespace ScreenDox.Server.Api.Tests.Models
{
    [TestClass]
    public class GuardTest
    {

        private readonly string[] SupportedCollumns = new[] { "FullName", "ExportedDate" };

        [TestMethod]
        public void Guard_ValidateOrderByClause_Valid()
        {
            var payload = new PagedScreeningResultFilterModel
            {
                OrderBy = "FullName ASC"
            };

            Action act = () => Guard.ValidateOrderByClause(payload, SupportedCollumns);

            act.Should().NotThrow();
        }

        [TestMethod]
        public void Guard_ValidateOrderByClause_SeveralCollums_Should_Throw()
        {
            var payload = new PagedScreeningResultFilterModel
            {
                OrderBy = "FullName ASC, LastCheckInDate DESC"
            };

            Action act = () => Guard.ValidateOrderByClause(payload, SupportedCollumns);

            act.Should().Throw<HttpResponseException>();
        }
    }
}
