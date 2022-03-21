import { IFollowUpInnerInnerItem } from 'actions/follow-up';
import { 
    IActionPayload, EVisitReportActions,
} from '../../../actions';
import  { 
    TChoiceItem, IVisitReportResponse, TOtherScreeningToolsItem 
} from '../../../actions/visit/report';

export interface IVisitReportState {
    isVisitReportLoading: boolean;
    reportResponse: IVisitReportResponse | null;
    drugChoiceOptions: Array<TChoiceItem>;
    drugPrimaryOption: TChoiceItem | null;
    drugSecondaryOption: TChoiceItem | null;
    drugTertiaryOption: TChoiceItem | null;
    otherScreeningTools: Array<TOtherScreeningToolsItem>;
    tritmentActionTools: Array<TChoiceItem>;
    tritmentOptions: Array<TChoiceItem>;
    referalRecomendationOptions: Array<TChoiceItem>;
    referalRecomendationAcceptedOptions: Array<TChoiceItem>;
    referalRecomendationNotAcceptedOptions: Array<TChoiceItem>;
    dischargedOptions: Array<TChoiceItem>;
    referralRecDescription: string | null;
    referralRecId: number | null;
    referralRecommendationAccepted: number | null;
    referralRecommendationNotAccepted: number | null;
    visitDate: string | null;
    discharged: number | null;
    visitNode: string | null;
    followUp: string | null;
    followUpDate: string | null;
    isSaveChanges: boolean;
    isCompleted: boolean;
    followUpVisitArray: Array<IFollowUpInnerInnerItem>
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

const visitReportInitState: IVisitReportState =  {
    isSaveChanges: false,
    isCompleted: false,
    followUp: null,
    followUpDate: null,
    visitNode: null,
    visitDate: null,
    discharged: null,
    referralRecId: null,
    reportResponse: null,
    tritmentOptions: [],
    drugChoiceOptions: [],
    dischargedOptions: [],
    drugPrimaryOption: null,
    drugTertiaryOption: null,
    otherScreeningTools: [{
        ScoreOrResult: '',
        ToolName: ''
    }],
    drugSecondaryOption: null,
    tritmentActionTools: [{
        Id: 0,
        Name: '',
        OrderIndex: 0,
        Description: '',
    }],
    isVisitReportLoading: true,
    referralRecDescription: null,
    referalRecomendationOptions: [],
    referralRecommendationAccepted: null,
    referralRecommendationNotAccepted: null,
    referalRecomendationAcceptedOptions: [],
    referalRecomendationNotAcceptedOptions: [],
    followUpVisitArray: []
}

const visitReportReducer = (state: IVisitReportState = visitReportInitState, action: IActionPayload) => {
    switch(action.type) {
        case EVisitReportActions.updateVisitReportRequestStart:
            return {
                ...state,
                isSaveChanges: true,
            }
        case EVisitReportActions.updateVisitReportRequestError:
        case EVisitReportActions.updateVisitReportRequestSuccess:
            return {
                ...state,
                isSaveChanges: false,
            }
        case EVisitReportActions.getVisitReportByIdStart:
            return {
                ...state,
                ...visitReportInitState,
                isVisitReportLoading: true,
            }
        case EVisitReportActions.getTritmentActionRequestStart:
        case EVisitReportActions.getDrugChoiceOptionRequestStart:
        case EVisitReportActions.getVisitDischargedOptionsRequestStart:
        case EVisitReportActions.getVisitNewReferalRecomendationRequestStart:
        case EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestStart:
        case EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestStart:
            return {
                ...state,
                isVisitReportLoading: true,
            }
        case EVisitReportActions.getVisitReportByIdError:
        case EVisitReportActions.getTritmentActionRequestError:
        case EVisitReportActions.getDrugChoiceOptionRequestError:
        case EVisitReportActions.getVisitDischargedOptionsRequestError:
        case EVisitReportActions.getVisitNewReferalRecomendationRequestError:
        case EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestError:
        case EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestError:
            return {
                ...state,
                isVisitReportLoading: false,
            }
        case EVisitReportActions.setVisitFollowUp:
            return {
                ...state,
                followUp: action.payload.value,
            }
        case EVisitReportActions.setVisitFollowUpDate:
            return {
                ...state,
                followUpDate: action.payload.value,
            }
        case EVisitReportActions.setVisitNotes:
            return {
                ...state,
                visitNode: action.payload.value,
            }
        case EVisitReportActions.setVisitDischarged:
            return {
                ...state,
                discharged: action.payload.value,
            }
        case EVisitReportActions.setVisitDate:
            return {
                ...state,
                visitDate: action.payload.value,
            }
        case EVisitReportActions.setVisitReferralRecommendationAccepted:
            return {
                ...state,
                referralRecommendationAccepted: action.payload.value,
            }
        case EVisitReportActions.setVisitReferralRecommendationNotAccepted:
            return {
                ...state,
                referralRecommendationNotAccepted: action.payload.value,
            }
        case EVisitReportActions.setVisitReferralRecommendationDescription:
            return {
                ...state,
                referralRecDescription: action.payload.value,
            }
        case EVisitReportActions.setVisitReferralRecommendationId:
            return {
                ...state,
                referralRecId: action.payload.value,
            }
        case EVisitReportActions.setDrugPrimaryItem:
            return {
                ...state,
                drugPrimaryOption: action.payload.option,
            }
        case EVisitReportActions.setDrugSecondaryItem:
            return {
                ...state,
                drugSecondaryOption: action.payload.option,
            }
        case EVisitReportActions.setDrugTertiaryItem:
            return {
                ...state,
                drugTertiaryOption: action.payload.option,
            }
        case EVisitReportActions.getVisitDischargedOptionsRequestSuccess:
            return {
                ...state,
                dischargedOptions: action.payload.options,
            }
        case EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequestSuccess:
            return {
                ...state,
                referalRecomendationAcceptedOptions: action.payload.options,
            }
        case EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequestSuccess:
            return {
                ...state,
                referalRecomendationNotAcceptedOptions: action.payload.options,
            }
        case EVisitReportActions.getVisitNewReferalRecomendationRequestSuccess:
            return {
                ...state,
                referalRecomendationOptions: action.payload.options,
            }
        case EVisitReportActions.initTritmentActionTools:
            return {
                ...state,
                tritmentActionTools: action.payload.tritmentActionTools,
            }
        case EVisitReportActions.initOtherScreeningTools:
            return {
                ...state,
                otherScreeningTools: action.payload.otherScreeningTools,
            }
        case EVisitReportActions.getTritmentActionRequestSuccess:
            return {
                ...state,
                isVisitReportLoading: false,
                tritmentOptions: action.payload.options,
            }
        case EVisitReportActions.getDrugChoiceOptionRequestSuccess:
            return {
                ...state,
                isVisitReportLoading: false,
                drugChoiceOptions: action.payload.options,
            }
        case EVisitReportActions.getVisitReportByIdSuccess:
            return {
                ...state,
                isVisitReportLoading: false,
                reportResponse: action.payload.report,
            }
        
        case EVisitReportActions.setVisitReportIsCompleted:
            return {
                ...state,
                isCompleted: action.payload.isCompleted,
            }
        case EVisitReportActions.setFollowUpVisitArray:
            return {
                ...state,
                followUpVisitArray: action.payload
            }  
        default: return state;
    }
}

export default visitReportReducer;