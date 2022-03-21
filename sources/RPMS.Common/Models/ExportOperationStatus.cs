using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPMS.Common.Models
{
    public enum ExportOperationStatus
    {
        Unknown,
        AllSucceed,
        AllFailed,
        SomeOperationsFailed
    };


    /// <summary>
    /// Helper methods for ExportResult and collections
    /// </summary>
    public static class ExportResultExtensions
    {
        public static ExportOperationStatus GetExportOperationStatus(this IEnumerable<ExportResult> exportResults)
        {
            ExportOperationStatus result = ExportOperationStatus.Unknown;
            if (exportResults != null && exportResults.Any())
            {


                var results = exportResults.ToList();

                if (results.All(x => x.IsSuccessful))
                {
                    result = ExportOperationStatus.AllSucceed;
                }
                else if (results.All(x => !x.IsSuccessful))
                {
                    result = ExportOperationStatus.AllFailed;
                }
                else
                {
                    result = ExportOperationStatus.SomeOperationsFailed;
                }

            }
            return result;
        }
    }
}
