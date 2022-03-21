import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';

import { 
    postFilteredReportsRequest,
    postFilteredReportsRequestStart,
    postFilteredReportsRequestSuccess,
    postFilteredReportsRequestError,
    postFilteredReportsByAgeRequest,
    postFilteredReportsByAgeRequestStart,
    postFilteredReportsByAgeRequestSuccess,
    postFilteredReportsByAgeRequestError,
    postFilteredDrugByAgeRequest,
    postFilteredDrugByAgeRequestStart,
    postFilteredDrugByAgeRequestSuccess,
    postFilteredDrugByAgeRequestError, 
    postFilteredPatientDemographicsRequest,
    postFilteredPatientDemographicsRequestStart,
    postFilteredPatientDemographicsRequestSuccess,
    postFilteredPatientDemographicsRequestError,
    postFilteredFollowupOutcomesRequest,
    postFilteredFollowupOutcomesRequestStart,
    postFilteredFollowupOutcomesRequestSuccess,
    postFilteredFollowupOutcomesRequestError,
    postFilteredVisitsOutcomesRequest,
    postFilteredVisitsOutcomesRequestStart,
    postFilteredVisitsOutcomesRequestSuccess,
    postFilteredVisitsOutcomesRequestError,
    postFilteredScreenTimeLogRequest,
    postFilteredScreenTimeLogRequestStart,
    postFilteredScreenTimeLogRequestSuccess,
    postFilteredScreenTimeLogRequestError,
    reportAgeGroupByAgeRequestStart,
    reportAgeGroupByAgeRequestError,
    reportAgeGroupByAgeRequestSuccess,
    IReportsRequest,   
    getLocationListActionRequestSuccess,
    resetReportsSearchParameters,
    reportEarliestDateRequestStart,
    reportEarliestDateRequestError,
    reportEarliestDateRequestSuccess,
    getScreeningResultReportsBySortRequestError,
    getScreeningResultReportsBySortRequestStart,
    IScreeningReportResultsBySortItem,
    getScreeningResultReportsBySortRequestSuccess,   
    postFilteredIncludeScreeningsRequestSuccess,
    postFilteredIncludeDemographicsRequestSuccess,
    postFilteredIncludeVisitsRequestSuccess,
    postFilteredIncludeFollowUpsRequestSuccess,
    postFilteredIncludeDrugsOfChoiceRequestSuccess,
    postFilteredIncludeCombinedRequestSuccess,
    reportGPRAPeriodRequestSuccess,
    getScreeningResultReportsBySortRequest,
    setScreeningResultReportsBySortAutoStatus,
    getInternalReportsListItemDataRequest,
    getInternalReportsListItemDataRequestStart,
    getInternalReportsListItemDataRequestError,
    getInternalReportsListItemDataRequestSuccess,   
} from '../../actions/reports';
import {ILocationItemResponse,EReportsActions, TGPRAPeriodResponseItem, IActionPayload} from '../../actions';

import postReportByProblem from '../../api/calls/post-report-by-problem';
import postReportByAge from 'api/calls/post-report-by-age';
import postDrugByAge from 'api/calls/post-drug-by-age';
import postPatientDemographics  from 'api/calls/post-patient-demographics';
import postFollowupOutcomes  from 'api/calls/post-followup-outcomes';
import postVisitsOutcomes  from 'api/calls/post-visits-outcomes';
import postScreenTimeLog  from 'api/calls/post-screen-time-log';
import getAgeGroup   from 'api/calls/get-age-group-setting';
import getEarliestDate from 'api/calls/get-earliest-date';
import getLocationsListCall from '../../api/calls/get-locations';
import postScreeningResultReportsBySort from 'api/calls/post-reports-by-sort';

