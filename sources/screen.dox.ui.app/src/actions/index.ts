import { Action } from 'redux';

export enum EDashboardActionType {
    getSystemSettingsRequest = 'GET_SYSTEM_SETTINGS_REQUEST',
    getSystemSettingsRequestStart = 'GET_SYSTEM_SETTINGS_START',
    getSystemSettingsRequestSuccess = 'GET_SYSTEM_SETTINGS_SUCCESS',
    getSystemSettingsRequestError = 'GET_SYSTEM_SETTINGS_ERROR',
}

export enum EUserProfileActionType {
    getProfileRequest = 'GET_PROFILE_REQUEST',
    getProfileRequestStart = 'GET_PROFILE_REQUEST_START',
    getProfileRequestError = 'GET_PROFILE_REQUEST_ERROR',
    getProfileRequestSuccess = 'GET_PROFILE_REQUEST_SUCCESS',

    setProfile = 'SET_PROFILE',

    updateProfileRequest = 'UPDATE_PROFILE_REQUEST',
    updateProfileRequestStart = 'UPDATE_PROFILE_REQUEST_START',
    updateProfileRequestError = 'UPDATE_PROFILE_REQUEST_ERROR',
    updateProfileRequestSuccess = 'UPDATE_PROFILE_REQUEST_SUCCESS',
}

export enum EScreenActionType {
    postScreenListSearchRequest = 'POST_SCREEN_LIST_SEARCH_REQUEST',
    postScreenListSearchRequestStart = 'POST_SCREEN_LIST_SEARCH_REQUEST_START',
    postScreenListSearchRequestSuccess = 'POST_SCREEN_LIST_SEARCH_REQUEST_SUCCESS',
    postScreenListSearchRequestError = 'POST_SCREEN_LIST_SEARCH_REQUEST_ERROR',

    postScreenListFilterRequest = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST',
    postScreenListFilterRequestStart = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_START',
    postScreenListFilterRequestSucess = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_SUCCESS',
    postScreenListFilterRequestError = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_ERROR',

    postScreenListFilterAutoUpdateSet = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_AUTO_UPDATE_SET',
    postScreenListFilterAutoUpdat = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_AUTO_UPDATE',
    postScreenListFilterAutoUpdatRecall = 'POST_SCREEN_LIST_SEARCH_FILTER_REQUEST_AUTO_UPDATE_RECALL',

    changeActiveSort = 'CHANGE_ACTIVE_SORT',
    changeActiveKey = 'CHANGE_ACTIVE_KEY',
    changeActiveDirection = 'CHANGE_ACTIVE_DIRECTION',

    getInternalScreenListItemDataRequest = 'GET_INTERNAL_SCREEN_LIST_ITEM_DATA_REQUEST',
    getInternalScreenListItemDataRequestStart = 'GET_INTERNAL_SCREEN_LIST_ITEM_DATA_START',
    getInternalScreenListItemDataRequestSuccess = 'GET_INTERNAL_SCREEN_LIST_ITEM_DATA_SUCCESS',
    getInternalScreenListItemDataRequestError = 'GET_INTERNAL_SCREEN_LIST_ITEM_DATA_ERROR',
    
    setFirstName = 'SET_FIRST_NAME',
    setLastName = 'SET_LAST_NAME',
    setScreendoxReultId = 'SET_SCREENDOX_RESULT_ID',
    setRequestScreeningId = 'SET_REQUEST_SCREENING_ID',
    setScreendoxStartDate = 'SET_SCREENDOX_START_DATE_ID',
    setScreendoxEndDate = 'SET_SCREENDOX_END_DATE_ID',
    setScreendoxGRPAPeriod = 'SET_SCREENDOX_GRPA_PERIOD',
    clearSearchParameters = 'CLEAR_SEARCH_PARAMETERS',
    triggerClearSearchParameters = 'TRIGGER_CLEAR_SEARCH_PARAMETERS',
    triggerSearchParameters = 'TRIGGER_SEARCH_PARAMETERS',

    setCurrentScreenListOffset = 'SET_CURRENT_SCREEN_LIST_OFFSET',
    setCurrentScreenListPage = 'SET_CURRENT_SCREEN_LIST_PAGE',

    getGPRAPeriodRequest = 'GET_GPRA_PERIOD_REQUEST',
    getGPRAPeriodStart = 'GET_GPRA_PERIOD_START',
    getGPRAPeriodSuccess = 'GET_GPRA_PERIOD_SUCCESS',
    getGPRAPeriodError = 'GET_GPRA_PERIOD_ERROR',

    getLocationListRequest = 'GET_LOCATION_LIST_REQUEST',
    getLocationListRequestStart = 'GET_LOCATION_LIST_START',
    getLocationListRequestSuccess = 'GET_LOCATION_LIST_SUCCESS',
    getLocationListRequestError = 'GET_LOCATION_LIST_ERROR',

    setLocationId = 'SET_LOCATION_ID',

    getScreenListEhrExportPatientInfoRequest = 'GET_SCREENLIST_EHR_EXPORT_PATIENT_INFO_REQUEST',
    getScreenListEhrExportPatientInfoRequestSuccess = 'GET_SCREENLIST_EHR_EXPORT_PATIENT_INFO_REQUEST_SUCCESS',

    postScreenListEhrExportPatientInfoRequest = 'POST_SCREEN_LIST_EHR_EXPORT_PATIENT_INFO_REQUEST',
    postScreenListEhrExportPatientInfoRequestStart = 'POST_SCREEN_LIST_EHR_EXPORT_PATIENT_INFO_REQUEST_START',
    postScreenListEhrExportPatientInfoRequestSuccess = 'POST_SCREEN_LIST_EHR_EXPORT_PATIENT_INFO_REQUEST_SUCCESS',
    postScreenListEhrExportPatientInfoRequestError = 'POST_SCREEN_LIST_EHR_EXPORT_PATIENT_INFO_REQUEST_ERROR',

    getScreenListEhrExportPatientRecordsSuccess = 'GET_SCREEN_LIST_EHR_EXPORT_PATIENT_RECORDS_SUCCESS',
    getScreenListEhrExportPatientRecordsError = 'GET_SCREEN_LIST_EHR_EXPORT_PATIENT_RECORDS_ERROR',
    getScreenListEhrExportPatientRecordsStart = 'GET_SCREEN_LIST_EHR_EXPORT_PATIENT_RECORDS_START',

    setEhrExportPatientRecordSelectedId = 'SET_EHR_EXPORT_PATIENT_RECORD_SELECTED_ID',
    setEhrExportPatientRecordsCurrentPage = 'SET_EHR_EXPORT_PATIENT_RECORDS_CURRENT_PAGE',

    getScreenListEhrExportVisitRecordsRequest = 'GET_SCREEN_LIST_EHR_EXPORT_VISIT_RECORDS_REQUEST',
    getScreenListEhrExportVisitRecordsRequestStart = 'GET_SCREEN_LIST_EHR_EXPORT_VISIT_RECORDS_REQUEST_START',
    getScreenListEhrExportVisitRecordsRequestSuccess = 'GET_SCREEN_LIST_EHR_EXPORT_VISIT_RECORDS_REQUEST_SUCCESS',
    getScreenListEhrExportVisitRecordsRequestError = 'GET_SCREEN_LIST_EHR_EXPORT_VISIT_RECORDS_REQUEST_ERROR',

    setEhrExportVisitRecordSelectedId = 'SET_EHR_EXPORT_VISIT_RECORD_SELECTED_ID',
    setEhrExportVisitRecordsCurrentPage = 'SET_EHR_EXPORT_VISIT_RECORDS_CURRENT_PAGE',

    ehrExportFinalResultRequest = 'EHR_EXPORT_FINAL_RESULT_REQUEST',
    ehrExportFinalResultRequestStart = 'EHR_EXPORT_FINAL_RESULT_REQUEST_START',
    ehrExportFinalResultRequestError = 'EHR_EXPORT_FINAL_RESULT_REQUEST_ERROR',
    ehrExportFinalResultRequestSuccess = 'EHR_EXPORT_FINAL_RESULT_REQUEST_SUCCESS',

    setEhrExportScreeningResultId = 'SET_EHR_EXPORT_SCREEING_RESULT_ID',
    setEhrExportScreeningDate = 'SET_EHR_EXPORT_SCREEING_DATE',
    setEhrExportSelectedTab = 'SET_EHR_EXPORT_SELECTED_TAB',

    setEhrExportScreeningResults = 'SET_EHR_EXPORT_SCREENING_RESULTS',
}

export enum EApplicationSettingsActions {
    switchLoadingOn = 'SWITCH_LOADING_ON',
    switchLoadingOff = 'SWITCH_LOADING_OFF',

    sideDrawerIn = 'SIDE_DRAWER_IN',
    sideDrawerOut = 'SIDE_DRAWER_OUT',
    triggerSideDrawerState = 'TRIGGER_SIDE_DRAWER_STATE',
    notifyError = 'SET_NOTIFICATION_ERROR',
    notifySuccess = 'SET_NOTIFICATION_SUCCESS',
    resetNotify = 'RESET_NOTIFICATION',
    setCurrentPage = 'SET_CURRENT_PAGE',

    modalWindowOpen = 'MODAL_WINDOW_OPEN',
    modalWindowClose = 'MODAL_WINDOW_CLOSE'

}

export enum EScreenReportActions {
    getScreenReportDefinitionRequest = 'GET_SCREEN_REPORT_DEFINITION',
    getScreenReportDefinitionStart = 'GET_SCREEN_REPORT_DEFINITION_START',
    getScreenReportDefinitionError = 'GET_SCREEN_REPORT_DEFINITION_ERROR',
    getScreenReportDefinitionSuccess = 'GET_SCREEN_REPORT_DEFINITION_SUCCESS',

    getScreeningReportRequest = 'GET_SCREENING_REPORT_REQUEST',
    getScreeningReportRequestStart = 'GET_SCREENING_REPORT_REQUEST_START',
    getScreeningReportRequestSuccess = 'GET_SCREENING_REPORT_REQUEST_SUCCESS',
    getScreeningReportRequestError = 'GET_SCREENING_REPORT_REQUEST_ERROR',
    resetScreeningReportRequest = 'REST_SCREENING_REPORT_REQUEST',

    setScreeningReportSectionsObjet = 'SET_SCREENING_REPORT_SECTIONS_OBJECT',

    deleteScreeningReportRequest = 'DELETE_SCREENING_REPORT_REQUEST',
    deleteScreeningReportRequestStart = 'DELETE_SCREENING_REPORT_REQUEST_START',
    deleteScreeningReportRequestError = 'DELETE_SCREENING_REPORT_REQUEST_ERROR',
    deleteScreeningReportRequestSuccess = 'DELETE_SCREENING_REPORT_REQUEST_SUCCESS',


    getScreenReportVisitId = 'GET_SCREEN_REPORT_VISIT_ID',
    getScreenReportVisitIdRequestSuccess = 'GET_SCREEN_REPORT_VISIT_ID_REQUEST_SUCCESS',
    getScreenReportVisitIdRequestStart = 'GET_SCREEN_REPORT_VISIT_ID_REQUEST_START'
}


