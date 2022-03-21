import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import {  
    screenListCurrentPageSelector, screenListOffsetSelector, isScreenListAutoUpdateSelector,
    isScreenListActiveSortSelector, isScreenListActiveSortDirectionSelector, getScreenListFirstNameSelector,
    getScreenListLastNameSelector, getScreenListScreendoxResultIdSelector, getScreenListStartDateSelector,
    getScreenListEndDateSelector, getScreenListGrpaPeriodKey, getScreenListGrpaPeriodList,
    getScreenListLocationList, getScreenListSelectedLocationId, screenListSelector, getScreenListEhrExportPatientInfoSelector, getScreenListEhrExportPatientRecordsCurrentPageSelector, getEhrExportVisitRecordsCurrentPageSelector, getEhrExportPatientRecordSelectedIdSelector, getEhrExportVisitRecordSelectedIdSelector
} from '../../selectors/screen';
import { EScreenActionType, IActionPayload } from '../../actions';
import {
    IScreenListInnerItem, IScreenListResponseItem, postScreenListFilterRequestAutoUpdateSet, 
    postScreenListFilterRequestError, postScreenListFilterRequestStart, postScreenListFilterRequestSuccess, 
    postScreenListRequestError,  postScreenListRequestStart, postScreenListRequestSuccess, changeActiveSortKey,
    postScreenListFilterRequestAutoUpdateRecall, changeActiveSortDirection, postScreenListItemRequestStart,
    postScreenListItemRequestSuccess, postScreenListItemRequestError, setCurrentScreenListOffset,
    setCurrentScreenListPage, clearScreeningListSearchBarParameters, setCurrentGPRAPeriodRangeChange,
    getGPRAPeriodsSuccessAction, getGPRAPeriodsStartAction, getGPRAPeriodsErrorAction, 
    getLocationListStartAction, getLocationListSuccessAction, getLocationListErrorAction, IScreenListEhrExportFinalResultResponse,
    setLocationIdAction, setCurrentScreenListStartDate, setCurrentScreenListEndDate, getScreenListEhrExportPatientInfoRequestSuccess, postScreenListEhrExportPatientInfoRequest, postScreenListEhrExportPatientInfoRequestStart, postScreenListEhrExportPatientInfoRequestError, postScreenListEhrExportPatientInfoRequestSuccess, getScreenListEhrExportPatientRecordsSuccess, getScreenListEhrExportPatientRecordsError, getScreenListEhrExportPatientRecordsStart, getScreenListEhrExportVisitRecordsRequest, getScreenListEhrExportVisitRecordsRequestError, getScreenListEhrExportVisitRecordsRequestStart, IScreenListEhrExportVisitRecord, getScreenListEhrExportVisitRecordsRequestSuccess, ehrExportFinalResultRequestStart, ehrExportFinalResultRequestError, ehrExportFinalResultRequestSuccess, setEhrExportScreeningResultId, setEhrExportScreeningResults, setEhrExportScreeningDate, setEhrExportPatientRecordsCurrentPage, setEhrExportPatientRecordSelectedId, setEhrExportVisitRecordSelectedId, postScreenListFilterRequest,
} from '../../actions/screen';
import { TGPRAPeriodResponseItem, ILocationItemResponse } from '../../actions';
import postScreenList from '../../api/calls/post-screen-list-search';
import postFilterScreenList, { IGetFilteredScreenListItemsRequest } from '../../api/calls/post-screen-list-filter-search';
import postFilterScreenListItem from '../../api/calls/post-screen-list-filter-search-item';
import getGPRAPeriods from '../../api/calls/get-grpa-periods';
import getLocationsListCall from '../../api/calls/get-locations';
import { MAXIMUM_RECORDS_PER_PAGE } from '../../states/screen';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess } from '../../actions/settings';
import { IScreeningReport, IScreeningReportPatientInfo } from 'actions/screen/report';
import getScreeningReport from 'api/calls/get-screening-report';
import postPatientInfoEdit from 'api/calls/post-patient-info-edit';
import { IEhrRecordPatientsItem } from 'actions/find-patient-address';
import postEhrRecordPatients from 'api/calls/post-ehr-record-patients';
import postEhrVisitRecords from 'api/calls/post-ehr-visit-records';
import postEhrExportResult from 'api/calls/post-ehr-export-final-result';
import {  setEhrExportSelectedTab } from 'actions/screen';
import { setCreateDateCustomOrGPRASelector } from 'selectors/shared';

const MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE = 5;

function* doScreenListChange() {
    try {
        yield put(postScreenListRequestStart());
        const screenList: { Items: IScreenListResponseItem[] } = yield call(postScreenList);
        yield put(postScreenListRequestSuccess(screenList));
    } catch (e) {
        yield put(postScreenListRequestError());
    }
}

function* prepareScreenItemsReauest(): any {
    try {
        const currentOffset: number = yield select(screenListOffsetSelector);
        const sortKey: string = yield select(isScreenListActiveSortSelector);
        const sortDirection: string = yield select(isScreenListActiveSortDirectionSelector);
        const firstName: string = yield select(getScreenListFirstNameSelector);
        const lastName: string = yield select(getScreenListLastNameSelector);
        const screendoxResultId: number = yield select(getScreenListScreendoxResultIdSelector);
        const StartDate: string = yield select(getScreenListStartDateSelector);
        const EndDate: string = yield select(getScreenListEndDateSelector);
        const gpraPeriosKey: string = yield select(getScreenListGrpaPeriodKey);
        const isCreateDateCustom: boolean = yield select(setCreateDateCustomOrGPRASelector);
        let gpraPerios: Array<TGPRAPeriodResponseItem> = yield select(getScreenListGrpaPeriodList);
        if (!Array.isArray(gpraPerios)) {
            gpraPerios = yield call(getGPRAPeriods);
        }
        const locations: Array<ILocationItemResponse> = yield select(getScreenListLocationList);
        const locationId: number = yield select(getScreenListSelectedLocationId);
        const loc = locations.find(l => l.ID === locationId);
        let location = null;
        if (locationId && (locationId > 0) && loc) {
            location = loc.ID;
        }
        let startDateGPRAPeriod;
        let endDateGPRAPeriod;
        const endDateStat = new Date().toISOString();
        if(!isCreateDateCustom) {
            if (gpraPeriosKey) {
                const periodObject = gpraPerios.find(p => p.Label === gpraPeriosKey);
                startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
                endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
            } else {
                const listOfScreens: IScreenListResponseItem[]  = yield select(screenListSelector);
                const list: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
                if (Array.isArray(list) && list.length) {
                    const periodObject = list[0];
                    startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
                    endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
                }
            }
        }
        const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'LastCheckinDate DESC';
        return { 
            StartRowIndex: currentOffset,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
            FirstName: firstName || null,
            LastName: lastName || null,
            ScreeningResultID: screendoxResultId || null,
            StartDate: startDateGPRAPeriod || StartDate || "2020-10-01",
            EndDate: endDateGPRAPeriod ||  EndDate || endDateStat,
            Location: location,
            OrderBy,
        }
    }catch(e) {
        return null;
    }
}

function* doScreenListFilterUpdateAutomatically() {
    try {
        const currentPage: number = yield select(screenListCurrentPageSelector);
        const reqBody: IGetFilteredScreenListItemsRequest = yield call(prepareScreenItemsReauest);
        const response: { 
            Items: IScreenListResponseItem[], 
            TotalCount: number
        } = yield call(postFilterScreenList, reqBody);
        if (!response) {
            return // error handler
        }
        yield put(postScreenListFilterRequestSuccess({
            Total: Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
            Items: response.Items,
            Offset: reqBody.StartRowIndex || 0,
            CurrentPage: currentPage,
        }));
    } catch(e) {
        // error handling
    }
}

function* doListItemReport(action: IActionPayload) {
    try {
        const { ScreeningResultID = 0 } = action.payload || {};
        yield put(postScreenListItemRequestStart());
        const reqBody: IGetFilteredScreenListItemsRequest = yield call(prepareScreenItemsReauest);
        if (!reqBody || Number.isNaN(ScreeningResultID)) {
            yield put(postScreenListItemRequestError());
            return;
        } else {
            const checkedId = Number.isNaN(ScreeningResultID) ? 0 : ScreeningResultID;
            const response: IScreenListInnerItem[] = yield call(postFilterScreenListItem,
                checkedId, 
                {
                    StartRowIndex: reqBody.StartRowIndex,
                    MaximumRows: reqBody.MaximumRows,
                    FirstName: reqBody.FirstName,
                    LastName: reqBody.LastName,
                    // ScreeningResultID: checkedId,
                    StartDate: reqBody.StartDate,
                    EndDate: reqBody.EndDate,
                    Location: reqBody.Location,
                    OrderBy: reqBody.OrderBy,
                }
            );
            if (!response) {
                yield put(postScreenListItemRequestError());
                return;
            }
            yield put(postScreenListItemRequestSuccess({ Items: response, ScreeningResultID}));
        }
    } catch(e) {
        yield put(postScreenListItemRequestError());
    }
}

