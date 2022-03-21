using FrontDesk.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;
using FrontDesk.Server.Screening.Services;
using FrontDesk;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Common.Services;

namespace FrontDesk_UnitTest
{
    /// <summary>
    ///This is a test class for KioskTest and is intended
    ///to contain all KioskTest Unit Tests
    ///</summary>
    [TestClass()]
    public class KioskTest
    {
        private readonly IBranchLocationService branchLocationService = new BranchLocationService();
        private readonly IKioskService kioskService = new KioskService();

        public KioskTest()
        {

        }

        /// <summary>
        /// Test script
        /// 1 Add kiosk 
        /// 2 Get kiosk by ID
        /// 3 Attempt to add a kiosk with  name that in use
        /// 4 if application exeption delete kiosk
        /// </summary>
        [TestMethod()]
        [TestCategory("E2E")]
        public void Kiosk_HandleNameDublicate()
        {
            short tmpKioskId = 0;
            short tmpKioskId2 = 0;

            var tmpBranch = CreateTempBranchLocation();

            try
            {

                Kiosk kiosk = new Kiosk
                {
                    Name = "Kiosk 1",
                    Description = "Description",
                    CreatedDate = DateTimeOffset.Now,
                    BranchLocationID = tmpBranch.BranchLocationID,
                    LastActivityDate = null
                };

                //Add kiosk
                tmpKioskId = FrontDesk.Server.KioskHelper.Add(kiosk);

                kiosk.KioskID.Should().NotBe(0);

                //Get kiosk by id
                Kiosk getKiosk = null;
                getKiosk = kioskService.GetByID(kiosk.KioskID);
                Assert.AreNotEqual(null, getKiosk);

                //dublicate
                Action action = (() => tmpKioskId2 = FrontDesk.Server.KioskHelper.Add(kiosk));
                action.Should().Throw<ApplicationException>("because kiosk with the same name exists");

            }
            finally
            {
                if(tmpKioskId > 0)
                {
                    kioskService.Delete(tmpKioskId);
                }

                if (tmpKioskId2 > 0)
                {
                    kioskService.Delete(tmpKioskId2);
                }
                branchLocationService.Delete(tmpBranch.BranchLocationID);
            }
        }

        [TestMethod()]
        /// Test script
        /// 1 Add kiosk 
        /// 2 Update kiosk
        /// 3 Change last Activity Date of kiosk
        /// </summary>
        [TestCategory("E2E")]
        public void Kiosk_CreateUpdateAndDelete()
        {

            short tmpKioskId = 0;
            var tmpBranch = CreateTempBranchLocation();
            try
            {
                Kiosk kiosk = new Kiosk
                {
                    Name = "Kiosk 1",
                    Description = "Description",
                    CreatedDate = DateTime.Now,
                    BranchLocationID = tmpBranch.BranchLocationID,
                    LastActivityDate = null
                };

                //Add kiosk
                tmpKioskId = FrontDesk.Server.KioskHelper.Add(kiosk);
                kiosk.KioskID.Should().NotBe(0);

                //Update Test
                kiosk.Name = "Kiosk 2";
                kiosk.Description = "Comments";
                kioskService.Update(kiosk);

                //Get update kiosk
                var updatedKiosk = kioskService.GetByID(kiosk.KioskID);
                updatedKiosk.Name.Should().Be("Kiosk 2");
                updatedKiosk.Description.Should().Be("Comments");


                //Change last activity date
                var currentDate = DateTimeOffset.Now;
                kioskService.ChangeLastActivityDate(updatedKiosk.KioskID, DateTimeOffset.Now);

                //Get kiosk with new LastActivityDate
                updatedKiosk = kioskService.GetByID(kiosk.KioskID);
                updatedKiosk.LastActivityDate.Should().Be(currentDate);
            }
            finally
            {
                if (tmpKioskId > 0)
                {
                    kioskService.Delete(tmpKioskId);
                }
                branchLocationService.Delete(tmpBranch.BranchLocationID);
            }
        }

