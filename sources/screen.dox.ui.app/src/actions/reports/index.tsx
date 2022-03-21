import { Action } from 'redux';
import { IActionPayload, EReportsActions } from '../';
import { IScreenListInnerItem } from 'actions/screen';

export type EReportType = {
    key:   string;
    name:  string;
    value: number;
}

export const reportsIndicateReportTypeArray: Array<EReportType> = [
    {
        key: 'Unique Patients',
        name: 'Unique Patients',
        value: 0,
    },
    {
        key: 'Total Reports',
        name: 'Total Reports',
        value: 1,
    }

];

export const reportSortByFilterArray = [
    {
        name: 'Tobacco',
        items: [
            {
                title: 'Tobacco Exposure(0)',
                name: "TCC",
                value: 1,
                checked: false,
            },
            {
                title: 'Tobacco (Smoking)(0)',
                name: 'TCC2',
                value: 2,
                checked: false,
            },
            {
                title: 'Tobacco Use (Smokeless) (0)',
                name: 'TCC3',
                value: 3,
                checked: false,
            },
            {
                title: 'Tobacco Use (Ceremony) (0)',
                name: 'TCC1',
                value: 1,
                checked: false,
            }
        ]
    },
    {
        name: 'Alcolhol Use (CAGE)',
        items: [
            { title: 'At Risk (0)', name: "CAGE", value: 1,checked: false, },
            { title: 'Current Problem (0)', name: "CAGE", value: 2, checked: false, },
            { title: 'Dependence (0)', name: "CAGE", value: 3, checked: false, },
        ]
    },
    {
        name: 'None-Medical Drug Use (Dast-10)',
        items: [
            { title: 'Low (0)', name: 'DAST', value: 1, checked: false, },
            { title: 'Moderate (0)', name: 'DAST', value: 2, checked: false, },
            { title: 'Substantial (0)', name: 'DAST',value: 3, checked: false, },
            { title: 'Severe (0)', name: 'DAST', value: 4, checked: false, },
        ]
    },
    {
        name: 'Drug Use Type',
        items: [
            { title: 'Marijuana/Cannabis/Was/Hashish (0)', name: "DOCH", value: 1, checked: false, },
            { title: 'Methamphetamine (0)', name: "DOCH", value: 2, checked: false, },
            { title: 'Other Amphetamines (0)', name: "DOCH", value: 3, checked: false, },
            { title: "Benzodiazepines(0)", name: "DOCH", value: 4, checked: false, },
            { title: 'Opiod/Herion(0)', name: "DOCH", value: 5, checked: false, },
            { title: 'Opioid/Medication(0)', name: "DOCH", value: 6, checked: false, },
            { title: "Cocaine/Crack(0)", name: "DOCH", value: 7, checked: false, },
            { title: "Hallucinogens/Psychedelis(0)", name: "DOCH", value: 8, checked: false, },
            { title: "Sedative/Hypnotics/Non-Benzo Tranquilizers(0)", name: "DOCH", value: 9, checked: false, },
            { title: 'Inhalants(0)', name: "DOCH", value: 10, checked: false, },
            { title: "Barbiturates/Downers(0)", name: "DOCH", value: 11, checked: false, },
            { title: "PCP/Ketamine/GHB/Desinger Drugs(0)", name: "DOCH", value: 12, checked: false, },
            { title: "Other Stimulants(0)", name: "DOCH", value: 13, checked: false, },
            { title: 'Other(0)', name: "DOCH", value: 14, checked: false, },
        ]
    },
    {
        name: 'Anxiety (GAD-2 / GAD-7)',
        items: [
            { title: "Mild (0)", name: 'GAD7A', value: 1, checked: false, },
            { title: "Moderate (0)", name: 'GAD7A', value: 2, checked: false, },
            { title: "Severe (0)", name: 'GAD7A', value: 3, checked: false, },
        ]
    },
    {
        name: 'Depression (PHQ-2 / PHQ-9)',
        items: [
           {  title: "Mild (0)", name: "PHQ1", value: 2, checked: false, },
           {  title: "Moderate (0)", name: "PHQ1", value: 3, checked: false, },
           {  title: "Moderate-Severe (0)", name: "PHQ1", value: 4, checked: false, },
           {  title: "Severe (0)", name: "PHQ1", value: 5, checked: false, },
        ]
    },
    {
        name: "Suicidal Ideation (PHQ-9)",
        items: [
           {  title: "Several Days (0)", name: "PHQ2", value: 1, checked: false, },
           {  title: "More Than Half the Days (0)", name: "PHQ2", value: 2, checked: false, },
           {  title: "Nearly Every Day (0)", name: "PHQ2", value: 3, checked: false, }
        ]
    },
    {
        name: 'Domestic/Intimate Partner Violence (HITS)',
        items: [
           { title: 'Current problem (0)', name: "HITS", value: 1, checked: false, }
        ]
    },
    {
        name: 'Problem Gambling (BBGS)',
        items: [
            { title: "Evidence of PROBLEM GAMBLING (0)", name: "BBGS", value: 1, checked: false, }
        ]
    }
]