function* doScreenListFilterChange(action: IActionPayload) {
    try {
        const { page } = action.payload || {};
        const sortKey: string = yield select(isScreenListActiveSortSelector);
        const sortDirection: string = yield select(isScreenListActiveSortDirectionSelector);
        yield put(postScreenListFilterRequestStart());
        let gpraPerios: Array<TGPRAPeriodResponseItem> = yield select(getScreenListGrpaPeriodList);
        if (!Array.isArray(gpraPerios)) {
            gpraPerios = yield call(getGPRAPeriods);
        }
        const StartRowIndex = ((page * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const requestBody: IGetFilteredScreenListItemsRequest = {
            StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
        } 
        if (Array.isArray(gpraPerios) && gpraPerios.length) {
            const firstPeriod = gpraPerios[0];
            requestBody.StartDate = firstPeriod.StartDate;
            requestBody.EndDate = firstPeriod.EndDate;
        } else {
            const listOfScreens: IScreenListResponseItem[]  = yield select(screenListSelector);
            const list: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
            if (Array.isArray(list) && list.length && !listOfScreens.length) {
                const periodObject = list[0];
                requestBody.StartDate = !!periodObject ? periodObject.StartDate : "2020-10-01";
                requestBody.EndDate = !!periodObject ? periodObject.EndDate : new Date().toISOString();
            }
        }
        const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'LastCheckinDate DESC';
        requestBody.OrderBy = OrderBy;
        const response: { 
            Items: IScreenListResponseItem[], 
            TotalCount: number
        } = yield call(postFilterScreenList, requestBody);
        if (!response) {
            return; // error handler
        }
        yield put(postScreenListFilterRequestSuccess({
            Total: Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
            Items: response.Items,
            Offset: StartRowIndex,
            CurrentPage: page,
        }));
        let isAutomated: boolean = yield select(isScreenListAutoUpdateSelector);
        while (isAutomated) {
            isAutomated = yield select(isScreenListAutoUpdateSelector);     
            yield doScreenListFilterUpdateAutomatically();
            yield delay(60000);
        }
    } catch (e) {
        yield put(postScreenListFilterRequestError());
    }
}

function* doAutoUpdateScreenList() {
    let isAutomated: boolean = yield select(isScreenListAutoUpdateSelector);
    console.log(isAutomated);
    // yield put(postScreenListFilterRequestAutoUpdateSet(!isAutomated));
    // isAutomated = yield select(isScreenListAutoUpdateSelector);
    while (isAutomated) {
        // isAutomated = yield select(isScreenListAutoUpdateSelector);     
        yield doScreenListFilterUpdateAutomatically();
        yield delay(60000);
    }
}

function* doChangeScreenTableSort(action: IActionPayload) {
    try {
        const { key, direction } = action.payload || {};
        yield put(setCurrentScreenListOffset(0));
        yield put(setCurrentScreenListPage(1));
        yield put(changeActiveSortKey(key));
        yield put(changeActiveSortDirection(key, direction));
        yield put(postScreenListFilterRequestAutoUpdateRecall());
    } catch (e) {
        yield put(notifyError(`Failed to sort`));
    }
}

function* doScreenListGPRAPeriodRequest() {
    try {
        yield put(getGPRAPeriodsStartAction());
        const gpraPeriods: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
        if (Array.isArray(gpraPeriods) && gpraPeriods.length) {
            const currentKey = gpraPeriods[0].Label;
            yield put(setCurrentScreenListStartDate(null));
            yield put(setCurrentScreenListEndDate(null));
            yield put(setCurrentGPRAPeriodRangeChange(currentKey));
        }
        yield put(getGPRAPeriodsSuccessAction(gpraPeriods));
    } catch (e) {
        yield put(getGPRAPeriodsErrorAction());
    }
}

function* doScreenListLocationListRequest() {
    try {
        yield put(getLocationListStartAction());
        const locations: Array<ILocationItemResponse> = yield call(getLocationsListCall);
        yield put(getLocationListSuccessAction(locations));
    } catch (e) {
        yield put(getLocationListErrorAction());
    }
}

function* doClearScreenListSearchBar() {
    const periods: Array<TGPRAPeriodResponseItem> = yield select(getScreenListGrpaPeriodList);
    yield put(setCurrentGPRAPeriodRangeChange(''))
    yield put(setCurrentScreenListOffset(0));
    yield put(setCurrentScreenListPage(1));
    yield put(clearScreeningListSearchBarParameters());
    yield put(setLocationIdAction(0));
    const currentKey = !!periods[0] ? periods[0].Label : '';
    yield put(setCurrentScreenListStartDate(null));
    yield put(setCurrentScreenListEndDate(null));
    yield put(setCurrentGPRAPeriodRangeChange(currentKey));
    yield put(postScreenListFilterRequestAutoUpdateRecall());
}


function* doScreenListSearchBar() {
    yield put(setCurrentScreenListOffset(0));
    yield put(setCurrentScreenListPage(1));
    yield put(postScreenListFilterRequestAutoUpdateRecall());
}

function* doScreenListEhrExportPatientInfoRequest(action: IActionPayload) {
    const { id } = action.payload;
    try {
        const response: IScreeningReport = yield call(getScreeningReport, id);
        yield put(getScreenListEhrExportPatientInfoRequestSuccess(response.PatientInfo));
        yield put(setEhrExportScreeningResultId(response.ID));
        yield put(setEhrExportScreeningDate(response.CreatedDateLabel));
        yield put(getScreenListEhrExportPatientRecordsStart());
        const currentPage: number = yield select(getScreenListEhrExportPatientRecordsCurrentPageSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE) - MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE);
        const ehrPatientRecords: { Items: Array<IEhrRecordPatientsItem>, TotalCount: number } = yield call(postEhrRecordPatients, id, {
            StartRowIndex,
            MaximumRows: MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE     
        });
        yield put(getScreenListEhrExportPatientRecordsSuccess({ ...ehrPatientRecords, TotalCount: Math.ceil(ehrPatientRecords.TotalCount / MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE)}))
    } catch (e) {
        yield put(getScreenListEhrExportPatientRecordsError());
    }
}

function* doScreenListEhrExportPatientInfoSavingRequest(action: IActionPayload) {
    const { id } = action.payload;
    try {
        yield put(postScreenListEhrExportPatientInfoRequestStart());
        const patientInfo: IScreeningReportPatientInfo = yield select(getScreenListEhrExportPatientInfoSelector);
        const response: string = yield call(postPatientInfoEdit, { ...patientInfo }, id );
        yield put(postScreenListEhrExportPatientInfoRequestSuccess());
        yield put(notifySuccess('Patient Record Saved.'));
    } catch(e) {
        yield put(postScreenListEhrExportPatientInfoRequestError());
        yield put(notifyError('Patient Record could not be saved.'));
    }
}

function* doScreenListEhrExportVisitRecordsRequest(action: IActionPayload) {
    const { patientId } = action.payload;
    try {
        yield put(getScreenListEhrExportVisitRecordsRequestStart());
        const currentPage: number = yield select(getEhrExportVisitRecordsCurrentPageSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE) - MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE);
        const response: { Items: Array<IScreenListEhrExportVisitRecord>, TotalCount: number } = yield call(postEhrVisitRecords, patientId, {
            StartRowIndex,
            MaximumRows: MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE     
        });
        yield put(getScreenListEhrExportVisitRecordsRequestSuccess({ ...response, TotalCount: Math.ceil(response.TotalCount / MAXIMUM_EHR_PATIENT_RECORDS_PER_PAGE)}));
    } catch(e) {
        yield put(getScreenListEhrExportVisitRecordsRequestError());
    }
}

