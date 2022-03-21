import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { EAutoExportLogsActions, IActionPayload } from '../../actions';
import { getAutoExportLogsRequest, getAutoExportLogsRequestError, getAutoExportLogsRequestStart, getAutoExportLogsRequestSuccess, IAutoExportLogsResponseItem, IAutoExportLogsStatisticsItem, setAutoExportLogsCurrentPage, setAutoExportLogsStatistics, setAutoExportLogsTotals } from '../../actions/auto-export-logs';
import postAutoExportLogs from '../../api/calls/post-auto-export-logs';
import postAutoExportLogsStatistics from 'api/calls/post-auto-export-statistics';
import { getAutoExportLogsCurrentPageSelector, getAutoExportLogsEndDateSelector, getAutoExportLogsFilterNameSelector, getAutoExportLogsStartDateSelector } from '../../selectors/auto-export-logs';
import { notifyError } from 'actions/settings';


export const MAXIMUM_RECORDS_PER_PAGE = 20;


function* doGetAutoExportLogsRequest() {
    try {
        yield put(getAutoExportLogsRequestStart());
        const currentPage: number = yield select(getAutoExportLogsCurrentPageSelector);
        const stateDate: string = yield select(getAutoExportLogsStartDateSelector);
        const endDate: string = yield select(getAutoExportLogsEndDateSelector);
        const filterName: string = yield select(getAutoExportLogsFilterNameSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const requestBody = {
            StartDate: stateDate,
            EndDate: endDate,
            StartRowIndex: StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
            nameFilter: filterName
        };
       
        const response: Array<IAutoExportLogsResponseItem> = yield call(postAutoExportLogs, requestBody);
        const statisticsRes: IAutoExportLogsStatisticsItem = yield call(postAutoExportLogsStatistics, {
            "StartDate": stateDate,
            "EndDate": endDate
        })

        yield put(setAutoExportLogsStatistics(statisticsRes));
        yield put(setAutoExportLogsTotals(Math.ceil(response.length / MAXIMUM_RECORDS_PER_PAGE)));
        yield put(getAutoExportLogsRequestSuccess(
            response
        ));
    } catch (error) {
        yield put(getAutoExportLogsRequestError());
        yield put(notifyError('something went wrong!'))
    }   
}

function* doChangeCurrentPageRequest(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    yield put(setAutoExportLogsCurrentPage(page));
    yield put(getAutoExportLogsRequest());
}

function* watchGetAutoExportLogsRequest() {
    yield takeLatest(EAutoExportLogsActions.getAutoExportLogsRequest, doGetAutoExportLogsRequest);
}


function* watchChangeCurrentPageRequest() {
    yield takeLatest(EAutoExportLogsActions.changeAutoExportLogsCurrentPageRequest, doChangeCurrentPageRequest);
}

const autoExportLogsSagas = [
    fork(watchGetAutoExportLogsRequest),
    fork(watchChangeCurrentPageRequest),
];

export default autoExportLogsSagas;