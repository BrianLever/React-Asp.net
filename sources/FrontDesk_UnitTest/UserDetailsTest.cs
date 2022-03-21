using FrontDesk.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace FrontDesk_UnitTest
{


    [Ignore]
    [TestClass()]
    public class FDUserTest
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
        ///A test for GetAllUser
        ///</summary>
        [TestMethod()]
        public void GetAllUserTest()
        {
            DataSet expected = null;
            DataSet actual;
            actual = FDUser.GetAllUser();
            Assert.AreNotEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetUserByID
        ///</summary>
        [TestMethod()]
        public void GetUserByIDTest()
        {
            int userID = 1;
            FDUser user = null;
            user = FDUser.GetUserByID(userID);
            Assert.AreNotEqual(null, user);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            int userID = 1;
            //Get user
            FDUser userDetails = FDUser.GetUserByID(userID);

            //Create update user
            FDUser updateUser = userDetails;
            updateUser.AddressLine1 = userDetails.AddressLine1 + " address 1";
            updateUser.AddressLine2 = userDetails.AddressLine2 + " address 2";
            updateUser.City = userDetails.City + " city";
            updateUser.Comments = userDetails.Comments + " comments";
            updateUser.ContactPhone = userDetails.ContactPhone + " phone";
            updateUser.Email = userDetails.Email + " test@test.com";
            updateUser.FirstName = userDetails.FirstName + " test";
            updateUser.LastName = userDetails.LastName + " test";
            updateUser.MiddleName = userDetails.MiddleName + " test";
            updateUser.PostalCode = userDetails.PostalCode + " 11111";
            updateUser.StateCode = "ID";

            //Get update user and get result
            FDUser.Update(updateUser);
        }
    }
}
