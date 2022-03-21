import { IActionPayload, EDashboardActionType } from '../../actions';
import { ISystemSettings } from '../../actions/dashboard';

export interface IDashboard {
    systemSettings: ISystemSettings;
    isLoadingSystemSettings: boolean;
    isSystemSettingsError: boolean;
}

export const dashboardInitState: IDashboard = {
    systemSettings: {
        AppVersion: '',
        HasActiveLicense: false,
        License: undefined,
        Summary: undefined,
        IsEhrCredentialsExpirationAlert: false,
        EhrCredentialsExpirationAlertMessage: '',
        CentralLoggingUrl: '',
    },
    isLoadingSystemSettings: false,
    isSystemSettingsError: false,
}

const dashboardReducer = (state: IDashboard = dashboardInitState, action: IActionPayload) => {
    switch(action.type) {
        case EDashboardActionType.getSystemSettingsRequestStart:
            return {
                ...state,
                isSystemSettingsError: false,
                isLoadingSystemSettings: true,
            }
        case EDashboardActionType.getSystemSettingsRequestSuccess:
            return {
                ...state,
                systemSettings: action.payload.systemSettings,
                isSystemSettingsError: false,
                isLoadingSystemSettings: false,
            }
        case EDashboardActionType.getSystemSettingsRequestError:
            return {
                ...state,
                isSystemSettingsError: true,
                isLoadingSystemSettings: false,
            }
        default: return state;
    }
}

export default dashboardReducer;