import { 
    getReportSelectedLocationIdSelector, getReportLocationsSelector,
    getReportStartDateSelector, getReportEndDateSelector,
    getReportBsrReportTypeSelector,getReportGrpaPeriodKeySelector,getReportGrpaPeriodsSelector, getReportsBySortFilterArraySelector, isReportAutoRefreshStatusSelector
} from '../../selectors/reports';
import { notifyError } from 'actions/settings';
import getGPRAPeriods from 'api/calls/get-grpa-periods';
import { IScreenListInnerItem } from 'actions/screen';
import postFilterScreenListItem from 'api/calls/post-screen-list-filter-search-item';

function* prepareFilterProps() {
    const type: number = yield select(getReportBsrReportTypeSelector);    
    const StartDate: string = yield select(getReportStartDateSelector);
    const EndDate: string = yield select(getReportEndDateSelector);
    const gpraPeriosKey: string = yield select(getReportGrpaPeriodKeySelector);               
    const locations: Array<ILocationItemResponse> = yield select(getReportLocationsSelector);
    const locationId: number = yield select(getReportSelectedLocationIdSelector);
    const loc = locations.find(l => l.ID === locationId);
    console.log(StartDate); 
    let location = null;
    if (locationId && (locationId > 0) && loc) {
        location = loc.ID;
    }
    let startDateGPRAPeriod;
    let endDateGPRAPeriod; 
    if (gpraPeriosKey) {
        const gpraPeriods: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);                      
        let periodObject = gpraPeriods.find(p => p.Label === gpraPeriosKey);        
        startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
        endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : "2021-9-30";
    }       
    else {
         const list: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
         if (Array.isArray(list)) {
           const periodObject = list[0];
           startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
           endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : "2021-9-30";
       } 
    }     
    return {            
        StartDate: StartDate || startDateGPRAPeriod || "2020-10-01",
        EndDate: EndDate || endDateGPRAPeriod || "2021-9-30",
        Location: location,
        RenderUniquePatientsReportType: !type,      
    };
}
    // Report By Problem