export interface FilterBySort {
    name: string;
    items: Array<FilterBySortItem>;
}

export interface FilterBySortItem {
    name: string;
    title: string;
    value: number;
    checked: boolean;
}

export const postFilteredReportsRequest = ():Action  => ({ type: EReportsActions.postReportsByProblemRequest });
export const postFilteredReportsRequestStart = ():Action  => ({ type: EReportsActions.postReportsByProblemRequestStart });
export const postFilteredReportsRequestError = ():Action  => ({ type: EReportsActions.postReportsByProblemRequestError });

export const postFilteredReportsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postReportsByProblemRequestSuccess,
    payload
});

export const postFilteredReportsByAgeRequest = ():Action  => ({ type: EReportsActions.postReportsByAgeRequest });
export const postFilteredReportsByAgeRequestStart = ():Action  => ({ type: EReportsActions.postReportsByAgeRequestStart });
export const postFilteredReportsByAgeRequestError = ():Action  => ({ type: EReportsActions.postReportsByAgeRequestError });

export const postFilteredReportsByAgeRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postReportsByAgeRequestSuccess,
    payload
});

export const postFilteredDrugByAgeRequest = ():Action  => ({ type: EReportsActions.postDrugByAgeRequest });
export const postFilteredDrugByAgeRequestStart = ():Action  => ({ type: EReportsActions.postDrugByAgeRequestStart });
export const postFilteredDrugByAgeRequestError = ():Action  => ({ type: EReportsActions.postDrugByAgeRequestError });

export const postFilteredDrugByAgeRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postDrugByAgeRequestSuccess,
    payload
});

export const postFilteredPatientDemographicsRequest = ():Action  => ({ type: EReportsActions.postPatientDemographicsRequest });
export const postFilteredPatientDemographicsRequestStart = ():Action  => ({ type: EReportsActions.postPatientDemographicsRequestStart });
export const postFilteredPatientDemographicsRequestError = ():Action  => ({ type: EReportsActions.postPatientDemographicsRequestError });

export const postFilteredPatientDemographicsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postPatientDemographicsRequestSuccess,
    payload
});

export const postFilteredFollowupOutcomesRequest = ():Action  => ({ type: EReportsActions.postFollowupOutcomesRequest });
export const postFilteredFollowupOutcomesRequestStart = ():Action  => ({ type: EReportsActions.postFollowupOutcomesRequestStart });
export const postFilteredFollowupOutcomesRequestError = ():Action  => ({ type: EReportsActions.postFollowupOutcomesRequestError });

export const postFilteredFollowupOutcomesRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postFollowupOutcomesRequestSuccess,
    payload
});

export const postFilteredVisitsOutcomesRequest = ():Action  => ({ type: EReportsActions.postVisitsOutcomesRequest });
export const postFilteredVisitsOutcomesRequestStart = ():Action  => ({ type: EReportsActions.postVisitsOutcomesRequestStart });
export const postFilteredVisitsOutcomesRequestError = ():Action  => ({ type: EReportsActions.postVisitsOutcomesRequestError });

export const postFilteredVisitsOutcomesRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postVisitsOutcomesRequestSuccess,
    payload
});

export const postFilteredScreenTimeLogRequest = ():Action  => ({ type: EReportsActions.postScreenTimeLogRequest });
export const postFilteredScreenTimeLogRequestStart = ():Action  => ({ type: EReportsActions.postScreenTimeLogRequestStart });
export const postFilteredScreenTimeLogRequestError = ():Action  => ({ type: EReportsActions.postScreenTimeLogRequestError });

export const postFilteredScreenTimeLogRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postScreenTimeLogRequestSuccess,
    payload
});

export const postFilteredIncludeScreeningsRequest = ():Action  => ({ type: EReportsActions.postIncludeScreeningsRequest });
export const postFilteredIncludeScreeningsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeScreeningsRequestSuccess,
    payload
});

export const postFilteredIncludeDemographicsRequest = ():Action  => ({ type: EReportsActions.postIncludeDemographicsRequest });
export const postFilteredIncludeDemographicsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeDemographicsRequestSuccess,
    payload
});

