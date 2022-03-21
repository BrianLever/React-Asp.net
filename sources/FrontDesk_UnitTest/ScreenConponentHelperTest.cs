using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows;
using FrontDesk.Kiosk.Screens;

namespace FrontDesk_UnitTest
{


    /// <summary>
    ///This is a test class for ScreenConponentHelperTest and is intended
    ///to contain all ScreenConponentHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScreenConponentHelperTest
    {

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for FormatTextIntoInlines
        ///</summary>
        [TestMethod()]
        public void FormatTextIntoInlinesTest()
        {
            IVisualScreen screenComponent = null; // TODO: Initialize to an appropriate value
            ScreenComponentHelper target = new ScreenComponentHelper(screenComponent); // TODO: Initialize to an appropriate value
            string formattedText;


            List<Inline> expected = null;

            for (int caseId = 0; caseId < 5; caseId++)
            {
                expected = GetTestPattern(caseId, out formattedText);

                List<Inline> actual;
                actual = target.FormatTextIntoInlines(formattedText);
                Assert.AreEqual<int>(expected.Count, actual.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].GetType(), actual[i].GetType());
                    var expectedText = expected[i].ContentStart.GetTextInRun(LogicalDirection.Forward);
                    var actualText = actual[i].ContentStart.GetTextInRun(LogicalDirection.Forward);

                    Assert.AreEqual<string>(expectedText, actualText);

                }
            }
        }


        protected List<Inline> GetTestPattern(int caseID, out string formattedText)
        {
            Run temp;
            List<Inline> expected = new List<Inline>();
            formattedText = string.Empty;
            if (caseID == 0)
            {
                formattedText = @"Over the <b>LAST 2 WEEKS</b>, how often have you been \nbothered by any of the following problems?";
                expected.Add(new Run("Over the "));

                temp = new Run("LAST 2 WEEKS");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);
                expected.Add(new Run(", how often have you been"));
                expected.Add(new LineBreak());
                expected.Add(new Run("bothered by any of the following problems?"));
            }
            else if (caseID == 1)
            {
                formattedText = @"Have you ever felt you should <b>CUT</b> down on your drinking?";
                expected.Add(new Run("Have you ever felt you should "));

                temp = new Run("CUT");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);

                expected.Add(new Run(" down on your drinking?"));
            }
            else if (caseID == 2)
            {
                formattedText = @"<b>SCREAM</b> or curse at you?";
              
                temp = new Run("SCREAM");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);

                expected.Add(new Run(" or curse at you?"));
            }
            else if (caseID == 3)
            {
                formattedText = @"Have you ever had a drink first thing in the morning \nto steady your nerves or get rid of a hangover (<b>EYE-OPENER</b>)?";

                expected.Add(new Run("Have you ever had a drink first thing in the morning"));
                expected.Add(new LineBreak());
                expected.Add(new Run("to steady your nerves or get rid of a hangover ("));

                temp = new Run("EYE-OPENER");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);

                expected.Add(new Run(")?"));
            }
            else if (caseID == 4)
            {
                formattedText = @"If you checked off <b>ANY</b> problems, how <b>DIFFICULT</b> have these \nproblems made it for you to do your work, \ntake care of things at home, or get along with other people?";

                expected.Add(new Run("If you checked off "));

                temp = new Run("ANY");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);

                expected.Add(new Run(" problems, how "));
                temp = new Run("DIFFICULT");
                temp.FontWeight = FontWeights.SemiBold;
                expected.Add(temp);

                expected.Add(new Run(" have these"));
                expected.Add(new LineBreak());

                expected.Add(new Run("problems made it for you to do your work,"));
                expected.Add(new LineBreak());
                expected.Add(new Run("take care of things at home, or get along with other people?"));
            }
            //
            return expected;
        }
    }
}
