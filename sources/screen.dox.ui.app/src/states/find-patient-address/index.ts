import { IActionPayload, EFindPatientAddressActions } from '../../actions';
import { IEhrRecordPatientsItem } from '../../actions/find-patient-address';


export interface IFindPatientAddressState {
    ehrRecordsPatients: Array<IEhrRecordPatientsItem>;
    isEhrRecordPatientsLoading: boolean;
    phoneNumber: number | null;
    streetAddress: string;
    city: string;
    state: string;
    zipCode: string;
    patientID: number | null;
    currentPage: number;
    isLoading: boolean;
    ehrExportPatientRecordSelectedId: number | null;
    isEhrRecordPatientsLoadingError: boolean;
    ehrRecordsTotal: number;
    visitId: number | null;
}

export const findPatientAddressInitState: IFindPatientAddressState = {
    ehrRecordsPatients: [],
    isEhrRecordPatientsLoading: false,
    isEhrRecordPatientsLoadingError: false,
    phoneNumber: null,
    streetAddress: '',
    city: '',
    state: '',
    zipCode: '',
    patientID: null,
    currentPage: 1,
    isLoading: false,
    ehrExportPatientRecordSelectedId: null,
    ehrRecordsTotal: 0,
    visitId: null,
}

const findPatientAddressReducer = (state: IFindPatientAddressState = findPatientAddressInitState, action: IActionPayload) => {
    switch(action.type) {
        case EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestStart:
            return {
                ...state,
                isEhrRecordPatientsLoading: true,
            }
        case EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestError:
            return {
                ...state,
                isEhrRecordPatientsLoading: false,
                isEhrRecordPatientsLoadingError: true,
            }
        case EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequestSuccess:
            return {
                ...state,
                isEhrRecordPatientsLoading: false,
                ehrRecordsPatients: action.payload.Items,
                ehrRecordsTotal:action.payload.TotalCount,
                isEhrRecordPatientsLoadingError: false,
            }
        case EFindPatientAddressActions.setFindPatientAddressPhoneNumber:
            return {
                ...state,
                phoneNumber: action.payload.phoneNumber
            }
        case EFindPatientAddressActions.setFindPatientAddressCity:
            return {
                ...state,
                city: action.payload.city
            }
        case EFindPatientAddressActions.setFindPatientAddressState:
            return {
                ...state,
                state: action.payload.state,
            }
        case EFindPatientAddressActions.setFindPatientAddressZipCode:
            return {
                ...state,
                zipCode: action.payload.zipCode
            }
        case EFindPatientAddressActions.setFindPatientAddressStreetAddress:
            return {
                ...state,
                streetAddress: action.payload.streetAddress
            }
        case EFindPatientAddressActions.postFindPatientAddressRequestStart: 
            return {
                ...state,
                isLoading: true,
            }
        case EFindPatientAddressActions.postFindPatientAddressRequestError:
            return {
                ...state,
                isLoading: false
            }
        case EFindPatientAddressActions.postFindPatientAddressRequestSuccess:
            return {
                ...state,
                isLoading: false,
            }
        case EFindPatientAddressActions.setFindPatientAddressEhrExportPatientRecordSelectedId: 
            return {
                ...state,
                ehrExportPatientRecordSelectedId: action.payload.selectedId
            }
        case EFindPatientAddressActions.setFindPatientAddressEhrExportRecordCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page
            }
        default: return state;
    }
}

export default findPatientAddressReducer;