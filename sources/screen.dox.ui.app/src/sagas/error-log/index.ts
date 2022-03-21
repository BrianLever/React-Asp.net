import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { EErrorLogActions, IActionPayload } from '../../actions';
import { getErrorLogByIdRequestError, getErrorLogByIdRequestStart, getErrorLogByIdRequestSuccess, getErrorLogRequest, getErrorLogRequestError, getErrorLogRequestStart, getErrorLogRequestSuccess, IErrorLogListResponseItem, setErrorLogCurrentPage } from '../../actions/error-log';
import { getErrorLogCurrentPageSelector, getErrorLogStartDateSelector, getErrorLogEndDateSelector} from '../../selectors/error-log';
import postErrorLog from '../../api/calls/post-error-log-list';
import getErrorLogItem from '../../api/calls/get-error-log-by-id';
import deleteErrorLogs from 'api/calls/delete-error-logs';
import { EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from 'actions/settings';
import printErrorLogs from 'api/calls/post-print-all-error-logs';
import { IExcelFileDownload, IPdfFileDownload } from 'api/axios';
import * as fileDownload from 'js-file-download';

export const MAXIMUM_RECORDS_PER_PAGE = 20;


function* doGetErrorLogRequest() {
    try {
        yield put(getErrorLogRequestStart());
        const currentPage: number = yield select(getErrorLogCurrentPageSelector);
        const stateDate: string = yield select(getErrorLogStartDateSelector);
        const endDate: string = yield select(getErrorLogEndDateSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const requestBody = {
            StartDate: stateDate,
            EndDate: endDate,
            StartRowIndex: StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE
        };
        const response: { Items: Array<IErrorLogListResponseItem>, TotalCount: number } = yield call(postErrorLog, requestBody);
        yield put(getErrorLogRequestSuccess(
            {
                Items: [...response.Items],
                TotalCount:  Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE)
            }
        ));
    } catch (error) {
        yield put(getErrorLogRequestError());
    }   
}


function* doChangeCurrentPageRequest(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    yield put(setErrorLogCurrentPage(page));
    yield put(getErrorLogRequest());
}

function* doGetErrorLogDetailRequest(action: IActionPayload) {
    try {
        yield put(openModalWindow(EModalWindowKeys.errorLogDetail));
        yield put(getErrorLogByIdRequestStart());
        const { id } = action.payload;
        const response: IErrorLogListResponseItem = yield call(getErrorLogItem, id);
        yield put(getErrorLogByIdRequestSuccess(response));       
    } catch (e) {
        yield put(getErrorLogByIdRequestError());
        yield put(notifyError('Please try again.'));
    }
}


function* doDeleteErrorLogsRequest() {
    try {
        const response: number = yield call(deleteErrorLogs);
        yield put(notifySuccess('Deleted Successfully.'));
        yield put(getErrorLogRequest());
    } catch(e) {
        yield put(notifyError('Please try again.'));
    }
}

function* doPrintErrorLogsExcelRequest() {
    try {
        const currentPage: number = yield select(getErrorLogCurrentPageSelector);
        const stateDate: string = yield select(getErrorLogStartDateSelector);
        const endDate: string = yield select(getErrorLogEndDateSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const requestBody = {
            StartDate: stateDate,
            EndDate: endDate,
            StartRowIndex: StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE
        };
        const response: IExcelFileDownload = yield call(printErrorLogs, requestBody);
        fileDownload.default(response.Data, response.Filename);
    } catch(e) {
        yield put(notifyError('Please try again.'));
    }
}

function* watchGetErrorLogRequest() {
    yield takeLatest(EErrorLogActions.getErrorLogRequest, doGetErrorLogRequest);
}

function* watchChangeCurrentPageRequest() {
    yield takeLatest(EErrorLogActions.changeErrorLogCurrentPageRequest, doChangeCurrentPageRequest);
}

function* watchGetErrorLogDetailRequest() {
    yield takeLatest(EErrorLogActions.getErrorLogByIdRequest, doGetErrorLogDetailRequest);
}

function* watchDeleteErrorLogsRequest() {
    yield takeLatest(EErrorLogActions.deleteErrorLogsRequest, doDeleteErrorLogsRequest);
}

function* watchPrintErrorLogsExcelRequest() {
    yield takeLatest(EErrorLogActions.printErrorLogsExcelRequest, doPrintErrorLogsExcelRequest);
}

const errorLogSagas = [
    fork(watchGetErrorLogRequest),
    fork(watchChangeCurrentPageRequest),
    fork(watchGetErrorLogDetailRequest),
    fork(watchDeleteErrorLogsRequest),
    fork(watchPrintErrorLogsExcelRequest),
];

export default errorLogSagas;