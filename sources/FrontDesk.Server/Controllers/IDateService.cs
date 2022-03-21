using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Controllers
{
    public interface IDateService
    {
        DateTime GetCurrentDate();
    }

    public class DefaultDateService : IDateService
    {
        public DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }
    }

}
