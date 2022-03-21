import { 
    EEhrLoginActions,
    IActionPayload
} from '../../actions';
import { IEhrLoginResponseItem } from 'actions/ehr-login';

export interface IEhrLoginState {
    isLoading: boolean;
    ehrLoginList: IEhrLoginResponseItem[];
    selectedId: string | null;
    accessCode: string | null;
    verifyCode: string | null;
    expireOn: string;
    isEhrActionLoading: boolean;
}   

export const IEhrLoginInitState: IEhrLoginState = {
    isLoading : false,
    ehrLoginList: [],
    selectedId: '',
    accessCode: '',
    verifyCode: '',
    expireOn: '',
    isEhrActionLoading: false
}

const ehrLoginReducer = (state: IEhrLoginState = IEhrLoginInitState, action: IActionPayload) => {
    switch(action.type) {
        case EEhrLoginActions.getEhrLoginListRequestStart:
            return {
                ...state,
                isLoading: true,
            }
        case EEhrLoginActions.getEhrLoginListRequestError:
            return {
                ...state,
                isLoading: false,
            }
        case EEhrLoginActions.getEhrLoginListRequestSuccess:
            return {
                ...state,
                isLoading: false,
                ehrLoginList: action.payload
            }
        case EEhrLoginActions.setEhrLoginAccessCode:
            return {
                ...state,
                accessCode: action.payload.accessCode,
            }
        case EEhrLoginActions.setEhrLoginVerifyCode:
            return {
                ...state,
                verifyCode: action.payload.verifyCode,
            }
        case EEhrLoginActions.setEhrLoginExpireOn:
            return {
                ...state,
                expireOn: action.payload.expireOn,
            }
        case EEhrLoginActions.setEhrLoginSelectedId:
            return {
                ...state,
                selectedId: action.payload.selectedId
            }
        case EEhrLoginActions.ehrLoginCreateRequestStart:
            return {
                ...state,
                isEhrActionLoading: true,
            }
        case EEhrLoginActions.ehrLoginCreateRequestError:
            return {
                ...state,
                isEhrActionLoading: false,
            }
        case EEhrLoginActions.ehrLoginCreateRequestSuccess:
            return {
                ...state,
                isEhrActionLoading: false,
            }
        default: return state;
    }
}

export default ehrLoginReducer;