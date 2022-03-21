import { notifyError, notifySuccess } from 'actions/settings';
import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { EChangePasswordActions, IActionPayload } from '../../actions';
import putChangepassword from 'api/calls/put-change-password';


function* doChangePasswordRequest(action: IActionPayload) {
    try {
      const { CurrentPassword, NewPassword } = action.payload;
      var regex = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
      if(regex.test(NewPassword) == false){
        yield put(notifyError('Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters'))
        return 
      }

      if(CurrentPassword === NewPassword) {
          yield put(notifyError('New password has already been used. Please enter another.'));
          return;
      }

      const res: string = yield call(putChangepassword, { CurrentPassword: CurrentPassword, NewPassword: NewPassword });
      yield put(notifySuccess("Success!"))
    } catch(e) {
        yield put(notifyError('You have entered invalid password value.'));
    }
}

function* watchChangePasswordRequest() {
    yield takeLatest(EChangePasswordActions.changePasswordRequest, doChangePasswordRequest)
}

const changePasswordSagas = [
    fork(watchChangePasswordRequest),
];

export default changePasswordSagas;