export enum EVisitActions {
    getVisitRequest =        'GET_ALL_VISIT_REQUEST',
    getVisitRequestStart =   'GET_ALL_VISIT_REQUEST_START',
    getVisitRequestSuccess = 'GET_ALL_VISIT_REQUEST_SUCCESS',
    getVisitRequestError =   'GET_ALL_VISIT_REQUEST_ERROR',
    postVisitListFilterRequest =        'POST_VISIT_LIST_SEARCH_FILTER_REQUEST',
    postVisitListFilterRequestStart =   'POST_VISIT_LIST_SEARCH_FILTER_REQUEST_START',
    postVisitListFilterRequestSucess =  'POST_VISIT_LIST_SEARCH_FILTER_REQUEST_SUCCESS',
    postVisitListFilterRequestError =   'POST_VISIT_LIST_SEARCH_FILTER_REQUEST_ERROR',
    setVisitOffset = 'SET_VISIT_LIST_OFFSET',
    setVisitCurrentPage = 'SET_VISIT_CURRENT_PAGE',
    requestVisitCurrentPage = 'REQUEST_VISIT_CURRENT_PAGE',
    setVisitListSortKey = 'SET_VISIT_LIST_SORT_KEY',
    setVisitListSortDirection = 'SET_VISIT_LIST_SORT_DIRECTION',
    requestVisitListSort = 'REQUEST_VISIT_LIST_SORT',
    setVisitSearchFirstName = 'SET_VISIT_SEARCH_FIRST_NAME',
    setVisitSearchLastName = 'SET_VISIT_SEARCH_LAST_NAME',
    setVisitSearchScreeningID = 'SET_VISIT_SEARCH_SCREENING_ID',
    getVisitLocationListRequest = 'GET_VISIT_LOCATION_LIST_REQUEST',
    getVisitLocationListRequestStart = 'GET_VISIT_LOCATION_LIST_START',
    getVisitLocationListRequestSuccess = 'GET_VISIT_LOCATION_LIST_SUCCESS',
    getVisitLocationListRequestError = 'GET_VISIT_LOCATION_LIST_ERROR',
    setVisitLocationId = 'SET__VISIT_LOCATION_ID',
    setVisitScreendoxStartDate = 'SET_VISIT_SCREENDOX_START_DATE',
    setVisitScreendoxEndDate = 'SET_VISIT_SCREENDOX_END_DATE',
    setVisitScreendoxGRPAPeriod = 'SET_VISIT_SCREENDOX_GRPA_PERIOD',
    triggerVisitSearchParameters = 'TRIGGER_VISIT_SEARCH_PARAMETERS_REQUEST',
    cleanVisitSearchParameters = 'CLEAN_VISIT_SEARCH_PARAMETERS',
    requestTocleanVisitSearchParameters = 'REQUEST_TO_CLEAN_VISIT_SEARCH_PARAMETERS',
    setBSRReportType = 'SET_BSR_REPORT_TYPE',
    setVisitScreeningId = 'SET_VISIT_SCREENING_ID',
    getRelatedVisitByIDRequest = 'GET_RELATED_VISIT_BY_ID_REQUEST',
    getRelatedVisitByIDRequestStart = 'GET_RELATED_VISIT_BY_ID_REQUEST_START',
    getRelatedVisitByIDRequestError = 'GET_RELATED_VISIT_BY_ID_REQUEST_ERROR',
    getRelatedVisitByIDRequestSuccess = 'GET_RELATED_VISIT_BY_ID_REQUEST_SUCCESS',
    changeAutoUpdateStatus = 'CHANGE_AUTO_UPDATE_STATUS',
    changeAutoUpdateStatusRequest = 'CHANGE_AUTO_UPDATE_STATUS_REQUEST',
}

export enum EVisitReportActions {
    getVisitReportByIdRequest = 'GET_VISIT_REPORT_BY_ID_REQUEST',
    getVisitReportByIdStart = 'GET_VISIT_REPORT_BY_ID_REQUEST_START',
    getVisitReportByIdError = 'GET_VISIT_REPORT_BY_ID_REQUEST_ERROR',
    getVisitReportByIdSuccess = 'GET_VISIT_REPORT_BY_ID_REQUEST_SUCCESS',

    getDrugChoiceOptionRequest = 'GET_DRUG_CHOICE_OPTION_REQUEST',
    getDrugChoiceOptionRequestStart = 'GET_DRUG_CHOICE_OPTION_REQUEST_START',
    getDrugChoiceOptionRequestError = 'GET_DRUG_CHOICE_OPTION_REQUEST_ERROR',
    getDrugChoiceOptionRequestSuccess = 'GET_DRUG_CHOICE_OPTION_REQUEST_SUCCESS',

    getTritmentActionRequest = 'GET_TRITMENT_ACTION_REQUEST',
    getTritmentActionRequestStart = 'GET_TRITMENT_ACTION_REQUEST_START',
    getTritmentActionRequestError = 'GET_TRITMENT_ACTION_REQUEST_ERROR',
    getTritmentActionRequestSuccess = 'GET_TRITMENT_ACTION_REQUEST_SUCCESS',

    getVisitNewReferalRecomendationRequest = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_REQUEST',
    getVisitNewReferalRecomendationRequestStart = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_REQUEST_START',
    getVisitNewReferalRecomendationRequestError = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_REQUEST_ERROR',
    getVisitNewReferalRecomendationRequestSuccess = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_REQUEST_SUCCESS',

    getVisitNewReferalRecomendationAcceptedRequest = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_ACCEPTED_REQUEST',
    getVisitNewReferalRecomendationAcceptedRequestStart = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_ACCEPTED_REQUEST_START',
    getVisitNewReferalRecomendationAcceptedRequestError = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_ACCEPTED_REQUEST_ERROR',
    getVisitNewReferalRecomendationAcceptedRequestSuccess = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_ACCEPTED_REQUEST_SUCCESS',

    getVisitNewReferalRecomendationNotAcceptedRequest = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_NOT_ACCEPTED_REQUEST',
    getVisitNewReferalRecomendationNotAcceptedRequestStart = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_NOT_ACCEPTED_REQUEST_START',
    getVisitNewReferalRecomendationNotAcceptedRequestError = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_NOT_ACCEPTED_REQUEST_ERROR',
    getVisitNewReferalRecomendationNotAcceptedRequestSuccess = 'GET_VISIT_NEW_REFERAL_RECOMENDATION_NOT_ACCEPTED_REQUEST_SUCCESS',

    getVisitDischargedOptionsRequest = 'GET_VISIT_DISCHARGED_OPTIONS_REQUEST',
    getVisitDischargedOptionsRequestStart = 'GET_VISIT_DISCHARGED_OPTIONS_REQUEST_START',
    getVisitDischargedOptionsRequestError = 'GET_VISIT_DISCHARGED_OPTIONS_REQUEST_ERROR',
    getVisitDischargedOptionsRequestSuccess = 'GET_VISIT_DISCHARGED_OPTIONS_REQUEST_SUCCESS',

    setOtherScreeningToolsItem = 'SET_OTHER_SCREENING_TOOLS_ITEM',
    initOtherScreeningTools = 'INIT_OTHER_SCREENING_TOOLS',
    initTritmentActionTools = 'INIT_TRITMENT_ACTION_TOOLS',

    setDrugPrimaryItem = 'SET_DRUG_PRIMARY_ITEM',
    setDrugSecondaryItem = 'SET_DRUG_SECONDARY_ITEM',
    setDrugTertiaryItem = 'SET_DRUG_TERTIARY_ITEM',

    setOtherEvaluation = 'SET_OTHER_EVALUATION',
    setOtherEvaluationScore = 'SET_OTHER_EVALUATION_SCORE',

    updateVisitReportRequest = 'UPDATE_VISIT_REPORT_REQUEST',
    updateVisitReportRequestStart = 'UPDATE_VISIT_REPORT_REQUEST_START',
    updateVisitReportRequestError = 'UPDATE_VISIT_REPORT_REQUEST_ERROR',
    updateVisitReportRequestSuccess = 'UPDATE_VISIT_REPORT_REQUEST_SUCCESS',

    setVisitReferralRecommendationDescription = 'SET_VISIT_REFERRAL_RECOMMENDATION_DESCRIPTION',
    setVisitReferralRecommendationId = 'SET_VISIT_REFERRAL_RECOMMENDATION_ID',
    setVisitReferralRecommendationAccepted = 'SET_VISIT_REFERRAL_RECOMMENDED_ACCEPTED',
    setVisitReferralRecommendationNotAccepted = 'SET_VISIT_REFERRAL_RECOMMENDED_NOT_ACCEPTED',
    setVisitDischarged = 'SET_VISIT_DISCHARGED',
    setVisitDate = 'SET_VISIT_DATE',
    setVisitNotes = 'SET_VISIT_NOTES',
    setVisitFollowUp = 'SET_VISIT_FOLLOW_UP',
    setVisitFollowUpDate = 'SET_VISIT_FOLLOW_UP_DATE',
    setVisitReportIsCompleted = 'SET_VISIT_REPORT_IS_COMPLETED',
    
    setFollowUpVisitArray = 'SET_FOLLOW_UP_VISIT_ARRAY',
    getFollowUPVisitArrayRequest = 'SET_FOLLOW_UP_VISIT_ARRAY_REQUEST'
}

export enum EVisitDemographicReportActions {

    getVisitDemographicReportByIdRequest = 'GET_VISIT_DEMOGRAPHIC_REPORT',
    getVisitDemographicReportByIdRequestStart = 'GET_VISIT_DEMOGRAPHIC_REPORT_START',
    getVisitDemographicReportByIdRequestError = 'GET_VISIT_DEMOGRAPHIC_REPORT_ERROR',
    getVisitDemographicReportByIdRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_REPORT_SUCCESS',

    getVisitDemographicRaceRequest = 'GET_VISIT_DEMOGRAPHIC_RACE_REQUEST',
    getVisitDemographicRaceRequestStart = 'GET_VISIT_DEMOGRAPHIC_RACE_REQUEST_START',
    getVisitDemographicRaceRequestError = 'GET_VISIT_DEMOGRAPHIC_RACE_REQUEST_ERROR',
    getVisitDemographicRaceRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_RACE_REQUEST_SUCCESS',

    setVisitDemographicSelectedRace = 'SET_VISIT_DEMOGRAPHIC_SELECTED_RACE',

    updateVisitDemographicReportRequest = 'UPDATE_VISIT_DEMOGRAPHIC_REPORT_REQUEST',
    updateVisitDemographicReportRequestStart = 'UPDATE_VISIT_DEMOGRAPHIC_REPORT_REQUEST_START',
    updateVisitDemographicReportRequestError = 'UPDATE_VISIT_DEMOGRAPHIC_REPORT_REQUEST_ERROR',
    updateVisitDemographicReportRequestSuccess = 'UPDATE_VISIT_DEMOGRAPHIC_REPORT_REQUEST_SUCCESS',

