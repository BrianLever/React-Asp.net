import { EAutoExportLogsActions, IActionPayload } from '../../actions';
import { IAutoExportLogsResponseItem, IAutoExportLogsStatisticsItem } from '../../actions/auto-export-logs';
const defaultDate = new Date().toISOString();
export interface IAutoExportLogsState {
   autoExportLogs: Array<IAutoExportLogsResponseItem>;
   isAutoExportLogsLoading: boolean;
   startDate: string | null;
   endDate: string | null;
   currentPage: number;
   filterName: string;
   total: number;
   statistics: IAutoExportLogsStatisticsItem;
}

export const autoExportLogsInitState: IAutoExportLogsState = {
    autoExportLogs: [],
    isAutoExportLogsLoading: false,
    startDate: null,
    endDate: defaultDate,
    currentPage: 1,
    filterName: '',
    total: 0,
    statistics: {
        "Succeed": 0,
        "Failed": 0,
        "Total": 0
    }
}

const autoExportLogsReducer = (state: IAutoExportLogsState = autoExportLogsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EAutoExportLogsActions.getAutoExportLogsRequestStart:
            return {
                ...state,
                isAutoExportLogsLoading: true
            }
        case EAutoExportLogsActions.getAutoExportLogsRequestStart:
            return {
                ...state,
                isAutoExportLogsLoading: false
            }
        case EAutoExportLogsActions.getAutoExportLogsRequestSuccess:
            return {
                ...state,
                isAutoExportLogsLoading: false,
                autoExportLogs: action.payload
            }
        case EAutoExportLogsActions.setAutoExportLogsCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page
            }
        case EAutoExportLogsActions.setAutoExportLogsTotals:
            return {
                ...state,
                total: action.payload.total
            }
        case EAutoExportLogsActions.setAutoExportLogsStartDate:
            return {
                ...state,
                startDate: action.payload.startDate
            }
        case EAutoExportLogsActions.setAutoExportLogsEndDate:
            return {
                ...state,
                endDate: action.payload.endDate
            }
        case EAutoExportLogsActions.setAutoExportLogsFilterName:
            return {
                ...state,
                filterName: action.payload.filterName
            }
        case EAutoExportLogsActions.setAutoExportLogsStatistics:
            return {
                ...state,
                statistics: action.payload
            }
        default: return state;
    }
}

export default autoExportLogsReducer;