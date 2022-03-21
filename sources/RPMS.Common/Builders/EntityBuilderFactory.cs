using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common.Models;

namespace RPMS.Common.Builders
{
    public static class EntityBuilderFactory
    {
        public static IEntityBuilder<Patient> GetPatientBuilder()
        {
            return new PatientBuilder();
        }

        public static IEntityBuilder<DateTime> GetPatientDateOfBirthBuilder()
        {
            return new DataOfBirthBuilder();
        }
    }
}
