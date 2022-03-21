using FluentAssertions;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Extensions.JsonContractResolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Tests.Extensions
{
    [TestClass]
    public class ContextTypeOnlyJsonContractResolverTests
    {
        protected ScreeningResult GetModel()
        {
            var data = new ScreeningResult
            {
                FirstName = "John",
                LastName = "Doe",
                Birthday = new DateTime(1972, 02, 15),
                StreetAddress = "123 Some street",
                StateID = "IL",
                ZipCode = "111",
                ExportedToHRN = "0123456789",
                CreatedDate = new DateTimeOffset(new DateTime(2020, 01, 10), TimeSpan.FromHours(2)),
                KioskID = 1001,
            };

            data.SectionAnswers.Add(new ScreeningSectionResult
            {
                ScreeningSectionID = ScreeningSectionDescriptor.Tobacco,
                AnswerValue = 1,
                Score = 1,
            });

            return data;
        }

        [TestMethod]
        public void JsonIncludesPropertiesOfTargetType()
        {
            var model = GetModel();

            var jsonModel = (ScreeningPatientIdentityWithAddress)model;

            var json = jsonModel.ToJsonStrict();

            json.Should().Contain(@"""StateID"":""IL""");
        }

        [TestMethod]
        public void JsonIncludesPropertiesOfDerivedType()
        {
            var model = GetModel();

            var jsonModel = (ScreeningPatientIdentityWithAddress)model;

            var json = jsonModel.ToJsonStrict();

            json.Should().Contain(@"""FirstName"":""John""");
        }

        [TestMethod]
        public void JsonIgnoresPropertiesOfDerivedType()
        {
            var model = GetModel();

            var jsonModel = (ScreeningPatientIdentityWithAddress)model;

            var json = jsonModel.ToJsonStrict();

            json.Should().NotContain("KioskID");
        }


        [TestMethod]
        public void JsonIgnoresListsOfDerivedType()
        {
            var model = GetModel();

            var jsonModel = (ScreeningPatientIdentityWithAddress)model;

            var json = jsonModel.ToJsonStrict();

            json.Should().NotContain("_sectionAnswers");
        }

    }
}
