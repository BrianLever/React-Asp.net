import { call, fork, put, select, takeLatest, delay } from 'redux-saga/effects';
import { 
    EVisitActions, IActionPayload, ILocationItemResponse, TGPRAPeriodResponseItem 
} from '../../actions';
import { 
    getAllVisitRequestStart, getAllVisitRequestError, getAllVisitRequestSuccess, 
    IVisitResponseItem, getVisitFilteredItemsStart, getVisitFilteredItemsSuccess,
    getVisitFilteredItemsError, IVisitRequest, setVisitCurrentPage, getVisitFilteredItems,
    setVisitListOffset, setVisitListSortKey, setVisitListSortDirection, changeAutoUpdateStatus,
    setVisitSearchLocationsSuccess, setVisitSearchLocationsStart, setVisitSearchLocationsError,
    cleanVisitSearchParameters, getRelatedByIdVisitRequestStart, getRelatedByIdVisitRequestError,
    getRelatedByIdVisitRequestSuccess, IVisitReportsResponse, setVisitScreeningID
} from '../../actions/visit';
import { 
    visitListCurrentPageSelector, visitListOffsetSelector, getVisitListActiveSortKeySelector,
    getVisitListActiveSortDirectionSelector, getVisitListFirstNameSelector, getVisitListLastNameSelector,
    getVisitListvisitdoxResultIdSelector, getVisitListStartDateSelector, getVisitListEndDateSelector,
    getVisitListGrpaPeriodKey, getVisitListGrpaPeriodList, getVisitListLocationList, 
    getVisitListSelectedLocationId, getBSRReportType, getVisitItemsSelector, isVisitListAutoUpdateSelector
} from '../../selectors/visit';
import { MAXIMUM_RECORDS_PER_PAGE } from '../../states/visit';
import postAllVisit from '../../api/calls/post-all-visits';
import getLocationsListCall from '../../api/calls/get-locations';
import postRelatedVisitById from '../../api/calls/post-related-visit-by-id';
import { switchOnLoading, switchOffLoading, notifyError } from '../../actions/settings';
import getGPRAPeriods from '../../api/calls/get-grpa-periods';
import { getScreenListGrpaPeriodList } from 'selectors/screen';
import { setCreateDateCustomOrGPRASelector } from 'selectors/shared';