function* doFetchReportsRequest() {    
    try {

        yield put(postFilteredReportsRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const reportbyproblemList: Array<any> = yield call(postReportByProblem, body);
        yield put(postFilteredReportsRequestSuccess(reportbyproblemList));
    } catch (e) {
        yield put(postFilteredReportsRequestError());
    }
}

    // Report By Age
function* doFetchReportByAgeRequest() {
    try {

        yield put(postFilteredReportsByAgeRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const reportbyageList: Array<any> = yield call(postReportByAge, body);
        yield put(postFilteredReportsByAgeRequestSuccess(reportbyageList));
    } catch (e) {
        yield put(postFilteredReportsByAgeRequestError());
    }
}

    // Drug By Age
function* doFetchDrugByAgeRequest() {
    try {

        yield put(postFilteredDrugByAgeRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const drugbyageList: Array<any> = yield call(postDrugByAge, body);
        yield put(postFilteredDrugByAgeRequestSuccess(drugbyageList));
    } catch (e) {
        yield put(postFilteredDrugByAgeRequestError());
    }
}

   // Patient Demographics
function* doFetchPatientDemographicsRequest() {
    try {

        yield put(postFilteredPatientDemographicsRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const postDemographicsList: Array<any> = yield call(postPatientDemographics, body);
        yield put(postFilteredPatientDemographicsRequestSuccess(postDemographicsList));
    } catch (e) {
        yield put(postFilteredPatientDemographicsRequestError());
    }
}
    // Followup Outcomes
function* doFetchFollowupOutcomesRequest() {
    try {

        yield put(postFilteredFollowupOutcomesRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const postFollowupOutcomesList: Array<any> = yield call(postFollowupOutcomes, body);
        yield put(postFilteredFollowupOutcomesRequestSuccess(postFollowupOutcomesList));
    } catch (e) {
        yield put(postFilteredFollowupOutcomesRequestError());
    }
}
    // Visits Outcomes
function* doFetchVisitsOutcomesRequest() {
    try {

        yield put(postFilteredVisitsOutcomesRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const postVisitsOutcomesList: Array<any> = yield call(postVisitsOutcomes, body);
        yield put(postFilteredVisitsOutcomesRequestSuccess(postVisitsOutcomesList));
    } catch (e) {
        yield put(postFilteredVisitsOutcomesRequestError());
    }
}
    // Screen Time Log
function* doFetchScreenTimeLogRequest() {
    try {

        yield put(postFilteredScreenTimeLogRequestStart());       
        const body: IReportsRequest = yield call(prepareFilterProps);
        const postScreenTimeLogList: Array<any> = yield call(postScreenTimeLog, body);
        yield put(postFilteredScreenTimeLogRequestSuccess(postScreenTimeLogList));
    } catch (e) {
        yield put(postFilteredScreenTimeLogRequestError());
    }
}
    // IncludeScreenings
function* doFetchIncludeScreeningsRequest() {              
    yield put(postFilteredIncludeScreeningsRequestSuccess(true));
    yield put(postFilteredIncludeDemographicsRequestSuccess(false));
    yield put(postFilteredIncludeVisitsRequestSuccess(false));
    yield put(postFilteredIncludeFollowUpsRequestSuccess(false));
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(false));
    yield put(postFilteredIncludeCombinedRequestSuccess(false));
   
}
    // IncludeDemographics
function* doFetchIncludeDemographicsRequest() {              
    yield put(postFilteredIncludeDemographicsRequestSuccess(true));
    yield put(postFilteredIncludeScreeningsRequestSuccess(false));   
    yield put(postFilteredIncludeVisitsRequestSuccess(false));
    yield put(postFilteredIncludeFollowUpsRequestSuccess(false));
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(false));
    yield put(postFilteredIncludeCombinedRequestSuccess(false));
   
}
    // IncludeVisits
function* doFetchIncludeVisitsRequest() {              
    yield put(postFilteredIncludeVisitsRequestSuccess(true));
    yield put(postFilteredIncludeDemographicsRequestSuccess(false));
    yield put(postFilteredIncludeScreeningsRequestSuccess(false));    
    yield put(postFilteredIncludeFollowUpsRequestSuccess(false));
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(false));
    yield put(postFilteredIncludeCombinedRequestSuccess(false));
   
}
    // IncludeFollowUps
function* doFetchIncludeFollowUpsRequest() {              
    yield put(postFilteredIncludeFollowUpsRequestSuccess(true));
    yield put(postFilteredIncludeVisitsRequestSuccess(false));
    yield put(postFilteredIncludeDemographicsRequestSuccess(false));
    yield put(postFilteredIncludeScreeningsRequestSuccess(false));     
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(false));
    yield put(postFilteredIncludeCombinedRequestSuccess(false));
   
}
    // IncludeDrugsOfChoice
function* doFetchIncludeDrugsOfChoiceRequest() {              
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(true));
    yield put(postFilteredIncludeFollowUpsRequestSuccess(false));
    yield put(postFilteredIncludeVisitsRequestSuccess(false));
    yield put(postFilteredIncludeDemographicsRequestSuccess(false));
    yield put(postFilteredIncludeScreeningsRequestSuccess(false));       
    yield put(postFilteredIncludeCombinedRequestSuccess(false));
   
}
    // IncludeCombined
function* doFetchIncludeCombinedRequest() {              
    yield put(postFilteredIncludeCombinedRequestSuccess(true));
    yield put(postFilteredIncludeDrugsOfChoiceRequestSuccess(false));
    yield put(postFilteredIncludeFollowUpsRequestSuccess(false));
    yield put(postFilteredIncludeVisitsRequestSuccess(false));
    yield put(postFilteredIncludeDemographicsRequestSuccess(false));
    yield put(postFilteredIncludeScreeningsRequestSuccess(false));      
   
   
}
    // Report By Problem reset
function* doResetReportSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredReportsRequest());
}
    //Report By Age reset
function* doResetReportByAgeSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredReportsByAgeRequest());
}
    // Drug By AGe reset
function* doResetDrugByAgeSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredDrugByAgeRequest());
}
    // Patient Demographics reset
function* doResetPatientDemographicsSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredPatientDemographicsRequest());
}
    // Followup Outcomes reset
function* doResetFollowupOutcomesSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredFollowupOutcomesRequest());
}
    // Visits Outcomes reset
function* doResetVisitsOutcomesSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredVisitsOutcomesRequest());
}
    // Screen Time Log
function* doResetScreenTimeLogSearchParametersRequest() {
    yield put(resetReportsSearchParameters());
    yield put(postFilteredScreenTimeLogRequest());
}

    // Export to Excel
function* doResetExportToExcelSearchParametersRequest() {
     yield put(resetReportsSearchParameters());     
}
    // AgeGroup
function* doFetchReportAgeGroupByAgeRequest() {
    try {

        yield put(reportAgeGroupByAgeRequestStart());           
        const reportAgeGroupbyageList: Array<any> = yield call(getAgeGroup);
        yield put(reportAgeGroupByAgeRequestSuccess(reportAgeGroupbyageList));
    } catch (e) {
        yield put(reportAgeGroupByAgeRequestError());
    }
}

    // Earliest Date
function* doFetchReportEarliestDateRequest() {
    try {

        yield put(reportEarliestDateRequestStart());           
        const reportEarliestDate: string = yield call(getEarliestDate);
        yield put(reportEarliestDateRequestSuccess(reportEarliestDate));
    } catch (e) {
        yield put(reportEarliestDateRequestError());
    }
}
    //GPRA Periods
function* doFetchReportGPRAPeriodsRequest() {

    const gpraPeriods: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods); 
    yield put(reportGPRAPeriodRequestSuccess(gpraPeriods));
   
}

    //Location list
function* doReportLocationListRequest() {
    try {
        const locations: Array<ILocationItemResponse> = yield call(getLocationsListCall);
        yield put(getLocationListActionRequestSuccess(locations));
    } catch (e) {
        console.log(e);
    }
}
    // Reports by sort
function* doFetchReportsBySortRequest() {
    try {
        yield put(getScreeningResultReportsBySortRequestStart());
        const filterBody: { EndDate: string | null, StartDate: string | null, Location: number | null } =  yield call(prepareFilterProps);
        const filterBySortArray: Array<{ ScreeningSection: string, MinScoreLevel: number }> = yield select(getReportsBySortFilterArraySelector);
        const requestData = {
            "EndDate": filterBody.EndDate,
            "Location": filterBody.Location,
            "StartDate": filterBody.StartDate,
            "ProblemScoreFilter": {
                Filters: filterBySortArray
            }
        }

        const response: Array<IScreeningReportResultsBySortItem> = yield call(postScreeningResultReportsBySort, {
            ...requestData
        });
        yield put(getScreeningResultReportsBySortRequestSuccess(response));
     
    } catch(e) {
        yield put(getScreeningResultReportsBySortRequestError());
        yield put(notifyError('Something went wrong.'));
    }
}



function* doScreeningResultReportsBySortAutoUpdateRequest() {
    try {
        yield put(setScreeningResultReportsBySortAutoStatus())
        const isAutoUpdate: boolean = yield select(isReportAutoRefreshStatusSelector);
        while(isAutoUpdate) {
            yield put(getScreeningResultReportsBySortRequest());
            yield delay(60000)
        }
    } catch(e) {

    }
}

function* doChangeReportsTableSort(action: IActionPayload) {
    try {
        const { key, direction } = action.payload || {};
    } catch (e) {
        yield put(notifyError(`Failed to sort`));
    }
}

