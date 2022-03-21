import { IActionPayload, EErrorLogActions } from '../../actions';
import { IErrorLogListResponseItem } from '../../actions/error-log';
const defaultDate = new Date().toISOString();
export interface IErrorLogState {
    errorLogList: Array<IErrorLogListResponseItem>;
    isErrorLogLoading: boolean;
    startDate: string | null;
    endDate: string | null;
    currentPage: number;
    totalErrorLogs: number;
    errorLogItem: IErrorLogListResponseItem;
    isErrorLogDetailLoading: boolean;
}

export const errorLogInitState: IErrorLogState = {
    errorLogList: [],
    isErrorLogLoading: false,
    startDate:  null,
    endDate:  defaultDate,
    currentPage: 1,
    totalErrorLogs: 0,
    errorLogItem: {
        ErrorLogID: 0,
        KioskID: null,
        KioskLabel: '',
        ErrorMessage: '',
        ErrorTraceLog: '',
        CreatedDate: '',
        CreatedDateFormatted: ''
    },
    isErrorLogDetailLoading: false,
}

const errorLogReducer = (state: IErrorLogState = errorLogInitState, action: IActionPayload) => {
    switch(action.type) {
        case EErrorLogActions.getErrorLogRequestStart:
            return {
                ...state,
                isErrorLogLoading: true
            }
        case EErrorLogActions.getErrorLogRequestError:
            return {
                ...state,
                isErrorLogLoading: false,
            }
        case EErrorLogActions.getErrorLogRequestSuccess:
            return {
                ...state,
                isErrorLogLoading: false,
                errorLogList: action.payload.Items,
                totalErrorLogs: action.payload.TotalCount,
            }
        case EErrorLogActions.setErrorLogCurrentPage: 
            return {
                ...state,
                currentPage: action.payload.page
            }
        case EErrorLogActions.getErrorLogByIdRequestStart:
            return {
                ...state,
                isErrorLogDetailLoading: true
            }
        case EErrorLogActions.getErrorLogByIdRequestError:
            return {
                ...state,
                isErrorLogDetailLoading: false,
            }
        case EErrorLogActions.getErrorLogByIdRequestSuccess:
            return {
                ...state,
                isErrorLogDetailLoading: false,
                errorLogItem: action.payload.errorLog
            }
        case EErrorLogActions.setErrorLogStartDate: 
            return {
                ...state,
                startDate: action.payload.startDate
            }
        case EErrorLogActions.setErrorLogEndDate: 
            return {
                ...state,
                endDate: action.payload.endDate
            }
        default: return state;
    }
}

export default errorLogReducer;