using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScreenDox.EHR.Common.SmartExport.AutoCorrection;

namespace ScreenDox.EHR.Common.Tests.SmartExport.AutoCorrection
{
    public class CorrectionStrategyTestsBase<T> where T: class, IPatientAutoCorrectionStrategy, new()
    {

        protected T Sut()
        {
            return new T();
        }

       
    }
}
