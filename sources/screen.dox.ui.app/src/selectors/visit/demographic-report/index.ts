import { IVisitDemographicReportResponse, TVisitDemographicPatientInfo } from '../../../actions/visit/demographic-report';
import { IRootState } from '../../../states';

const COMMON_EMPTY_RESPONSE = 'N/A';

export const getVisitDemographicReportSelector = (state: IRootState) => state.visitDemographicReport.demographicReport;
export const getSelectedVisitDemographicRaceSelector = (state: IRootState) => state.visitDemographicReport.selectedRace;
export const getSelectedVisitDemographicGenderSelector = (state: IRootState) => state.visitDemographicReport.selectedGender;
export const getSelectedVisitDemographicSexualOrientationSelector = (state: IRootState) => state.visitDemographicReport.selectedSexualOrientation;
export const getSelectedVisitDemographicEducationLevelSelector = (state: IRootState) => state.visitDemographicReport.selectedEducationLevel;
export const getSelectedVisitDemographicMaritalStatusSelector = (state: IRootState) => state.visitDemographicReport.selectedMeritalStatus;
export const getSelectedVisitDemographicLivingOnReservationSelector = (state: IRootState) => state.visitDemographicReport.selecteLivingOnReservation;
export const getSelectedVisitDemographicTribalAffiliationSelector = (state: IRootState) => state.visitDemographicReport.tribalAffiliationValue;
export const getSelectedVisitDemographicCountyOfResidenceValueSelector = (state: IRootState) => state.visitDemographicReport.countyOfResidenceValue;

export const getVisitDemographicRaceOptionsSelector = (state: IRootState) => state.visitDemographicReport.raceOptions;
export const getVisitDemographicGenderOptionsSelector = (state: IRootState) => state.visitDemographicReport.genderOptions;
export const getVisitDemographicSexualOrientationOptionsSelector = (state: IRootState) => state.visitDemographicReport.sexualOrientationOptions;
export const getVisitDemographicEducationLevelOptionsSelector = (state: IRootState) => state.visitDemographicReport.educationLevelOptions;
export const getVisitDemographicMaritalStatusOptionsSelector = (state: IRootState) => state.visitDemographicReport.maritalStatusOptions;
export const getVisitDemographicLivingOnReservationSelector = (state: IRootState) => state.visitDemographicReport.livingOnReservationOptions;
export const getVisitDemographicMilitaryExperienceSelector = (state: IRootState) => state.visitDemographicReport.militaryExperienceOptions;
export const geSelectedVisitDemographicMilitaryExperienceSelector = (state: IRootState) => state.visitDemographicReport.selectedMilitaryExperienceOptions;

export const isVisitDemographicReportLoadingSelector = (state: IRootState) => state.visitDemographicReport.isVisitDemographicLoading;

export const getVisitDemographicTribalAffiliationArraySelector = (state: IRootState) => state.visitDemographicReport.tribalAffiliationArrayValues;
export const getVisitDemographicTribalAffiliationArrayLoadingSelector = (state: IRootState) => state.visitDemographicReport.isTribalAffiliationLoading;


export const getVisitDemographicCountyOfResidenceArraySelector = (state: IRootState) => state.visitDemographicReport.countyOfResidenceArrayValues;
export const getVisitDemographicCountyOfResidenceArrayLoadingSelector = (state: IRootState) => state.visitDemographicReport.isCountyOfResidenceArrayLoading;

export const getVisitDemoGraphicStaffNameCompltedSelector = (state: IRootState) => state.visitDemographicReport.staffNameCompleted;
export const getVisitDemoGraphicCompletedDateSelector  = (state: IRootState) => state.visitDemographicReport.completeDate;

const getReceivedPatientInfo = (visitReport: IVisitDemographicReportResponse | null): TVisitDemographicPatientInfo | null => {
    if (!visitReport || !visitReport.Result || !visitReport.Result.PatientInfo) {
        return null;
    } else {
        return visitReport.Result.PatientInfo;
    }
}

export const getVisitDemographicReportPatientLastName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.LastName || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientFirstName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.FirstName || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientFullName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.FullName || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientPhone = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.Phone || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientBirthDate = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.Birthday || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientMiddleName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.MiddleName || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientStreetAddress = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.StreetAddress || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientZipCode = (state: IRootState): string => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.ZipCode || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientStateName = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.StateName || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportPatientCity = (state: IRootState) => {
    const patientInfo = getReceivedPatientInfo(state.visitDemographicReport.demographicReport);
    if (!patientInfo) { return COMMON_EMPTY_RESPONSE; }
    return patientInfo.City || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportExportedToHRN = (state: IRootState) => {
    if (!state.visitDemographicReport || !state.visitDemographicReport.demographicReport || !state.visitDemographicReport.demographicReport.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitDemographicReport.demographicReport.Result.ExportedToHRN || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportLocation = (state: IRootState) => {
    if (!state.visitDemographicReport || !state.visitDemographicReport.demographicReport || !state.visitDemographicReport.demographicReport.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitDemographicReport.demographicReport.Result.LocationLabel || COMMON_EMPTY_RESPONSE;
}

export const getVisitDemographicReportCreatedDate = (state: IRootState) => {
    if (!state.visitDemographicReport || !state.visitDemographicReport.demographicReport || !state.visitDemographicReport.demographicReport.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitDemographicReport.demographicReport.Result.CreatedDate || COMMON_EMPTY_RESPONSE;
}

export const getVisitReportScreeningResultId = (state: IRootState) => {
    if (!state.visitDemographicReport || !state.visitDemographicReport.demographicReport || !state.visitDemographicReport.demographicReport.Result) { 
        return COMMON_EMPTY_RESPONSE; 
    }
    return state.visitDemographicReport.demographicReport.ScreeningResultID || COMMON_EMPTY_RESPONSE;
}