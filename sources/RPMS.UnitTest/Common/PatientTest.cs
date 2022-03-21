using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPMS.Common.Models;

namespace RPMS.UnitTest.Common
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        public void Can_parse_fullname_with_two_word_middle()
        {
            string fullname = "DEMO,PATIENT JANE II";
            var patient = new Patient();

            patient.FullName = fullname;

            Assert.AreEqual<string>("DEMO", patient.LastName);
            Assert.AreEqual<string>("PATIENT", patient.FirstName);
            Assert.AreEqual<string>("JANE II", patient.MiddleName);



        }


        [TestMethod]
        public void Can_parse_fullname_with_middle()
        {
            string fullname = "DEMO,PATIENT JANET";
            var patient = new Patient();

            patient.FullName = fullname;

            Assert.AreEqual<string>("DEMO", patient.LastName);
            Assert.AreEqual<string>("PATIENT", patient.FirstName);
            Assert.AreEqual<string>("JANET", patient.MiddleName);



        }


        [TestMethod]
        public void Can_parse_fullname_without_middle()
        {
            string fullname = "DEMO,PATIENT ";
            var patient = new Patient();

            patient.FullName = fullname;

            Assert.AreEqual<string>("DEMO", patient.LastName);
            Assert.AreEqual<string>("PATIENT", patient.FirstName);
            Assert.AreEqual<string>(null, patient.MiddleName);



        }
    }
}
