using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Dto;
using FluentAssertions;

namespace RPMS.Data.CareBridge.Tests.Dto
{
    [TestCategory("RPMS")]
    [TestCategory("CareBridge_NextGen")]
    [TestClass]
    public class GetPatientRecordDtoTests
    {

        const string GetPatientRecordRequestPayload = "{\"params\":{\"RowID\":1663}}";

        const string GetPatientRecordResponsePayload = @"
{
""patients"": [
{
            ""RowID"": ""1663"",
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
        }
]
}";

        [TestMethod]
        public void GetPatientRecord_CanSerializeRequest()
        {
            var model = new RequestWithParam<GetPatientRequest>(new GetPatientRequest
            {
                PatientId = 1663
            });
           
            var actual = JsonConvert.SerializeObject(model);

            actual.Should().Be(GetPatientRecordRequestPayload);
        }

        [TestMethod]
        public void GetPatientRecord_Response_NotNull()
        {
            var actual = JsonConvert.DeserializeObject<GetPatientResponse>(GetPatientRecordResponsePayload);

            actual.Should().NotBeNull();

        }

        [TestMethod]
        public void GetPatientRecord_Response_HasTwoItems()
        {
            var actual = JsonConvert.DeserializeObject<GetPatientResponse>(GetPatientRecordResponsePayload);

            actual.Patients.Count.Should().Be(1);
        }


        [TestMethod]
        public void GetPatientRecord_Response_ParseAll()
        {
            var expected = new PatientRecord
            {
                RowID = 1663,
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

            var actual = JsonConvert.DeserializeObject<GetPatientResponse>(GetPatientRecordResponsePayload);

            actual.Patients[0].Should().BeEquivalentTo(expected);
        }
    }
}