function* doScreenListEhrExportFinalResultRequest(action: IActionPayload) {
    const { screeningResultId } = action.payload;
    try {
        yield put(ehrExportFinalResultRequestStart());
        const patientId: number = yield select(getEhrExportPatientRecordSelectedIdSelector);
        const visitId: number = yield select(getEhrExportVisitRecordSelectedIdSelector);
        const response: IScreenListEhrExportFinalResultResponse = yield call(postEhrExportResult, screeningResultId, {
            "PatientId": patientId,
            "VisitId": visitId
        });
        console.log(response);
        if(response.IsSuccessful) {
            yield put(notifySuccess('EHR Export completed successfully'));
            yield put(setEhrExportSelectedTab(2));
            yield put(setEhrExportScreeningResults(response));
            yield put(postScreenListFilterRequest({page: 1}));
            yield delay(10000);
            yield put(closeModalWindow(EModalWindowKeys.screenListSelectEHRRecord));
            yield put(setEhrExportPatientRecordSelectedId(null));
            yield put(setEhrExportVisitRecordSelectedId(null));
            yield put(setEhrExportSelectedTab(0));
        } else {
            yield put(notifyError('BHS Report export has failed.'));
        }
        yield put(ehrExportFinalResultRequestSuccess());
    } catch(e) {
        yield put(ehrExportFinalResultRequestError());
    }
}