        [TestCategory("E2E")]
        [TestMethod()]
        public void Kiosk_SetDisabledStatus_CanDisableAndEnable()
        {
            short newKioskId = 0;
            var newBranch = CreateTempBranchLocation();

            try
            {
                Assert.AreEqual<bool>(false, newBranch.Disabled);
                newBranch = branchLocationService.Get(newBranch.BranchLocationID);
                Assert.AreEqual<bool>(false, newBranch.Disabled);


                var k = new Kiosk
                {
                    BranchLocationID = newBranch.BranchLocationID,
                    Name = "Test Kiosk - " + Guid.NewGuid().ToString(),
                    Description = "Test Kiosk for testing KioskTest.SetEnabledStatusTest method"
                };

                FrontDesk.Server.KioskHelper.Add(k);
                newKioskId = k.KioskID;

                k = kioskService.GetByID(k.KioskID);
                k.Disabled.Should().BeFalse("Kiosk should be enabled");


                //try to disable kiosk
                FrontDesk.Server.KioskHelper.SetDisabledStatus(k.KioskID, true);
                k = kioskService.GetByID(k.KioskID);
                k.Disabled.Should().BeTrue("Kiosk should be disabled");


                //the same operations when BL is disabled
                branchLocationService.SetDisabledStatus(newBranch.BranchLocationID, true);
                newBranch = branchLocationService.Get(newBranch.BranchLocationID);
                newBranch.Disabled.Should().BeTrue("Branch should be disabled");


                //try to enable kiosk
                //enable
                FrontDesk.Server.KioskHelper.SetDisabledStatus(k.KioskID, false);
                k = kioskService.GetByID(k.KioskID);
                k.Disabled.Should().BeTrue("With disabled location kiosk should remain disabled");


                //try to disable 
                FrontDesk.Server.KioskHelper.SetDisabledStatus(k.KioskID, true);
                k = kioskService.GetByID(k.KioskID);
                k.Disabled.Should().BeTrue("With disabled location kiosk should remain disabled");

            }
            finally
            {

                if(newKioskId != 0)
                {
                    //delete
                    kioskService.Delete(newKioskId);
                }
                //clean
                branchLocationService.Delete(newBranch.BranchLocationID);
            }

        }


        protected BranchLocation CreateTempBranchLocation()
        {
            var newBranch = new BranchLocation
            {
                Name = "Unit Test BL - " + Guid.NewGuid().ToString(),
                Description = "Test BL for testing KioskTest.SetEnabledStatusTest method",
                ScreeningProfileID = ScreeningProfile.DefaultProfileID
            };
            branchLocationService.Add(newBranch);

            return newBranch;
        }
        ///<summary>
        ///A test for Add Kiosk To Disabled Location
        ///</summary>
        [TestCategory("E2E")]
        [TestMethod()]
        public void Kisk_CannotBeAddedToDisableLocation()
        {

            var newBranch = CreateTempBranchLocation();
            try
            {
                Assert.AreEqual<bool>(false, newBranch.Disabled);

                newBranch = branchLocationService.Get(newBranch.BranchLocationID);
                Assert.AreEqual<bool>(false, newBranch.Disabled);

                //disable location
                branchLocationService.SetDisabledStatus(newBranch.BranchLocationID, true);
                newBranch = branchLocationService.Get(newBranch.BranchLocationID);
                Assert.AreEqual<bool>(true, newBranch.Disabled);



                var k = new Kiosk();
                k.BranchLocationID = newBranch.BranchLocationID;
                k.Name = "Test Kiosk - " + Guid.NewGuid().ToString();
                k.Description = "Test Kiosk for testing KioskTest.SetEnabledStatusTest method";

                FrontDesk.Server.KioskHelper.Add(k);
                try
                {
                    Assert.AreEqual<bool>(false, k.Disabled);

                    k = kioskService.GetByID(k.KioskID);

                    Assert.AreEqual<bool>(true, k.Disabled); //kiosk should be disabled


                    
                }
                finally
                {
                    //clean
                    kioskService.Delete(k.KioskID);
                }
            }
            finally
            {
                branchLocationService.Delete(newBranch.BranchLocationID);
            }
        }
    }
}
