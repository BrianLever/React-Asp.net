using RPMS.Data.BMXNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RPMS.Data.BMXNet.Framework;

namespace RPMS.Data.BMXNet.Tests
{
    
    
    /// <summary>
    ///This is a test class for BMXNetConnectionBuilderTest and is intended
    ///to contain all BMXNetConnectionBuilderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BMXNetConnectionBuilderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for FromConnectionString
        ///</summary>
        [TestMethod()]
        public void FromConnectionStringTest()
        {
            string connectionString = "Server=68.105.77.142; Port=10501; Namespace=TRG; Access code=rpullen1; Verify code=Frontdesk-1;";
            ConnectionInfo expected = new ConnectionInfo
            {
                ServerAddress = "68.105.77.142",
                Port = 10501,
                Namespace = "TRG",
                AccessCode = "rpullen1",
                VerifyCode = "Frontdesk-1"
            };
            ConnectionInfo actual;
            actual = BMXNetConnectionBuilder.FromConnectionString(connectionString);
           
            Assert.AreEqual(expected.ServerAddress, actual.ServerAddress);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Namespace, actual.Namespace);
            Assert.AreEqual(expected.AccessCode, actual.AccessCode);
            Assert.AreEqual(expected.VerifyCode, actual.VerifyCode);
            Assert.AreEqual(expected.Division, actual.Division);
            Assert.AreEqual(expected.AppContext, actual.AppContext);


        }


        /// <summary>
        ///A test for FromConnectionString
        ///</summary>
        [TestMethod()]
        public void Parse_String_Without_Access()
        {
            string connectionString = "Server=68.105.77.142; Port=10501; Namespace=TRG;";
            ConnectionInfo expected = new ConnectionInfo
            {
                ServerAddress = "68.105.77.142",
                Port = 10501,
                Namespace = "TRG",
                AccessCode = null,
                VerifyCode = null
            };
            ConnectionInfo actual;
            actual = BMXNetConnectionBuilder.FromConnectionString(connectionString);

            Assert.AreEqual(expected.ServerAddress, actual.ServerAddress);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Namespace, actual.Namespace);
            Assert.AreEqual(expected.AccessCode, actual.AccessCode);
            Assert.AreEqual(expected.VerifyCode, actual.VerifyCode);
            Assert.AreEqual(expected.Division, actual.Division);
            Assert.AreEqual(expected.AppContext, actual.AppContext);


        }
    }
}
