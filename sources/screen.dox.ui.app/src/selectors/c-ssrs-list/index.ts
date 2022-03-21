import {  ICssrsListRequest, ICssrsListResponse } from '../../actions/c-ssrs-list';
import { IRootState } from '../../states';

export const isCssrsListLoadingSelector = (state: IRootState): boolean => state.cssrsList.isLoading;
export const getCssrsListSelector = (state: IRootState):  Array<ICssrsListResponse> => state.cssrsList.cssrsList;
export const getCssrsListCurrentPageSelector = (state: IRootState): number => state.cssrsList.currentPage;
export const getCssrsListOffsetSelector = (state: IRootState): number => state.cssrsList.offset;
export const getCssrsListSortKeySelector = (state: IRootState) => state.cssrsList.activeSort;
export const getCssrsListSortDirectionSelector = (state: IRootState) => state.cssrsList.activeDirection;
export const getCssrsListFirstNameSelector = (state: IRootState) => state.cssrsList.firstName;
export const getCssrsListLastNameSelector = (state: IRootState) => state.cssrsList.lastName;
export const getCssrsListDateofBirthSelector=(state:IRootState)=> state.cssrsList.dateofBirth;
export const getCssrsListStartDateSelector = (state: IRootState) => state.cssrsList.startDate;
export const getCssrsListEndDateSelector = (state: IRootState) => state.cssrsList.endDate;
export const getCssrsListGpraPeriodKeySelector = (state: IRootState) => state.cssrsList.grpaPeriodKey;
export const getCssrsListGpraPeriodsSelector = (state: IRootState) => state.cssrsList.grpaPeriods;
export const getCssrsListReportTypeSelector = (state: IRootState) => state.cssrsList.bsrReportType;
export const getCssrsListLocationsSelector = (state: IRootState) => state.cssrsList.locations;
export const getCssrsListLocationIdSelector = (state: IRootState) => state.cssrsList.selectedLocationId;
export const getCssrsListTotalItem = (state: IRootState) => state.cssrsList.totalCssrsList;
export const getCssrsListAutoUpdateSelector = (state: IRootState) => state.cssrsList.isAutoUpdate;
export const isCssrsVisitReportLoadingSelector = (state: IRootState) => state.cssrsList.isCssrsVisitLoading;
export const getCssrsVisitReportSelector = (state: IRootState) => state.cssrsList.cssrsVisitReport;