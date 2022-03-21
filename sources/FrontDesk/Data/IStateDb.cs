using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FrontDesk.Data
{
    internal interface IStateDb
    {
        DataSet GetAllState();
        List<State> GetList();
        State GetByStateCode(string stateCode);
    }
}
