using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrontDesk.Common;

namespace FrontDesk.Kiosk.Services
{
    public interface ITypeaheadDataSourceBase<T> where T : class
    {
        ReadOnlyCollection<T> GetAll();
        T GetByName(string textInput);
        List<T> GetByPartOfName(string startsWithInput);
        void Refresh();
    }

    public interface ILookupDataSourceBase : ITypeaheadDataSourceBase<LookupValue> 
    {
    }
}