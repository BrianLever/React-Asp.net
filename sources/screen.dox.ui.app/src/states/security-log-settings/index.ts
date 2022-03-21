import { IActionPayload, ESecurityLogSettingsActions } from '../../actions';
import { ISecurityLogSettingsCategory, ISecurityLogSettingsItem } from '../../actions/security-log-settings';

export interface ISecurityLogSettingsState {
   isSecurityLogSettingsLoading: boolean;
   securityLogSettingsItems: Array<ISecurityLogSettingsItem>;
   securityLogSettingsCategory: Array<ISecurityLogSettingsCategory>;
}

export const securityLogSettingsInitState: ISecurityLogSettingsState = {
    isSecurityLogSettingsLoading: false,
    securityLogSettingsItems: [],
    securityLogSettingsCategory: [],
}

const securityLogSettingsReducer = (state: ISecurityLogSettingsState = securityLogSettingsInitState, action: IActionPayload) => {
    switch(action.type) {
        case ESecurityLogSettingsActions.getSecurityLogSettingsRequestStart:
            return {
                ...state,
                isSecurityLogSettingsLoading: true,
            }
        case ESecurityLogSettingsActions.getSecurityLogSettingsRequestError:
            return {
                ...state,
                isSecurityLogSettingsLoading: false
            }
        case ESecurityLogSettingsActions.getSecurityLogSettingsRequestSuccess:
            return {
                ...state,
                isSecurityLogSettingsLoading: false,
                securityLogSettingsItems: action.payload.securityLogSettingsItems,
                securityLogSettingsCategory: action.payload.securityLogSettingsCategory,
            }
        default: return state;
    }
}

export default securityLogSettingsReducer;