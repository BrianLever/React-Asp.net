import { IRootState } from '../../states';

export const isReportLoadingSelector = (state: IRootState):boolean => state.reports.isReportsLoading;
export const getReportGrpaPeriodKeySelector = (state: IRootState) => state.reports.grpaPeriodKey;
export const getReportGrpaPeriodsSelector = (state: IRootState) => state.reports.grpaPeriods;
export const getReportLocationsSelector = (state: IRootState) => state.reports.locations;
export const getReportSelectedLocationIdSelector = (state: IRootState) => state.reports.selectedLocationId;
export const getReportStartDateSelector = (state: IRootState) => state.reports.startDate;
export const getReportEndDateSelector = (state: IRootState) => state.reports.endDate;
export const getReportBsrReportTypeSelector = (state: IRootState): number => state.reports.bsrReportType;

export const getListReportByProblemSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListReportByAgeSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListDrugByAgeSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListPatientDemographicsSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListFollowupOutcomesSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListVisitsOutcomesSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListScreenTimeLogSelector = (state: IRootState) => state.reports.reportsResponseList;
export const getListReportAgeGroupByAgeSelector= (state: IRootState) => state.reports.ageGroupList;
export const getEarliestDateSelector= (state: IRootState) => state.reports.earliestDate;

export const getScreeningResultReportsBySortSelector = (state: IRootState) => state.reports.screeningResultReportsBySort;
export const getReportsCurrentPageSelector = (state: IRootState) => state.reports.currentPage;
export const getReportSortKeySelector = (state: IRootState) => state.reports.activeSort;
export const getReportSortDirectionSelector = (state: IRootState) => state.reports.activeDirection;
export const isReportAutoRefreshStatusSelector = (state: IRootState) => state.reports.isAutoUpdate;
export const isInnerReportsBySortLoadingSelector = (state: IRootState) => state.reports.isInnerReportsBySortLoading;
export const isInnerReportsBySortLoadingErrorSelector = (state: IRootState) => state.reports.isInnerReportsBySortLoadingError;
export const getInnerReportsBySortSelector = (state: IRootState) => state.reports.reportObject;
export const getReportsBySortFilterArraySelector = (state: IRootState) => state.reports.reportsBySortFilterArray;


export const getIncludeScreeningsSelector= (state: IRootState) => state.reports.IncludeScreenings;
export const getIncludeDemographicsSelector= (state: IRootState) => state.reports.IncludeDemographics;
export const getIncludeVisitsSelector= (state: IRootState) => state.reports.IncludeVisits;
export const getIncludeFollowUpsSelector= (state: IRootState) => state.reports.IncludeFollowUps;
export const getIncludeDrugsOfChoiceSelector= (state: IRootState) => state.reports.IncludeDrugsOfChoice;
export const getIncludeCombinedSelector= (state: IRootState) => state.reports.IncludeCombined;

