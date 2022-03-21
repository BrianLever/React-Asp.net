using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPMS.Common;
using RPMS.Common.Models;

namespace RPMS.Data.FakeObjects
{
    public class FakeVisitRepository : IVisitRepository
    {
        private List<Visit> visits = new List<Visit>
        {
             new Visit{
                ID = 1000,
                Date = new DateTime(2012,03,4, 9, 0, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 950,
                Date = new DateTime(2012,02,3, 15, 30, 0),
                ServiceCategory = "OBSERVATION",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 900,
                Date = new DateTime(2012,02,3, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2020,
                    Name = "SANTA MARIA HOSPITAL"
                }
            },
            new Visit{
                ID = 850,
                Date = new DateTime(2011,05,15, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 800,
                Date = new DateTime(2011,05,15, 11, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 750,
                Date = new DateTime(2011,04,01, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 700,
                Date = new DateTime(2011,03,01, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 650,
                Date = new DateTime(2011,02,01, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 600,
                Date = new DateTime(2010,12,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 550,
                Date = new DateTime(2010,11,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 500,
                Date = new DateTime(2010,10,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 450,
                Date = new DateTime(2010,9,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 400,
                Date = new DateTime(2010,8,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 350,
                Date = new DateTime(2010,5,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 300,
                Date = new DateTime(2010,4,11, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 250,
                Date = new DateTime(2010,1,13, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 200,
                Date = new DateTime(2009,6,13, 9, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
            new Visit{
                ID = 150,
                Date = new DateTime(2009,6,13, 13, 00, 0),
                ServiceCategory = "AMBULATORY",
                Location = new Location{
                    ID = 2001,
                    Name = "2010 DEMO INDIAN HOSPITAL"
                }
            },
        };


       

        public List<Visit> GetVisitsByPatient(int patientID, int startRow, int rowsPerPage)
        {
            return visits.Skip(startRow).Take(rowsPerPage).ToList();
        }


        public int GetVisitsByPatientCount(int patientID)
        {
            return visits.Count;
        }

      
        public Visit GetVisitRecord(int visitId, PatientSearch patientSearch)
        {
            return visits.FirstOrDefault(p => p.ID == visitId);
        }

        public List<Visit> GetVisitsByPatient(PatientSearch patientSearch, int startRow, int maxRows)
        {
            return visits.Skip(startRow).Take(maxRows).ToList();
        }

        public int GetVisitsByPatientCount(PatientSearch patientSearch)
        {
            return visits.Count;
        }
    
    }
}