    getVisitDemographicGenderRequest = 'GET_VISIT_DEMOGRAPHIC_GENDER_REQUEST',
    getVisitDemographicGenderRequestStart = 'GET_VISIT_DEMOGRAPHIC_GENDER_REQUEST_START',
    getVisitDemographicGenderRequestError = 'GET_VISIT_DEMOGRAPHIC_GENDER_REQUEST_ERROR',
    getVisitDemographicGenderRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_GENDER_REQUEST_SUCCESS',
    setSelectedVisitDemographicGender = 'SET_VISIT_DEMOGRAPHIC_GENDER',


    getVisitDemographicSexualOrientationRequest = 'GET_VISIT_DEMOGRAPHIC_SEXUAL_ORIENTATION_REQUEST',
    getVisitDemographicSexualOrientationRequestStart = 'GET_VISIT_DEMOGRAPHIC_SEXUAL_ORIENTATION_REQUEST_START',
    getVisitDemographicSexualOrientationRequestError = 'GET_VISIT_DEMOGRAPHIC_SEXUAL_ORIENTATION_REQUEST_ERROR',
    getVisitDemographicSexualOrientationRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_SEXUAL_ORIENTATION_REQUEST_SUCCESS',
    setSelectedVisitDemographicSexualOrientation = 'SET_VISIT_DEMOGRAPHIC_SEXUAL_ORIENTATION',

    getVisitDemographicEducationLevelRequest = 'GET_VISIT_DEMOGRAPHIC_EDUCATIONAL_LEVEL_REQUEST',
    getVisitDemographicEducationLevelRequestStart = 'GET_VISIT_DEMOGRAPHIC_EDUCATIONAL_LEVEL_REQUEST_START',
    getVisitDemographicEducationLevelRequestError = 'GET_VISIT_DEMOGRAPHIC_EDUCATIONAL_LEVEL_REQUEST_ERROR',
    getVisitDemographicEducationLevelRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_EDUCATIONAL_LEVEL_REQUEST_SUCCESS',
    setSelectedVisitDemographicEducationLevel = 'SET_VISIT_DEMOGRAPHIC_EDUCATIONAL_LEVEL',

    getVisitDemographicMeritalStatusRequest = 'GET_VISIT_DEMOGRAPHIC_MERITAL_STATUS_REQUEST',
    getVisitDemographicMeritalStatusRequestStart = 'GET_VISIT_DEMOGRAPHIC_MERITAL_STATUS_REQUEST_START',
    getVisitDemographicMeritalStatusRequestError = 'GET_VISIT_DEMOGRAPHIC_MERITAL_STATUS_REQUEST_ERROR',
    getVisitDemographicMeritalStatusRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_MERITAL_STATUS_REQUEST_SUCCESS',
    setVisitDemographicMeritalStatus = 'SET_VISIT_DEMOGRAPHIC_MERITAL_STATUS',

    getVisitDemographicLivingOnReservationRequest = 'GET_VISIT_DEMOGRAPHIC_LIVING_0N_RESERVATION_REQUEST',
    getVisitDemographicLivingOnReservationRequestStart = 'GET_VISIT_DEMOGRAPHIC_LIVING_0N_RESERVATION_REQUEST_START',
    getVisitDemographicLivingOnReservationRequestError = 'GET_VISIT_DEMOGRAPHIC_LIVING_0N_RESERVATION_REQUEST_ERROR',
    getVisitDemographicLivingOnReservationRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_LIVING_0N_RESERVATION_REQUEST_SUCCESS',
    setVisitDemographicLivingOnReservation = 'SET_VISIT_DEMOGRAPHIC_LIVING_0N_RESERVATION',
    setVisitDemographicTribalAffiliation = 'SET_VISIT_DEMOGRAPHIC_TRIBAL_AFFILIATION',
    setVisitDemographicCountyOfResidence = 'SET_VISIT_DEMOGRAPHIC_COUNTY_OF_RESIDENCE',

    getVisitDemographicMilitaryExperienceRequest = 'GET_VISIT_DEMOGRAPHIC_MILITARY_EXPERIENCE_REQUEST',
    getVisitDemographicMilitaryExperienceRequestStart = 'GET_VISIT_DEMOGRAPHIC_MILITARY_EXPERIENCE_REQUEST_START',
    getVisitDemographicMilitaryExperienceRequestError = 'GET_VISIT_DEMOGRAPHIC_MILITARY_EXPERIENCE_REQUEST_ERROR',
    getVisitDemographicMilitaryExperienceRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_MILITARY_EXPERIENCE_REQUEST_SUCCESS',
    changeVisitDemographicMilitaryExperience = 'CHANGE_VISIT_DEMOGRAPHIC_MILITARY_EXPERIENCE',

    getVisitDemographicTribalAffiliationRequest = 'GET_VISIT_DEMOGRAPHIC_TRIBAL_AFFILIATION_REQUEST',
    getVisitDemographicTribalAffiliationRequestStart = 'GET_VISIT_DEMOGRAPHIC_TRIBAL_AFFILIATION_REQUEST_START',
    getVisitDemographicTribalAffiliationRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_TRIBAL_AFFILIATION_REQUEST_SUCCESS',
    getVisitDemographicTribalAffiliationRequestError = 'GET_VISIT_DEMOGRAPHIC_TRIBAL_AFFILIATION_REQUEST_ERROR',

    getVisitDemographicCountyOfResidenceArrayRequest = 'GET_VISIT_DEMOGRAPHIC_COUNTY_OF_RESIDENCE_ARRAY_REQUEST',
    getVisitDemographicCountyOfResidenceArrayRequestSuccess = 'GET_VISIT_DEMOGRAPHIC_COUNTY_OF_RESIDENCE_ARRAY_REQUEST_SUCCESS',
    getVisitDemographicCountyOfResidenceArrayRequestStart = 'GET_VISIT_DEMOGRAPHIC_COUNTY_OF_RESIDENCE_ARRAY_REQUEST_START',
    getVisitDemographicCountyOfResidenceArrayRequestError = 'GET_VISIT_DEMOGRAPHIC_COUNTY_OF_RESIDENCE_ARRAY_REQUEST_ERROR',

    getVisitDemoGraphicCompletedDate = 'GET_VISIT_DEMOGRAPHIC_COMPLETED_DATE',
    getVisitDemoGraphicStaffNameComplted = 'GET_VISIT_DEMOGRAPHIC_STAFF_NAME_COMPLETED'
}

export enum EFollowUpActions {
    postAllFollowUpRequest =        'POST_ALL_FOLLOW_UP_REQUEST',
    postAllFollowUpRequestStart =   'POST_ALL_FOLLOW_UP_REQUEST_START',
    postAllFollowUpRequestError =   'POST_ALL_FOLLOW_UP_REQUEST_ERROR',
    postAllFollowUpRequestSuccess = 'POST_ALL_FOLLOW_UP_REQUEST_SUCCESS',

    getInternalFollowUpListItemDataRequest = 'GET_INTERNAL_FOLLOW_UP_LIST_ITEM_DATA_REQUEST',
    getInternalFollowUpListItemDataRequestStart = 'GET_INTERNAL_FOLLOW_UP_LIST_ITEM_DATA_START',
    getInternalFollowUpListItemDataRequestSuccess = 'GET_INTERNAL_FOLLOW_UP_LIST_ITEM_DATA_SUCCESS',
    getInternalFollowUpListItemDataRequestError = 'GET_INTERNAL_FOLLOW_UP_LIST_ITEM_DATA_ERROR',

    changeActiveFollowUpSort = 'CHANGE_ACTIVE_FOLLOW_UP_SORT',
    setCurrentFollowUpOffset = 'SET_CURRENT_FOLLOW_UP_OFFSET',
    setCurrentFollowUpPage = 'SET_FOLLOW_UP_PAGE',
    changeFollowUpActiveKey = 'CHANGE_FOLLOW_UP_ACTIVE_KEY',
    changeFollowUpActiveDirection = 'CHANGE_FOLLOW_UP_ACTIVE_DIRECTION',
    setFollowUpPageChangeRequest = 'SET_FOLLOW_UP_PAGE_CHANGE_REQUEST',

    setFollowUpFirstName = 'SET_FOLLOW_UP_FIRST_NAME',
    setFollowUpLastName = 'SET_FOLLOW_UP_SECOND_NAME',
    setFollowUpScreendoxResultId = 'SET_FOLLOW_UP_SCREENDOX_RESULT_ID',
    setFollowUpLocationId = 'SET_FOLLOW_UP_LOCATION_ID',
    setFollowUpScreendoxStartDate = 'SET_FOLLOW_UP_START_DATE',
    setFollowUpScreendoxEndDate = 'SET_FOLLOW_UP_END_DATE',
    setFollowUpScreendoxGRPAPeriod = 'SET_FOLLOW_UP_GRPA_PERIOD',
    setFollowUpBSRReportType = 'SET_FOLLOW_UP_BSR_REPORT_TYPE',
    resetFollowUpSearchParameters = 'RESET_FOLLOW_UP_SEARCH_PARAMETERS',
    resetFollowUpSearchParametersRequest = 'RESET_FOLLOW_UP_SEARCH_PARAMETERS_REQUEST',

    getRelatedReportByIdRequest = 'GET_RELATED_REPORT_BY_ID_REQUEST',
    getRelatedReportByIdRequestStart = 'GET_RELATED_REPORT_BY_ID_REQUEST_START',
    getRelatedReportByIdRequestError = 'GET_RELATED_REPORT_BY_ID_REQUEST_ERROR',
    getRelatedReportByIdRequestSuccess = 'GET_RELATED_REPORT_BY_ID_REQUEST_SUCCESS',

    getLocationListRequest = 'GET_LOCATION_LIST_REQUEST',
    getLocationListRequestStart = 'GET_LOCATION_LIST_START',
    getLocationListRequestSuccess = 'GET_LOCATION_LIST_SUCCESS',
    getLocationListRequestError = 'GET_LOCATION_LIST_ERROR',

    changeAutoUpdateStatus = 'FOLLOW_UP_CHANGE_AUTO_UPDATE_STATUS',
    changeAutoUpdateStatusRequest = 'FOLLOW_UP_CHANGE_AUTO_UPDATE_STATUS_REQUEST'
    
}

export enum EReportsActions {
    postReportsByProblemRequest =        'POST_REPORTS_BY_PROBLEM_REQUEST',
    postReportsByProblemRequestStart =   'POST_REPORTS_BY_PROBLEM_REQUEST_START',
    postReportsByProblemRequestError =   'POST_REPORTS_BY_PROBLEM_REQUEST_ERROR',
    postReportsByProblemRequestSuccess = 'POST_REPORTS_BY_PROBLEM_REQUEST_SUCCESS',

    postReportsByAgeRequest =        'POST_REPORTS_BY_AGE_REQUEST',
    postReportsByAgeRequestStart =   'POST_REPORTS_BY_AGE_REQUEST_START',
    postReportsByAgeRequestError =   'POST_REPORTS_BY_AGE_REQUEST_ERROR',
    postReportsByAgeRequestSuccess = 'POST_REPORTS_BY_AGE_REQUEST_SUCCESS',

