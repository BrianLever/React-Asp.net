using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;
using RPMS.Common.Models;

namespace RPMS.Common.Export
{
    public abstract class AbstractHealthFactorCalculator : AbstractCalculator<HealthFactor>, IHealthFactorCalculator
    {
        public AbstractHealthFactorCalculator()
        {
           
        }

       
    }
}
