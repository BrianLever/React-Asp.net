using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Dto;
using FluentAssertions;

namespace RPMS.Data.CareBridge.Tests.Dto
{
    [TestCategory("Rpms")]
    [TestCategory("CareBridge_NextGen")]
    [TestClass]
    public class FindPatientDtoTests
    {

        const string FindPatientRequestPayload = "{\"params\":{\"LastName\":\"TEST\",\"DOB\":\"19650201\"}}";

        const string FindPatientResponsePayload = @"
{
""patients"": [
{
            ""RowID"": ""1661"",
            ""HealthRecordNumber"": ""000000000539"",
            ""PatientName"": {
                ""LastName"": ""Test"",
                ""FirstName"": ""Dental Interview 5"",
                ""MiddleName"": ""A""
            },
            ""DOB"": ""19720715"",
            ""StreetAddress"": {
                ""Address1"": ""1305 Remington Road"",
                ""Address2"": ""Suite P"",
                ""City"": ""Schaumburg"",
                ""State"": ""IL "",
                ""Zip"": ""60173""
            },
            ""HomePhone"": ""8474906869"",
            ""OfficePhone"": ""8474906891""
        },
        {
            ""RowID"": ""1574"",
            ""HealthRecordNumber"": ""000000000465"",
            ""PatientName"": {
                ""LastName"": ""Test"",
                ""FirstName"": ""Mother"",
                ""MiddleName"": """"
            },
            ""DOB"": ""19650201"",
            ""StreetAddress"": {
                ""Address1"": ""101 Main St"",
                ""Address2"": """",
                ""City"": ""Chandler"",
                ""State"": ""AZ"",
                ""Zip"": ""85226""
            },
            ""HomePhone"": ""4805555555"",
            ""OfficePhone"": ""4802222222""
        }
]
}";

        [TestMethod]
        public void FindPatient_CanSerializeRequest()
        {
            var model = new RequestWithParam<FindPatientRequest>(new FindPatientRequest
            {
                LastName = "TEST",
                Birthday = new DateTime(1965,02,01)
            });
           
            var actual = JsonConvert.SerializeObject(model);

            actual.Should().Be(FindPatientRequestPayload);
        }

        [TestMethod]
        public void FindPatient_Response_NotNull()
        {
            var actual = JsonConvert.DeserializeObject<FindPatientResponse>(FindPatientResponsePayload);

            actual.Should().NotBeNull();

        }

        [TestMethod]
        public void FindPatient_Response_HasTwoItems()
        {
            var actual = JsonConvert.DeserializeObject<FindPatientResponse>(FindPatientResponsePayload);

            actual.Items.Count.Should().Be(2);
        }


        [TestMethod]
        public void FindPatient_Response_ParseAll()
        {
            var expected = new PatientRecord
            {
                RowID = 1661,
                HealthRecordNumber = "000000000539",
                PatientName = new PatientName
                {
                    LastName = "Test",
                    FirstName = "Dental Interview 5",
                    MiddleName = "A"
                },
                Birthday = new DateTime(1972, 07, 15),
                StreetAddress = new StreetAddress
                {
                    Address1 = "1305 Remington Road",
                    Address2 = "Suite P",
                    City = "Schaumburg",
                    State = "IL",
                    Zip = "60173",
                },
                HomePhone = "8474906869",
                OfficePhone = "8474906891"
            };

            var actual = JsonConvert.DeserializeObject<FindPatientResponse>(FindPatientResponsePayload);

            actual.Items[0].Should().BeEquivalentTo(expected);
        }
    }
}
