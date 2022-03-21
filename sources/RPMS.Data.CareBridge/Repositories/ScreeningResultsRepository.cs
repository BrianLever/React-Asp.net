using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Common.Logging;
using RPMS.Common;
using RPMS.Common.Extensions;
using RPMS.Common.Models;
using RPMS.Data.CareBridge.Dto;
using FrontDesk.Common.Extensions;

namespace RPMS.Data.CareBridge
{
    public class ScreeningResultsRepository : RestApiRepository, IScreeningResultsRepository
    {
        private readonly ILog _logger = LogManager.GetLogger<ScreeningResultsRepository>();
        private readonly ILog _screeningResultLogger = LogManager.GetLogger<ScreeningSectionRequest>();

        protected override string BaseApiUrlSettingPropertyName => "CareBridgeResultsApiUrl";

        public ScreeningResultsRepository(IApiCredentialsService credentialsProvider)
          : base(credentialsProvider, "ScreeningSection")
        {

        }

        public void ExportCrisisAlerts(int patientID, int visitID, List<CrisisAlert> list)
        {
            throw new NotSupportedException();
        }

        public void ExportExams(int patientID, int visitID, List<Exam> list)
        {
            throw new NotSupportedException();
        }

        public void ExportHealthFactors(int patientID, int visitID, List<HealthFactor> list)
        {
            throw new NotSupportedException();
        }

        public void ExportScreeningData(ScreeningResultRecord screeningResultRecord)
        {
            var request = Mapper.Map<ScreeningSectionRequest>(screeningResultRecord);

            _logger.InfoFormat("ExportScreeningData. Sending screening results for Screening ID: {0}", screeningResultRecord.ScreendoxRecordNo);

            // logging results if enabled
            request.LogPayload(_screeningResultLogger);


            //if no data in sections, do no call API. It will generate the error

            if(!screeningResultRecord.Sections.IsNullOrEmpty())
            {
                _logger.WarnFormat("ExportScreeningData. Screening result is empty. Skipped sending result to API. Screening ID: {0}", screeningResultRecord.ScreendoxRecordNo);
                return;
            }

            var response = GetPost<ScreeningSectionRequest, ScreeningSectionResponse>(request);
            
            // logging response
            response.LogPayload(_screeningResultLogger);

            if (response == null ) // not found issue
            {
                throw new ApplicationException($"ExportScreeningData failed. Service returned Not Found status code.  Screening ID: {screeningResultRecord.ScreendoxRecordNo}");
            }

        }
    }
}