    postDrugByAgeRequest =        'POST_DRUG_BY_AGE_REQUEST',
    postDrugByAgeRequestStart =   'POST_DRUG_BY_AGE_REQUEST_START',
    postDrugByAgeRequestError =   'POST_DRUG_BY_AGE_REQUEST_ERROR',
    postDrugByAgeRequestSuccess = 'POST_DRUG_BY_AGE_REQUEST_SUCCESS',

    postPatientDemographicsRequest =        'POST_PATIENT_DEMOGRAPHICS_REQUEST',
    postPatientDemographicsRequestStart =   'POST_PATIENT_DEMOGRAPHICS_REQUEST_START',
    postPatientDemographicsRequestError =   'POST_PATIENT_DEMOGRAPHICS_REQUEST_ERROR',
    postPatientDemographicsRequestSuccess = 'POST_PATIENT_DEMOGRAPHICS_REQUEST_SUCCESS',

    postFollowupOutcomesRequest =        'POST_FOLLOWUP_OUTCOMES_REQUEST',
    postFollowupOutcomesRequestStart =   'POST_FOLLOWUP_OUTCOMES_REQUEST_START',
    postFollowupOutcomesRequestError =   'POST_FOLLOWUP_OUTCOMES_REQUEST_ERROR',
    postFollowupOutcomesRequestSuccess = 'POST_FOLLOWUP_OUTCOMES_REQUEST_SUCCESS',

    postVisitsOutcomesRequest =        'POST_VISITS_OUTCOMES_REQUEST',
    postVisitsOutcomesRequestStart =   'POST_VISITS_OUTCOMES_REQUEST_START',
    postVisitsOutcomesRequestError =   'POST_VISITS_OUTCOMES_REQUEST_ERROR',
    postVisitsOutcomesRequestSuccess = 'POST_VISITS_OUTCOMES_REQUEST_SUCCESS',

    postScreenTimeLogRequest =        'POST_SCREEN_TIME_LOG_REQUEST',
    postScreenTimeLogRequestStart =   'POST_SCREEN_TIME_LOG_REQUEST_START',
    postScreenTimeLogRequestError =   'POST_SCREEN_TIME_LOG_REQUEST_ERROR',
    postScreenTimeLogRequestSuccess = 'POST_SCREEN_TIME_LOG_REQUEST_SUCCESS',

    postIncludeScreeningsRequest =        'POST_INCLUDE_SCREENINGS_REQUEST',    
    postIncludeScreeningsRequestSuccess = 'POST_INCLUDE_SCREENINGS_REQUEST_SUCCESS',

    postIncludeDemographicsRequest =        'POST_INCLUDE_DEMOGRAPHICS_REQUEST',    
    postIncludeDemographicsRequestSuccess = 'POST_INCLUDE_DEMOGRAPHICS_REQUEST_SUCCESS',

    postIncludeVisitsRequest =        'POST_INCLUDE_VISITS_REQUEST',    
    postIncludeVisitsRequestSuccess = 'POST_INCLUDE_VISITS_REQUEST_SUCCESS',

    postIncludeFollowUpsRequest =        'POST_INCLUDE_FOLLOWUPS_REQUEST',    
    postIncludeFollowUpsRequestSuccess = 'POST_INCLUDE_FOLLOWUPS_REQUEST_SUCCESS',

    postIncludeDrugsOfChoiceRequest =        'POST_INCLUDE_DRUGSOFCHOICE_REQUEST',    
    postIncludeDrugsOfChoiceRequestSuccess = 'POST_INCLUDE_DRUGSOFCHOICE_REQUEST_SUCCESS',

    postIncludeCombinedRequest =        'POST_INCLUDE_COMBINED_REQUEST',    
    postIncludeCombinedRequestSuccess = 'POST_INCLUDE_COMBINED_REQUEST_SUCCESS',
    
    reportAgeGroupRequest=        'REPORT_AGE_GROUP_REQUEST',
    reportAgeGroupRequestStart=   'REPORT_AGE_GROUP_REQUEST_START',
    reportAgeGroupRequestError=   'REPORT_AGE_GROUP_REQUEST_ERROR',
    reportAgeGroupRequestSuccess= 'REPORT_AGE_GROUP_REQUEST_SUCCESS',

    reportEarliestDateRequest=    'REPORT_EARLIEST_DATE_REQUEST',
    reportEarliestDateRequestStart=    'REPORT_EARLIEST_DATE_REQUEST_START',
    reportEarliestDateRequestError=    'REPORT_EARLIEST_DATE_REQUEST_ERROR',
    reportEarliestDateRequestSuccess=    'REPORT_EARLIEST_DATE_REQUEST_SUCCESS',

    reportGPRAPeriodsRequest=    'REPORT_GPRA_PERIODS_REQUEST',   
    reportGPRAPeriodsRequestSuccess=    'REPORT_GPRA_PERIODS_REQUEST_SUCCESS',

    getInternalReportsListItemDataRequest = 'GET_INTERNAL_REPORTS_LIST_ITEM_DATA_REQUEST',
    getInternalReportsListItemDataRequestStart = 'GET_INTERNAL_REPORTS_LIST_ITEM_DATA_START',
    getInternalReportsListItemDataRequestSuccess = 'GET_INTERNAL_REPORTS_LIST_ITEM_DATA_SUCCESS',
    getInternalReportsListItemDataRequestError = 'GET_INTERNAL_REPORTS_LIST_ITEM_DATA_ERROR',

    changeActiveReportsSort = 'CHANGE_ACTIVE_REPORTS_SORT',
    setCurrentReportsOffset = 'SET_CURRENT_REPORTS_OFFSET',
    setCurrentReportsPage = 'SET_REPORTS_PAGE',
    changeReportsActiveKey = 'CHANGE_REPORTS_ACTIVE_KEY',
    changeReportsActiveDirection = 'CHANGE_REPORTS_ACTIVE_DIRECTION',
    setReportsPageChangeRequest = 'SET_REPORTS_PAGE_CHANGE_REQUEST',

    setReportsFirstName = 'SET_REPORTS_FIRST_NAME',
    setReportsLastName = 'SET_REPORTS_SECOND_NAME',
    setReportsScreendoxResultId = 'SET_REPORTS_SCREENDOX_RESULT_ID',
    setReportsLocationId = 'SET_REPORTS_LOCATION_ID',
    setReportsScreendoxStartDate = 'SET_REPORTS_START_DATE',
    setReportsScreendoxEndDate = 'SET_REPORTS_END_DATE',
    setReportsScreendoxGRPAPeriod = 'SET_REPORTS_GRPA_PERIOD',
    setReportsBSRReportType = 'SET_REPORTS_BSR_REPORT_TYPE',

    resetReportsSearchParameters = 'RESET_REPORTS_SEARCH_PARAMETERS',
    resetReportsSearchParametersRequest = 'RESET_REPORTS_SEARCH_PARAMETERS_REQUEST',

    resetReportsByAgeSearchParameters = 'RESET_REPORTS_BY_AGE_SEARCH_PARAMETERS',
    resetReportsByAgeSearchParametersRequest = 'RESET_REPORTS_BY_AGE_SEARCH_PARAMETERS_REQUEST',

    resetDrugsByAgeSearchParameters = 'RESET_DRUGS_BY_AGE_SEARCH_PARAMETERS',
    resetDrugsByAgeSearchParametersRequest = 'RESET_DRUGS_BY_AGE_SEARCH_PARAMETERS_REQUEST',
    
    resetPatientDemographicsSearchParameters = 'RESET_PATIENT_DEMOGRAPHICS_SEARCH_PARAMETERS',
    resetPatientDemographicsSearchParametersRequest = 'RESET_PATIENT_DEMOGRAPHICS_SEARCH_PARAMETERS_REQUEST',

    resetFollowupOutcomesSearchParameters = 'ESET_FOLLOWUP_OUTCOMES_SEARCH_PARAMETERS',
    resetFollowupOutcomesSearchParametersRequest = 'RESET_FOLLOWUP_OUTCOMES_SEARCH_PARAMETERS_REQUEST',

    resetVisitsOutcomesSearchParameters = 'ESET_VISITS_OUTCOMES_SEARCH_PARAMETERS',
    resetVisitsOutcomesSearchParametersRequest = 'RESET_VISITS_OUTCOMES_SEARCH_PARAMETERS_REQUEST',

    resetScreenTimeLogSearchParameters = 'RESET_SCREEN_TIME_LOG_SEARCH_PARAMETERS',
    resetScreenTimeLogSearchParametersRequest = 'RESET_SCREEN_TIME_LOG_SEARCH_PARAMETERS_REQUEST',

    
    resetExportToExcelSearchParameters = 'RESET_EXPORT_TO_EXCEL_SEARCH_PARAMETERS',
    resetExportToExcelSearchParametersRequest = 'RESET_EXPORT_TO_EXCEL_SEARCH_PARAMETERS_REQUEST',

    getRelatedReportByIdRequest = 'GET_RELATED_REPORT_BY_ID_REQUEST',
    getRelatedReportByIdRequestStart = 'GET_RELATED_REPORT_BY_ID_REQUEST_START',
    getRelatedReportByIdRequestError = 'GET_RELATED_REPORT_BY_ID_REQUEST_ERROR',
    getRelatedReportByIdRequestSuccess = 'GET_RELATED_REPORT_BY_ID_REQUEST_SUCCESS',    
    getReportLocationsListRequest = 'GET_REPORT_LOATION_LIST_REQUEST',
    getLocationListActionRequestSuccess = 'GET_REPORTS_LOCATIONS_LIST_REQUEST_SUCCCESS',   
    
    getScreeningResultReportsBySortRequest = "GET_SCREENING_RESULT_REPORTS_BY_SORT_REQUEST",
    getScreeningResultReportsBySortRequestStart = "GET_SCREENING_RESULT_REPORTS_BY_SORT_REQUEST_START",
    getScreeningResultReportsBySortRequestSuccess = "GET_SCREENING_RESULT_REPORTS_BY_SORT_REQUEST_SUCCESS",
    getScreeningResultReportsBySortRequestError = "GET_SCREENING_RESULT_REPORTS_BY_SORT_REQUEST_ERROR",

    setScreeningResultReportsBySortFilterArray = 'SET_SCREENING_RESULT_REPORTS_BY_SORT_FILTER_ARRAY',

    setScreeningResultReportsBySortAutoStatus = 'SET_SCREENING_RESULT_REPORTS_BY_SORT_AUTO_STATUS',

    getScreeningResultReportsBySortAutoUpdateRequest = 'GET_SCREENING_RESULT_REPORTS_BY_SORT_AUTO_UPDATE_REQUEST',
    
}


export enum EFollowUpRelatedReportActions {
    getReportRequest = 'GET_REPORT_REQUEST',
    getReportRequestStart = 'GET_REPORT_REQUEST_START',
    getReportRequestError = 'GET_REPORT_REQUEST_ERROR',
    getReportRequestSuccess = 'GET_REPORT_REQUEST_SUCCESS',

    getFollowUpReportPatientAttendedVisitOptionsRequest = 'GET_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_OPTIONS_REQUEST',
    getFollowUpReportPatientAttendedVisitOptionsRequestStart = 'GET_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_OPTIONS_REQUEST_START',
    getFollowUpReportPatientAttendedVisitOptionsRequestError = 'GET_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_OPTIONS_REQUEST_ERROR',
    getFollowUpReportPatientAttendedVisitOptionsRequestSuccess = 'GET_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_OPTIONS_REQUEST_SUCCESS',

