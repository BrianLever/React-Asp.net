using AutoMapper;

using Common.Logging;

using FrontDesk.Server.Data.Logging;

using ScreenDox.Server.Models;
using ScreenDox.Server.Models.SearchFilters;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ScreenDox.Server.Services
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly ILog _logger = LogManager.GetLogger<ErrorLogService>();
        private readonly IErrorLogRepository _repository;


        public ErrorLogService(
            IErrorLogRepository repository
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get unique patient records that has screenings in given period of time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<ErrorLogItem> GetAll(PagedDateRangeNameFilter filter)
        {
            var result = new SearchResponse<ErrorLogItem>();

            filter = filter ?? new PagedDateRangeNameFilter();

            if (!filter.StartDate.HasValue)
            {
                filter.StartDate = DateTimeOffset.MinValue;
            }
            if (!filter.EndDate.HasValue)
            {
                filter.EndDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            result.TotalCount = GetCount(filter.StartDate, filter.EndDate);

            result.Items = GetList(filter.StartDate, filter.EndDate, filter.StartRowIndex, filter.MaximumRows);

            return result;
        }

        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">End date. If null, select all items till the next day after today</param>
        /// <returns></returns>
        protected List<ErrorLogItem> GetList(DateTimeOffset? startDate,
                                          DateTimeOffset? endDate,
                                          int startRowIndex,
                                          int maximumRows)
        {
            return _repository.Get(startDate.Value, endDate.Value, startRowIndex, maximumRows)
                .Select(x => Mapper.Map<ErrorLogItem>(x))
                .ToList();
        }

        protected int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTimeOffset.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                endDate = endDate.Value.AddDays(1);
            }

            return _repository.GetCount(startDate.Value, endDate.Value);
        }

        /// <summary>
        /// Get single error log item
        /// </summary>
        public ErrorLogItem Get(long errorLogID)
        {
            return Mapper.Map<ErrorLogItem>(_repository.Get(errorLogID));
        }

        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">End date. If null, select all items till the next day after today</param>
        /// <returns></returns>
        public DataSet GetForExport(DateTimeOffset? startDate, DateTimeOffset? endDate, int startRowIndex, int maximumRows)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTimeOffset.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                endDate = endDate.Value.AddDays(1);
            }

            return _repository.GetAsDataSet(startDate.Value, endDate.Value, startRowIndex, maximumRows);
        }

        /// <summary>
        /// Get single error log item
        /// </summary>
        public void DeleteAll()
        {
            _repository.DeleteAll();
        }



    }
}
