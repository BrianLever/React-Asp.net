import { Action } from 'redux';
import { EFindPatientAddressActions, IActionPayload } from '..';


export interface IEhrRecordPatientsItem {
    NotMatchesFields: Array<string>;
    ID: number;
    City: string | null;
    Phone: string | null;
    StateID: string | null;
    StateName: string | null;
    StreetAddress: string | null;
    ZipCode: string | null;
    Birthday: string | null;
    FirstName: string | null;
    LastName: string | null;
    MiddleName: string | null;
    FullName: string;
    Age: number;
    BirthdayFormatted: string;
}

export interface IFindPatientAddressRequestItem {
    PatientID: number,
    City: string;
    Phone: string;
    StateID: string;
    StreetAddress: string;
    ZipCode: string;
}

export const  getFindPatientAddressEhrRecordPatientRequest = (screeningResultId: number): IActionPayload => ({ type: EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequest, payload: { screeningResultId } });
export const  getFindPatientAddressEhrRecordPatientRequestStart = (): Action => ({ type: EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestStart });
export const  getFindPatientAddressEhrRecordPatientRequestError = (): Action => ({ type: EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestError });
export const  getFindPatientAddressEhrRecordPatientRequestSuccess = (payload: { Items: Array<IEhrRecordPatientsItem>, TotalCount: number }): IActionPayload => ({ type: EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestSuccess, payload });

export const setFindPatientAddressPhoneNumber = (phoneNumber: string): IActionPayload => ({ type: EFindPatientAddressActions.setFindPatientAddressPhoneNumber, payload: { phoneNumber } });
export const setFindPatientAddressStreetAddress = (streetAddress: string): IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressStreetAddress, payload: { streetAddress }
})
export const setFindPatientAddressCity = (city: string): IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressCity, payload: { city }
})
export const setFindPatientAddressState = (state: string): IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressState, payload: { state }
})
export const setFindPatientAddressZipCode = (zipCode: string): IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressZipCode, payload: { zipCode }
})

export const postFindPatientAddressRequest = (id: number): IActionPayload => ({
    type: EFindPatientAddressActions.postFindPatientAddressRequest,
    payload: { id }
})


export const postFindPatientAddressRequestStart = (): Action => ({
    type: EFindPatientAddressActions.postFindPatientAddressRequestStart
})

export const postFindPatientAddressRequestError = (): Action => ({
    type: EFindPatientAddressActions.postFindPatientAddressRequestError
})

export const postFindPatientAddressRequestSuccess = (): Action => ({
    type: EFindPatientAddressActions.postFindPatientAddressRequestSuccess
})

export const setFindPatientAddressEhrExportPatientRecordSelectedId = (selectedId: number):IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressEhrExportPatientRecordSelectedId, payload: { selectedId }
})

export const setFindPatientAddressEhrExportRecordCurrentPage = (page: number): IActionPayload => ({
    type: EFindPatientAddressActions.setFindPatientAddressEhrExportRecordCurrentPage, payload: { page }
})