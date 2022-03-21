import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { 
    getFollowUpActiveDirectionSelector, getFollowUpActivePageSelector, getFollowUpActiveSortKeySelector, 
    getFollowUpEndDateSelector, getFollowUpFirstNameSelector, getFollowUpGrpaPeriodKeySelector, 
    getFollowUpGrpaPeriodsSelector, getFollowUpLastNameSelector, getFollowUpLocationsSelector, 
    getFollowUpOffsetSelector, getFollowUpScreeningResultIdSelector, getFollowUpSelectedLocationIdSelector, 
    getFollowUpStartDateSelector, getFollowUpBsrReportTypeSelector, isAutomatedFollowUpSelector
} from '../../selectors/follow-up';
import { EFollowUpActions, IActionPayload, ILocationItemResponse, TGPRAPeriodResponseItem } from '../../actions';

import { 
    changeFollowUpActiveSortDirection, changeFollowUpActiveSortKey, setCurrentFollowUpPage, IFollowUpResponseItem,
    postFilteredFollowUpsRequest, postFilteredFollowUpsRequestError, postFilteredFollowUpsRequestStart, 
    postFilteredFollowUpsRequestSuccess, resetFollowUpSearchParameters, setCurrentFollowUpOffset, getFollowUpRelatedReportRequestStart, 
    getFollowUpRelatedReportRequestError, IFollowUpRelatedReportResponse, getFollowUpRelatedReportRequestSuccess, IFollowUpRequest, getLocationListAction,
    getLocationListStartAction, getLocationListSuccessAction, getLocationListErrorAction, changeAutoUpdateStatus
} from '../../actions/follow-up';
import postAllFollowUp from '../../api/calls/post-all-follow-up';
import postFollowUpRelatedReport from '../../api/calls/post-follow-up-related-report';
import { notifyError } from '../../actions/settings';
import getLocationsListCall from '../../api/calls/get-locations';
import { setCreateDateCustomOrGPRASelector } from 'selectors/shared';

export const MAXIMUM_RECORDS_PER_PAGE = 20;

function* prepareFilterProps() {
    const type: number = yield select(getFollowUpBsrReportTypeSelector);
    const currentOffset: number = yield select(getFollowUpOffsetSelector);
    const sortKey: string = yield select(getFollowUpActiveSortKeySelector);
    const sortDirection: string = yield select(getFollowUpActiveDirectionSelector);
    const firstName: string = yield select(getFollowUpFirstNameSelector);
    const lastName: string = yield select(getFollowUpLastNameSelector);
    const screendoxResultId: number = yield select(getFollowUpScreeningResultIdSelector);
    const StartDate: string = yield select(getFollowUpStartDateSelector);
    const EndDate: string = yield select(getFollowUpEndDateSelector);
    const gpraPeriosKey: string = yield select(getFollowUpGrpaPeriodKeySelector);
    const gpraPerios: Array<TGPRAPeriodResponseItem> = yield select(getFollowUpGrpaPeriodsSelector);
    const locations: Array<ILocationItemResponse> = yield select(getFollowUpLocationsSelector);
    const locationId: number = yield select(getFollowUpSelectedLocationIdSelector);
    const isCreateDateCustom: boolean = yield select(setCreateDateCustomOrGPRASelector);
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
        }   
    }
    const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'LastFollowUpDate DESC';
    return {
        StartRowIndex: currentOffset,
        MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
        FirstName: firstName || null,
        LastName: lastName || null,
        ScreeningResultID: screendoxResultId || null,
        StartDate: StartDate || startDateGPRAPeriod || "2019-10-01",
        EndDate: EndDate || endDateGPRAPeriod || endDateStat,
        Location: location,
        ReportType: type,
        OrderBy
    };
}

function* doFollowUpLocationListRequest() {
    try {
        yield put(getLocationListStartAction());
        const locations: Array<ILocationItemResponse> = yield call(getLocationsListCall);
        yield put(getLocationListSuccessAction(locations));
    } catch (e) {
        yield put(getLocationListErrorAction());
    }
}



