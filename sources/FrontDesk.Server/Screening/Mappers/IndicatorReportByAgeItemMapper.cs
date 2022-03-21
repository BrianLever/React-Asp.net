using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Web.Controls;

namespace FrontDesk.Server.Screening.Mappers
{
    public static class IndicatorReportByAgeItemMapper
    {

        public static IEnumerable<IndicatorReportByAgeItemViewModel> ToViewModel(
            this IEnumerable<IndicatorReportByAgeItem> items, int[] ageGroups)
        {

            //assumption - the Items collection must be sorted by section

            IndicatorReportByAgeItemViewModel viewModel = null;

            foreach (var indicatorReportByAgeItem in items)
            {
                if (viewModel != null && (
                    viewModel.ScreeningSectionID != indicatorReportByAgeItem.ScreeningSectionID ||
                    viewModel.ScreeningSectionQuestion != indicatorReportByAgeItem.ScreeningSectionQuestion))
                {
                    yield return viewModel;
                    viewModel = null;
                }


                if (viewModel == null)
                {
                    viewModel = new IndicatorReportByAgeItemViewModel
                    {
                        ScreeningSectionID = indicatorReportByAgeItem.ScreeningSectionID,
                        ScreeningSectionIndicates = indicatorReportByAgeItem.ScreeningSectionIndicates,
                        ScreeningSectionQuestion = indicatorReportByAgeItem.ScreeningSectionQuestion,
                    };

                    foreach (var @group in ageGroups)
                    {
                         viewModel.PositiveScreensByAge.Add(@group, 0);
                    }
                   
                }

                var item = indicatorReportByAgeItem;
                var age = ageGroups.TakeWhile(x => x <= item.Age).LastOrDefault();
                
                viewModel.PositiveScreensByAge[age] += indicatorReportByAgeItem.PositiveCount;

            }

            if (viewModel != null)
            {
                yield return viewModel;
            }
        }



        public static IEnumerable<BhsIndicatorReportByAgeItemViewModel> ToViewModel(
            this IEnumerable<BhsIndicatorReportByAgeItem> items, int[] ageGroups)
        {

            //assumption - the Items collection must be sorted by section

            BhsIndicatorReportByAgeItemViewModel viewModel = null;

            foreach (var indicatorReportByAgeItem in items)
            {
                if (viewModel != null && viewModel.Indicator != indicatorReportByAgeItem.IndicatorName)
                {
                    yield return viewModel;
                    viewModel = null;
                }


                if (viewModel == null)
                {
                    viewModel = new BhsIndicatorReportByAgeItemViewModel
                    {
                        Indicator = indicatorReportByAgeItem.IndicatorName,
                        IndicatorId = indicatorReportByAgeItem.IndicatorID,
                        CategoryID = indicatorReportByAgeItem.CategoryID,
                    };

                    foreach (var @group in ageGroups)
                    {
                        viewModel.TotalByAge.Add(@group, 0);
                    }

                }

                var item = indicatorReportByAgeItem;
                var age = ageGroups.TakeWhile(x => x <= item.Age).LastOrDefault();

                viewModel.TotalByAge[age] += indicatorReportByAgeItem.Count;

            }

            if (viewModel != null)
            {
                yield return viewModel;
            }
        }


    }
}
