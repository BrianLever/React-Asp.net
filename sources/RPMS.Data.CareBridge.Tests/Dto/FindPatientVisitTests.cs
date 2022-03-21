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
    public class FindPatientVisitTests
    {

        const string FindPatientVisitRequestPayload = "{\"params\":{\"RowID\":1663,\"PatientName\":{\"LastName\":\"Test\",\"FirstName\":\"Prod\",\"MiddleName\":\"\"}}}";

        const string FindPatientVisitResponsePayload = @"
{
""visits"": [
       {
            ""VisitID"": ""1173"",
            ""AdmitDate"": ""20170119140418"",
            ""ServiceCategory"": ""Psychotherapy - Group"",
            ""PatientName"": {
                ""LastName"": ""Test"",
                ""FirstName"": ""Prod"",
                ""MiddleName"": """"
            },
            ""LocationName"": ""Westside Medical Office"",
            ""DOB"": ""19480819""
        },
        {
            ""VisitID"": ""1060"",
            ""AdmitDate"": ""20160829172846"",
            ""ServiceCategory"": """",
            ""PatientName"": {
                ""LastName"": ""Test"",
                ""FirstName"": ""Prod"",
                ""MiddleName"": """"
            },
            ""LocationName"": ""CHC Medical"",
            ""DOB"": ""19480819""
        }
]
}";

        [TestMethod]
        public void FindPatient_CanSerializeRequest()
        {
            var model = new RequestWithParam<FindPatientVisitRequest>(new FindPatientVisitRequest
            {
                PatientId = 1663,
                PatientName = new PatientName
                {
                    LastName = "Test",
                    FirstName = "Prod"
                }
            });
           
            var actual = JsonConvert.SerializeObject(model);

            actual.Should().Be(FindPatientVisitRequestPayload);
        }

        [TestMethod]
        public void FindPatient_Response_NotNull()
        {
            var actual = JsonConvert.DeserializeObject<FindPatientResponse>(FindPatientVisitResponsePayload);

            actual.Should().NotBeNull();

        }

        [TestMethod]
        public void FindPatient_Response_HasTwoItems()
        {
            var actual = JsonConvert.DeserializeObject<FindPatientVisitResponse>(FindPatientVisitResponsePayload);

            actual.Items.Count.Should().Be(2);
        }


        [TestMethod]
        public void FindPatient_Response_ParseAll()
        {
            var expected = new PatientVisitRecord
            {
                VisitId = 1173,
                AdmitionDate = new DateTime(2017, 01, 19, 14,4,18),
                ServiceCategory = "Psychotherapy - Group",
                PatientName = new PatientName
                {
                    LastName = "Test",
                    FirstName = "Prod",
                    MiddleName = ""
                },
                LocationName = "Westside Medical Office",
                Birthday = new DateTime(1948, 08, 19),
            };

            var actual = JsonConvert.DeserializeObject<FindPatientVisitResponse>(FindPatientVisitResponsePayload);

            actual.Items[0].Should().BeEquivalentTo(expected);
        }
    }
}