export const postFilteredIncludeVisitsRequest = ():Action  => ({ type: EReportsActions.postIncludeVisitsRequest });
export const postFilteredIncludeVisitsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeVisitsRequestSuccess,
    payload
});

export const postFilteredIncludeFollowUpsRequest = ():Action  => ({ type: EReportsActions.postIncludeFollowUpsRequest });
export const postFilteredIncludeFollowUpsRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeFollowUpsRequestSuccess,
    payload
});

export const postFilteredIncludeDrugsOfChoiceRequest = ():Action  => ({ type: EReportsActions.postIncludeDrugsOfChoiceRequest });
export const postFilteredIncludeDrugsOfChoiceRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeDrugsOfChoiceRequestSuccess,
    payload
});

export const postFilteredIncludeCombinedRequest = ():Action  => ({ type: EReportsActions.postIncludeCombinedRequest });
export const postFilteredIncludeCombinedRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.postIncludeCombinedRequestSuccess,
    payload
});

export const reportAgeGroupByAgeRequest = ():Action  => ({ type: EReportsActions.reportAgeGroupRequest });
export const reportAgeGroupByAgeRequestStart = ():Action  => ({ type: EReportsActions.reportAgeGroupRequestStart });
export const reportAgeGroupByAgeRequestError = ():Action  => ({ type: EReportsActions.reportAgeGroupRequestError });

export const reportAgeGroupByAgeRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.reportAgeGroupRequestSuccess,
    payload
});

export const reportEarliestDateRequest = ():Action  => ({ type: EReportsActions.reportEarliestDateRequest });
export const reportEarliestDateRequestStart = ():Action  => ({ type: EReportsActions.reportEarliestDateRequestStart });
export const reportEarliestDateRequestError = ():Action  => ({ type: EReportsActions.reportEarliestDateRequestError });

export const reportEarliestDateRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.reportEarliestDateRequestSuccess,
    payload
});

export const reportGPRAPeriodsRequest = ():Action  => ({ type: EReportsActions.reportGPRAPeriodsRequest });
export const reportGPRAPeriodRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.reportGPRAPeriodsRequestSuccess,
    payload
});


export interface IReportsRequest {
    Location?: number | null;
    StartDate?: string | null;
    EndDate?: string | null;      
    ReportType?: number;   
    RenderUniquePatientsReportType?:boolean;
}
export const postReportsListItemRequest = (): Action => ({
    type: EReportsActions.getInternalReportsListItemDataRequest,
});

export const postReportsListItemRequestStart = (): Action => ({
    type: EReportsActions.getInternalReportsListItemDataRequestStart,
});

export const postReportsListItemRequestSuccess = (payload: any): IActionPayload => ({
    type: EReportsActions.getInternalReportsListItemDataRequestSuccess,
    payload
});


export const postReportsListItemRequestError = (): Action => ({
    type: EReportsActions.getInternalReportsListItemDataRequestError,
});

export interface IReportsInnerItem {
    ID: number;  
    CreatedDate: string;    
    ReportsDate: string;         
    ReportName: string;    
}

export interface IScreeningReportResultsBySortItem {
    PatientName: string;
    Birthday: string | null;
    ReportItems: Array<IReportBySortItem>;
    LastCreatedDate: string | null;
}

export interface IReportBySortItem {
    PatientName: string;
    Birthday: string | null;
    ScreeningResultID: number;
    ScreeningName: string;
    CreatedDate: string | null;
    CreatedDateLabel: string | null;
    IsPositive: boolean;
    ExportDate: string | null;
    ExportDateLabel: string | null;
    ExportedToHRN: string | null;
    HasAnyScreening: boolean;
    HasAddress: boolean;
    ShowBeginExportButton: boolean;
    IsPositiveCssClass: string;
}

export const setReportsLocationsId = (id: number): IActionPayload => ({ 
    type: EReportsActions.setReportsLocationId, payload: { id }
});

export const setReportsCurrentStartDate = (startDate: string | null): IActionPayload => {
    return ({
        type: EReportsActions.setReportsScreendoxStartDate, payload: { startDate } 
    })
}

export const setReportsCurrentEndDate = (endDate: string | null): IActionPayload => {
    return ({
        type: EReportsActions.setReportsScreendoxEndDate, payload: { endDate } 
    })
}

export const setReportsCurrentGPRAPeriodRangeChange = (periodKey: string): IActionPayload => {
    return ({
        type: EReportsActions.setReportsScreendoxGRPAPeriod, payload: { periodKey } 
    })
}


export const setReportBSRReportType = (reportType: number): IActionPayload => {
    return ({
        type: EReportsActions.setReportsBSRReportType, payload: { reportType } 
    })
}