    setCurrentSelectedFollowUpReportPatientAttendedVisitOption = 'SET_CURRENTLY_SELECTED_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_OPTION',
    setCurrentSelectedFollowUpReportPatientAttendedVisitDate = 'SET_CURRENTLY_SELECTED_FOLLOW_UP_REPORT_PATIENT_ATTENDED_VISIT_DATE',

    getFollowUpReportFollowUpContactOutcome = 'GET_FOLLOW_UP_REPORT_FOLLOW_UP_CONTACT_OUTCOME_REQUEST',
    getFollowUpReportFollowUpContactOutcomeStart = 'GET_FOLLOW_UP_REPORT_FOLLOW_UP_CONTACT_OUTCOME_REQUEST_START',
    getFollowUpReportFollowUpContactOutcomeError = 'GET_FOLLOW_UP_REPORT_FOLLOW_UP_CONTACT_OUTCOME_REQUEST_ERROR',
    getFollowUpReportFollowUpContactOutcomeSuccess = 'GET_FOLLOW_UP_REPORT_FOLLOW_UP_CONTACT_OUTCOME_REQUEST_SUCCESS',

    getFollowUpReportNewVisitReferralRecommendation = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFERRAL_RECOMMENDATION_REQUEST',
    getFollowUpReportNewVisitReferralRecommendationStart = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFERRAL_RECOMMENDATION_REQUEST_START',
    getFollowUpReportNewVisitReferralRecommendationError = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFERRAL_RECOMMENDATION_REQUEST_ERROR',
    getFollowUpReportNewVisitReferralRecommendationSuccess = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFERRAL_RECOMMENDATION_REQUEST_SUCCESS',

    setFollowUpReportContactOutcomeOption = 'SET_FOLLOW_UP_REPORT_CONTACT_OUTCOME_OPTION',
    setFollowUpReportNewVisitReferralRecommendationOption = 'SET_FOLLOW_UP_REPORT_NEW_VISIT_REFERRAL_RECOMMENDATION_OPTION',
    setNewVisitReferralRecommendation = 'SET_NEW_VISIT_REFERRAL_RECOMMENDATION',

    getFollowUpReportNewVisitReferralRecommendationAcceptedRequest = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_ACCEPTED_REQUEST',
    getFollowUpReportNewVisitReferralRecommendationAcceptedRequestStart = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_ACCEPTED_REQUEST_START',
    getFollowUpReportNewVisitReferralRecommendationAcceptedRequestError = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_ACCEPTED_REQUEST_ERROR',
    getFollowUpReportNewVisitReferralRecommendationAcceptedRequestSuccess = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_ACCEPTED_REQUEST_SUCCESS',

    setFollowUpReportNewVisitReferralRecommendationAcceptedOption = 'SET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_ACCEPTED_OPTION',

    getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_NOT_ACCEPTED_REQUEST',
    getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestStart = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_NOT_ACCEPTED_REQUEST_START',
    getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestError = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_NOT_ACCEPTED_REQUEST_ERROR',
    getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequestSuccess = 'GET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_NOT_ACCEPTED_REQUEST_SUCCESS',

    setFollowUpReportNewVisitReferralRecommendationNotAcceptedOption = 'SET_FOLLOW_UP_REPORT_NEW_VISIT_REFFERAL_RECOMMENDATION_NOT_ACCEPTED_OPTION',
    setFollowUpReportCreateFlag = 'SET_FOLLOW_UP_REPORT_CREATE_FLAG',
    setFollowUpReportCreateDate = 'SET_FOLLOW_UP_REPORT_CREATE_DATE',
    setFollowUpReportNewVisitDate = 'SET_FOLLOW_UP_REPORT_NEW_VISIT_DATE',

    getFollowUpReportDischargedRequest = 'GET_FOLLOW_UP_REPORT_DISCHARGED_REQUEST',
    getFollowUpReportDischargedRequestStart = 'GET_FOLLOW_UP_REPORT_DISCHARGED_REQUEST_START',
    getFollowUpReportDischargedRequestError = 'GET_FOLLOW_UP_REPORT_DISCHARGED_REQUEST_ERROR',
    getFollowUpReportDischargedRequestSuccess = 'GET_FOLLOW_UP_REPORT_DISCHARGED_REQUEST_SUCCESS',
    setFollowUpReportDischargedOption = 'SET_FOLLOW_UP_REPORT_DISCHARGED_OPTION',
    
    setFollowUpCreate = 'SET_FOLLOW_UP_CREATE',
    setFollowUpDate = 'SET_FOLLOW_UP_DATE',
    setFollowUpNote = 'SET_FOLLOW_UP_NOTE',

    updateFollowUpReportRequest = 'UPDATE_FOLLOW_UP_REPORT_REQUEST',
    updateFollowUpReportRequestStart = 'UPDATE_FOLLOW_UP_REPORT_REQUEST_START',
    updateFollowUpReportRequestError = 'UPDATE_FOLLOW_UP_REPORT_REQUEST_ERROR',
    updateFollowUpReportRequestSuccess = 'UPDATE_FOLLOW_UP_REPORT_REQUEST_SUCCESS',

    setFollowUpReportIsCompleted = 'SET_FOLLOW_UP_REPORT_IS_COMPLETED'
}

export enum EManageDevicesActions {
    getManagedDevicesListRequest = 'GET_MANAGED_DEVICES_REQUEST',
    getManagedDevicesListRequestStart = 'GET_MANAGED_DEVICES_REQUEST_START',
    getManagedDevicesListRequestError = 'GET_MANAGED_DEVICES_REQUEST_ERROR',
    getManagedDevicesListRequestSuccess = 'GET_MANAGED_DEVICES_REQUEST_SUCCESS',

    changeActiveSort = 'CHANGE_MANAGE_DEVICE_ACTIVE_SORT_ORDER',
    changeActiveDirection = 'CHANGE_MANAGE_DEVICE_ACTIVE_DIRECTION',
    changeActiveKey = 'CHANGE_MANAGE_DEVICE_ACTIVE_KEY',

    selectAddNewKioskBranchLocation = 'SELECT_MANAGE_DEVICE_ADD_NEW_KIOSK_BRANCH_LOCATION',
    selectAddNewKioskDeviceName = 'SELECT_MANAGE_DEVICE_ADD_NEW_KIOSK_DEVICE_NAME',
    selectAddNewKioskScreenProfile = 'SELECT_MANAGE_DEVICE_ADD_NEW_KIOSK_SCREEN_PROFILE',
    selectAddNewKioskSecretKey = 'SELECT_MANAGE_DEVICE_ADD_NEW_KIOSK_SECRET_KEY',
    selectAddNewKioskDescription = 'SELECT_MANAGE_DEVICE_ADD_NEW_KIOSK_DESCRIPTION',
    addNewKioskInconsistencyInFields = 'ADD_MANAGE_DEVICE_NEW_KIOSK_INCONSISTENCY_IN_FIELDS',

    createNewKioskRequest = 'CREATE_MANAGE_DEVICE_NEW_KIOSK_REQUEST',
    createNewKioskRequestStart = 'CREATE_MANAGE_DEVICE_NEW_KIOSK_REQUEST_START',
    createNewKioskRequestError = 'CREATE_MANAGE_DEVICE_NEW_KIOSK_REQUEST_ERROR',
    createNewKioskRequestSuccess = 'CREATE_MANAGE_DEVICE_NEW_KIOSK_REQUEST_SUCCESS',

    setCurrentPage = 'SET_MANAGE_DEVICE_CURRENT_PAGE',
    changeCurrentPageRequst = 'CHANGE_MANAGE_DEVICE_CURRENT_PAGE_REQUEST',

    setFilterBranchLocationId = 'SET_MANAGE_DEVICE_FILTER_BRANCH_LOCATION_ID',
    setFilterLocationNameKey = 'SET_MANAGE_DEVICE_FILTER_LOCATION_NAME_KEY',
    setFilterShowDisabled = 'SET_MANAGE_DEVICE_FILTER_SHOW_DISABLED',
    setScreeningProfileId = 'SET_FILTER_SCREENING_PROFILE_ID',

    resetKioskFilter = 'REST_MANAGE_DEVICE_KIOSK_FILTER',
    resetKioskFilterRequest = 'REST_MANAGE_DEVICE_KIOSK_FILTER_REQUEST',

    getEditKioskDetailsByIdRequest = 'GET_MANAGE_DEVICE_EDIT_KIOSK_DETAILS_BY_ID_REQUEST',
    getEditKioskDetailsByIdRequestStart = 'GET_MANAGE_DEVICE_EDIT_KIOSK_DETAILS_BY_ID_REQUEST_START',
    getEditKioskDetailsByIdRequestError = 'GET_MANAGE_DEVICE_EDIT_KIOSK_DETAILS_BY_ID_REQUEST_ERROR',
    getEditKioskDetailsByIdRequestSuccess = 'GET_MANAGE_DEVICE_EDIT_KIOSK_DETAILS_BY_ID_REQUEST_SUCCESS',

    changeEditKioskParamaterDescription = 'CHANGE_MANAGE_DEVICE_EDIT_KIOSK_PARAMETERS_DESCRIPTION',
    changeEditKioskParamaterBranchLocation = 'CHANGE_MANAGE_DEVICE_EDIT_KIOSK_PARAMETERS_BRANCH_LOCATION',

    updateKioskByIdRequest = 'UPDATE__MANAGE_DEVICEKIOSK_BY_ID_REQUEST',
    updateKioskByIdRequestStart = 'UPDATE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_START',
    updateKioskByIdRequestError = 'UPDATE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_ERROR',
    updateKioskByIdRequestSuccess = 'UPDATE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_SUCCESS',

    deleteKioskByIdRequest = 'DELETE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST',
    deleteKioskByIdRequestStart = 'DELETE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_START',
    deleteKioskByIdRequestError = 'DELETE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_ERROR',
    deleteKioskByIdRequestSuccess = 'DELETE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST_SUCCESS',

    disableKioskByIdRequest = 'DISABLE_MANAGE_DEVICE_KIOSK_BY_ID_REQUEST',

    setManageDeviceAutoStatus = 'SET_MANAGE_DEVICE_AUTO_STATUS',
    manageDeviceAutoRequest = 'MANAGE_DEVICE_AUTO_REQUEST'
}

export enum EBranchLocationsActions {
    getBranchLocationsListRequest = 'GET_BRANCH_LOCATIONS_LIST_REQUEST',
    getBranchLocationsListRequestStart = 'GET_BRANCH_LOCATIONS_LIST_REQUEST_START',
    getBranchLocationsListRequestError = 'GET_BRANCH_LOCATIONS_LIST_REQUEST_ERROR',
    getBranchLocationsListRequestSuccess = 'GET_BRANCH_LOCATIONS_LIST_REQUEST_SUCCESS',

