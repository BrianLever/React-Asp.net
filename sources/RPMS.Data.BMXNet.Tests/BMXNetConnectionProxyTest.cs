using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using RPMS.Data.BMXNet.Framework;
using IndianHealthService.BMXNet;

namespace RPMS.Data.BMXNet.Tests
{


    /// <summary>
    ///This is a test class for BMXNetConnectionProxyTest and is intended
    ///to contain all BMXNetConnectionProxyTest Unit Tests
    ///</summary>
    [TestClass]
    [Ignore]
    public class BMXNetConnectionProxyTest
    {

        /// <summary>
        ///A test for OpenConnection
        ///</summary>
        [TestMethod()]
        public void OpenConnectionTest()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            BMXNetConnectionProxy target = new BMXNetConnectionProxy(connectionString);
          
            BMXNetConnection actual;
            actual = target.OpenConnection();
            Assert.IsNotNull(actual);
            Assert.AreEqual(target.BMXRpcProxy, actual.bmxNetLib, "bmxNetLib failed");
            Assert.IsTrue(target.BMXRpcProxy.Connected, "BMXRpcProxy.Connected failed");


            target.CloseConnection();

            Assert.IsFalse(target.BMXRpcProxy.Connected, "BMXRpcProxy.Connected failed after disconnected");

           
        }
    }
}