export const resetReportsSearchParameters = (): Action => ({ type: EReportsActions.resetReportsSearchParameters });
export const resetReportsSearchParametersRequest = (): Action => ({ type: EReportsActions.resetReportsSearchParametersRequest });

export const resetReportsByAgeSearchParameters = (): Action => ({ type: EReportsActions.resetReportsByAgeSearchParameters });
export const resetReportsByAgeSearchParametersRequest = (): Action => ({ type: EReportsActions.resetReportsByAgeSearchParametersRequest });

export const resetDrugsByAgeSearchParameters = (): Action => ({ type: EReportsActions.resetDrugsByAgeSearchParameters });
export const resetDrugsByAgeSearchParametersRequest = (): Action => ({ type: EReportsActions.resetDrugsByAgeSearchParametersRequest });

export const resetPatientDemographicsSearchParameters = (): Action => ({ type: EReportsActions.resetPatientDemographicsSearchParameters });
export const resetPatientDemographicsSearchParametersRequest = (): Action => ({ type: EReportsActions.resetPatientDemographicsSearchParametersRequest });

export const resetFollowupOutcomesSearchParameters = (): Action => ({ type: EReportsActions.resetFollowupOutcomesSearchParameters});
export const resetFollowupOutcomesSearchParametersRequest = (): Action => ({ type: EReportsActions.resetFollowupOutcomesSearchParametersRequest });

export const resetVisitsOutcomesSearchParameters = (): Action => ({ type: EReportsActions.resetVisitsOutcomesSearchParameters});
export const resetVisitsOutcomesSearchParametersRequest = (): Action => ({ type: EReportsActions.resetVisitsOutcomesSearchParametersRequest });

export const resetScreenTimeLogSearchParameters = (): Action => ({ type: EReportsActions.resetScreenTimeLogSearchParameters});
export const resetScreenTimeLogSearchParametersRequest = (): Action => ({ type: EReportsActions.resetScreenTimeLogSearchParametersRequest});

export const resetExportToExcelSearchParameters = (): Action => ({ type: EReportsActions.resetExportToExcelSearchParameters});
export const resetExportToExcelParametersRequest = (): Action => ({ type: EReportsActions.resetExportToExcelSearchParametersRequest});

export const getLocationListActionRequest = () : Action => ({ type: EReportsActions.getReportLocationsListRequest})
export const getLocationListActionRequestSuccess = (payload: any) :IActionPayload  => ({ 
    type: EReportsActions.getLocationListActionRequestSuccess,
    payload
});

export const getScreeningResultReportsBySortRequest = (): Action => ({ type: EReportsActions.getScreeningResultReportsBySortRequest });
export const getScreeningResultReportsBySortRequestStart = (): Action => ({ type: EReportsActions.getScreeningResultReportsBySortRequestStart });
export const getScreeningResultReportsBySortRequestError = (): Action => ({ type: EReportsActions.getScreeningResultReportsBySortRequestError });
export const getScreeningResultReportsBySortRequestSuccess = (payload: Array<IScreeningReportResultsBySortItem>): IActionPayload => ({ type: EReportsActions.getScreeningResultReportsBySortRequestSuccess, payload });

export const changeReportsActiveSortObject = (key: string, direction: string): IActionPayload => {
    return ({
        type: EReportsActions.changeActiveReportsSort, payload: { key, direction } 
    })
}

export const setScreeningResultReportBySortFilterArray = (payload: Array<{ ScreeningSection: string, MinScoreLevel: number }>): IActionPayload => ({
    type: EReportsActions.setScreeningResultReportsBySortFilterArray, payload
})

export const setScreeningResultReportsBySortAutoStatus = (): Action => ({
    type: EReportsActions.setScreeningResultReportsBySortAutoStatus
})

export const getScreeningResultReportsBySortAutoUpdateRequest = ():Action => ({
    type: EReportsActions.getScreeningResultReportsBySortAutoUpdateRequest
})

export const getInternalReportsListItemDataRequest = (screeningResultId: number): IActionPayload => ({
    type: EReportsActions.getInternalReportsListItemDataRequest, payload: { screeningResultId }
})

export const getInternalReportsListItemDataRequestStart = (): Action => ({
    type: EReportsActions.getInternalReportsListItemDataRequestStart
})

export const getInternalReportsListItemDataRequestError = (): Action => ({
    type: EReportsActions.getInternalReportsListItemDataRequestError
})

export const getInternalReportsListItemDataRequestSuccess = (payload: { Items: IScreenListInnerItem[], ScreeningResultId: number}): IActionPayload => ({
    type: EReportsActions.getInternalReportsListItemDataRequestSuccess, payload
})