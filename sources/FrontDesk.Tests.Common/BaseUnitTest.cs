using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Tests.Common
{
    public abstract class BaseUnitTest<T>
    {
        protected T sut;

        protected abstract void construct();

        protected virtual void given() { }

        protected virtual void when() { }


        [TestInitialize]
        public void OnSetup()
        {
            construct();
            given();
            when();
        }
    }
}
