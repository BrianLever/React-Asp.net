using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Controls;

namespace FrontDesk.Server.Screening.Mappers
{
    public static class BhsVisitListItemDtoModelMapper
    {

        public static IList<BhsVisitListItemPrintoutModel> ToViewModel(
            this IList<BhsVisitListItemDtoModel> items)
        {
            List<BhsVisitListItemPrintoutModel> viewModel = new List<BhsVisitListItemPrintoutModel>(items.Count / 2);
            Dictionary<string, BhsVisitListItemPrintoutModel> patientsLookup = new Dictionary<string, BhsVisitListItemPrintoutModel>(items.Count / 2);
            //assumption - items collection are sorted by create date, so patients might appear not in the order

            BhsVisitListItemPrintoutModel patientModel;
            foreach (var dto in items)
            {
                if (!patientsLookup.TryGetValue(dto.GetUniquePatientKey(), out patientModel))
                {
                    //create parent record to patient
                    patientModel = new BhsVisitListItemPrintoutModel()
                    {
                        Birthday = dto.Birthday,
                        PatientName = dto.PatientName,

                    };

                    patientsLookup.Add(patientModel.GetUniquePatientKey(), patientModel);
                    viewModel.Add(patientModel);

                    //first record - demographics
                    if(dto.DemographicsID.HasValue)
                    {
                        patientModel.ReportItems.Add(new BhsVisitViewModel
                        {
                            IsVisitRecordType = false,
                            ID = dto.DemographicsID.Value,
                            ScreeningResultID = dto.ScreeningResultID,
                            CreatedDate = dto.DemographicsCreatedDate.Value,
                            ScreeningDate = dto.DemographicsScreeningDate.Value,
                            CompletedDate = dto.DemographicsCompleteDate
                        });
                    }
                }
                patientModel.ReportItems.Add((BhsVisitViewModel)dto);
            }

            return viewModel;
        }


        public static IList<BhsFollowUpListItemPrintoutModel> ToViewModel(
            this IList<BhsFollowUpListItemDtoModel> items)
        {
            var viewModel = new List<BhsFollowUpListItemPrintoutModel>(items.Count);
            var patientsLookup = new Dictionary<string, BhsFollowUpListItemPrintoutModel>(items.Count);
            //assumption - items collection are sorted by follow-up date, so patients might appear not in the order

            BhsFollowUpListItemPrintoutModel patientModel;
            foreach (var dto in items)
            {
                if (!patientsLookup.TryGetValue(dto.GetUniquePatientKey(), out patientModel))
                {
                    //create parent record to patient
                    patientModel = new BhsFollowUpListItemPrintoutModel()
                    {
                        Birthday = dto.Birthday,
                        PatientName = dto.PatientName,

                    };

                    patientsLookup.Add(patientModel.GetUniquePatientKey(), patientModel);
                    viewModel.Add(patientModel);

                   
                }
                patientModel.ReportItems.Add((BhsFollowUpViewModel)dto);
            }

            return viewModel;
        }


        public static IList<BhsCheckInItemPrintoutModel> ToViewModel(
           this IList<PatientCheckInDtoModel> items)
        {
            var viewModel = new List<BhsCheckInItemPrintoutModel>(items.Count);
            var patientsLookup = new Dictionary<string, BhsCheckInItemPrintoutModel>(items.Count);
            //assumption - items collection are sorted by follow-up date, so patients might appear not in the order

            BhsCheckInItemPrintoutModel patientModel;
            foreach (var dto in items)
            {
                if (!patientsLookup.TryGetValue(dto.GetUniquePatientKey(), out patientModel))
                {
                    //create parent record to patient
                    patientModel = new BhsCheckInItemPrintoutModel()
                    {
                        Birthday = dto.Birthday,
                        PatientName = dto.PatientName,

                    };

                    patientsLookup.Add(patientModel.GetUniquePatientKey(), patientModel);
                    viewModel.Add(patientModel);


                }
                patientModel.ReportItems.Add((PatientCheckInViewModel)dto);
            }

            return viewModel;
        }

    }
}
