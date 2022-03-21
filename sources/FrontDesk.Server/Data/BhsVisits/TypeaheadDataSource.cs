using System.Collections.Generic;

namespace FrontDesk.Server.Data.BhsVisits
{
    public interface ITypeaheadDataSourceFactory
    {
        ITypeaheadDataSource Counties();
        List<string> GetCounty(string query = null);
        List<string> GetTribe(string query = null);
        ITypeaheadDataSource Tribes();
    }

    public class TypeaheadDataSource : ITypeaheadDataSourceFactory
    {   
        public List<string> GetTribe(string query = null)
        {
            return new TypeaheadDb("Tribe").GetList(query);
        }

        public ITypeaheadDataSource Tribes()
        {
            return new TypeaheadDb("Tribe");
        }

        public List<string> GetCounty(string query = null)
        {
            return new TypeaheadDb("County").GetList(query);
        }

        public ITypeaheadDataSource Counties()
        {
            return new TypeaheadDb("County");
        }
    }
}
