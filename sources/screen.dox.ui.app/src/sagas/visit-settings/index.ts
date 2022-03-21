import { call, delay, fork, put, select, takeLatest, all } from 'redux-saga/effects';
import { getVisitSettingsListSelector } from 'selectors/visit-settings';
import { IActionPayload,  EAgeGroupsActions, EVisitSettingsActions } from '../../actions';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from '../../actions/settings';
import { VisitSettingsResponseItem, getVisitSettingsRequestSuccess, getVisitSettingsRequestStart, getVisitSettingsRequestError, updateVisitSettingsRequestStart, updateVisitSettingsRequestError } from '../../actions/visit-settings';
import getVisitSettings from '../../api/calls/get-visit-settings-list';
import updateVisitSettings from '../../api/calls/post-visit-settings-update';


function* doGetVisitSettingsRequest() {
    try {
        yield put(getVisitSettingsRequestStart())
        const response: Array<VisitSettingsResponseItem> = yield call(getVisitSettings);
        yield put(getVisitSettingsRequestSuccess(response));
    } catch(e) {
        yield put(getVisitSettingsRequestError())
        yield put(notifyError('Something went wrong, Please try again.'));
    }
}

function* doUpdateVisitSettingsRequest() {
    try {
        const visitSettings:  Array<VisitSettingsResponseItem> = yield select(getVisitSettingsListSelector);
        const validateList = visitSettings.filter((item, i) => {
            if(item.CutScore >= 1  && item.CutScore <= 30) {
            } else {
                return item;
            }
        })

        if(validateList.length !== 0) {
            yield all(
                validateList.map((d) => { 
                    return put(notifyError(`${d.Name}: 'Cut Score' field accepts values from 1 to 30. Default value is '1'.`))
                })
            );
            return;
        }
        
        yield put(updateVisitSettingsRequestStart());
        const response: Array<VisitSettingsResponseItem> = yield call(updateVisitSettings, { Items: visitSettings });
        yield put(getVisitSettingsRequestSuccess(response));
        yield put(notifySuccess('Visit Settings have been updated successfully.'));
    } catch(e) {
        yield put(updateVisitSettingsRequestError());
        yield put(notifyError('Please try again.'))
    }
}

function* watchGetVisitSettingsRequest() {
    yield takeLatest(EVisitSettingsActions.getVisitSettingsRequest, doGetVisitSettingsRequest)
}

function* watchUpdateVisitSettingsRequest() {
    yield takeLatest(EVisitSettingsActions.updateVisitSettingsRequest, doUpdateVisitSettingsRequest);
}

const visitSettingsSagas = [
    fork(watchGetVisitSettingsRequest),
    fork(watchUpdateVisitSettingsRequest)
];

export default visitSettingsSagas;