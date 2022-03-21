using Common.Logging;
using Frontdesk.Server.SmartExport.Data;
using FrontDesk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frontdesk.Server.SmartExport.Services
{
    public interface ISmartExportService
    {
        List<long> GetScreeningResultIdForExport(int batchSize);
    }

    public class SmartExportService : ISmartExportService
    {
        public const int DefaultBatchSize = 100;

        private readonly ILog _logger = LogManager.GetLogger<SmartExportService>();

        private readonly ISmartExportRepository _repository;


        public SmartExportService(ISmartExportRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public SmartExportService(): this(new SmartExportDb())
        {

        }



        public List<long> GetScreeningResultIdForExport(int batchSize)
        {
            _logger.DebugFormat($"[SmartExportService] Calling GetDataForExport, batchSize: {0}", batchSize);


            if (batchSize <= 0) { batchSize = DefaultBatchSize }
            
            var result = _repository.GetScreeningResultsForExport();

            _logger.InfoFormat("[SmartExportService] Found {0} items for export.", result.Count);

            if (result.Count > batchSize)
            {
                result = result.Take(batchSize).ToList();
            }

            return result;
        }

    }
}
