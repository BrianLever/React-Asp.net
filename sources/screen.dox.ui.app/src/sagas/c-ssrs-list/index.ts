import { call, fork, put, select, takeLatest, delay } from 'redux-saga/effects';
import {  ECssrsListActions, IActionPayload, ILocationItemResponse, TGPRAPeriodResponseItem  } from '../../actions';
import { MAXIMUM_RECORDS_PER_PAGE } from '../../states/c-ssrs-list';
import getLocationsListCall from '../../api/calls/get-locations';
import { switchOnLoading, switchOffLoading, notifyError } from '../../actions/settings';
import getGPRAPeriods from '../../api/calls/get-grpa-periods';
import { cssrsListchangeAutoUpdateStatus, getCssrsListRequest, getCssrsListRequestError, getCssrsListRequestStart, getCssrsListRequestSuccess, getCssrsLocationListActionRequestSuccess, getRelatedByIdCssrsRequestError, getRelatedByIdCssrsRequestStart, getRelatedByIdCssrsRequestSuccess, ICssrsListRequest, ICssrsListResponse, ICssrsVisitResponseItem, setCssrsListCurrentPage, setCssrsListOffset, setCssrsListSortDirection, setCssrsListSortKey } from 'actions/c-ssrs-list';
import {  getCssrsListCurrentPageSelector, getCssrsListEndDateSelector, getCssrsListFirstNameSelector, getCssrsListGpraPeriodKeySelector, getCssrsListGpraPeriodsSelector, getCssrsListLastNameSelector, getCssrsListLocationsSelector, getCssrsListReportTypeSelector, getCssrsListSortDirectionSelector, getCssrsListSortKeySelector, getCssrsListStartDateSelector, getCssrsListLocationIdSelector, getCssrsListSelector, getCssrsListAutoUpdateSelector, getCssrsListDateofBirthSelector, getCssrsListOffsetSelector } from 'selectors/c-ssrs-list';
import postCssrsList from 'api/calls/post-cssrs-list';
import { IVisitResponseItem } from 'actions/visit';
import postCssrsRelatedVisitById from 'api/calls/post-cssrs-related-visit-by-id';


function* prepareFilterProperties() {
    const currentPage: number = yield select(getCssrsListCurrentPageSelector);
    const sortKey: string = yield select(getCssrsListSortKeySelector);
    const sortDirection: string = yield select(getCssrsListSortDirectionSelector);
    const firstName: string = yield select(getCssrsListFirstNameSelector);
    const lastName: string = yield select(getCssrsListLastNameSelector);  
    const dateofBirth: string=yield select(getCssrsListDateofBirthSelector)
    const StartDate: string = yield select(getCssrsListStartDateSelector);
    const EndDate: string = yield select(getCssrsListEndDateSelector);
    const gpraPeriodsKey: string = yield select(getCssrsListGpraPeriodKeySelector);
    const gpraPeriods: Array<TGPRAPeriodResponseItem> = yield select(getCssrsListGpraPeriodsSelector);   
    const locations: Array<ILocationItemResponse> = yield select(getCssrsListLocationsSelector);
    const locationId: number = yield select(getCssrsListLocationIdSelector);
    const loc = locations.find(l => l.ID === locationId);
    const currentOffset: number = yield select(getCssrsListOffsetSelector);
    let location = null;
    if (locationId && (locationId > 0) && loc) {
        location = loc.ID;
    }
    let startDateGPRAPeriod;
    let endDateGPRAPeriod;
    const endDateStat = new Date().toISOString();
    if (gpraPeriodsKey) {
        const gpraPeriods: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods); 
        const periodObject = gpraPeriods.find(p => p.Label === gpraPeriodsKey);
        startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
        endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
    }
    //  else {        
    //     const list: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
    //     if (Array.isArray(list)) {
    //         const periodObject = list[0];
    //         startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
    //         endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
    //     }
    // }
    const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'LastCreatedDate desc';
    const body: ICssrsListRequest = { 
        StartRowIndex: currentOffset,
        MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
        FirstName: firstName || null,
        LastName: lastName || null,
        DateofBirth:dateofBirth || null,
        StartDate: startDateGPRAPeriod || StartDate || "2020-10-01",
        EndDate: endDateGPRAPeriod || EndDate || endDateStat,
        Location: location || null,
        OrderBy: OrderBy,
        // ReportType: ReportType,
    };
    return body;
}

