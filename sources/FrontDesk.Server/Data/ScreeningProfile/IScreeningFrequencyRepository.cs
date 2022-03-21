using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Configuration
{
    public interface IScreeningFrequencyRepository
    {
        void Save(IEnumerable<ScreeningFrequencyItem> items);

        ScreeningFrequencyItem Get(string ID);
        IEnumerable<ScreeningFrequencyItem> GetAll();
    }
}
