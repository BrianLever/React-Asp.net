﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.UnitTest.Factory
{
    public static class PatientFactory
    {
        public static Patient CreateANDREA()
        {
            return new Patient
            {
                ID = 16009,
                LastName = "DEMO",
                FirstName = "ANDREA",
                MiddleName = "Chris", 
                EHR = "908070",
                DateOfBirth = new DateTime(1972, 2, 22),
                StreetAddress = "6385 WEST OTTAWA",
                PhoneHome = "(775) 219-8620",
                PhoneOffice = "",
                StateID = "NV",
                ZipCode = "89436",
                City = "SPANISH SPRINGS"

            };
        }

        public static Patient CreateGARERD()
        {
            return new Patient
            {
                LastName = "GARERD",
                FirstName = "ADELA",
                MiddleName = "Don Sr.",
                DateOfBirth = new DateTime(1965, 9, 9),
                StreetAddress = "Fake Street",
                PhoneHome = "111-111-11111",
                StateID = "CA",
                ZipCode = "92061",
                City = "AMADOR COUNTY"

            }; 
        }

        public static Patient CreateAddi()
        {
            return new Patient
            {
                LastName = "ADDI",
                FirstName = "ALVIN",
                MiddleName = "JOHN JR",
                DateOfBirth = new DateTime(1965, 9, 9),
                StreetAddress = "P.O. BOX 82 100100 TERMINATOR DR 100",
                PhoneHome = "111-111-11111",
                StateID = "CA",
                ZipCode = "92061",
                City = "SOUTH GATE"

            };
        }

        public static Patient CreateSANDRA()
        {
            return new Patient
            {
                LastName = "SMITH",
                FirstName = "SANDRA",
                DateOfBirth = new DateTime(1965, 9, 9),
                StreetAddress = "64777 ELM ST PO BOX 110 SOUTH OF HOSPITAL",
                PhoneHome = "111-111-11111",
                StateID = "CA",
                ZipCode = "92061",
                City = "SOUTH GATE"

            };
        }


        public static Patient CreateNopah()
        {
            return new Patient
            {
                LastName = "NOPAH",
                FirstName = "DELENE",
                MiddleName = "Gafoya Misi",
                DateOfBirth = new DateTime(1965, 9, 9),
                StreetAddress = "",
                PhoneHome = "",
                StateID = "",
                ZipCode = "",
                City = ""

            };
        }


        public static Patient CreateCHRIS()
        {
            return new Patient
            {
                LastName = "DEMO",
                FirstName = "CHRIS",
                DateOfBirth = new DateTime(1985, 1, 1),
                StreetAddress = "1125 WEST 6TH STREET",
                PhoneHome = "213-202-3975",
                StateID = "CA",
                ZipCode = "90013",
                City = "LOS ANGELES"

            };
        }

        public static Patient CreateANGELA()
        {
            return new Patient
            {
                LastName = "DEMO",
                FirstName = "ANGELA",
                DateOfBirth = new DateTime(1985, 1, 1),
                StreetAddress = "1125 W. 6TH ST, STE 103",
                PhoneHome = "213-202-3970",
                StateID = "CA",
                ZipCode = "90017",
                City = "LOS ANGELES"

            };
        }
    }
}
