import { 
    IActionPayload, EVisitSettingsActions
} from '../../actions';
import { VisitSettingsResponseItem }  from '../../actions/visit-settings'; 


export interface IVisitSettingsState {
    visitSettingsList: Array<VisitSettingsResponseItem>,
    isLoadingVisitSettingsList: boolean
}

export const visitSettingsInitState: IVisitSettingsState = {
    visitSettingsList: [],
    isLoadingVisitSettingsList: false
}

const visitSettingsReducer = (state: IVisitSettingsState = visitSettingsInitState, action: IActionPayload) => {
    switch(action.type) {
        case EVisitSettingsActions.getVisitSettingsRequestStart:
            return {
                ...state,
                isLoadingVisitSettingsList: true,
            }
        case EVisitSettingsActions.getVisitSettingsRequestError:
            return {
                ...state,
                isLoadingVisitSettingsList: false,
            }
        case EVisitSettingsActions.getVisitSettingsRequestSuccess:
            return {
                ...state,
                isLoadingVisitSettingsList: false,
                visitSettingsList: action.payload
            }
        default: return state;
    }
}

export default visitSettingsReducer;