function* doCssrsListRequest() {
    try {
        yield put(getCssrsListRequestStart());
        const body: ICssrsListRequest = yield call(prepareFilterProperties);
        const response: { Items: Array<ICssrsListResponse>, TotalCount: number } = yield call(postCssrsList, body);
        yield put(getCssrsListRequestSuccess({
            Items: response.Items,
            TotalCount: Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE),}));
    } catch(e) {
        yield put(getCssrsListRequestError());
        yield put(notifyError('Something went wrong.'));
    }
}

function* doCssrsListChangeSortRequest(action: IActionPayload) {
    const { key, direction } = action.payload || {};
    try {
        yield put(setCssrsListCurrentPage(1));
        yield put(setCssrsListSortKey(key));
        yield put(setCssrsListSortDirection(direction));
        yield put(getCssrsListRequest());
    } catch (e) {
        yield put(notifyError(`Visit sort by ${key} direction ${direction} failed`));
    }
}

function* doChangeAutoUpdateStatusRequest() {
    yield put(cssrsListchangeAutoUpdateStatus());
    let isAutomated: boolean = yield select(getCssrsListAutoUpdateSelector);
    while (isAutomated) {
        yield delay(60000);
        yield put(getCssrsListRequest());
    } 
}


function* doCssrsListCurrentPageRequest(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    yield put(setCssrsListCurrentPage(page));
    const offset = (page < 2) ? 0 : ((page - 1) * MAXIMUM_RECORDS_PER_PAGE);   
    yield put(setCssrsListOffset(offset));
    yield put(getCssrsListRequest());
}

function* doCssrsIdRelatedRequest(action: IActionPayload) {
    const { id } = action.payload || {};
    try {
        yield put(getRelatedByIdCssrsRequestStart());
        const body: ICssrsListRequest = yield call(prepareFilterProperties);
        const reports: Array<ICssrsVisitResponseItem> = yield call(postCssrsRelatedVisitById, id, body);
        yield put(getRelatedByIdCssrsRequestSuccess(reports));
    } catch (e) {
        yield put(getRelatedByIdCssrsRequestError());
        yield put(notifyError(`Failed to load Visit by id ${id}`));
    }
}


    //Location list
function* doCssrsLocationListRequest() {
     try {
         const locations: Array<ILocationItemResponse> = yield call(getLocationsListCall);
        yield put(getCssrsLocationListActionRequestSuccess(locations));
    } catch (e) {
        console.log(e);
    }
}
       //Location List
function* watchFetchCssrsLocationsRequest() {
    yield takeLatest(ECssrsListActions.getCssrsLocationsListRequest, doCssrsLocationListRequest)
}

function* watchCssrsListRequest() {
    yield takeLatest(ECssrsListActions.getCssrsListRequest, doCssrsListRequest)
}

function* watchCssrsListChangeSortRequest() {
    yield takeLatest(ECssrsListActions.cssrsListSortRequest, doCssrsListChangeSortRequest);
}

function* watchChangeAutoUpdateStatusRequest() {
    yield takeLatest(ECssrsListActions.cssrsListchangeAutoUpdateStatusRequest, doChangeAutoUpdateStatusRequest)
}

function* watchCssrsListCurrentPageRequest() {
    yield takeLatest(ECssrsListActions.cssrsListCurrentPageRequest, doCssrsListCurrentPageRequest)
}

function* watchCssrsIdRelatedRequest() {
    yield takeLatest(ECssrsListActions.getRelatedByIdCssrsRequest, doCssrsIdRelatedRequest);
}

const cssrsListSagas = [
    fork(watchCssrsListRequest),
    fork(watchCssrsListChangeSortRequest),
    fork(watchChangeAutoUpdateStatusRequest),
    fork(watchCssrsListCurrentPageRequest),
    fork(watchCssrsIdRelatedRequest),
    fork(watchFetchCssrsLocationsRequest),
];

export default cssrsListSagas;
