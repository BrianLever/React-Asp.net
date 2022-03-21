import { 
    IVisitReportResponse, TChoiceItem, TOtherScreeningToolsItem, TVisitPatientInfo, TTreatmentActionsItem
} from '../../../actions/visit/report';
import { IRootState } from '../../../states';

import { COMMON_EMPTY_RESPONSE, EMPTY_STRING_VALUE } from 'helpers/general';
import { convertDateToStringFormat } from 'helpers/dateHelper';
import { DEFAULT_DATETIMEOFFSET_FORMAT } from 'helpers/general';
import { IFollowUpInnerInnerItem } from 'actions/follow-up';

export const getVisitFollowUpSelector = (state: IRootState) => state.visitReport.followUp;
export const getVisitFollowUpDateSelector = (state: IRootState) => state.visitReport.followUpDate;
export const getVisitFollowNoteSelector = (state: IRootState) => state.visitReport.visitNode;
export const getVisitDischargedSelector = (state: IRootState) => state.visitReport.discharged;
export const getVisitDateSelector = (state: IRootState) => state.visitReport.visitDate;
export const getVisitReferralRecommendationDescriptionSelector = (state: IRootState) => state.visitReport.referralRecDescription;
export const getVisitReferralRecommendationAcceptedSelector = (state: IRootState) => state.visitReport.referralRecommendationAccepted;
export const getVisitReferralRecommendationNotAcceptedSelector = (state: IRootState) => state.visitReport.referralRecommendationNotAccepted;
export const getVisitReferralRecommendationIdSelector = (state: IRootState) => state.visitReport.referralRecId;
export const getCurrentVisitReportSelector = (state: IRootState) => state.visitReport.reportResponse;
export const getVisitDrugChoiceOptionSelector = (state: IRootState): Array<TChoiceItem> => state.visitReport.drugChoiceOptions;

export const getVisitReferalRecomendationOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.visitReport.referalRecomendationOptions;

export const getVisitReferalRecomendationAcceptedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.visitReport.referalRecomendationAcceptedOptions;

export const getVisitReferalRecomendationNotAcceptedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.visitReport.referalRecomendationNotAcceptedOptions;

export const getVisitDischargedOptionsSelector = (state: IRootState)
: Array<TChoiceItem> => state.visitReport.dischargedOptions;

export const getVisitDrugPrimaryItemSelector = (state: IRootState)
: TChoiceItem | null => state.visitReport.drugPrimaryOption;

export const getVisitDrugSecondaryItemSelector = (state: IRootState)
: TChoiceItem | null => state.visitReport.drugSecondaryOption;

export const getVisitDrugTertiaryItemSelector = (state: IRootState)
: TChoiceItem | null => state.visitReport.drugTertiaryOption;

export const getVisitProblemGamblingFlagScoreLevelLabelSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.ProblemGamblingFlag) {
        return '';
    }
    return state.visitReport.reportResponse.ProblemGamblingFlag.ScoreLevelLabel;
};

export const getVisitProblemGamblingFlagScoreLevelSelector = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.ProblemGamblingFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.ProblemGamblingFlag.ScoreLevel;
};

export const getVisitAnxietyFlagScoreLevelLabelSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.AnxietyFlag) {
        return '';
    }
    return state.visitReport.reportResponse.AnxietyFlag.ScoreLevelLabel;
};

export const getVisitPartnerViolenceFlagScoreLevelLabelSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.PartnerViolenceFlag) {
        return '';
    }
    return state.visitReport.reportResponse.PartnerViolenceFlag.ScoreLevelLabel;
};

export const getVisitPartnerViolenceFlagScoreLevelSelector = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.PartnerViolenceFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.PartnerViolenceFlag.ScoreLevel;
};

export const getVisitAnxietyFlagScoreLevelSelector = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.AnxietyFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.AnxietyFlag.ScoreLevel;
};

export const getVisitDepressionFlagScoreLevelLabelSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.DepressionFlag) {
        return '';
    }
    return state.visitReport.reportResponse.DepressionFlag.ScoreLevelLabel;
};

export const getVisitDepressionFlagScoreLevelSelector = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.DepressionFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.DepressionFlag.ScoreLevel;
};

export const getVisitDepressionThinkOfDeathAnswerSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse) {
        return '';
    }
    return state.visitReport.reportResponse.DepressionThinkOfDeathAnswer;
};


const getReceivedPatientInfo = (visitReport: IVisitReportResponse | null): TVisitPatientInfo | null => {
    if (!visitReport || !visitReport.Result || !visitReport.Result.PatientInfo) {
        return null;
    } else {
        return visitReport.Result.PatientInfo;
    }
}

export const getVisitReportSubstanceAbuseScoreLevel = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.SubstanceAbuseFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.SubstanceAbuseFlag.ScoreLevel || 0;
}

