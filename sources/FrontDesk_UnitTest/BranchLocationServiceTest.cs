using FrontDesk.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using FrontDesk.Server.Screening.Services;
using FrontDesk;
using ScreenDox.Server.Common.Services;
using ScreenDox.Server.Common.Models;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for BranchLocationTest and is intended
    ///to contain all BranchLocationTest Unit Tests
    ///</summary>
    [TestClass()]
    [Ignore]
    public class BranchLocationServiceTest
    {
        private readonly KioskService kioskService = new KioskService();

        private BranchLocationService Sut()
        {
            return new BranchLocationService();
        }

        [TestCategory("E2E")]
        [TestMethod()]
        public void AddTest()
        {
            // 1. Create OK
            BranchLocation model = new BranchLocation()
            {
                Name = "Test branch location",
                Description = "Test branch location description"
            };

            var sut = Sut();

            int locationID = sut.Add(model);
            Assert.AreNotEqual(0, locationID);
            Assert.AreNotEqual(0, model.BranchLocationID);

            // 2. Too long name
            model = new BranchLocation()
            {
                Name = "Too long name for Test branch location Test branch location Test branch location Test branch location",
                Description = "Test branch location description"
            };

            try
            {
                locationID = sut.Add(model);
            }
            catch (SqlException ex)
            {
                //if (!ex.Message.StartsWith("String or binary data would be truncated."))
                if (ex.Number != 8152)
                {
                    throw;
                }
            }

            //Assert.IsFalse(success);  // got exception - no correct return value
            Assert.AreEqual(0, model.BranchLocationID);


            // 3. Null name
            model = new BranchLocation()
            {
                Name = null,
                Description = null
            };

            try
            {
                locationID = sut.Add(model);
            }
            catch (SqlException ex)
            {
                if (ex.Number != 8178)
                {
                    throw;
                }
            }

            Assert.AreEqual(0, model.BranchLocationID);

            // 4. another create OK
            model = new BranchLocation()
            {
                Name = "Test branch location",
                Description = null
            };

            locationID = sut.Add(model);

            Assert.AreNotEqual(0, locationID);
            Assert.AreNotEqual(0, model.BranchLocationID);
        }

        /// <summary>
        ///A test for Get
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void GetTest()
        {
            // 1. Create first
            BranchLocation model = new BranchLocation()
            {
                Name = "Test branch location, get",
                Description = "Test branch location description, get"
            };

            var sut = Sut();

            int locationID = sut.Add(model);
            Assert.AreNotEqual(0, locationID);

            // 2. Read from DB
            BranchLocation expected = new BranchLocation(locationID)
            {
                //BranchLocationID = target.BranchLocationID,
                Name = model.Name,
                Description = model.Description
            };

            BranchLocation actual = sut.Get(locationID);

            Assert.AreEqual(expected.BranchLocationID, actual.BranchLocationID);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        /// <summary>
        ///A test for GetAll
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void GetAllTest()
        {
            var sut = Sut();

            // 1. Insert at least one
            BranchLocation model = new BranchLocation()
            {
                Name = "Test branch location, get",
                Description = "Test branch location description, get"
            };

            int locationID = sut.Add(model);
            Assert.AreNotEqual(0, locationID);

            // 2. get all
            List<BranchLocation> actual;
            actual = sut.GetAll();
            Assert.AreNotEqual(0, actual.Count);
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void DeleteTest()
        {
            var sut = Sut();

            // 1. Insert at least one
            BranchLocation model = new BranchLocation()
            {
                Name = "Test branch location, delete",
                Description = "Test branch location description, delete"
            };

            int locationID = sut.Add(model);
            Assert.AreNotEqual(0, locationID);

            // 2. delete it
            bool success = sut.Delete(locationID);
            Assert.IsTrue(success);

            // 3. delete nonexistent
            success = sut.Delete(10000);
            Assert.IsFalse(success);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void UpdateTest()
        {
            var sut = Sut();

            // 1. Insert at least one
            BranchLocation model = new BranchLocation()
            {
                Name = "Test branch location, update",
                Description = "Test branch location description, update"
            };

            int targetID = sut.Add(model);
            Assert.AreNotEqual(0, targetID);

            // 2. update
            model.Name = "updated name";
            model.Description = null;
            bool success = sut.Update(model);
            Assert.IsTrue(success);

            // 3. re-read from db
            BranchLocation updated = sut.Get(model.BranchLocationID);
            Assert.AreEqual(updated.Name, "updated name");
            Assert.IsNull(updated.Description);


            // 4. update with invalid data
            model.Name = null;
            try
            {
                sut.Update(model);
            }
            catch (SqlException ex)
            {
                if (ex.Number != 8178)
                {
                    throw;
                }
            }

            // 5. update nonexistent
            BranchLocation nonexistent = new BranchLocation(10000)
            {
                Name = "nonexistent",
                Description = "nonexistent"
            };

            success = sut.Update(nonexistent);
            Assert.IsFalse(success);
        }


        /// <summary>
        ///A test for SetDisabledStatus
        ///</summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void SetDisabledStatusTest()
        {
            var sut = Sut();

            var newBranch = new BranchLocation();
            newBranch.Name = "Unit Test BL - " + Guid.NewGuid().ToString();
            newBranch.Description = "Test BL for testing BranchLocationTest.SetDisabledStatusTest method";
            sut.Add(newBranch);

            //disable
            sut.SetDisabledStatus(newBranch.BranchLocationID, true);
            newBranch = sut.Get(newBranch.BranchLocationID);
            Assert.AreEqual<bool>(true, newBranch.Disabled);

            //enable
            sut.SetDisabledStatus(newBranch.BranchLocationID, false);
            newBranch = sut.Get(newBranch.BranchLocationID);
            Assert.AreEqual<bool>(false, newBranch.Disabled);

            //disable with kiosks

            var k = new Kiosk();
            k.BranchLocationID = newBranch.BranchLocationID;
            k.Name = "Test Kiosk - " + Guid.NewGuid().ToString();
            k.Description = "Test Kiosk for testing BranchLocationTest.SetDisabledStatusTest method";
            FrontDesk.Server.KioskHelper.Add(k);
            Assert.AreEqual<bool>(false, k.Disabled);
            k = kioskService.GetByID(k.KioskID);
            Assert.AreEqual<bool>(false, k.Disabled);

            //disable

            sut.SetDisabledStatus(newBranch.BranchLocationID, true);


            newBranch = sut.Get(newBranch.BranchLocationID);
            Assert.AreEqual<bool>(false, newBranch.Disabled); //we have 1 active kiosk - update should be not do anything

            //disable kiosk
            FrontDesk.Server.KioskHelper.SetDisabledStatus(k.KioskID, true);

            //and disable location
            sut.SetDisabledStatus(newBranch.BranchLocationID, true);
            newBranch = sut.Get(newBranch.BranchLocationID);
            Assert.AreEqual<bool>(true, newBranch.Disabled); //now it should be disabled


            //enable location
            sut.SetDisabledStatus(newBranch.BranchLocationID, false);

            newBranch = sut.Get(newBranch.BranchLocationID);
            Assert.AreEqual<bool>(false, newBranch.Disabled); //should be active


            //delete
            kioskService.Delete(k.KioskID);
            sut.Delete(newBranch.BranchLocationID);


        }
    }
}
