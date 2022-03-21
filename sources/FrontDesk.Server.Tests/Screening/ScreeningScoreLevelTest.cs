using System.Collections.Generic;
using System.Data;
using System.Linq;
using FrontDesk.Server.Screening;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontDesk.Server.Tests.Screening
{
    
    
    /// <summary>
    ///This is a test class for ScreeningScoreLevelTest and is intended
    ///to contain all ScreeningScoreLevelTest Unit Tests
    ///</summary>
    [Ignore]
    [TestClass()]
    public class ScreeningScoreLevelTest
    {

        /// <summary>
        ///A test for GetAllScoreLevelsByScreeningID
        ///</summary>
        [TestMethod()]
        public void GetAllScoreLevelsByScreeningIdTest()
        {
            string screeningID = "BHS"; 
            DataView actual;
            actual = ScreeningScoreLevel.GetAllScoreLevelsByScreeningID(screeningID);



            actual.RowFilter = "ScreeningSectionID = 'CAGE'";
            Assert.AreEqual(4, actual.Count, "CAGE section is broken");

            actual.RowFilter = "ScreeningSectionID = 'PHQ-9'";
            Assert.AreEqual(5, actual.Count, "PHQ-9 section is broken");

            actual.RowFilter = "ScreeningSectionID = 'HITS'";
            Assert.AreEqual(2, actual.Count, "HITS section is broken");

            actual.RowFilter = "ScreeningSectionID = 'DAST'";
            Assert.AreEqual(5, actual.Count, "DAST section is broken");

            actual.RowFilter = "ScreeningSectionID = 'TCC'";
            Assert.AreEqual(2, actual.Count, "TCC section is broken");

            actual.RowFilter = "ScreeningSectionID = 'SIH'";
            Assert.AreEqual(2, actual.Count, "SIH section is broken");
            
            actual.RowFilter = "";
            Assert.AreEqual(20, actual.Count);
        }

        /// <summary>
        ///A test for GetScoreLevelsBySectionID
        ///</summary>
        [TestMethod()]
        public void GetScoreLevelsBySectionIDTest()
        {   ScreeningScoreLevel scoreLevel = new ScreeningScoreLevel();

            IList<ScreeningScoreLevel> actual;
            actual = scoreLevel.GetScoreLevelsBySectionID("TCC");
            Assert.AreEqual(2, actual.Count, "TCC failed");

            actual = scoreLevel.GetScoreLevelsBySectionID("CAGE");
            Assert.AreEqual(4, actual.Count, "CAGE failed");
            Assert.AreEqual("Evidence of DEPENDENCE until ruled out", actual[3].Name);

            actual = scoreLevel.GetScoreLevelsBySectionID("PHQ-9");
            Assert.AreEqual(5, actual.Count, "PHQ-9 failed");
            Assert.AreEqual("SEVERE depression severity", actual.First(x => x.ScoreLevel == 5).Name);


            actual = scoreLevel.GetScoreLevelsBySectionID("HITS");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("Evidence of CURRENT PROBLEM", actual[1].Name, "HITS failed");


        }
    }
}
