import { call, fork, put, takeLatest, select, delay, all } from 'redux-saga/effects';
import { EEhrLoginActions, IActionPayload } from '../../actions';
import { ehrLoginCreateRequestError, ehrLoginCreateRequestStart, getEhrLoginListRequest, getEhrLoginListRequestError, getEhrLoginListRequestStart, getEhrLoginListRequestSuccess, IEhrLoginResponseItem } from '../../actions/ehr-login';
import getEhrLoginList from '../../api/calls/get-ehr-login-list';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess } from '../../actions/settings';
import { getEhrLoginAccessCodeSelector, getEhrLoginExpireOnSelector, getEhrLoginVerifyCodeSelector } from 'selectors/ehr-login';
import postEhrLoginCreate from 'api/calls/post-create-ehr-login';
import deleteEhrLogin from 'api/calls/delete-ehr-login';


export const MAXIMUM_RECORDS_PER_PAGE = 20;

function* getGetEhrLoginListRequest() {
    try {
        yield put(getEhrLoginListRequestStart());
        const response: IEhrLoginResponseItem[] = yield call(getEhrLoginList);
        yield put(getEhrLoginListRequestSuccess(response));
    } catch(e) {
        yield put(getEhrLoginListRequestError());
        yield put(notifyError(e.response.data.Errors[0]));
    }
}

function* doEhrLoginCreateRequest() {
    try {
        yield put(ehrLoginCreateRequestStart());
        const AccessCode: string | null = yield select(getEhrLoginAccessCodeSelector);
        const VerifyCode: string | null = yield select(getEhrLoginVerifyCodeSelector);
        const ExpireAt: string = yield select(getEhrLoginExpireOnSelector);

        const response: string = yield call(postEhrLoginCreate, {
            AccessCode,
            VerifyCode,
            ExpireAt
        });
        yield put(getEhrLoginListRequest());
        yield put(closeModalWindow(EModalWindowKeys.ehrLoginCreate));
        yield put(notifySuccess('EHR Login has been created successfully.'));

    } catch(e) {
        yield put(ehrLoginCreateRequestError());
        const errors = e.response.data.Errors; 
        yield all(errors && errors.map((error: string) => put(notifyError(error))));
    }
}

function* doEhrLoginDeleteRequest(action: IActionPayload) {
    try {
        const { id } = action.payload;
        const response: string = yield call(deleteEhrLogin, id);
        yield put(getEhrLoginListRequest());
        yield put(closeModalWindow(EModalWindowKeys.ehrLoginDelete));
        yield put(notifySuccess('EHR Login has been deleted successfully.'));
    } catch(e) {   
        yield put(notifyError(e.response.data.Errors[0]))
    }
}


function* watchGetEhrLoginListRequest() {
    yield takeLatest(EEhrLoginActions.getEhrLoginListRequest, getGetEhrLoginListRequest);
}

function* watchEhrLoginCreateRequest() {
    yield takeLatest(EEhrLoginActions.ehrLoginCreateRequest, doEhrLoginCreateRequest);
}

function* watchEhrLoginDeleteRequest() {
    yield takeLatest(EEhrLoginActions.ehrLoginDeleteRequest, doEhrLoginDeleteRequest);
}

const ehrLoginSagas = [
    fork(watchGetEhrLoginListRequest),
    fork(watchEhrLoginCreateRequest),
    fork(watchEhrLoginDeleteRequest),
];

export default ehrLoginSagas;