using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Tests.Extensions
{
    [TestClass]
    public class LogExtentionsTests
    {

        [TestMethod]
        public void MaskFullName_ByPass_WhenEmpty()
        {
            string name = null;

            name.AsMaskedFullName().Should().BeEmpty();
        }

        [TestMethod]
        public void MaskFullName_Returns_NotEmpty()
        {
            var name = "MARTIN, CENDY CHARLES JR";

            name.AsMaskedFullName().Should().NotBeEmpty();
        }

        [TestMethod]
        public void MaskFullName_Middle_and_Jr_Correct()
        {
            var name = "MARTIN, CENDY CHARLES JR";

            name.AsMaskedFullName().Should().Be("MAR***, C**** C****** J*");
        }


        [TestMethod]
        public void MaskFullName_Middle_One_Letter_Correct()
        {
            var name = "MARTIN, CENDY A";

            name.AsMaskedFullName().Should().Be("MAR***, C**** *");
        }

        [TestMethod]
        public void MaskFullName_Just_Last_Name_Correct()
        {
            var name = "MARTIN";

            name.AsMaskedFullName().Should().Be("MAR***");
        }

        [TestMethod]
        public void MaskFullName_NoMiddle_Correct()
        {
            var name = "MARTIN, CENDY";

            name.AsMaskedFullName().Should().Be("MAR***, C****");
        }

        [TestMethod]
        public void MaskFullName_LastName_Has_Three_Letters_Correct()
        {
            var name = "LEE, CENDY";

            name.AsMaskedFullName().Should().Be("L**, C****");
        }

        [TestMethod]
        public void MaskFullName_LastName_Has_Less_TThan_Six_Letters_Correct()
        {
            var name = "SMITH, CENDY";

            name.AsMaskedFullName().Should().Be("SM***, C****");
        }
    }
}