export const getVisitReportSubstanceAbuseScoreLevelLabel = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.SubstanceAbuseFlag) {
        return '';
    }
    return state.visitReport.reportResponse.SubstanceAbuseFlag.ScoreLevelLabel || '';
}

export const getVisitReportTobacoExposureSmokerInHomeFlag = (state: IRootState): boolean => {
    if (!state.visitReport.reportResponse) {
        return false;
    }
    return state.visitReport.reportResponse.TobacoExposureSmokerInHomeFlag || false;
}

export const getVisitReportTobacoExposureSmokingFlag = (state: IRootState): boolean => {
    if (!state.visitReport.reportResponse) {
        return false;
    }
    return state.visitReport.reportResponse.TobacoExposureSmokingFlag || false;
}

export const getVisitReportTobacoExposureSmoklessFlag = (state: IRootState): boolean => {
    if (!state.visitReport.reportResponse) {
        return false;
    }
    return state.visitReport.reportResponse.TobacoExposureSmoklessFlag || false;
}

export const getVisitReportAlcoholUseScoreLevelLabelSelector = (state: IRootState): string => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.AlcoholUseFlag) {
        return '';
    }
    return state.visitReport.reportResponse.AlcoholUseFlag.ScoreLevelLabel || '';
}

export const getVisitReportAlcoholUseScoreLevelSelector = (state: IRootState): number => {
    if (!state.visitReport.reportResponse || !state.visitReport.reportResponse.AlcoholUseFlag) {
        return 0;
    }
    return state.visitReport.reportResponse.AlcoholUseFlag.ScoreLevel || 0;
}

export const getVisitReportPatientLastName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.LastName || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientFirstName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.FirstName || COMMON_EMPTY_RESPONSE;
}



export const getVisitReportPatientFullName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.FullName || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientPhone = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.Phone || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientBirthDate = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.Birthday || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientMiddleName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.MiddleName || EMPTY_STRING_VALUE;
}

export const getVisitReportPatientStreetAddress = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.StreetAddress || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientZipCode = (state: IRootState): string => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.ZipCode || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientStateName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.StateName || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportPatientCity = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitReport.reportResponse);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.City || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportExportedToHRN = (state: IRootState) => {
    if (!state.visitReport || !state.visitReport.reportResponse || !state.visitReport.reportResponse.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitReport.reportResponse.Result.ExportedToHRN || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportLocation = (state: IRootState) => {
    if (!state.visitReport || !state.visitReport.reportResponse || !state.visitReport.reportResponse.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitReport.reportResponse.Result.LocationLabel || COMMON_EMPTY_RESPONSE;
}

export const getVisitReporStaffNameCompleted = (state: IRootState) => {
    if (!state.visitReport || !state.visitReport.reportResponse) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitReport.reportResponse.StaffNameCompleted || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportScreeningResultIdSelector = (state: IRootState) => {
    if (!state.visitReport || !state.visitReport.reportResponse) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitReport.reportResponse.ScreeningResultID || COMMON_EMPTY_RESPONSE;
}
// export const getVisitReportScreeningResultIdSelector = (state: IRootState) => state.visitReport.reportResponse?.ScreeningResultID;

export const getVisitReportCompleteDate = (state: IRootState) => {
    if (!state.visitReport || !state.visitReport.reportResponse) { 
        return COMMON_EMPTY_RESPONSE; 
    }

    return convertDateToStringFormat(new Date(Date.parse(state.visitReport.reportResponse.CompleteDate)), DEFAULT_DATETIMEOFFSET_FORMAT) || COMMON_EMPTY_RESPONSE;
}


export const getVisitReporScreendoxId = (state: IRootState) : string => {
    if (!state.visitReport || !state.visitReport.reportResponse) { 
        return COMMON_EMPTY_RESPONSE; 
    }

    return state.visitReport.reportResponse.ID.toString();
}


export const getOtherScreeningToolsSelector = (state: IRootState):  Array<TOtherScreeningToolsItem> => {
    if (!state.visitReport || !state.visitReport.otherScreeningTools) {
        return [];
    }
    return state.visitReport.otherScreeningTools;
};

export const getTritmentActionToolsSelector = (state: IRootState):  Array<TTreatmentActionsItem> => {
    if (!state.visitReport || !state.visitReport.tritmentActionTools) {
        return [];
    }
    return state.visitReport.tritmentActionTools;
};

export const getTritmenOptionsSelector = (state: IRootState):  Array<TTreatmentActionsItem> => {
    if (!state.visitReport || !state.visitReport.tritmentOptions) {
        return [];
    }
    return state.visitReport.tritmentOptions;
};

export const getSaveChangesFlag = (s: IRootState): boolean => s.visitReport.isSaveChanges;

export const isCompletedSelector = (s: IRootState): boolean => s.visitReport.isCompleted;

export const getFollowUpVisitArraySelector = (s: IRootState): Array<IFollowUpInnerInnerItem> => s.visitReport.followUpVisitArray;