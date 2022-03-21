import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { EChangeSecurityQuestionActions, IActionPayload } from '../../actions';
import { changeSecurityQuestionUpdateRequestError, changeSecurityQuestionUpdateRequestStart, changeSecurityQuestionUpdateRequestSuccess, getSecurityQuestionListRequestSuccess } from '../../actions/change-security-question';
import getSecurityQuestions from '../../api/calls/get-security-questions';
import { notifyError, notifySuccess } from '../../actions/settings';
import putChangeSecurityQuestion from 'api/calls/put-change-security-question';


function* doChangeSecurityQuestionListRequest() {
    try {
        const res: Array<string> = yield call(getSecurityQuestions);
        yield put(getSecurityQuestionListRequestSuccess(res));
    } catch(e) {
        yield put(notifyError('Failed to get security question.'));
    }
}

function* doChangeSecurityQuestionUpdateRequest(action: IActionPayload) {
    try {
        yield put(changeSecurityQuestionUpdateRequestStart());
        const {  Password,  SecurityQuestion, SecurityQuestionAnswer } = action.payload;
        var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
        if(regex.test(Password) == false){
            yield put(notifyError('Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters'))
            return 
        }

        if(!SecurityQuestion) {
            yield put(notifyError('Security question is required'));
            return;
        }

        if(!SecurityQuestionAnswer) {
            yield (put(notifyError('Security answer is required.')));
            return;
        }

        const res: string = yield call(putChangeSecurityQuestion, {
            Password,
            SecurityQuestion,
            SecurityQuestionAnswer,
        })
        yield put(changeSecurityQuestionUpdateRequestSuccess());
        yield put(notifySuccess('Security Question has been updated.'));    
    } catch(e) {
        yield put(changeSecurityQuestionUpdateRequestError());
        yield put(notifyError('You have entered invalid password value.'));
    }
}

function* watchChangeSecurityQuestionListRequest() {
    yield takeLatest(EChangeSecurityQuestionActions.securityQuestionListRequest, doChangeSecurityQuestionListRequest);
}


function* watchChangeSecurityQuestionUpdateRequest() {
    yield takeLatest(EChangeSecurityQuestionActions.changeSecurityQuestionUpdateRequest, doChangeSecurityQuestionUpdateRequest)
}


const changeSecurityQuestionSagas = [
    fork(watchChangeSecurityQuestionListRequest),
    fork(watchChangeSecurityQuestionUpdateRequest),
];

export default changeSecurityQuestionSagas;