    changeActiveSort = 'CHANGE_BRANCH_LOCATIONS_ACTIVE_SORT',
    changeActiveDirection = 'CHANGE_BRANCH_LOCATIONS_ACTIVE_DIRECTORY',
    changeActiveKey = 'CHANGE_BRANCH_LOCATIONS_ACTIVE_KEY',

    setCurrentPage = 'SET_BRANCH_LOCATIONS_CURRENT_PAGE',
    changeCurrentPageRequst = 'CHANGE_BRANCH_LOCATIONS_CURRENT_PAGE_REQUEST',

    setFilterLocationNameKey = 'SET_BRANCH_LOCATION_FILTER_LOCATION_NAME_KEY',
    setScreeningProfileId = 'SET_BRANCH_LOCATION_SCREENING_PROFILE_ID',
    setFilterBranchLocationId = 'SET_BRANCH_LOCATION_FILTER_BRANCH_LOCATION_ID',
    setFilterShowDisabled = 'SET_BRANCH_LOCATION_FILTER_SHOW_DISABLED',
    
    resetBranchLocationsFilterRequest = 'RESET_BRANCH_LOCATIONS_FILTER_REQUEST',
    resetBranchLocationsFilter = 'RESET_BRANCH_LOCATION_FILTERS',

    createBranchLocationName = 'CREATE_BRANCH_LOCATION_NAME',
    createBranchLocationScreenProfile = 'CREATE_BRANCH_LOCATION_SCREEN_PROFILE',
    createBranchLocationDescription = 'CREATE_BRANCH_LOCATION_DESCRIPTION',

    createNewBranchLocationRequest = 'CREATE_NEW_BRANCH_LOCATION_REQUEST',
    createNewBranchLocationRequestStart = 'CREATE_NEW_BRANCH_LOCATION_REQUEST_START',
    createNewBranchLocationRequestError = 'CREATE_NEW_BRANCH_LOCATION_REQUEST_ERROR',
    createNewBranchLocationRequestSuccess = 'CREATE_NEW_BRANCH_LOCATION_REQUEST_SUCCESS',

    getBranchLocationByIdRequest = 'GET_BRANCH_LOCATION_BY_ID_REQUEST',
    getBranchLocationByIdRequestStart = 'GET_BRANCH_LOCATION_BY_ID_REQUEST_START',
    getBranchLocationByIdRequestError = 'GET_BRANCH_LOCATION_BY_ID_REQUEST_ERROR',
    getBranchLocationByIdRequestSuccess = 'GET_BRANCH_LOCATION_BY_ID_REQUEST_SUCCESS',

    setCurrentBranchLocation = 'SET_CURRENT_BRANCH_LOCATION',

    updateBranchLocationRequest = 'UPDATE_BRANCH_LOCATION__REQUEST',
    updateBranchLocationRequestStart = 'UPDATE_BRANCH_LOCATION__REQUEST_START',
    updateBranchLocationRequestError = 'UPDATE_BRANCH_LOCATION__REQUEST_ERROR',
    updateBranchLocationRequestSuccess = 'UPDATE_BRANCH_LOCATION__REQUEST_SUCCESS',

    deleteBranchLocationRequest = 'DELETE_BRANCH_LOCATION__REQUEST',
    deleteBranchLocationRequestStart = 'DELETE_BRANCH_LOCATION__REQUEST_START',
    deleteBranchLocationRequestError = 'DELETE_BRANCH_LOCATION__REQUEST_ERROR',
    deleteBranchLocationRequestSuccess = 'DELETE_BRANCH_LOCATION__REQUEST_SUCCESS',

    setNewBranchLocationLoading = 'SET_NEW_BRANCH_LOCATION_LOADING',

    setBranchLocationListAutoStatus = 'SET_BRANCH_LOCATION_LIST_AUTO_STATUS',
    branchLocationListAutoRequest = 'BRANCH_LOCATION_LIST_AUTO_REQUEST',

    setBranchLocationDisabled = 'SET_BRANCH_LOCATION_DISABLED'

}

export enum ESharedActions {

    getLocationsRequest = 'GET_LOCATIONS_REQUEST',
    getLocationsRequestStart = 'GET_LOCATIONS_REQUEST_START',
    getLocationsRequestError = 'GET_LOCATIONS_REQUEST_ERROR',
    getLocationsRequestSuccess = 'GET_LOCATIONS_REQUEST_SUCCESS',

    getScreeningProfileListRequest = 'GET_SCREENING_PROFILE_LIST_REQUEST',
    getScreeningProfileListRequestStart = 'GET_SCREENING_PROFILE_LIST_REQUEST_START',
    getScreeningProfileListRequestError = 'GET_SCREENING_PROFILE_LIST_REQUEST_ERROR',
    getScreeningProfileListRequestSuccess = 'GET_SCREENING_PROFILE_LIST_REQUEST_SUCCESS',

    setCreateDateCustomOrGPRA = 'SET_CREATE_DATE_CUSTOM_OR_GRPA',

}

export enum EScreenProfileActions {
    getScreenProfileListRequest = 'GET_SCREEN_PROFILES_LIST_REQUEST',
    getScreenProfileListRequestStart = 'GET_SCREEN_PROFILES_LIST_REQUEST_START',
    getScreenProfileListRequestSuccess = 'GET_SCREEN_PROFILES_LIST_REQUEST_SUCCESS',
    getScreenProfileListRequestError = 'GET_SCREEN_PROFILES_LIST_REQUEST_ERROR',
    changeCurrentPageRequst = 'SCREEN_PROFILES_CHANGE_CURRENT_PAGE_REQUEST',
    setCurrentPage = 'SCREEN_PROFILES_SET_CURRENT_PAGE',
    changeActiveSort = 'SCREEN_PROFILES_CHANGE_ACTIVE_SORT',
    changeActiveDirection = 'SCREEN_PROFILES_ACTIVE_DIRECTION',
    changeActiveKey = 'SCREEN_PROFILES_ACTIVE_KEY',
    setFilterByName = 'SCREEN_PROFILES_SET_FILTER_BY_NAME',
    clearSearchParamsRequest = 'SCREEN_PROFILES_CLEAR_SEARCH_PARAMS_REQUEST',
    setCreateScreenProfileDescription = 'SET_CREATE_SCREEN_PROFILE_DESCRIPTION',
    setCreateScreenProfileName = 'SET_CREATE_SCREEN_PROFILE_NAME',
    createNewScreenProfileRequest = 'CREATE_NEW_SCREEN_PROFILE_REQUEST',
    setNewScreenProfileLoading = 'SET_NEW_SCREEN_PROFILE_LOADING',
    getScreenProfileByIdRequest = 'GET_SCREEN_PROFILE_BY_ID_REQUEST',
    setScreenProfileId = 'SET_SCREEN_PROFILE_ID',

    updateScreenProfileRequest = 'UPDATE_SCREEN_PROFILE_REQUEST',
    updateScreenProfileRequestStart = 'UPDATE_SCREEN_PROFILE_REQUEST_START',
    updateScreenProfileRequestSuccess = 'UPDATE_SCREEN_PROFILE_REQUEST_SUCCESS',
    updateScreenProfileRequestError = 'UPDATE_SCREEN_PROFILE_REQUEST_ERROR',

    deleteScreenProfileRequest = 'DELETE_SCREEN_PROFILE_REQUEST',

    screenProfileMinimumAgeListRequest = 'SCREEN_PROFILE_MINIMUM_AGE_LIST_REQUEST',
    screenProfileMinimumAgeListRequestStart = 'SCREEN_PROFILE_MINIMUM_AGE_LIST_REQUEST_START',
    screenProfileMinimumAgeListRequestSuccess = 'SCREEN_PROFILE_MINIMUM_AGE_LIST_REQUEST_SUCCESS',
    screenProfileMinimumAgeListRequestError = 'SCREEN_PROFILE_MINIMUM_AGE_LIST_REQUEST_ERROR',

    selectedScreenProfileEditOption = 'SELECTED_SCREEN_PROFILE_EDIT_OPTION',

    screenProfileMinimumAgeUpdateRequest = 'SCREEN_PROFILE_MINIMUM_AGE_UPDATE_REQUEST',
    screenProfileMinimumAgeUpdateRequestStart = 'SCREEN_PROFILE_MINIMUM_AGE_UPDATE_REQUEST_START',
    screenProfileMinimumAgeUpdateRequestSuccess = 'SCREEN_PROFILE_MINIMUM_AGE_UPDATE_REQUEST_SUCCESS',
    screenProfileMinimumAgeUpdateRequestError = 'SCREEN_PROFILE_MINIMUM_AGE_UPDATE_REQUEST_ERROR',
    
    screenProfileFrequencyListRequest = 'SCREEN_PROFILE_FREQUENCY_LIST_REQUEST',
    screenProfileFrequencyListRequestStart = 'SCREEN_PROFILE_FREQUENCY_LIST_REQUEST_START',
    screenProfileFrequencyListRequestSuccess = 'SCREEN_PROFILE_FREQUENCY_LIST_REQUEST_SUCCESS',
    screenProfileFrequencyListRequestError = 'SCREEN_PROFILE_FREQUENCY_LIST_REQUEST_ERROR',
    screenProfileDefaultFrequencyListSuccess = 'SCREEN_PROFILE_DEFAULT_FREQUENCY_LIST_SUCCESS', 

    screenProfileFrequencyUpdateRequest = 'SCREEN_PROFILE_FREQUENCY_UPDATE_REQUEST',
    screenProfileFrequencyUpdateRequestStart = 'SCREEN_PROFILE_FREQUENCY_UPDATE_REQUEST_START',
    screenProfileFrequencyUpdateRequestSuccess = 'SCREEN_PROFILE_FREQUENCY_UPDATE_REQUEST_SUCCESS',
    screenProfileFrequencyUpdateRequestError = 'SCREEN_PROFILE_FREQUENCY_UPDATE_REQUEST_ERROR',

    screenProfileAgeGroupsListRequest = 'SCREEN_PROFILE_AGE_GROUPS_LIST_REQUEST',
    screenProfileAgeGroupsListRequestSuccess = 'SCREEN_PROFILE_AGE_GROUPS_LIST_REQUEST_SUCCESS',
}

export enum EAgeGroupsActions {
    getAgeGroupsRequest = 'GET_AGE_GROUPS_REQUEST',
    getAgeGroupsRequestStart = 'GET_AGE_GROUPS_REQUEST_START',
    getAgeGroupsRequestSuccess = 'GET_AGE_GROUPS_REQUEST_SUCCESS',
    getAgeGroupsRequestError = 'GET_AGE_GROUPS_REQUEST_ERROR',
    setAgeGroupsValue = 'SET_AGE_GROUPS_VALUE',
    updateAgeGroupRequest = 'UPDATE_AGE_GROUP_REQUEST',
    updateAgeGroupRequestStart = 'UPDATE_AGE_GROUP_REQUEST_START',
    updateAgeGroupRequestSuccess = 'UPDATE_AGE_GROUP_REQUEST_SUCCESS',
    updateAgeGroupRequestError = 'UPDATE_AGE_GROUP_REQUEST_ERROR',
}