function* doFetchFollowUpsRequest() {
    try {
        yield put(postFilteredFollowUpsRequestStart());
        const currentPage: number = yield select(getFollowUpActivePageSelector);
        const body: IFollowUpRequest = yield call(prepareFilterProps);
        const followUpList: {
            Items: Array<IFollowUpResponseItem>;
            TotalCount: number;
        } = yield call(postAllFollowUp, body);
        yield put(postFilteredFollowUpsRequestSuccess({
            Items: followUpList.Items,
            TotalCount: Math.ceil(followUpList.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
            CurrentPage: currentPage,
            Offset: body.StartRowIndex || 0,
        }));
    } catch (e) {
        yield put(postFilteredFollowUpsRequestError());
    }
}

function* doChangeFollowUpTableSort(action: IActionPayload) {
    try {
        const { key, direction } = action.payload || {};
        yield put(setCurrentFollowUpOffset(0));
        yield put(setCurrentFollowUpPage(1));
        yield put(changeFollowUpActiveSortKey(key));
        yield put(changeFollowUpActiveSortDirection(key, direction));
        yield put(postFilteredFollowUpsRequest());
    } catch (e) {
        yield put(notifyError(`Failed to sort`));
    }
}

function* doChangeFollowUpTablePageSort(action: IActionPayload) {
    try {
        const { page } = action.payload || {};
        yield put(setCurrentFollowUpOffset(0));
        yield put(setCurrentFollowUpPage(page));
        yield put(postFilteredFollowUpsRequest());
    } catch (e) {
        yield put(notifyError(`Failed move page`));
    }
}

function* doResetFollowUpSearchParametersRequest() {
    yield put(resetFollowUpSearchParameters());
    yield put(postFilteredFollowUpsRequest());
}

function* doFollowUpRelatedReportRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        const body: IFollowUpRequest = yield call(prepareFilterProps);
        yield put(getFollowUpRelatedReportRequestStart());
        const result: Array<IFollowUpRelatedReportResponse> = yield call(postFollowUpRelatedReport, id, body);
        yield put(getFollowUpRelatedReportRequestSuccess(result, id));
    } catch (e) {
        yield put(getFollowUpRelatedReportRequestError());
        yield put(notifyError(`Failed to load Visit by `));
    }
}

function* doFollowUpAutoChangeUpdateRequest() {
    yield put(changeAutoUpdateStatus());
    let isAutomated: boolean = yield select(isAutomatedFollowUpSelector);
    while (isAutomated) {
        yield delay(60000);
        yield call(doFetchFollowUpsRequest);
    } 
}

function* watchFetchFollowUpsRequest() {
    yield takeLatest(EFollowUpActions.postAllFollowUpRequest, doFetchFollowUpsRequest);
}
function* watchChangeFollowUpTableSort() {
    yield takeLatest(EFollowUpActions.changeActiveFollowUpSort, doChangeFollowUpTableSort);
}

function* watchChangeFollowUpTablePageSort() {
    yield takeLatest(EFollowUpActions.setFollowUpPageChangeRequest, doChangeFollowUpTablePageSort);
}

function* watchResetFollowUpSearchParametersRequest() {
    yield takeLatest(EFollowUpActions.resetFollowUpSearchParametersRequest, doResetFollowUpSearchParametersRequest);
}

function* watchFollowUpRelatedReportRequest() {
    yield takeLatest(EFollowUpActions.getRelatedReportByIdRequest, doFollowUpRelatedReportRequest);
}

function* watchFollowUpLocationList() {
    yield takeLatest(EFollowUpActions.getLocationListRequest, doFollowUpLocationListRequest);
}

function* watchFollowUpAutoChangeUpdateRequest() {
    yield takeLatest(EFollowUpActions.changeAutoUpdateStatusRequest, doFollowUpAutoChangeUpdateRequest);
}
const followUpSagas = [
    fork(watchFetchFollowUpsRequest),
    fork(watchChangeFollowUpTableSort),
    fork(watchChangeFollowUpTablePageSort),
    fork(watchResetFollowUpSearchParametersRequest),
    fork(watchFollowUpRelatedReportRequest),
    fork(watchFollowUpLocationList),
    fork(watchFollowUpAutoChangeUpdateRequest)
];

export default followUpSagas;