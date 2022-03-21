import { call, fork, put, takeLatest } from 'redux-saga/effects';
import { EDashboardActionType } from '../../actions';
import { 
    getSystemSettingsError, getSystemSettingsStart, getSystemSettingsSuccess, ISystemSettings,
} from '../../actions/dashboard';
import getSystemSettings from '../../api/calls/get-system-settings';
import { switchOffLoading, switchOnLoading } from '../../actions/settings'

function* doSystemSettingsChange() {
    try {
        yield put(switchOnLoading());
        yield put(getSystemSettingsStart());
        const systemSettings: ISystemSettings = yield call(getSystemSettings);
        yield put(getSystemSettingsSuccess(systemSettings));
        yield put(switchOffLoading());
    } catch (e) {
        yield put(getSystemSettingsError());
        yield put(switchOffLoading());
    }
}

function* watchSystemSettingsChange() {
    yield takeLatest(EDashboardActionType.getSystemSettingsRequest, doSystemSettingsChange);
}

const dashboardSagas = [
    fork(watchSystemSettingsChange)
];

export default dashboardSagas;