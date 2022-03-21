import { 
    ELicenseKeysActions,
    IActionPayload
} from '../../actions';
import { ILicenseKeysResponseItem } from 'actions/license-keys';

export interface ILicenseKeysState {
  licenseKeys: Array<ILicenseKeysResponseItem>;
  isLicenseKeysLoading: boolean;
  licenseKey: string | null;
  isLicenseKeyCreateLoading: boolean;
  licenseActivationKey: string | null;
  summary: any | null;
}   

export const ILicenseKeysInitState: ILicenseKeysState = {
    licenseKeys: [],
    isLicenseKeysLoading: false,
    licenseKey: null,
    isLicenseKeyCreateLoading: false,
    licenseActivationKey: null,
    summary: null
}

const licenseKeysReducer = (state: ILicenseKeysState = ILicenseKeysInitState, action: IActionPayload) => {
    switch(action.type) {
        case ELicenseKeysActions.getLicenseKeysRequestStart:
            return {
                ...state,
                isLicenseKeysLoading: true,
            }
        case ELicenseKeysActions.getLicenseKeysRequestError:
            return {
                ...state,
                isLicenseKeysLoading: false,
            }
        case ELicenseKeysActions.getLicenseKeysRequestSuccess:
            return {
                ...state,
                isLicenseKeysLoading: false,
                licenseKeys: action.payload,
            }
        case ELicenseKeysActions.setLicenseKey:
            return {
                ...state,
                licenseKey: action.payload.value 
            }
        case ELicenseKeysActions.setLicenseKeyCreateLoading:
            return {
                ...state,
                isLicenseKeyCreateLoading: action.payload.value
            }
        case ELicenseKeysActions.setLicenseActivationKey: 
            return {
                ...state,
                licenseActivationKey: action.payload.value
            }
        case ELicenseKeysActions.setLicenseKeysSystemSettingsSummary:
            return {
                ...state,
                summary: action.payload
            }
        default: return state;
    }
}

export default licenseKeysReducer;