using Microsoft.VisualStudio.TestTools.UnitTesting;
using FrontDesk.Kiosk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace FrontDesk.Kiosk.Services.Tests
{
    [TestClass()]
    public class TextSearchWordStartsWithTests
    {

        protected TextSearchWordStartsWith Sut()
        {
            return new TextSearchWordStartsWith();
        }

        /*
         * 
         * Value	LastModifiedDateUTC
Choctaw Nation of Oklahoma, Oklahoma	6/4/2019 12:00:00 AM
Jena Band of Choctaw Indians, Louisiana	6/4/2019 12:00:00 AM
Mechoopda Indian Tribe of Chico Rancheria, California	6/4/2019 12:00:00 AM
Mississippi Band of Choctaw Indians, Mississippi	6/4/2019 12:00:00 AM

    */

        [TestMethod()]
        public void IsMatched_Ignores_InsideWord()
        {
            Sut().IsMatched("cho", "Mechoopda Indian Tribe of Chico Rancheria, California").Should().BeFalse();
        }


        [TestMethod()]
        public void IsMatched_Finds_BeginningOfString()
        {
            Sut().IsMatched("cho", "Choctaw Nation of Oklahoma, Oklahoma").Should().BeTrue();
        }

        [TestMethod()]
        public void IsMatched_Finds_InMiddle()
        {
            Sut().IsMatched("cho", "Jena Band of Choctaw Indians, Louisiana").Should().BeTrue();
        }

        [TestMethod()]
        public void IsMatched_Finds_SeveralWords()
        {
            Sut().IsMatched("At Vil", "Atqasuk Village (Atkasook)").Should().BeTrue();
        }
    }
}
 