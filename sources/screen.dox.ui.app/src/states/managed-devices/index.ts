import { IActionPayload, EManageDevicesActions } from '../../actions';
import { 
    IKioskDetailsResponse, IManegeDevicesListResponse 
} from '../../actions/manage-devices';

export interface IManageDeviceState {
    isManagedDevicesListLoading: boolean;
    manageDevicesList: Array<IManegeDevicesListResponse>;
    totalCount: number;
    currentPage: number;
    activeSort: string;
    activeDirection: string;
    selectedFilterBranchLocationId: number;
    selectedFilterBranchLocationNameKey: string;
    selectedAddNewKioskBranchLocationId: number;
    selectedAddNewKioskDeviceName: string;
    selectedAddNewKioskScreenProfile: string;
    selectedAddNewKioskSecretKey: string;
    selectedAddNewKioskDescription: string;
    inconsistencyInFieldsFlag: boolean;
    selectedFilterShowDisabled: number;
    selectedFilterScreeningProfileId: number;
    kioskDetails: IKioskDetailsResponse | any;
    kioskEditDetailsDescription: string;
    kioskEditDetailsBranchLocation: number;
    currentEditKioskId: number;
}

export const manageDeviceInitState: IManageDeviceState = {
    isManagedDevicesListLoading: false,
    manageDevicesList: [],
    totalCount: 0,
    currentPage: 1,
    activeSort: 'KioskID',
    activeDirection: 'asc',

    selectedFilterBranchLocationId: 0,
    selectedFilterBranchLocationNameKey: '',
    selectedFilterShowDisabled: 0,
    selectedFilterScreeningProfileId: 0,

    selectedAddNewKioskBranchLocationId: 0,
    selectedAddNewKioskDeviceName: '',
    selectedAddNewKioskScreenProfile: '',
    selectedAddNewKioskSecretKey: '',
    selectedAddNewKioskDescription: '',
    inconsistencyInFieldsFlag: false,
    currentEditKioskId: 0,
    kioskDetails: {},
    kioskEditDetailsDescription: '',
    kioskEditDetailsBranchLocation: 0
}

const managedDevicesReducer = (state: IManageDeviceState = manageDeviceInitState, action: IActionPayload) => {
    switch(action.type) {
        case EManageDevicesActions.changeEditKioskParamaterDescription:
            return {
                ...state,
                kioskEditDetailsDescription: action.payload.description,
            }
        case EManageDevicesActions.changeEditKioskParamaterBranchLocation:
            return {
                ...state,
                kioskEditDetailsBranchLocation: action.payload.location,
            }
        case  EManageDevicesActions.getEditKioskDetailsByIdRequestSuccess:
            return {
                ...state,
                kioskDetails: action.payload.kiosk,
                currentEditKioskId: action.payload.id,
            }
        case EManageDevicesActions.setScreeningProfileId:
            return {
                ...state,
                selectedFilterScreeningProfileId: action.payload.id,
            }
        case EManageDevicesActions.resetKioskFilter:
            return {
                ...state,
                selectedFilterBranchLocationId: manageDeviceInitState.selectedFilterBranchLocationId,
                selectedFilterBranchLocationNameKey: manageDeviceInitState.selectedFilterBranchLocationNameKey,
                selectedFilterShowDisabled: manageDeviceInitState.selectedFilterShowDisabled,
                selectedFilterScreeningProfileId: manageDeviceInitState.selectedFilterScreeningProfileId
            }
        case EManageDevicesActions.setFilterShowDisabled:
            return {
                ...state,
                selectedFilterShowDisabled: action.payload.value,
            }
        case EManageDevicesActions.setFilterLocationNameKey:
            return {
                ...state,
                selectedFilterBranchLocationNameKey: action.payload.name,
            }
        case EManageDevicesActions.setFilterBranchLocationId:
            return {
                ...state,
                selectedFilterBranchLocationId: action.payload.id,
            }
        case EManageDevicesActions.setCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page,
            }
        case EManageDevicesActions.addNewKioskInconsistencyInFields:
            return {
                ...state,
                inconsistencyInFieldsFlag: action.payload.flag,
            }
        case EManageDevicesActions.selectAddNewKioskDescription:
            return {
                ...state,
                selectedAddNewKioskDescription: action.payload.description,
            }
        case EManageDevicesActions.selectAddNewKioskSecretKey:
            return {
                ...state,
                selectedAddNewKioskSecretKey: action.payload.key,
                kioskDetails: {
                    ...state.kioskDetails,
                    SecretKey: action.payload.key
                }
                
            }
        case EManageDevicesActions.selectAddNewKioskScreenProfile:
            return {
                ...state,
                selectedAddNewKioskScreenProfile: action.payload.profile,
            }
        case EManageDevicesActions.selectAddNewKioskDeviceName:
            return {
                ...state,
                selectedAddNewKioskDeviceName: action.payload.name,
                kioskDetails: {
                    ...state.kioskDetails,
                    Name: action.payload.name
                }
            }
        case EManageDevicesActions.selectAddNewKioskBranchLocation:
            return {
                ...state,
                selectedAddNewKioskBranchLocationId: action.payload.id,
            }
        case EManageDevicesActions.getManagedDevicesListRequestStart:
            return {
                ...state,
                isManagedDevicesListLoading: true,
            }
        case EManageDevicesActions.getManagedDevicesListRequestError:
            return {
                ...state,
                isManagedDevicesListLoading: false,
            }
        case EManageDevicesActions.getManagedDevicesListRequestSuccess:
            return {
                ...state,
                isManagedDevicesListLoading: false,
                manageDevicesList: action.payload.items,
                totalCount: action.payload.totalCount
            }
        case EManageDevicesActions.changeActiveKey:
            return {
                ...state,
                activeSort: action.payload.key,
            }
        case EManageDevicesActions.changeActiveDirection:
            return {
                ...state,
                activeDirection: action.payload.direction,
            }
        default: return state;
    }
}

export default managedDevicesReducer;