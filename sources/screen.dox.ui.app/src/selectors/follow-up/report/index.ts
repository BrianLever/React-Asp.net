import { IRootState } from '../../../states';
import { IFollowUpReportResponse, TFollowUpPatientInfo } from '../../../actions/follow-up/report';
import { TChoiceItem } from '../../../actions';
import { convertDateToStringFormat } from 'helpers/dateHelper';
import { DEFAULT_DATETIMEOFFSET_FORMAT } from 'helpers/general';
import { COMMON_EMPTY_RESPONSE } from 'helpers/general';


export const isFollowUpReportLoadingSelector = (state: IRootState): boolean => state.followUpReport.isFollowUpReport;
export const getFollowUpReportServerResponse = (state: IRootState): IFollowUpReportResponse | null => state.followUpReport.reportRespnse;
export const getCurrentSelectedPatientAttendedVisitOptionSelector = (state: IRootState)
: number => state.followUpReport.currentSelectedPatientAttendedVisitOption;
export const getCurrentSelectedPatientAttendedVisitDateSelector = (state: IRootState)
: string| null => state.followUpReport.currentSelectedPatientAttendedVisitDate;
export const getNewVisitReferralRecommendationSelector = (state: IRootState)
: string => state.followUpReport.newVisitReferralRecommendation;
export const getCurrentlySelectedNewVisitReferralRecommendationAcceptedOptionSelector = (state: IRootState)
: number => state.followUpReport.currentlySelectedNewVisitReferralRecommendationAcceptedOption;
export const getCurrentlySelectedNewVisitReferralRecommendationNotAcceptedOptionSelector = (state: IRootState)
: number => state.followUpReport.currentlySelectedNewVisitReferralRecommendationNotAcceptedOption;
export const getCurrentlySelectedFollowUpCreateFlagSelector = (state: IRootState)
: boolean => state.followUpReport.followUpReportCreateFlag;
export const getCurrentlySelectedFollowUpCreateDateSelector = (state: IRootState)
: string | null => state.followUpReport.followUpReportCreateDate;
export const getCurrentlySelectedDischargedOptionSelector = (state: IRootState)
: number => state.followUpReport.currentlySelectedDischargedOption;
export const getNewVisitDateSelector = (state: IRootState)
: string | null => state.followUpReport.newVisitDate;
export const getFollowUpSelector = (state: IRootState) => state.followUpReport.followUp;
export const getFollowUpDateSelector = (state: IRootState) => state.followUpReport.followUpDate;
export const getFollowUpNoteSelector = (state: IRootState): string => state.followUpReport.followUpNode;

export const getCurrentSelectedFollowUpContactOutcomeOption = (state: IRootState)
: number => state.followUpReport.currentSelectedFollowUpContactOutcomeOption;
export const getcurrentSelectedNewVisitReferralRecommendationOption = (state: IRootState)
: number => state.followUpReport.currentSelectedNewVisitReferralRecommendationOption;
export const getPatientAttendedVisitAllOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.followUpReport.patientAttendedVisitOptions;
export const getNewVisitReferralRecommendationAcceptedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.followUpReport.newVisitReferralRecommendationAcceptedOptions;
export const getNewVisitReferralRecommendationNotAcceptedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.followUpReport.newVisitReferralRecommendationNotAcceptedOptions;
export const getDischargedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.followUpReport.dischargedOptions;


export const getFollowUpReportPatientAttendedVisitOptionsSelector = 
(state: IRootState): Array<TChoiceItem> => state.followUpReport.patientAttendedVisitOptions;
export const getFollowUpReportFollowUpContactOutcomeOptionsSelector = 
(state: IRootState): Array<TChoiceItem> => state.followUpReport.followUpContactOutcomeOptions;
export const getFollowUpReportNewVisitReferralRecommendationOptionsSelector = 
(state: IRootState): Array<TChoiceItem> => state.followUpReport.newVisitReferralRecommendationOptions;

export const getValidFollowUpReportPatientInfo = (state: IRootState): TFollowUpPatientInfo | null => {
    try {
        const response = getFollowUpReportServerResponse(state);
        return response && response.Result.PatientInfo;
    } catch(e) {
        return null;
    }
}

export const getFollowUpReportPatientInfo = (state: IRootState): TFollowUpPatientInfo | null => {
    const patientInfo = getValidFollowUpReportPatientInfo(state);
    if (!patientInfo || typeof patientInfo === 'boolean') {
        return null;
    }
    return patientInfo;
}

export const getFollowUpReportPatientInfoCity = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.City || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoPhone = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.Phone || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoStateID = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.StateID || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoStateName = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.StateName || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoStreetAddress = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.StreetAddress || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoZipCode = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.ZipCode || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoBirthday = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.Birthday || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoFirstName = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.FirstName || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoLastName = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.LastName || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoFullName = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.FullName || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportStaffCompleted = (state: IRootState): string => {
    const report = getFollowUpReportServerResponse(state);
    if (!report) {
        return COMMON_EMPTY_RESPONSE;
    }
    return report.StaffNameCompleted || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportPatientInfoMiddleName = (state: IRootState): string => {
    const patientInfo = getFollowUpReportPatientInfo(state);
    if (!patientInfo) {
        return COMMON_EMPTY_RESPONSE;
    }
    return patientInfo.MiddleName || COMMON_EMPTY_RESPONSE;
}


export const getFollowUpReportPatientHealthRecordNo = (state: IRootState): string => {
    const report = getFollowUpReportServerResponse(state);
    if (!report?.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return report.Result.ExportedToHRN || COMMON_EMPTY_RESPONSE;
}


export const getFollowUpReportLocationSelector = (state: IRootState) => {
    const report = getFollowUpReportServerResponse(state);
    if (!report?.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return report.Result.LocationLabel || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReportCompleteDate = (state: IRootState) => {
    const report = getFollowUpReportServerResponse(state);
    if (!report?.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return convertDateToStringFormat(report.CompleteDate, DEFAULT_DATETIMEOFFSET_FORMAT) || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportCreatedDate = (state: IRootState) => {
    const report = getFollowUpReportServerResponse(state);
    if (!report?.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return report.Result.CreatedDate || COMMON_EMPTY_RESPONSE;
}

export const getFollowUpReporScreeningResultId = (state: IRootState) => {
    const report = getFollowUpReportServerResponse(state);
    if (!report?.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return report.Result.ID || COMMON_EMPTY_RESPONSE;
}

export const isCompletedSelector = (s: IRootState): boolean => s.followUpReport.isCompleted;