export enum EVisitSettingsActions {
    getVisitSettingsRequest = 'GET_VISIT_SETTINGS_REQUEST',
    getVisitSettingsRequestStart = 'GET_VISIT_SETTINGS_REQUEST_START',
    getVisitSettingsRequestSuccess = 'GET_VISIT_SETTINGS_REQUEST_SUCCESS',
    getVisitSettingsRequestError = 'GET_VISIT_SETTINGS_REQUEST_ERROR',

    updateVisitSettingsRequest = 'UPDATE_VISIT_SETTINGS_REQUEST',
    updateVisitSettingsRequestStart = 'UPDATE_VISIT_SETTINGS_REQUEST_START',
    updateVisitSettingsRequestError = 'UPDATE_VISIT_SETTINGS_REQUEST_ERROR',
    
}

export enum EErrorLogActions {
    getErrorLogRequest = 'GET_ERROR_LOG_REQUEST',
    getErrorLogRequestStart = 'GET_ERROR_LOG_REQUEST_START',
    getErrorLogRequestSuccess = 'GET_ERROR_LOG_REQUEST_SUCCESS',
    getErrorLogRequestError = 'GET_ERROR_LOG_REQUEST_ERROR',

    isErrorLogLoading = 'IS_ERROR_LOG_LOADING',

    changeErrorLogCurrentPageRequest = 'CHANGE_ERROR_LOG_CURRENT_PAGE_REQUEST',
    setErrorLogCurrentPage = 'SET_ERROR_LOG_CURRENT_PAGE',

    getErrorLogByIdRequest = 'GET_ERROR_LOG_BY_ID_REQUEST',
    getErrorLogByIdRequestStart = 'GET_ERROR_LOG_BY_ID_REQUEST_START',
    getErrorLogByIdRequestSuccess = 'GET_ERROR_LOG_BY_ID_REQUEST_SUCCESS',
    getErrorLogByIdRequestError = 'GET_ERROR_LOG_BY_ID_REQUEST_ERROR',

    setErrorLogStartDate = 'SET_ERROR_LOG_START_DATE',
    setErrorLogEndDate = 'SET_ERROR_LOG_END_DATE',

    deleteErrorLogsRequest = 'DELETE_ERROR_LOGS_REQUEST',

    printErrorLogsExcelRequest = 'PRINT_ERROR_LOGS_EXCEL_REQUEST',
    
}

export enum ESecurityLogSettingsActions {

    getSecurityLogSettingsRequest = 'SET_SECURITY_LOG_SETTINGS_REQUEST',
    getSecurityLogSettingsRequestStart = 'SET_SECURITY_LOG_SETTINGS_REQUEST_START',
    getSecurityLogSettingsRequestError = 'SET_SECURITY_LOG_SETTINGS_REQUEST_ERROR',
    getSecurityLogSettingsRequestSuccess = 'SET_SECURITY_LOG_SETTINGS_REQUEST_SUCCESS', 

    updateSecurityLogSettingsItemsRequest = 'UPDATE_SECURITY_LOG_SETTINGS_ITEMS_REQUEST',
}

export enum ELicenseKeysActions {
    getLicenseKeysRequest = 'GET_LICENSE_KEYS_REQUEST',
    getLicenseKeysRequestStart = 'GET_LICENSE_KEYS_REQUEST_START',
    getLicenseKeysRequestSuccess = 'GET_LICENSE_KEYS_REQUEST_SUCCESS',
    getLicenseKeysRequestError = 'GET_LICENSE_KEYS_REQUEST_ERROR',

    setLicenseKey = 'SET_LICENSE_KEY',

    deleteLicenseKeyRequest = 'DELETE_LICENSE_KEY_REQUEST',

    createLicenseKeyRequest = 'CREATE_LICENSE_KEY_REQUEST',
    setLicenseKeyCreateLoading = 'SET_LICENSE_KEY_CREATE_LOADING',
    setLicenseActivationKey = 'SET_LICENSE_ACTIVATION_KEY',

    getLicenseKeyDetailRequest = 'GET_LICENSE_KEY_DETAIL_REQUEST',

    activeLicenseKeyRequest = 'ACTIVE_LICENSE_KEY_REQUEST',

    setLicenseKeysSystemSettingsSummary = 'SET_LICENSE_KEYS_SYSTEM_SETTINGS_SUMMARY'

}

export enum ELoginActions { 
    loginRequest = 'LOGIN_REQUEST',
    loginRequestStart = 'LOGIN_REQUEST_START',
    loginRequestSuccess = 'LOGIN_REQUEST_SUCCESS',
    loginRequestError = 'LOGIN_REQUEST_ERROR',
    logoutRequest = 'LOGOUT_REQUEST',

    setLoginErrorList = 'SET_LOGIN_ERROR_LIST',
    refreshTokenRequest = 'REFRESH_TOKEN_REQUEST',
}


export enum EChangePasswordActions {
    changePasswordRequest = 'CHANGE_PASSWORD_REQUEST',
}

export enum EChangeSecurityQuestionActions {
    securityQuestionListRequest = 'SECURITY_QUESTION_LIST_REQUEST',
    securityQuestionListRequestSuccess = 'SECURITY_QUESTION_LIST_REQUEST_SUCCESS',
    securityQuestionListRequestError = 'SECURITY_QUESTION_LIST_REQUEST_ERROR',

    changeSecurityQuestionUpdateRequest = 'CHANGE_SECURITY_QUESTION_UPDATE_REQUEST',
    changeSecurityQuestionUpdateRequestStart = 'CHANGE_SECURITY_QUESTION_UPDATE_REQUEST_START',
    changeSecurityQuestionUpdateRequestError = 'CHANGE_SECURITY_QUESTION_UPDATE_REQUEST_ERROR',
    changeSecurityQuestionUpdateRequestSuccess = 'CHANGE_SECURITY_QUESTION_UPDATE_REQUEST_SUCCESS',
}


export enum EAutoExportLogsActions {
    getAutoExportLogsRequest = 'GET_AUTO_EXPORT_LOGS_REQUEST',
    getAutoExportLogsRequestStart = 'GET_AUTO_EXPORT_LOGS_REQUEST_START',
    getAutoExportLogsRequestSuccess = 'GET_AUTO_EXPORT_LOGS_REQUEST_SUCCESS',
    getAutoExportLogsRequestError = 'GET_AUTO_EXPORT_LOGS_REQUEST_ERROR',

    changeAutoExportLogsCurrentPageRequest = 'CHANGE_AUTO_EXPORT_LOGS_CURRENT_PAGE_REQUEST',
    setAutoExportLogsCurrentPage = 'SET_AUTO_EXPORT_LOGS_CURRENT_PAGE',

    setAutoExportLogsTotals = 'SET_AUTO_EXPORT_LOGS_TOTALS',
    setAutoExportLogsStartDate= 'SET_AUTO_EXPORT_LOGS_START_DATE',
    setAutoExportLogsEndDate= 'SET_AUTO_EXPORT_LOGS_END_DATE',
    setAutoExportLogsFilterName= 'SET_AUTO_EXPORT_LOGS_FILTER_NAME',
    setAutoExportLogsStatistics = 'SET_AUTO_EXPORT_LOGS_STATISTICS',
}

export enum EFindPatientAddressActions {
    getFindPatientAddressEhrRecordPatientsRequest = 'GET_FIND_PATIENT_ADDRESS_EHR_RECORD_PATIENTS_REQUEST',
    getFindPatientAddressEhrRecordPatientsRequestStart = 'GET_FIND_PATIENT_ADDRESS_EHR_RECORD_PATIENTS_REQUEST_START',
    getFindPatientAddressEhrRecordPatientsRequestSuccess = 'GET_FIND_PATIENT_ADDRESS_EHR_RECORD_PATIENTS_REQUEST_SUCCESS',
    getFindPatientAddressEhrRecordPatientsRequestError = 'GET_FIND_PATIENT_ADDRESS_EHR_RECORD_PATIENTS_REQUEST_ERROR',

    setFindPatientAddressPhoneNumber = 'SET_FIND_PATIENT_ADDRESS_PHONE_NUMBER',
    setFindPatientAddressStreetAddress = 'SET_FIND_PATIENT_ADDRESS_STREET_ADDRESS',
    setFindPatientAddressCity = 'SET_FIND_PATIENT_ADDRESS_CITY',
    setFindPatientAddressState = 'SET_FIND_PATIENT_ADDRESS_STATE',
    setFindPatientAddressZipCode = 'SET_FIND_PATIENT_ADDRESS_ZIP_CODE',

    postFindPatientAddressRequest = 'POST_FIND_PATIENT_ADDRESS_REQUEST',
    postFindPatientAddressRequestStart = 'POST_FIND_PATIENT_ADDRESS_REQUEST_START',
    postFindPatientAddressRequestError = 'POST_FIND_PATIENT_ADDRESS_REQUEST_ERROR',
    postFindPatientAddressRequestSuccess = 'POST_FIND_PATIENT_ADDRESS_REQUEST_SUCCESS',

    setFindPatientAddressEhrExportPatientRecordSelectedId = 'SET_FIND_PATIENT_ADDRESS_EHR_EXPORT_PATIENT_RECORD_SELECTED_ID',
    setFindPatientAddressEhrExportRecordCurrentPage = 'SET_FIND_PATIENT_ADDRESS_EHR_EXPORT_RECORD_CURRENT_PAGE',
    
}

export enum ECssrsListActions {
    getCssrsListRequest = 'GET_CSSRS_LIST_REQUEST',
    getCssrsListRequestStart = 'GET_CSSRS_LIST_REQUEST_START',
    getCssrsListRequestError = 'GET_CSSRS_LIST_REQUEST_ERROR',
    getCssrsListRequestSuccess = 'GET_CSSRS_LIST_REQUEST_SUCCESS',

    cssrsListSortRequest = 'CSSRS_LIST_SORT_REQUEST',
    setCssrsListCurrentPage = 'SET_CSSRS_LIST_CURRENT_PAGE',
    setCssrsListSortKey = 'SET_CSSRS_LIST_SORT_KEY',
    setCssrsListSortDirection = 'SET_CSSRS_LIST_SORT_DIRECTION',
    setCssrsListFirstName = 'SET_CSSRS_LIST_FIRST_NAME',
    setCssrsListLastName = 'SET_CSSRS_LIST_LAST_NAME',
    setCssrsListDateofBirth='SET_CSSRS_LIST_DATE_OF_BIRTH',
    setCssrsListScreeningResultId = 'SET_CSSRS_LIST_SCREENING_RESULT_ID',
    setCssrsListLocationId = 'SET_CSSRS_LIST_LOCATION_ID',
    setCssrsListStartDate = 'SET_CSSRS_LIST_START_DATE',
    setCssrsListEndDate = 'SET_CSSRS_LIST_END_DATE',
    setCssrsListGpraPeriodKey = 'SET_CSSRS_LIST_GPRA_PERIOD_KEY',
    setCssrsListBsrReportType = 'SET_CSSRS_LIST_BHS_REPORT_TYPE',
    setCssrsListOffset='SET_CSSRS_LIST_OFFSET',

    resetCssrsListParameter = 'RESET_CSSRS_LIST_PARAMETER',

