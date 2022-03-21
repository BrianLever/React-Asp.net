import { all, put, delay } from 'redux-saga/effects';
import systemSettingsSags from './dashboard';
import screenSagas from './screen';
import profileSagas from './profile';
import screenReportSagas from './screen/report';
import visitSagas from './visit';
import { getProfileRequest } from '../actions/profile';
import visitReportSagas from './visit/report';
import visitDemographicReportSagas from './visit/demographic-report';
import followUpSagas from './follow-up';
import reportsSagas from './reports';
import followUpReportSagas from './follow-up/report';
import manageDevicesSagas from './manage-devices';
import branchLocationsSagas from './branch-locations';
import sharedSagas from './shared';
import screenProfilesSagas from './screen-profiles';
import ageGroupsSagas from './age-groups';
import visitSettingsSagas from './visit-settings';
import errorLogSagas from './error-log';
import securityLogSettingsSagas from './security-log-settings'; 
import licenseKeysSagas from './license-keys';
import loginSagas from './login';
import changePasswordSagas from './change-password';
import changeSecurityQuestionSagas from './change-security-question';
import autoExportLogsSagas from './auto-export-logs';
import findPatientAddressSagas from './find-patient-address';
import cssrsListSagas from './c-ssrs-list';
import resetPasswordSagas from './reset-password';
import cssrsReportSagas from './c-ssrs-list/c-ssrs-report';
import manageUsersSagas from './manage-users';
import ehrLoginSagas from './ehr-login';
import { getToken } from 'helpers/auth';
import { refreshTokenRequest } from 'actions/login';
import * as config from 'config/app.json';
import { ERouterUrls } from 'router';
import { getSystemSettingsRequest } from 'actions/dashboard';


const user = getToken('user');
const duration = getToken('expire');
const pathname = window.location.pathname;

export default function* rootSaga() {
    yield all([
        ...visitSagas,
        ...sharedSagas,
        ...screenSagas,
        ...profileSagas,
        ...followUpSagas,
        ...visitReportSagas,
        ...screenReportSagas,
        ...systemSettingsSags,
        ...manageDevicesSagas,
        ...followUpReportSagas,
        ...branchLocationsSagas,
        ...visitDemographicReportSagas,
        ...reportsSagas,
        ...screenProfilesSagas,
        ...ageGroupsSagas,
        ...visitSettingsSagas,
        ...errorLogSagas,
        ...securityLogSettingsSagas,
        ...licenseKeysSagas,
        ...loginSagas,
        ...changePasswordSagas,
        ...changeSecurityQuestionSagas,
        ...autoExportLogsSagas,
        ...findPatientAddressSagas,
        ...cssrsListSagas,
        ...resetPasswordSagas,
        ...cssrsReportSagas,
        ...manageUsersSagas,
        ...ehrLoginSagas
    ]);
    if(user) {
        yield put(getProfileRequest(Number(user.user_id)));
        yield put(getSystemSettingsRequest());
    }

    if(pathname !== ERouterUrls.LOGIN && pathname !== config.BASEURL+ERouterUrls.LOGIN && user) {
        while(true) {
            yield put(refreshTokenRequest())
            console.log("[Login] "+ (new Date()) + ': scheduled refresh token action')
            yield delay(duration * 1000 * config.REFRESH_TOKEN_DURATION_PERCENT);
        }
    }
}