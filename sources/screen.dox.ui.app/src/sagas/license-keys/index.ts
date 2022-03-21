import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { ELicenseKeysActions, IActionPayload } from '../../actions';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from '../../actions/settings';
import { getLicenseKeysRequest, getLicenseKeysRequestStart, getLicenseKeysRequestSuccess, ILicenseKeysResponseItem, setLicenseActivationKey, setLicenseKey, setLicenseKeyCreateLoading, setLicenseKeysSystemSettingsSummary } from '../../actions/license-keys';
import getLicenseKeys from '../../api/calls/get-license-keys-list';
import deleteLicenseKey from 'api/calls/delete-license-key-by-id';
import { getLicenseKeySelector } from 'selectors/license-keys';
import createLicenseKey from 'api/calls/post-license-key-create';
import {  ISystemSettings } from '../../actions/dashboard';
import getSystemSettings from '../../api/calls/get-system-settings';


export const MAXIMUM_RECORDS_PER_PAGE = 20;

function* doGetLicenseKeysRequest() {
    try {
        yield put(getLicenseKeysRequestStart());
        const systemSettings: ISystemSettings = yield call(getSystemSettings);
        yield put(setLicenseKeysSystemSettingsSummary(systemSettings.Summary));
        const response: Array<ILicenseKeysResponseItem> = yield call(getLicenseKeys);
        yield put(getLicenseKeysRequestSuccess(response));
    } catch(e) {
        yield put(notifyError('Access Denied.'));
    }
}

function* doDeleteLicenseKeyRequest() {
    try {
        const licenseKey: string = yield select(getLicenseKeySelector);
        const response: string = yield call(deleteLicenseKey, {
            LicenseKey: licenseKey
        })
        yield put(getLicenseKeysRequest());
        yield put(closeModalWindow(EModalWindowKeys.licenseKeysDelete));
        yield put(notifySuccess('Deleted Successfully.'))
    } catch(e) {
        yield put(notifyError('Something went wrong.'));
    }
}

function* doCreateLicenseKeyRequest() {
    try {
        yield put(setLicenseKeyCreateLoading(true));
        const licenseKey: string = yield select(getLicenseKeySelector);
        if(!licenseKey) {
            yield put(notifyError('License key must not be empty.'));
            return;
        }
        const res: { LicenseKey: string } = yield call(createLicenseKey, {
            LicenseKey: licenseKey
        })
        
        yield put(getLicenseKeysRequest());
        yield put(closeModalWindow(EModalWindowKeys.licenseKeysCreate));
        yield put(notifySuccess('Created Successfully.'));
        yield put(setLicenseKeyCreateLoading(false));
    } catch(e) {
        yield put(notifyError('You have entered invalid license key.'));
        yield put(setLicenseKeyCreateLoading(false));
    }
}

function* doGetLicenseKeyDetailRequest(action: IActionPayload) {
    try {
        const { licenseKey } = action.payload;
        yield put(setLicenseKeyCreateLoading(true));
        yield put(setLicenseKey(licenseKey));
        const response: { LicenseString: string, ActivationRequestString: string } = yield call(createLicenseKey, {
            LicenseKey: licenseKey 
        });
        yield put(setLicenseActivationKey(response.ActivationRequestString));
        yield put(openModalWindow(EModalWindowKeys.licenseKeysActivate));
        yield put(setLicenseKeyCreateLoading(false));
        yield put(getLicenseKeysRequest());
    } catch(e) {
        yield put(closeModalWindow(EModalWindowKeys.licenseKeysActivate));
        yield put(notifyError('something went wrong.'));
        yield put(setLicenseKeyCreateLoading(false));
    }
}

function* doActivateLicenseKeyRequest(action: IActionPayload) {
    try {
        const { activationKey } = action.payload;
        if(!activationKey) {
            yield put(notifyError('Activation key field is requried.'));
            return
        }
        yield put(setLicenseKeyCreateLoading(true));
        const licenseKey: string = yield select(getLicenseKeySelector);
        const response: Array<ILicenseKeysResponseItem> = yield call(createLicenseKey, {
            LicenseKey: licenseKey,
            LicenseActivationKey: activationKey
        });
        yield put(closeModalWindow(EModalWindowKeys.licenseKeysActivate));
        yield put(notifySuccess('Created Successfully.'));
        yield put(setLicenseKeyCreateLoading(false));
        yield put(getLicenseKeysRequest())
    } catch(e) {
        yield put(notifyError('You have entered an invalid activation key. Please ensure that you have entered the activation key correctly.'));
        yield put(setLicenseKeyCreateLoading(false));
    }
}

function* watchLicenseKeysRequest() {
    yield takeLatest(ELicenseKeysActions.getLicenseKeysRequest, doGetLicenseKeysRequest);
}

function* watchDeleteLicenseKeyRequest() {
    yield takeLatest(ELicenseKeysActions.deleteLicenseKeyRequest, doDeleteLicenseKeyRequest);
}

function* watchCreateLicenseKeyRequest() {
    yield takeLatest(ELicenseKeysActions.createLicenseKeyRequest, doCreateLicenseKeyRequest);
}

function* watchGetLicenseKeyDetailRequest() {
    yield takeLatest(ELicenseKeysActions.getLicenseKeyDetailRequest, doGetLicenseKeyDetailRequest);
}

function* watchActivationLicenseKeyRequest() {
    yield takeLatest(ELicenseKeysActions.activeLicenseKeyRequest, doActivateLicenseKeyRequest);
}

const licenseKeysSagas = [
    fork(watchLicenseKeysRequest),
    fork(watchDeleteLicenseKeyRequest),
    fork(watchCreateLicenseKeyRequest),
    fork(watchGetLicenseKeyDetailRequest),
    fork(watchActivationLicenseKeyRequest),
];

export default licenseKeysSagas;