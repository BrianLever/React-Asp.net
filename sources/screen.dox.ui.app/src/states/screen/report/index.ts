import { IActionPayload, EScreenReportActions } from '../../../actions';
import { 
    IDefenitionResponse, IScreeningReport, IScreeningReportSections 
} from '../../../actions/screen/report';


export interface IScreenDefinitionState {
    isScreenReportLoading: boolean;
    definition: Array<IDefenitionResponse> | null;
    currentScreeningReport: IScreeningReport | null;
    currentScreeningReportObject: { [k: string]: IScreeningReportSections };
    screenVisitId: Number | null;
    isScreenVisitRequestLoading: boolean;
}

export const screenDefinitionState: IScreenDefinitionState = {
    isScreenReportLoading: false,
    definition: null,
    currentScreeningReport: null,
    currentScreeningReportObject: {},
    screenVisitId: null,
    isScreenVisitRequestLoading: true,
}

const screenDefinitionReducer = (state: IScreenDefinitionState = screenDefinitionState, action: IActionPayload) => {
    switch(action.type) {
        case EScreenReportActions.getScreeningReportRequestStart:
        case EScreenReportActions.getScreenReportDefinitionStart:
            return {
                ...state,
                isScreenReportLoading: true,
            }
        case EScreenReportActions.setScreeningReportSectionsObjet:
            return {
                ...state,
                currentScreeningReportObject: action.payload.obj,
            }
        case EScreenReportActions.resetScreeningReportRequest:
            return {
                ...state,
                currentScreeningReport: null,
                currentScreeningReportObject: {},
                isScreenReportLoading: false,
            }

        case EScreenReportActions.getScreenReportDefinitionSuccess:
            return {
                ...state,
                definition: action.payload.defenition,
                isScreenReportLoading: false,
            }

        case EScreenReportActions.getScreeningReportRequestSuccess:
            return {
                ...state,
                currentScreeningReport: action.payload,
                isScreenReportLoading: false,
            }

        // case EScreenReportActions.getScreenReportVisitId:

        case EScreenReportActions.getScreenReportVisitIdRequestSuccess:
            return {
                ...state,
                screenVisitId: action.payload,
                isScreenVisitRequestLoading: false
            }

        case EScreenReportActions.getScreenReportVisitIdRequestStart:
            return {
                ...state,
                isScreenVisitRequestLoading: true
            }

        case EScreenReportActions.getScreeningReportRequestError:
        case EScreenReportActions.getScreenReportDefinitionError:
            return {
                ...state,
                isScreenReportLoading: false,
            }
        default: return state;
    }
}

export default screenDefinitionReducer;