function* watchScreenListChange() {
    yield takeLatest(EScreenActionType.postScreenListSearchRequest, doScreenListChange);
}

function* watchScreenListFilterChanges() {
    yield takeLatest(EScreenActionType.postScreenListFilterRequest, doScreenListFilterChange);
}

function* watchAutoUpdateScreenList() {
    yield takeLatest(EScreenActionType.postScreenListFilterAutoUpdat, doAutoUpdateScreenList);
}

function* watchAutoUpdateScreenListRecall() {
    yield takeLatest(EScreenActionType.postScreenListFilterAutoUpdatRecall, doScreenListFilterUpdateAutomatically);
}

function* watchChangeScreenTableSort() {
    yield takeLatest(EScreenActionType.changeActiveSort, doChangeScreenTableSort);
}

function* watchListItemReport() {
    yield takeLatest(EScreenActionType.getInternalScreenListItemDataRequest, doListItemReport);
}

function* watchTriggerScreenListClearSearchBar() {
    yield takeLatest(EScreenActionType.triggerClearSearchParameters, doClearScreenListSearchBar);
}

function* watchTriggerScreenListSearchBar() {
    yield takeLatest(EScreenActionType.triggerSearchParameters, doScreenListSearchBar);
}

function* watchScreenListGPRAPeriod() {
    yield takeLatest(EScreenActionType.getGPRAPeriodRequest, doScreenListGPRAPeriodRequest);
}

function* watchScreenListLocationList() {
    yield takeLatest(EScreenActionType.getLocationListRequest, doScreenListLocationListRequest);
}

function* watchScreenListEhrExportPatientInfoRequest() {
    yield takeLatest(EScreenActionType.getScreenListEhrExportPatientInfoRequest, doScreenListEhrExportPatientInfoRequest);
}

function* watchScreenListEhrExportPatientInfoSavingRequest() {
    yield takeLatest(EScreenActionType.postScreenListEhrExportPatientInfoRequest, doScreenListEhrExportPatientInfoSavingRequest);
}

function* watchScreenListEhrExportVisitRecordsRequest() {
    yield takeLatest(EScreenActionType.getScreenListEhrExportVisitRecordsRequest, doScreenListEhrExportVisitRecordsRequest)
}

function* watchScreenListEhrExportFinalResultRequest() {
    yield takeLatest(EScreenActionType.ehrExportFinalResultRequest, doScreenListEhrExportFinalResultRequest);
}

const screenSagas = [
    fork(watchListItemReport),
    fork(watchScreenListChange),
    fork(watchScreenListGPRAPeriod),
    fork(watchAutoUpdateScreenList),
    fork(watchChangeScreenTableSort),
    fork(watchScreenListLocationList),
    fork(watchScreenListFilterChanges),
    fork(watchAutoUpdateScreenListRecall),
    fork(watchTriggerScreenListSearchBar),
    fork(watchTriggerScreenListClearSearchBar),
    fork(watchScreenListEhrExportPatientInfoRequest),
    fork(watchScreenListEhrExportPatientInfoSavingRequest),
    fork(watchScreenListEhrExportVisitRecordsRequest),
    fork(watchScreenListEhrExportFinalResultRequest),
];

export default screenSagas;