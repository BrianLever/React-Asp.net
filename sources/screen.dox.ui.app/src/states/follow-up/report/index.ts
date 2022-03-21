import { IActionPayload, EFollowUpRelatedReportActions, TChoiceItem } from '../../../actions';
import { IFollowUpReportResponse } from '../../../actions/follow-up/report';

export interface IFollowUpReportState {
    isFollowUpReport: boolean;
    reportRespnse: IFollowUpReportResponse | null;
    
    patientAttendedVisitOptions: Array<TChoiceItem>;
    followUpContactOutcomeOptions: Array<TChoiceItem>;
    newVisitReferralRecommendationOptions: Array<TChoiceItem>;
    newVisitReferralRecommendationAcceptedOptions: Array<TChoiceItem>;
    newVisitReferralRecommendationNotAcceptedOptions: Array<TChoiceItem>;
    dischargedOptions: Array<TChoiceItem>;

    currentSelectedPatientAttendedVisitOption: number; 
    currentSelectedFollowUpContactOutcomeOption: number;
    currentSelectedNewVisitReferralRecommendationOption: number;
    currentlySelectedNewVisitReferralRecommendationAcceptedOption: number;
    currentlySelectedNewVisitReferralRecommendationNotAcceptedOption: number;
    currentlySelectedDischargedOption: number;
    currentSelectedPatientAttendedVisitDate: string | null; 
    newVisitReferralRecommendation: string;
    followUpReportCreateFlag: boolean;
    followUpReportCreateDate: string | null;
    newVisitDate: string | null;
    followUp: boolean;
    followUpDate: string | null;
    followUpNode: string;
    isCompleted: boolean;
}

export const MAXIMUM_RECORDS_PER_PAGE = 20;

export const followUpReportInitState: IFollowUpReportState = {
    isFollowUpReport: false,
    reportRespnse: null,

    patientAttendedVisitOptions: [],
    followUpContactOutcomeOptions: [],
    newVisitReferralRecommendationOptions: [],
    newVisitReferralRecommendationAcceptedOptions: [],
    newVisitReferralRecommendationNotAcceptedOptions: [],
    dischargedOptions: [],

    currentlySelectedNewVisitReferralRecommendationAcceptedOption: 0,
    currentlySelectedNewVisitReferralRecommendationNotAcceptedOption: 0,
    currentSelectedPatientAttendedVisitOption: 0,
    currentSelectedFollowUpContactOutcomeOption: 0,
    currentSelectedNewVisitReferralRecommendationOption: 0,
    currentlySelectedDischargedOption: 0,
    currentSelectedPatientAttendedVisitDate: null,
    newVisitReferralRecommendation: '',
    followUpReportCreateFlag: false,
    followUpReportCreateDate: null,
    newVisitDate: null,
    followUp: false,
    followUpDate: null,
    followUpNode: '',
    isCompleted: false,
}

const followUpReportReducer = (state: IFollowUpReportState = followUpReportInitState, action: IActionPayload) => {
    switch(action.type) {
        case EFollowUpRelatedReportActions.setFollowUpNote:
            return {
                ...state,
                followUpNode: action.payload.value,
            }
        case EFollowUpRelatedReportActions.setFollowUpDate:
            return {
                ...state,
                followUpDate: action.payload.value,
            }
        case EFollowUpRelatedReportActions.setFollowUpCreate:
            return {
                ...state,
                followUp: action.payload.value,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportNewVisitDate:
            return {
                ...state,
                newVisitDate: action.payload.date,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportDischargedOption:
            return {
                ...state,
                currentlySelectedDischargedOption: action.payload.option,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportDischargedRequestSuccess:
            return {
                ...state,
                dischargedOptions: action.payload.options,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportCreateFlag:
            return {
                ...state,
                followUpReportCreateFlag: action.payload.flag,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportCreateDate:
            return {
                ...state,
                followUpReportCreateDate: action.payload.date,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationNotAcceptedOption:
            return {
                ...state,
                currentlySelectedNewVisitReferralRecommendationNotAcceptedOption: action.payload.options,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestSuccess:
            return {
                ...state,
                newVisitReferralRecommendationNotAcceptedOptions: action.payload.options,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationAcceptedOption:
            return {
                ...state,
                currentlySelectedNewVisitReferralRecommendationAcceptedOption: action.payload.option,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequestSuccess:
            return {
                ...state,
                newVisitReferralRecommendationAcceptedOptions: action.payload.options,
            }
        case EFollowUpRelatedReportActions.setNewVisitReferralRecommendation:
            return {
                ...state,
                newVisitReferralRecommendation: action.payload.value,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportNewVisitReferralRecommendationOption: 
            return {
                ...state,
                currentSelectedNewVisitReferralRecommendationOption: action.payload.option,
            }
        case EFollowUpRelatedReportActions.setFollowUpReportContactOutcomeOption: 
            return {
                ...state,
                currentSelectedFollowUpContactOutcomeOption: action.payload.option,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationSuccess:
            return {
                ...state,
                newVisitReferralRecommendationOptions: action.payload.options,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcomeSuccess:
            return {
                ...state,
                followUpContactOutcomeOptions: action.payload.options,
            }
        case EFollowUpRelatedReportActions.setCurrentSelectedFollowUpReportPatientAttendedVisitOption:
            return {
                ...state,
                currentSelectedPatientAttendedVisitOption: action.payload.option,
            }
        case EFollowUpRelatedReportActions.setCurrentSelectedFollowUpReportPatientAttendedVisitDate:
            return {
                ...state,
                currentSelectedPatientAttendedVisitDate: action.payload.date,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestStart:
            return {
                ...state,
                isFollowUpReport: true,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestError:
            return {
                ...state,
                isFollowUpReport: false,
            }
        case EFollowUpRelatedReportActions.getReportRequestStart:
            return {
                ...state,
                isFollowUpReport: true,
                reportRespnse: null,
            }
        case EFollowUpRelatedReportActions.getReportRequestError:
            return {
                ...state,
                isFollowUpReport: false,
            }
        case EFollowUpRelatedReportActions.getReportRequestSuccess:
            return {
                ...state,
                isFollowUpReport: false,
                reportRespnse: action.payload.report,
            }
        case EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequestSuccess:
            return {
                ...state,
                isFollowUpReport: false,
                patientAttendedVisitOptions: action.payload.options,
            }
        
        case EFollowUpRelatedReportActions.setFollowUpReportIsCompleted:
            return {
                ...state,
                isCompleted: action.payload.value,
            }
             
        default: return state;
    }
}

export default followUpReportReducer;