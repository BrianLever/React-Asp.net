using ScreenDox.Server.Models;
using ScreenDox.Server.Models.SearchFilters;

namespace ScreenDox.Server.Services
{
    public interface IErrorLogService
    {
        SearchResponse<ErrorLogItem> GetAll(PagedDateRangeNameFilter filter);
        ErrorLogItem Get(long id);
        void DeleteAll();
    }
}