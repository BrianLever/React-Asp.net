
import { Action } from 'redux';
import { EAutoExportLogsActions, IActionPayload } from '..';


export interface IAutoExportLogsResponseItem {
    OriginalPatientName: string;
    OriginalBirthday: string;
    CreatedDate: string;
    CorrectedPatientName: string;
    CorrectedBirthday: string;
    Comments: string;
}

export interface IAutoExportRequestItem {
    StartDate: string;
    EndDate: string;
    nameFilter: string;
    StartRowIndex: number;
    MaximumRows: number
}

export interface IAutoExportLogsStatisticsItem {
    Succeed: number;
    Failed: number;
    Total: number;
}

export const getAutoExportLogsRequest = (): Action => ({ type: EAutoExportLogsActions.getAutoExportLogsRequest });
export const getAutoExportLogsRequestStart = (): Action => ({ type: EAutoExportLogsActions.getAutoExportLogsRequestStart });
export const getAutoExportLogsRequestError = (): Action => ({ type: EAutoExportLogsActions.getAutoExportLogsRequestError });
export const getAutoExportLogsRequestSuccess = (payload: Array<IAutoExportLogsResponseItem>): IActionPayload => ({
    type: EAutoExportLogsActions.getAutoExportLogsRequestSuccess,
    payload
})

export const changeAutoExportLogsCurrentPageRequest = (page: number): IActionPayload => ({
    type: EAutoExportLogsActions.changeAutoExportLogsCurrentPageRequest, payload: { page }
})

export const setAutoExportLogsCurrentPage = (page: number): IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsCurrentPage, payload: { page }
})

export const setAutoExportLogsTotals = (total: number): IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsTotals, payload: { total }
})

export const setAutoExportLogsStartDate = (startDate: string | null):IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsStartDate, payload: { startDate }
})

export const setAutoExportLogsEndDate = (endDate: string | null):IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsEndDate, payload: { endDate }
})

export const setAutoExportLogsFilterName = (filterName: string | null):IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsFilterName, payload: { filterName }
})


export const setAutoExportLogsStatistics = (payload: IAutoExportLogsStatisticsItem): IActionPayload => ({
    type: EAutoExportLogsActions.setAutoExportLogsStatistics, payload
})