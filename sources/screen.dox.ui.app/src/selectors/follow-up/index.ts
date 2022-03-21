import { IRootState } from '../../states';

export const isFollowUpLoadingSelector = (state: IRootState) => state.followUp.isFollowUpLoading;
export const getFollowUpListTotalSelector = (state: IRootState) => state.followUp.totalFollowUps;
export const getListFollowUpSelector = (state: IRootState) => state.followUp.followUpResponseList;
export const getFollowUpGrpaPeriodKeySelector = (state: IRootState) => state.followUp.grpaPeriodKey;
export const getFollowUpGrpaPeriodsSelector = (state: IRootState) => state.followUp.grpaPeriods;
export const isAutomatedFollowUpSelector = (state: IRootState) => state.followUp.isAutoUpdate;
export const getFollowUpItemsToShowSelector = (state: IRootState) => state.followUp.itemsToShow;
export const getFollowUpLastNameSelector = (state: IRootState) => state.followUp.lastName;
export const getFollowUpFirstNameSelector = (state: IRootState) => state.followUp.firstName;
export const getFollowUpLocationsSelector = (state: IRootState) => state.followUp.locations;
export const getFollowUpOffsetSelector = (state: IRootState) => state.followUp.offset;
export const getFollowUpScreeningResultIdSelector = (state: IRootState) => state.followUp.screeningResultID;
export const getFollowUpSelectedLocationIdSelector = (state: IRootState) => state.followUp.selectedLocationId;
export const getFollowUpSortObjectSelector = (state: IRootState) => state.followUp.sortObject;
export const getFollowUpActiveDirectionSelector = (state: IRootState) => state.followUp.activeDirection;
export const getFollowUpActiveSortKeySelector = (state: IRootState) => state.followUp.activeSort;
export const getFollowUpActivePageSelector = (state: IRootState) => state.followUp.currentPage;
export const getFollowUpStartDateSelector = (state: IRootState) => state.followUp.startDate;
export const getFollowUpEndDateSelector = (state: IRootState) => state.followUp.endDate;
export const getFollowUpBsrReportTypeSelector = (state: IRootState): number => state.followUp.bsrReportType;
export const getFollowUpRelatedReportObjectSelector = (state: IRootState) => state.followUp.reportObject;

