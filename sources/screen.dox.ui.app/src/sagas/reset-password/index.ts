import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { EResetPasswordActions, IActionPayload } from '../../actions';
import { IResetPasswordGetSecurityQuestionResponse, resetPasswordGetSecurityQuestionRequestError, resetPasswordGetSecurityQuestionRequestStart, resetPasswordGetSecurityQuestionRequestSuccess, resetPasswordRequestError, resetPasswordRequestStart, resetPasswordRequestSuccess } from '../../actions/reset-password';
import getSecurityQuestion from '../../api/calls/get-security-question';
import postResetPassword from 'api/calls/post-reset-password';


function* doGetSecurityQuestionRequest(action: IActionPayload) {
    try {
        const { username } = action.payload;
        yield put(resetPasswordGetSecurityQuestionRequestStart())
        const response: IResetPasswordGetSecurityQuestionResponse = yield call(getSecurityQuestion, username);
        yield put(resetPasswordGetSecurityQuestionRequestSuccess(response));
    } catch(e) {
        yield put(resetPasswordGetSecurityQuestionRequestError());
    }
}

function* doResetPasswordRequest(action: IActionPayload) {
    try {
        yield put(resetPasswordRequestStart());
        const { username, ...other } = action.payload;
        const response: string = yield call(postResetPassword, username, {
            SecurityQuestionAnswer: other.SecurityQuestionAnswer,
            NewPassword: other.NewPassword
        })
        yield put(resetPasswordRequestSuccess());
    } catch(e) {
        yield put(resetPasswordRequestError(e?.response.data.Errors));
    }
}

function* watchGetSecurityQuestionRequest() {
    yield takeLatest(EResetPasswordActions.resetPasswordGetSecurityQuestionRequest, doGetSecurityQuestionRequest);
}

function* watchResetPasswordRequest() {
    yield takeLatest(EResetPasswordActions.resetPasswordRequest, doResetPasswordRequest)
}

const resetPasswordSagas = [
    fork(watchGetSecurityQuestionRequest),
    fork(watchResetPasswordRequest)
];

export default resetPasswordSagas;