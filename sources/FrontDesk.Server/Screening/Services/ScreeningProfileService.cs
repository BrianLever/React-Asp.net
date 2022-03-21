namespace FrontDesk.Server.Screening.Services
{
    using FrontDesk.Server.Data.ScreeningProfile;

    using global::Common.Logging;

    using ScreenDox.Server.Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IScreeningProfileService
    {
        int Add(ScreeningProfile model);
        int CountAll(string filterByName);
        bool Delete(int id);
        ScreeningProfile Get(int ID);

        [Obsolete("deprecated")]
        List<ScreeningProfile> GetAll(string filterByName, int startRowIndex, int maximumRows, string orderBy);
        /// <summary>
        /// Get search results
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        SearchResponse<ScreeningProfile> GetAll(SearchByNamePagedSearchFilter filter);

        bool Update(ScreeningProfile model);

        List<ScreeningProfile> GetAll();
    }

    public class ScreeningProfileService : IScreeningProfileService
    {
        private static ILog _logger = LogManager.GetLogger("ScreeningProfileService");
        private readonly IScreeningProfileRepository _repository;


        public ScreeningProfileService() : this(new ScreeningProfileDb())
        {

        }

        public ScreeningProfileService(IScreeningProfileRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public ScreeningProfile Get(int ID)
        {
            return _repository.Get(ID);
        }

        /// <summary>
        /// Get items for search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<ScreeningProfile> GetAll(SearchByNamePagedSearchFilter filter)
        {
            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var result = new SearchResponse<ScreeningProfile>();

            result.TotalCount = _repository.CountAll(filter.FilterByName);

            result.Items = result.TotalCount > 0 ?
                _repository.GetAll(filter.FilterByName, filter.StartRowIndex, filter.MaximumRows, filter.OrderBy)
                : new List<ScreeningProfile>();

            return result;
        }

        public List<ScreeningProfile> GetAll(string filterByName, int startRowIndex, int maximumRows, string orderBy)
        {
            return _repository.GetAll(filterByName, startRowIndex, maximumRows, orderBy);
        }

        /// <summary>
        /// Binding for drop down lists
        /// </summary>
        /// <returns></returns>
        public List<ScreeningProfile> GetAll()
        {
            var items = new List<ScreeningProfile>();

            var dbItems = GetAll(null, 0, 100, "Name");

            // adding Default first
            items.Add(dbItems.First(x => x.ID == ScreeningProfile.DefaultProfileID));
            items.AddRange(dbItems.Where(x => x.ID != ScreeningProfile.DefaultProfileID));

            return items;
        }

        public int CountAll(string filterByName)
        {
            return _repository.CountAll(filterByName);
        }

        public int Add(ScreeningProfile model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _repository.Add(model);
        }
        public bool Update(ScreeningProfile model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return _repository.Update(model);
        }
        public bool Delete(int id)
        {
            return _repository.Delete(id);
        }
    }
}
