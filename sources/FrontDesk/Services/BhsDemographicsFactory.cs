using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FrontDesk.Services
{
    public interface IBhsDemographicsFactory
    {
        BhsDemographics Create(ScreeningResult screeningResult);
        BhsDemographics InitFromReader(IDataReader reader);
    }

    public class BhsDemographicsFactory : IBhsDemographicsFactory
    {
        public BhsDemographics Create(ScreeningResult screeningResult)
        {
            if (screeningResult == null)
            {
                throw new ArgumentNullException(nameof(screeningResult));
            }

            BhsDemographics result = new BhsDemographics
            {
                LocationID = screeningResult.LocationID.Value,
                ScreeningResultID = screeningResult.ID,
                CreatedDate = DateTimeOffset.Now,
                ScreeningDate = screeningResult.CreatedDate,
                Birthday = screeningResult.Birthday,
                FirstName = screeningResult.FirstName,
                LastName = screeningResult.LastName,
                MiddleName = screeningResult.MiddleName,
                City = screeningResult.City,
                StateID = screeningResult.StateID,
                StreetAddress = screeningResult.StreetAddress,
                Phone = screeningResult.Phone,
                ZipCode = screeningResult.ZipCode
            };

            return result;
        }


        public BhsDemographics InitFromReader(IDataReader reader)
        {
            var model = new BhsDemographics {
                ID = reader.Get<long>("ID"),
                ScreeningResultID = reader.GetNullable<long>("ScreeningResultID"),
                LocationID = reader.Get<int>("LocationID"),
                LocationLabel = reader.Get<string>("LocationName"),
                CreatedDate = reader.Get<DateTimeOffset>("CreatedDate"),
                ScreeningDate = reader.Get<DateTimeOffset>("ScreeningDate"),
                BhsStaffNameCompleted = reader.Get<string>("BhsStaffNameCompleted"),
                CompleteDate = reader.GetNullable<DateTimeOffset>("CompleteDate"),
                FirstName = reader.Get<string>("FirstName"),
                LastName = reader.Get<string>("LastName"),
                MiddleName = reader.Get<string>("MiddleName"),
                Birthday = reader.Get<DateTime>("Birthday"),
                StreetAddress = reader.Get<string>("StreetAddress"),
                City = reader.Get<string>("City"),
                StateID = reader.Get<string>("StateID"),
                StateName = reader.Get<string>("StateName"),

                ZipCode = reader.Get<string>("ZipCode"),
                Phone = reader.Get<string>("Phone"),
                TribalAffiliation = reader.Get<string>("TribalAffiliation"),
                CountyOfResidence = reader.Get<string>("CountyOfResidence"),
                ExportedToHRN = reader.Get<string>("ExportedToHRN")
            };

            model.Race = new Common.LookupValue
            {
                Id = reader.Get<int>("RaceID", 0),
                Name = reader.Get<string>("RaceName"),
            };
            model.Gender = new Common.LookupValue
            {
                Id = reader.Get<int>("GenderID", 0),
                Name = reader.Get<string>("GenderName"),
            };
            model.SexualOrientation = new Common.LookupValue
            {
                Id = reader.Get<int>("SexualOrientationID", 0),
                Name = reader.Get<string>("SexualOrientationName"),
            };
            model.MaritalStatus = new Common.LookupValue
            {
                Id = reader.Get<int>("MaritalStatusID", 0),
                Name = reader.Get<string>("MaritalStatusName"),
            };

            model.LivingOnReservation = new Common.LookupValue
            {
                Id = reader.Get<int>("LivingOnReservationID", 0),
                Name = reader.Get<string>("LivingOnReservationName"),
            };

            model.EducationLevel = new Common.LookupValue
            {
                Id = reader.Get<int>("EducationLevelID", 0),
                Name = reader.Get<string>("EducationLevelName"),
            };

            var columns = reader.GetColumnNames();

            if (columns.Contains("MilitaryExperienceID") &&
                columns.Contains("MilitaryExperienceName"))
            {
                var militaryExperienceValues = reader.GetFromCsv<int>("MilitaryExperienceID");
                var militaryExperienceNames = reader.GetFromCsv<string>("MilitaryExperienceName");

                model.MilitaryExperience = new List<Common.LookupValue>(militaryExperienceValues.Count);

                for (int i = 0; i < militaryExperienceValues.Count; i++)
                {
                    model.MilitaryExperience.Add(new Common.LookupValue
                    {
                        Id = militaryExperienceValues[i],
                        Name = militaryExperienceNames[i]
                    });
                }

            }
            return model;
        }
    }
}
