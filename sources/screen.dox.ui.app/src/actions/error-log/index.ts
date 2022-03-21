import { Action } from 'redux';
import { IRootState } from 'states';
import { IActionPayload, EErrorLogActions } from '..';


export interface IErrorLogListResponseItem {
    ErrorLogID: number,
    KioskID: string | null,
    KioskLabel: string,
    ErrorMessage: string,
    ErrorTraceLog: string,
    CreatedDate: string,
    CreatedDateFormatted: string
}


export const  getErrorLogRequest = (): Action => ({ type: EErrorLogActions.getErrorLogRequest });
export const  getErrorLogRequestStart = (): Action => ({ type: EErrorLogActions.getErrorLogRequestStart });
export const  getErrorLogRequestSuccess = (payload: { Items: Array<IErrorLogListResponseItem>, TotalCount: number }): IActionPayload => ({ type: EErrorLogActions.getErrorLogRequestSuccess, payload });
export const  getErrorLogRequestError = (): Action => ({ type: EErrorLogActions.getErrorLogRequestError });
export const changeErrorLogCurrentPageRequest = (page: number): IActionPayload => ({
    type: EErrorLogActions.changeErrorLogCurrentPageRequest, payload: { page }
})
export const setErrorLogCurrentPage = (page: number) => ({
    type: EErrorLogActions.setErrorLogCurrentPage, payload: { page }
})
export const getErrorLogByIdRequest = (id: number): IActionPayload  => 
({ type: EErrorLogActions.getErrorLogByIdRequest, payload: { id } });
export const getErrorLogByIdRequestStart = (): Action  => 
({ type: EErrorLogActions.getErrorLogByIdRequestStart });
export const getErrorLogByIdRequestError = (): Action  => 
({ type: EErrorLogActions.getErrorLogByIdRequestError });
export const getErrorLogByIdRequestSuccess = (errorLog: IErrorLogListResponseItem): IActionPayload  => 
({ type: EErrorLogActions.getErrorLogByIdRequestSuccess, payload: { errorLog } });
export const setErrorLogStartDate = (startDate: string | null): IActionPayload => 
({ type: EErrorLogActions.setErrorLogStartDate, payload: { startDate } })
export const setErrorLogEndDate = (endDate: string | null): IActionPayload => 
({ type: EErrorLogActions.setErrorLogEndDate, payload: { endDate } })
export const deleteErrorLogsRequest = (): Action => ({
    type: EErrorLogActions.deleteErrorLogsRequest
})
export const printErrorLogsExcelRequest = (): Action => ({
    type: EErrorLogActions.printErrorLogsExcelRequest
})