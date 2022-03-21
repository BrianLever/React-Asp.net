import { TGPRAPeriodResponseItem } from '../../actions';
import { 
    IVisitReportsResponse, IVisitResponseItem 
} from '../../actions/visit';
import { IRootState } from '../../states';

export const isVisitLoadingSelector = (state: IRootState): boolean => state.visit.isVisitLoading;
export const getVisitItemsSelector = (state: IRootState):  Array<IVisitResponseItem> => state.visit.visitItems;
export const getTotalVisitsSelector = (state: IRootState): number => state.visit.totalVisitItems;
export const visitListCurrentPageSelector = (state: IRootState): number => state.visit.currentPage;
export const visitListOffsetSelector = (state: IRootState): number => state.visit.offset;
export const visitListItemsToShowSelector = (state: IRootState) => state.visit.itemsToShow;
export const isVisitListAutoUpdateSelector = (state: IRootState) => state.visit.isAutoUpdate;
export const getVisitListActiveSortKeySelector = (state: IRootState): string => state.visit.activeSort;
export const getVisitListActiveSortDirectionSelector = (state: IRootState): string => state.visit.activeDirection;
export const getVisitListFirstNameSelector = (state: IRootState): string => state.visit.firstName;
export const getVisitListLastNameSelector = (state: IRootState): string => state.visit.lastName;
export const getVisitListvisitdoxResultIdSelector = (state: IRootState): number | null => state.visit.screeningResultID;
export const getVisitListStartDateSelector = (state: IRootState): string | null => state.visit.startDate;
export const getVisitListEndDateSelector = (state: IRootState): string | null => state.visit.endDate;
export const getVisitListGrpaPeriodKey = (state: IRootState): string | null => state.visit.grpaPeriodKey;
export const getVisitListGrpaPeriodList = (state: IRootState): Array<TGPRAPeriodResponseItem> => state.visit.grpaPeriods;
export const getVisitListLocationList = (state: IRootState) => state.visit.locations;
export const getVisitListSelectedLocationId = (state: IRootState): number => state.visit.selectedLocationId;
export const getBSRReportType = (state: IRootState): number => state.visit.bsrReportType;
export const getVisitDescriptiveReport = (state: IRootState): Array<IVisitReportsResponse> => state.visit.reportDescriptors;
export const isVisitListErrorSelector = (state: IRootState): boolean => state.visit.isListError;