    cssrsListchangeAutoUpdateStatus = 'CSSRS_LIST_CHANGE_AUTO_UPDATE_STATUS',
    cssrsListchangeAutoUpdateStatusRequest = 'CSSRS_LIST_CHANGE_AUTO_UPDATE_STATUS_REQUEST',

    cssrsListCurrentPageRequest = 'CSSRS_LIST_CURRENT_PAGE_REQUEST',
    getCssrsLocationsListRequest='GET_CSSRS_LOCATIONS_LIST_REQUEST',
    getCssrsLocationsListRequestSuccess='GET_CSSRS_LOCATIONS_LIST_REQUEST_SUCCESS',

    getRelatedByIdCssrsRequest = 'GET_RELATED_BY_ID_CSSRS_REQUEST',
    getRelatedByIdCssrsRequestStart = 'GET_RELATED_BY_ID_CSSRS_REQUEST_START',
    getRelatedByIdCssrsRequestError = 'GET_RELATED_BY_ID_CSSRS_REQUEST_ERROR',
    getRelatedByIdCssrsRequestSuccess = 'GET_RELATED_BY_ID_CSSRS_REQUEST_SUCCESS',
    
}

export enum EResetPasswordActions {
    resetPasswordGetSecurityQuestionRequest = 'RESET_PASSWORD_GET_SECURITY_QUESTION_REQUEST',
    resetPasswordGetSecurityQuestionRequestStart = 'RESET_PASSWORD_GET_SECURITY_QUESTION_REQUEST_START',
    resetPasswordGetSecurityQuestionRequestError = 'RESET_PASSWORD_GET_SECURITY_QUESTION_REQUEST_ERROR',
    resetPasswordGetSecurityQuestionRequestSuccess = 'RESET_PASSWORD_GET_SECURITY_QUESTION_REQUEST_SUCCESS',

    resetPasswordRequest = 'RESET_PASSWORD_REQUEST',
    resetPasswordRequestStart = 'RESET_PASSWORD_REQUEST_START',
    resetPasswordRequestError = 'RESET_PASSWORD_REQUEST_ERROR',
    resetPasswordRequestSuccess = 'RESET_PASSWORD_REQUEST_SUCCESS',
}


export enum ECssrsReportActions {
    cssrsReportPatientRecordsRequest = 'CSSRS_REPORT_PATIENT_RECORDS_REQUEST',
    cssrsReportPatientRecordsRequestStart = 'CSSRS_REPORT_PATIENT_RECORDS_REQUEST_START',
    cssrsReportPatientRecordsRequestSuccess = 'CSSRS_REPORT_PATIENT_RECORDS_REQUEST_SUCCESS',
    cssrsReportPatientRecordsRequestError = 'CSSRS_REPORT_PATIENT_RECORDS_REQUEST_ERROR',

    setCssrsReportFirstName = 'SET_CSSRS_REPORT_FIRST_NAME',
    setCssrsReportLastName = 'SET_CSSRS_REPORT_LAST_NAME',
    setCssrsReportMiddleName = 'SET_CSSRS_REPORT_MIDDLE_NAME',
    setCssrsReportBirthday = 'SET_CSSRS_REPORT_BIRTHDAY',
    setCssrsReportLocationId = 'SET_CSSRS_REPORT_LOCATION_ID',
    setCssrsReportLocations = 'SET_CSSRS_REPORT_LOCATIONS',
    cssrsReportCreateRequest = 'CSSRS_REPORT_CREATE_REQUEST',
    cssrsReportCreateRequestStart = 'CSSRS_REPORT_CREATE_REQUEST_START',
    cssrsReportCreateRequestError = 'CSSRS_REPORT_CREATE_REQUEST_ERROR',
    cssrsReportCreateRequestSuccess = 'CSSRS_REPORT_CREATE_REQUEST_SUCCESS',

    resetCssrsReportParameter='RESET_CSSRS_REPORT_PARAMETER',

    cssrsReportDetailRequest = 'CSSRS_REPORT_DETAIL_REQUEST',
    cssrsReportDetailRequestStart = 'CSSRS_REPORT_DETAIL_REQUEST_START',
    cssrsReportDetailRequestError = 'CSSRS_REPORT_DETAIL_REQUEST_ERROR',
    cssrsReportDetailRequestSuccess = 'CSSRS_REPORT_DETAIL_REQUEST_SUCCESS',

    setCssrsReportEhrExportPatientRecordSelectedId = 'SET_CSSRS_REPORT_EHR_EXPORT_PATIENT_RECORD_SELECTED_ID',

    setCssrsReport = 'SET_CSSRS_REPORT',
    
    cssrsReportUpdateRequest = 'CSSRS_REPORT_UPDATE_REQUEST',
    cssrsReportUpdateRequestStart = 'CSSRS_REPORT_UPDATE_REQUEST_START',
    cssrsReportUpdateRequestError = 'CSSRS_REPORT_UPDATE_REQUEST_ERROR',
    cssrsReportUpdateRequestSuccess = 'CSSRS_REPORT_UPDATE_REQUEST_SUCCESS',

    cssrsReportCopyRequest = 'CSSRS_REPORT_COPY_REQUEST',
    cssrsReportCopyRequestStart = 'CSSRS_REPORT_COPY_REQUEST_START',
    cssrsReportCopyRequestError = 'CSSRS_REPORT_COPY_REQUEST_ERROR',
    cssrsReportCopyRequestSuccess = 'CSSRS_REPORT_COPY_REQUEST_SUCCESS',
    
}


export enum EManageUsersActions {
    getManageUsersListRequest = 'GET_MANAGE_USERS_LIST_REQUEST',
    getManageUsersListRequestStart = 'GET_MANAGE_USERS_LIST_REQUEST_START',
    getManageUsersListRequestError = 'GET_MANAGE_USERS_LIST_REQUEST_ERROR',
    getManageUsersListRequestSuccess = 'GET_MANAGE_USERS_LIST_REQUEST_SUCCESS',

    setManageUsersCurrentPage = 'SET_MANAGE_USERS_CURRENT_PAGE',
    setManageUsersOrderKey = 'SET_MANAGE_USERS_ORDER_KEY',
    setManageUsersOrderDirection = 'SET_MANAGE_USERS_ORDER_DIRECTION',
    setManageUsersLocations = 'SET_MANAGE_USERS_LOCATIONS',
    setManageUsrsSelectedLocationId = 'SET_MANAGE_USERS_SELECTED_LOCATION_ID',

    manageUsersChangeActiveSort = 'MANAGE_USERS_CHANGE_ACTIVE_SORT',
    
    setManageUsersUser = 'SET_MANAGE_USERS_USER',
    setManageUsersGroups = 'SET_MANAGE_USERS_GORUPS',

    manageUsersCreateRequest = 'MANAGE_USERS_CREATE_REQUEST',
    manageUsersCreateRequestStart = 'MANAGE_USERS_CREATE_REQUEST_START',
    manageUsersCreateRequestError = 'MANAGE_USERS_CREATE_REQUEST_ERROR',
    manageUsersCreateRequestSuccess = 'MANAGE_USERS_CREATE_REQUEST_SUCCESS',

    manageUsersDetailRequest = 'MANAGE_USERS_DETAIL_REQUEST',
    manageUsersDetailRequestStart = 'MANAGE_USERS_DETAIL_REQUEST_START',
    manageUsersDetailRequestError = 'MANAGE_USERS_DETAIL_REQUEST_ERROR',
    manageUsersDetailRequestSuccess = 'MANAGE_USERS_DETAIL_REQUEST_SUCCESS',

    setManageUsersSelectedUserId = 'SET_MANAGE_USERS_SELECTED_USER_ID',

    manageUsersUpdateRequest = 'MANAGE_USERS_UPDATE_REQUEST',
    manageUsersUpdateRequestStart = 'MANAGE_USERS_UPDATE_REQUEST_START',
    manageUsersUpdateRequestError = 'MANAGE_USERS_UPDATE_REQUEST_ERROR',
    manageUsersUpdateRequestSuccess = 'MANAGE_USERS_UPDATE_REQUEST_SUCCESS',

    manageUsersDeleteRequest = 'MANAGE_USERS_DELETE_REQUEST',
    manageUsersBlockRequest = 'MANAGE_USERS_BLOCK_REQUEST',
    manageUsersUnBlockRequest = 'MANAGE_USERS_UNBLOCK_REQUEST',
}

export enum EEhrLoginActions {
    getEhrLoginListRequest = 'GET_EHR_LOGIN_LIST_REQUEST',
    getEhrLoginListRequestStart = 'GET_EHR_LOGIN_LIST_REQUEST_START',
    getEhrLoginListRequestError = 'GET_EHR_LOGIN_LIST_REQUEST_ERROR',
    getEhrLoginListRequestSuccess = 'GET_EHR_LOGIN_LIST_REQUEST_SUCCESS',

    setEhrLoginAccessCode = 'SET_EHR_LOGIN_ACCESS_CODE',
    setEhrLoginVerifyCode = 'SET_EHR_LOGIN_VERIFY_CODE',
    setEhrLoginSelectedId = 'SET_EHR_LOGIN_SELECTED_ID',
    setEhrLoginExpireOn = 'SET_EHR_LOGIN_EXPIRE_ON',

    ehrLoginCreateRequest = 'EHR_LOGIN_CREATE_REQUEST',
    ehrLoginCreateRequestStart = 'EHR_LOGIN_CREATE_REQUEST_START',
    ehrLoginCreateRequestError = 'EHR_LOGIN_CREATE_REQUEST_ERROR',
    ehrLoginCreateRequestSuccess = 'EHR_LOGIN_CREATE_REQUEST_SUCCESS',

    ehrLoginDeleteRequest = 'EHR_LOGIN_DELETE_REQUEST',
}

export interface ILocationItemResponse {
    ID: number;
    Name: string;
}

export interface ISortState {
    direction: 'asc' | 'desc';
}

export type TGPRAPeriodResponseItem = {
    StartDate: string;
    EndDate: string;
    Label: string,
    Year: number;
}

export type TChoiceItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
    Description?: string;
}

export interface IActionPayload extends Action {
    type:   EVisitActions |   
            ESharedActions |
            EFollowUpActions |
            EReportsActions |
            EScreenActionType |  
            EVisitReportActions |
            EDashboardActionType | 
            EScreenReportActions | 
            EManageDevicesActions |
            EUserProfileActionType | 
            EBranchLocationsActions |
            EApplicationSettingsActions |
            EFollowUpRelatedReportActions |
            EVisitDemographicReportActions |
            EScreenProfileActions |
            EAgeGroupsActions |
            EVisitSettingsActions | 
            EErrorLogActions |
            ESecurityLogSettingsActions |
            ELicenseKeysActions |
            ELoginActions |
            EChangePasswordActions |
            EChangeSecurityQuestionActions |
            ELicenseKeysActions | 
            EAutoExportLogsActions |
            EFindPatientAddressActions |
            ECssrsListActions |
            EResetPasswordActions |
            ECssrsReportActions | 
            EManageUsersActions |
            EEhrLoginActions
    payload: { [k: string]: any }
}