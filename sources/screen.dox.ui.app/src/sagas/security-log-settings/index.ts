import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { IActionPayload,  ESecurityLogSettingsActions } from '../../actions';
import getSecurityLogSettingsItems from '../../api/calls/get-security-log-settings-items';
import getSecurityLogSettingsCategory from '../../api/calls/get-security-log-settings-category';
import { ISecurityLogSettingsCategory, ISecurityLogSettingsItem, getSecurityLogSettingsRequestStart, getSecurityLogSettingsRequestError, getSecurityLogSettingsRequestSuccess } from '../../actions/security-log-settings';
import { notifyError, notifySuccess } from 'actions/settings';
import { getSecurityLogSettingsItemsSelector } from 'selectors/security-log-settings';
import updateSecurityLogSettings from 'api/calls/post-security-log-settings-update';


function* doGetSecurityLogSettingsRequest() {
    try {
        yield put(getSecurityLogSettingsRequestStart());
        const responseItems: Array<ISecurityLogSettingsItem> = yield call(getSecurityLogSettingsItems);
        const responseCategory: Array<ISecurityLogSettingsCategory> = yield call(getSecurityLogSettingsCategory);
        yield put(getSecurityLogSettingsRequestSuccess(
            responseItems,
            responseCategory
        ))
    } catch(e) {
        yield put(getSecurityLogSettingsRequestError())
    }
}

function* doUpdateSecurityLogSettingsItems() {
    try {
        yield put(getSecurityLogSettingsRequestStart());
        
        const securityLogSettingsItems: Array<ISecurityLogSettingsItem> = yield select(getSecurityLogSettingsItemsSelector);
        const responseCategory: Array<ISecurityLogSettingsCategory> = yield call(getSecurityLogSettingsCategory);
        
        const response: Array<ISecurityLogSettingsItem> = yield call(updateSecurityLogSettings, {
            Items: securityLogSettingsItems
        });
       
        yield put(getSecurityLogSettingsRequestSuccess(
            response,
            responseCategory
        ));
        
        yield put(notifySuccess('Updated Successfully.'));

    } catch(e) {
        yield put(notifyError('Please try again.'));
    }
}

function* watchUpdateSecurityLogSettingsRequest() {
    yield takeLatest(ESecurityLogSettingsActions.updateSecurityLogSettingsItemsRequest, doUpdateSecurityLogSettingsItems);
}

function* watchGetSecurityLogSettingsRequest() {
    yield takeLatest(ESecurityLogSettingsActions.getSecurityLogSettingsRequest, doGetSecurityLogSettingsRequest);
}

const securityLogSettingsSagas = [
    fork(watchGetSecurityLogSettingsRequest),
    fork(watchUpdateSecurityLogSettingsRequest)
];

export default securityLogSettingsSagas;