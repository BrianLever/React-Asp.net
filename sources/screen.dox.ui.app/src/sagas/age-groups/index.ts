import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { IActionPayload,  EAgeGroupsActions } from '../../actions';
import getAgeGroups from '../../api/calls/get-age-groups';
import updateAgeGroups from '../../api/calls/post-age-groups-update';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from '../../actions/settings';
import { ageGroupsResponseItem, ageGroupValueItem, getAgeGroupRequestError, getAgeGroupRequestStart, getAgeGroupRequestSuccess, updateAgeGroupRequestError, updateAgeGroupRequestStart, updateAgeGroupRequestSuccess } from '../../actions/age-groups';
import { getAgeGroupsValueSelector } from 'selectors/age-groups';


function* doGetAgeGroupsRequest() {
    try {
        yield put(getAgeGroupRequestStart());
        const response: ageGroupsResponseItem = yield call(getAgeGroups);
        yield put(getAgeGroupRequestSuccess(response));
    } catch(e) {
        yield put(getAgeGroupRequestError())
        yield put(notifyError('Something went wrong, Please try again.'));
    }
}

function* doUpdateAgeGroupsRequest() {
    try {
        const value:  ageGroupValueItem = yield select(getAgeGroupsValueSelector);
        var patt = new RegExp(value.RegExp);
        var res = patt.test(value.Value);
        if(value.Value == '') {
            yield put(notifyError('Age groups value is required'))
            return;
        }
        if(!res) {
            yield put(notifyError(value.Description))
            return;
        }
        yield put(updateAgeGroupRequestStart());
        const response: ageGroupsResponseItem = yield call(updateAgeGroups, { Value: value });
        yield put(updateAgeGroupRequestSuccess(response));
        yield put(notifySuccess('Age groups have been updated successfully.'));
    } catch(e) {
        yield put(updateAgeGroupRequestError());
        yield put(notifyError('Age groups should follow the sample format: 0 - 9;10 - 11;12 - 17;18 - 24;25 - 54;55 or Older'));
    }
}

function* watchGetAgeGroupsRequest() {
    yield takeLatest(EAgeGroupsActions.getAgeGroupsRequest, doGetAgeGroupsRequest)
}

function* watchUpdateAgeGroupsRequest() {
    yield takeLatest(EAgeGroupsActions.updateAgeGroupRequest, doUpdateAgeGroupsRequest);
}

const ageGroupsSagas = [
    fork(watchGetAgeGroupsRequest),
    fork(watchUpdateAgeGroupsRequest)
];

export default ageGroupsSagas;