import { ECssrsReportActions, IActionPayload, ILocationItemResponse } from '../../../actions';
import {  ICssrsPatientRecordItem, ICssrsEditableReportItem   } from '../../../actions/c-ssrs-list/c-ssrs-report';


export interface ICssrsReportState {
    isPatientRecordsLoading: boolean;
    firstName: string | null;
    lastName: string | null;
    dateofBirth:string | null;
    middleName: string | null;
    screendoxPatientRecords: Array<ICssrsPatientRecordItem>;
    ehrPatientRecords: Array<ICssrsPatientRecordItem>;
    isReportCreateLoading: boolean;
    locationId: number;
    locations: Array<ILocationItemResponse>;
    isLoading: boolean;
    cssrsReport: ICssrsEditableReportItem | null;
    ehrExportPatientRecordSelectedId: number | null;
}


export const cssrsReportIntialState: ICssrsReportState = {
    isPatientRecordsLoading: false,
    firstName: null,
    lastName: 'DEMO',
    dateofBirth: '1985-01-01',
    middleName: null,
    screendoxPatientRecords: [],
    ehrPatientRecords: [],
    isReportCreateLoading: false,
    locationId: 0,
    locations: [],
    isLoading: false,
    cssrsReport: null,
    ehrExportPatientRecordSelectedId: null,
}

const cssrsReportReducer = (state: ICssrsReportState = cssrsReportIntialState, action: IActionPayload) => {
    switch(action.type) {
        case ECssrsReportActions.cssrsReportPatientRecordsRequestStart:
            return {
                ...state,
                isPatientRecordsLoading: true
            }
        case ECssrsReportActions.cssrsReportPatientRecordsRequestError:
            return {
                ...state,
                isPatientRecordsLoading: false
            }
        case ECssrsReportActions.cssrsReportPatientRecordsRequestSuccess:
            return {
                ...state,
                isPatientRecordsLoading: false,
                screendoxPatientRecords: action.payload.screendox,
                ehrPatientRecords: action.payload.ehr
            }
        case ECssrsReportActions.resetCssrsReportParameter:
            return{
                ...state,
                ...cssrsReportIntialState
            }
        case ECssrsReportActions.setCssrsReportFirstName:
            return {
                ...state,
                firstName: action.payload.firstName,
            }
        case ECssrsReportActions.setCssrsReportLastName:
            return {
                ...state,
                lastName: action.payload.lastName,
            }        
        case ECssrsReportActions.setCssrsReportMiddleName:
            return {
                ...state,
                middleName: action.payload.middleName,
            }
        case ECssrsReportActions.setCssrsReportBirthday:
            return {
                ...state,
                dateofBirth: action.payload.birthday,
            }
        case ECssrsReportActions.setCssrsReportLocationId:
            return {
                ...state,
                locationId: action.payload.locationId
            }
        case ECssrsReportActions.setCssrsReportLocations:
            return {
                ...state,
                locations: action.payload
            }
        case ECssrsReportActions.cssrsReportCreateRequestStart:
            return {
                ...state,
                isReportCreateLoading: true,
            }
        case ECssrsReportActions.cssrsReportCreateRequestError:
            return {
                ...state,
                isReportCreateLoading: false,
            }
        case ECssrsReportActions.cssrsReportCreateRequestSuccess:
            return {
                ...state,
                isReportCreateLoading: false,
            }
        case ECssrsReportActions.cssrsReportDetailRequestStart:
            return {
                ...state,
                isLoading: true,
            }
        case ECssrsReportActions.cssrsReportDetailRequestError:
            return {
                ...state,
                isLoading: false,
            }
        case ECssrsReportActions.cssrsReportDetailRequestSuccess:
            return {
                ...state,
                isLoading: false,
                cssrsReport: action.payload
            }
               
        case ECssrsReportActions.setCssrsReportEhrExportPatientRecordSelectedId:
            return {
                ...state,
                ehrExportPatientRecordSelectedId: action.payload.ehrExportPatientRecordSelectedId
            }
        case ECssrsReportActions.setCssrsReport:
            return {
                ...state,
                cssrsReport: action.payload
            }
        case ECssrsReportActions.cssrsReportUpdateRequestStart:
            return {
                ...state,
                isReportCreateLoading: true
            }
        case ECssrsReportActions.cssrsReportUpdateRequestError:
            return {
                ...state,
                isReportCreateLoading: false
            }
        case ECssrsReportActions.cssrsReportUpdateRequestSuccess:
            return {
                ...state,
                isReportCreateLoading: false
            }
        case ECssrsReportActions.cssrsReportCopyRequestStart:
            return {
                ...state,
                isReportCreateLoading: true
            }
        case ECssrsReportActions.cssrsReportCopyRequestError:
            return {
                ...state,
                isReportCreateLoading: false
            }
        case ECssrsReportActions.cssrsReportCopyRequestSuccess:
            return {
                ...state,
                isReportCreateLoading: false
            }
        default: return state;
    }
}

export default cssrsReportReducer;