function* doRequestVisitRequest() {
    try {
        yield put(getAllVisitRequestStart());
        const visitResult: {
            Items: Array<IVisitResponseItem>;
            TotalCount: number;
        } = yield call(postAllVisit, {});
        yield put(getAllVisitRequestSuccess({
            Items: visitResult.Items,
            TotalCount: Math.ceil(visitResult.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
        }));
    } catch (e) {
        yield put(getAllVisitRequestError());
    }
}

function* prepareFilterProperties() {
    const currentOffset: number = yield select(visitListOffsetSelector);
    const sortKey: string = yield select(getVisitListActiveSortKeySelector);
    const sortDirection: string = yield select(getVisitListActiveSortDirectionSelector);
    const firstName: string = yield select(getVisitListFirstNameSelector);
    const lastName: string = yield select(getVisitListLastNameSelector);
    const screendoxResultId: number = yield select(getVisitListvisitdoxResultIdSelector);
    const StartDate: string = yield select(getVisitListStartDateSelector);
    const EndDate: string = yield select(getVisitListEndDateSelector);
    const gpraPeriosKey: string = yield select(getVisitListGrpaPeriodKey);
    // const gpraPerios: Array<TGPRAPeriodResponseItem> = yield select(getVisitListGrpaPeriodList);
    const gpraPerios: Array<TGPRAPeriodResponseItem> = yield select(getScreenListGrpaPeriodList);
    const ReportType: number =  yield select(getBSRReportType);
    const isCreateDateCustom: boolean = yield select(setCreateDateCustomOrGPRASelector);
    const locations: Array<ILocationItemResponse> = yield select(getVisitListLocationList);
    const locationId: number = yield select(getVisitListSelectedLocationId);
    const loc = locations.find(l => l.ID === locationId);
    let location = null;
    if (locationId && (locationId > 0) && loc) {
        location = loc.ID;
    }
    let startDateGPRAPeriod;
    let endDateGPRAPeriod;
    const endDateStat = new Date().toISOString();
   
    if(isCreateDateCustom) {
        
    } else {
        if (gpraPeriosKey) {
            const periodObject = gpraPerios.find(p => p.Label === gpraPeriosKey);
            console.log(gpraPerios, periodObject, 'period object')
            startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
            endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
        } else {
            const listOfVisits: Array<IVisitResponseItem> = yield select(getVisitItemsSelector);
            const list: Array<TGPRAPeriodResponseItem> = yield call(getGPRAPeriods);
            
            if (Array.isArray(list) && list.length) {
                const periodObject = list[0];
                startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
                endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
            }
        }
    }

    const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'LastCreatedDate desc';

    const body: IVisitRequest = { 
        StartRowIndex: currentOffset,
        MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
        FirstName: firstName || null,
        LastName: lastName || null,
        ScreeningResultID: screendoxResultId || null,
        StartDate: startDateGPRAPeriod || StartDate,
        EndDate: endDateGPRAPeriod || EndDate || endDateStat,
        Location: location || null,
        OrderBy: OrderBy,
        ReportType: ReportType,
    };
    return body;
}

function* doVisitListFilterChange() {
    try {
        // yield put(switchOnLoading());
        // yield put(switchOffLoading());
        // yield put(getVisitFilteredItemsStart());
        const currentPage: number = yield select(visitListCurrentPageSelector);
        const currentOffset: number = yield select(visitListOffsetSelector);
        const body: IVisitRequest = yield call(prepareFilterProperties);
        console.log(body, "visit list search request body")
        const response: { 
            Items: IVisitResponseItem[];
            TotalCount: number;
        } = yield call(postAllVisit, body);
        
        if (!response) {
            return // error handler
        }
        yield put(getVisitFilteredItemsSuccess({
            Total: Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
            Items: response.Items,
            Offset: currentOffset,
            CurrentPage: currentPage,
        }));
        // yield put(switchOffLoading());
    } catch(e) {
        // yield put(switchOffLoading());
        yield put(getVisitFilteredItemsError());
        yield put(notifyError('something went wrong.'));
    }
}

function* doChangeAutoUpdateStatusRequest() {
    yield put(changeAutoUpdateStatus());
    let isAutomated: boolean = yield select(isVisitListAutoUpdateSelector);console.log(isAutomated)
    while (isAutomated) {
        yield delay(60000);
        yield put(getVisitFilteredItems());
    } 
}


function* doVisitCurrentPageChanges(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    // yield put(setVisitScreeningID(null));
    yield put(setVisitCurrentPage(page));
    const offset = (page < 2) ? 0 : ((page - 1) * MAXIMUM_RECORDS_PER_PAGE);
    yield put(setVisitListOffset(offset));
    yield put(getVisitFilteredItems());
}

function* doChangeVisitTableSort(action: IActionPayload) {
    const { key, direction } = action.payload || {};
    try {
        yield put(setVisitListOffset(0));
        yield put(setVisitCurrentPage(1));
        yield put(setVisitListSortKey(key));
        yield put(setVisitListSortDirection(direction));
        yield put(getVisitFilteredItems());
    } catch (e) {
        yield put(notifyError(`Visit sort by ${key} direction ${direction} failed`));
    }
}

function* doVisitListLocationListRequest() {
    try {
        yield put(setVisitSearchLocationsStart());
        const locations: Array<ILocationItemResponse> = yield call(getLocationsListCall);
        yield put(setVisitSearchLocationsSuccess(locations));
    } catch (e) {
        yield put(setVisitSearchLocationsError());
        yield put(notifyError('Failed to load locations please try later'));
    }
}


function* doVisitSearchRequest() {
    try {
        yield put(setVisitListOffset(0));
        yield put(setVisitCurrentPage(1));
        yield put(getVisitFilteredItems());
    } catch (e) {
        yield put(notifyError('Failed to change pages'));
    }
}


function* doClearScreenListSearchBar() {
    yield put(cleanVisitSearchParameters())
    yield put(getVisitFilteredItems());
}

function* doVisitIdRelatedReques(action: IActionPayload) {
    const { id } = action.payload || {};
    try {
        yield put(getRelatedByIdVisitRequestStart());
        const body: IVisitRequest = yield call(prepareFilterProperties);
        const reports: Array<IVisitReportsResponse> = yield call(postRelatedVisitById, id, body);
        if (!reports || !Array.isArray(reports) || !reports.length) {
            yield put(getRelatedByIdVisitRequestError());
        } else {
            yield put(getRelatedByIdVisitRequestSuccess(reports));
        }
    } catch (e) {
        yield put(getRelatedByIdVisitRequestError());
        yield put(notifyError(`Failed to load Visit by id ${id}`));
    }
}


function* watchRequestVisitRequest() {
    yield takeLatest(EVisitActions.getVisitRequest, doRequestVisitRequest);
}

function* watchVisitListFilterChanges() {
    yield takeLatest(EVisitActions.postVisitListFilterRequest, doVisitListFilterChange);
}

function* watchVisitCurrentPageChanges() {
    yield takeLatest(EVisitActions.requestVisitCurrentPage, doVisitCurrentPageChanges);
}

function* watchVisitListSortChanges() {
    yield takeLatest(EVisitActions.requestVisitListSort, doChangeVisitTableSort);
}

function* watchVisitSearchLocationRequest() {
    yield takeLatest(EVisitActions.getVisitLocationListRequest, doVisitListLocationListRequest);
}

function* watchVisitSearchRequest() {
    yield takeLatest(EVisitActions.triggerVisitSearchParameters, doVisitSearchRequest);
}

function* watchVisitSearchCleanRequest() {
    yield takeLatest(EVisitActions.requestTocleanVisitSearchParameters, doClearScreenListSearchBar);
}

function* watchVisitIdRelatedRequest() {
    yield takeLatest(EVisitActions.getRelatedVisitByIDRequest, doVisitIdRelatedReques);
}

function* changeAutoUpdateStatusRequest() {
    yield takeLatest(EVisitActions.changeAutoUpdateStatusRequest, doChangeAutoUpdateStatusRequest);
}

const visitSagas = [
    fork(watchVisitSearchRequest),
    fork(watchRequestVisitRequest),
    fork(watchVisitListSortChanges),
    fork(watchVisitIdRelatedRequest),
    fork(watchVisitListFilterChanges),
    fork(watchVisitCurrentPageChanges),
    fork(watchVisitSearchCleanRequest),
    fork(changeAutoUpdateStatusRequest),
    fork(watchVisitSearchLocationRequest),
];

export default visitSagas;