function* doSceeningResultInnerReportsBySortRequest(action: IActionPayload) {
    try {
        const { screeningResultId } = action.payload || {};
        yield put(getInternalReportsListItemDataRequestStart());
        const filterBody: { EndDate: string | null, StartDate: string | null, Location: number | null } =  yield call(prepareFilterProps);
        const filterBySortArray: Array<{ ScreeningSection: string, MinScoreLevel: number }> = yield select(getReportsBySortFilterArraySelector);
        const requestData = {
            "EndDate": filterBody.EndDate,
            "Location": filterBody.Location,
            "StartDate": filterBody.StartDate,
        }
        const response: IScreenListInnerItem[] = yield call(postFilterScreenListItem,
            screeningResultId, 
            { 
                EndDate: filterBody.EndDate,
                StartDate: filterBody.StartDate,
                Location:`${filterBody.Location}`
            }
        );
        yield put(getInternalReportsListItemDataRequestSuccess({ Items: response, ScreeningResultId: screeningResultId }));
    } catch(e) {
        yield put(getInternalReportsListItemDataRequestError())
    }
}
    //Report By Problem
function* watchFetchReportsRequest() {
    yield takeLatest(EReportsActions.postReportsByProblemRequest, doFetchReportsRequest);
}
    // Report By Age
function* watchFetchReportByAgeRequest() {
    yield takeLatest(EReportsActions.postReportsByAgeRequest, doFetchReportByAgeRequest)
}
    //Drug By Age
function* watchFetchDrugByAgeRequest() {
    yield takeLatest(EReportsActions.postDrugByAgeRequest, doFetchDrugByAgeRequest)
}
    // Patient Demographics
function* watchFetchPatientDemographicsRequest() {
    yield takeLatest(EReportsActions.postPatientDemographicsRequest, doFetchPatientDemographicsRequest)
}
    // Followup Outcomes
 function* watchFollowupOutcomesRequest() {
        yield takeLatest(EReportsActions.postFollowupOutcomesRequest, doFetchFollowupOutcomesRequest)
 }
    // Visits Outcomes
 function* watchVisitsOutcomesRequest() {
    yield takeLatest(EReportsActions.postVisitsOutcomesRequest, doFetchVisitsOutcomesRequest)
}
    // Screen Time Log
function* watchScreenTimeLogRequest() {
    yield takeLatest(EReportsActions.postScreenTimeLogRequest, doFetchScreenTimeLogRequest)
}
    //IncludeScreenings
function* watchIncludeScreeningsRequest() {
    yield takeLatest(EReportsActions.postIncludeScreeningsRequest, doFetchIncludeScreeningsRequest)
}
    // IncludeDemographics
function* watchIncludeDemographicsRequest() {
    yield takeLatest(EReportsActions.postIncludeDemographicsRequest, doFetchIncludeDemographicsRequest)
}
    //IncludeVisits
function* watchIncludeVisitsRequest() {
    yield takeLatest(EReportsActions.postIncludeVisitsRequest, doFetchIncludeVisitsRequest)
}
    //IncludeFollowUps
function* watchIncludeFollowUpsRequest() {
    yield takeLatest(EReportsActions.postIncludeFollowUpsRequest, doFetchIncludeFollowUpsRequest)
}
    //IncludeDrugsOfChoice
function* watchIncludeDrugsOfChoiceRequest() {
    yield takeLatest(EReportsActions.postIncludeDrugsOfChoiceRequest, doFetchIncludeDrugsOfChoiceRequest)
}
    //IncludeCombined
function* watchIncludeCombinedRequest() {
    yield takeLatest(EReportsActions.postIncludeCombinedRequest, doFetchIncludeCombinedRequest)
}

    //Report By Problem reset
function* watchResetReportSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetReportsSearchParametersRequest, doResetReportSearchParametersRequest);
}
    //Report By Age reset
function* watchResetReportByAgeSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetReportsByAgeSearchParametersRequest, doResetReportByAgeSearchParametersRequest);
}
    //Drug By Age reset
function* watchResetDrugByAgeSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetDrugsByAgeSearchParametersRequest, doResetDrugByAgeSearchParametersRequest);
}
    // Patient Demographics reset
function* watchResetPatientDemographicsSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetPatientDemographicsSearchParametersRequest, doResetPatientDemographicsSearchParametersRequest);
}
    // Followup Outcomes reset
function* watchResetFollowupOutcomesSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetFollowupOutcomesSearchParametersRequest, doResetFollowupOutcomesSearchParametersRequest);
}
    // Visits Outcomes reset
function* watchResetVisitsOutcomesSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetVisitsOutcomesSearchParametersRequest, doResetVisitsOutcomesSearchParametersRequest);
}
    // Screen Time Log reset
function* watchResetScreenTimeLogSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetScreenTimeLogSearchParametersRequest, doResetScreenTimeLogSearchParametersRequest);
}
    // Export to Excel reset
function* watchExportToExcelSearchParametersRequest() {
    yield takeLatest(EReportsActions.resetExportToExcelSearchParametersRequest, doResetExportToExcelSearchParametersRequest);
}
    //Age Group
function* watchFetchReportAgeGroupByAgeRequest() {
    yield takeLatest(EReportsActions.reportAgeGroupRequest, doFetchReportAgeGroupByAgeRequest)
}
    // Earliest Date
function* watchFetchReportEarliestDateRequest() {
    yield takeLatest(EReportsActions.reportEarliestDateRequest, doFetchReportEarliestDateRequest)
}
    //GPRA Periods
function* watchFetchReportGPRAPeriodsRequest() {
    yield takeLatest(EReportsActions.reportGPRAPeriodsRequest, doFetchReportGPRAPeriodsRequest)
}
    //Location List
function* watchFetchReportLocationsRequest() {
    yield takeLatest(EReportsActions.getReportLocationsListRequest, doReportLocationListRequest)
}

function* watchFetchReportsBySortRequest() {
    yield takeLatest(EReportsActions.getScreeningResultReportsBySortRequest, doFetchReportsBySortRequest);
}

function* watchScreeningResultReportsBySortAutoUpdateRequest() {
    yield takeLatest(EReportsActions.getScreeningResultReportsBySortAutoUpdateRequest, doScreeningResultReportsBySortAutoUpdateRequest)
}

function* watchChangeReportsTableSort() {
    yield takeLatest(EReportsActions.changeActiveReportsSort, doChangeReportsTableSort);
}

function* watchSceeningResultInnerReportsBySortRequest() {
    yield takeLatest(EReportsActions.getInternalReportsListItemDataRequest, doSceeningResultInnerReportsBySortRequest)
}


const reportsSagas = [
    fork(watchFetchReportsRequest),
    fork(watchFetchReportByAgeRequest),
    fork(watchFetchDrugByAgeRequest),
    fork(watchFetchReportLocationsRequest),
    fork(watchFetchReportAgeGroupByAgeRequest),
    fork(watchResetReportSearchParametersRequest),
    fork(watchFetchReportEarliestDateRequest),
    fork(watchResetReportByAgeSearchParametersRequest),
    fork(watchResetDrugByAgeSearchParametersRequest),
    fork(watchFetchPatientDemographicsRequest),
    fork(watchResetPatientDemographicsSearchParametersRequest),
    fork(watchFollowupOutcomesRequest),
    fork(watchResetFollowupOutcomesSearchParametersRequest),
    fork(watchScreenTimeLogRequest),
    fork(watchResetScreenTimeLogSearchParametersRequest),
    fork(watchVisitsOutcomesRequest),
    fork(watchResetVisitsOutcomesSearchParametersRequest),
    fork(watchFetchReportsBySortRequest),
    fork(watchChangeReportsTableSort),
    fork(watchExportToExcelSearchParametersRequest),
    fork(watchIncludeScreeningsRequest),
    fork(watchIncludeDemographicsRequest),
    fork(watchIncludeVisitsRequest),
    fork(watchIncludeFollowUpsRequest),
    fork(watchIncludeDrugsOfChoiceRequest),
    fork(watchIncludeCombinedRequest),
    fork(watchFetchReportGPRAPeriodsRequest),
    fork(watchScreeningResultReportsBySortAutoUpdateRequest),
    fork(watchSceeningResultInnerReportsBySortRequest),
];

export default reportsSagas;