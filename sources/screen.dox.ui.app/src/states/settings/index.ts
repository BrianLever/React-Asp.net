import { IActionPayload, EApplicationSettingsActions } from '../../actions';

export interface ISettingsState {
    isLoading: boolean;
    sideDrawerState: boolean;
    isNotification: boolean;
    notificationMessage: string;
    notificationStatus: 'default' | 'error' | 'success' | 'warning' | 'info';
    currentPageKey: string;
    currentPagePath: string;
    modalWindowObject: { [k: string]: boolean }
}

const settingsInitState: ISettingsState =  {
    isLoading: false,
    sideDrawerState: false,
    isNotification: false,
    notificationMessage: '',
    notificationStatus: 'default' ,
    currentPageKey: '',
    currentPagePath: '',
    modalWindowObject: {},
}

const settingsReducer = (state: ISettingsState = settingsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EApplicationSettingsActions.modalWindowOpen:
            return {
                ...state,
                modalWindowObject: {
                    ...state.modalWindowObject,
                    [action.payload.key]: true,
                }
            }
        case EApplicationSettingsActions.modalWindowClose:
            return {
                ...state,
                modalWindowObject: {
                    ...state.modalWindowObject,
                    [action.payload.key]: false,
                }
            }
        case EApplicationSettingsActions.setCurrentPage:
            return {
                ...state,
                currentPageKey: action.payload.key,
                currentPagePath: action.payload.path,
            }
        case EApplicationSettingsActions.notifyError:
            return {
                ...state,
                isNotification: true,
                notificationMessage: action.payload.message,
                notificationStatus: 'error' ,
            }
        case EApplicationSettingsActions.notifySuccess:
            return {
                ...state,
                isNotification: true,
                notificationMessage: action.payload.message,
                notificationStatus: 'success' ,
            }
        case EApplicationSettingsActions.resetNotify:
            return {
                ...state,
                isNotification: false,
                notificationMessage: '',
                notificationStatus: 'success' ,
            }
        case EApplicationSettingsActions.triggerSideDrawerState:
            return {
                ...state,
                sideDrawerState: action.payload.value,
            }
        case EApplicationSettingsActions.sideDrawerIn:
            return {
                ...state,
                sideDrawerState: true,
            }
        case EApplicationSettingsActions.sideDrawerOut:
            return {
                ...state,
                sideDrawerState: false,
            }
        case EApplicationSettingsActions.switchLoadingOff:
            return {
                ...state,
                isLoading: false,
            }
        case EApplicationSettingsActions.switchLoadingOn:
            return {
                ...state,
                isLoading: true,
            }
        default: return state;
    }
}

export default settingsReducer;