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
    public class UpdatePatientDtoTests
    {

        const string UpdatePatientRequestPayload = "{\"params\":{\"RowID\":1654,\"AddressChangeDate\":\"20181221\",\"AddressChangeReason\":\"addressreason\",\"PatientName\":{\"LastName\":\"Test\",\"FirstName\":\"Child\",\"MiddleName\":\"Mock\"},\"StreetAddress\":{\"Address1\":\"1234 Testing Way\",\"Address2\":\"\",\"City\":\"Mount Sidney\",\"State\":\"VA\",\"Zip\":\"24467\"},\"HomePhone\":\"5405556490\",\"OfficePhone\":\"5405556488\"}}";

        const string UpdatePatientResponsePayload = @"
{
    ""status"": ""SuccessfullyChanged""
}";

        [TestMethod]
        public void UpdatePatient_CanSerializeRequest()
        {
            var model = new RequestWithParam<UpdatePatientRequest>(new UpdatePatientRequest
            {
                RowId = 1654,
                AddressChangeDate = new DateTime(2018, 12,21),
           
                PatientName = new PatientName
                {
                    LastName = "Test",
                    FirstName = "Child",
                    MiddleName = "Mock"
                },
                StreetAddress = new StreetAddress
                {
                    Address1 = "1234 Testing Way",
                    Address2 = "",
                    City = "Mount Sidney",
                    State = "VA ",
                    Zip = "24467",
                },
                HomePhone = "5405556490",
                OfficePhone = "5405556488"
            });

            var actual = JsonConvert.SerializeObject(model);

            actual.Should().Be(UpdatePatientRequestPayload);
        }
       
        [TestMethod]
        public void UpdatePatient_Response_NotNull()
        {
            var actual = JsonConvert.DeserializeObject<GetPatientResponse>(UpdatePatientResponsePayload);

            actual.Should().NotBeNull();

        }


        [TestMethod]
        public void UpdatePatient_Response_CanParse()
        {
            var expected = new UpdatePatientResponse
            {
               Status = UpdatePatientResponseStatus.SuccessfullyChanged
            };

            var actual = JsonConvert.DeserializeObject<UpdatePatientResponse>(UpdatePatientResponsePayload);

            actual.Should().BeEquivalentTo(expected);
        }
      
    }
}
