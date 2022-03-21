import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { ELoginActions, IActionPayload } from '../../actions';
import { IloginRequest, IloginResponse, loginRequestError, loginRequestStart, loginRequestSuccess, setLoginErrorList } from '../../actions/login';
import postLogin from '../../api/calls/post-login-request';
import { loginEmailSelector, loginPasswordSelector } from '../../selectors/login';
import { setToken } from 'helpers/auth';
import { notifyError, notifySuccess } from 'actions/settings';
import jwt_decode from "jwt-decode";
import { getProfileRequest } from 'actions/profile';
import postLogout  from 'api/calls/post-logout';
import { getToken } from 'helpers/auth';
import postRefreshTokenRequest from 'api/calls/post-refresh-token-request';
import { ERouterUrls } from 'router';
import * as config  from '../../config/app.json';

function* doLoginRequest(action: IActionPayload) {
    const { Username, Password } = action.payload;
    try {

        yield put(loginRequestStart())
        const response: IloginResponse = yield call(postLogin, {
            "Username": Username,
            "Password": Password
        })
        
        setToken('token',response.AccessToken.Token, 0);
        setToken('user', jwt_decode(response.AccessToken.Token), 0);
        setToken('refreshToken', response.RefreshToken, 0);
        setToken('expire', response.AccessToken.ExpiresIn, 0);

        yield put(loginRequestSuccess(response));
        
        window.location.href = config.BASEURL+ERouterUrls.DASHBOARD;

    } catch(error) {
        yield put(loginRequestError())
        yield put(setLoginErrorList(error?.response.data.Errors));
    }
}

function* doLogoutRequest() {
    try {
        const refreshToken = getToken('refreshToken');
        const response: string = yield call(postLogout, { RefreshToken: refreshToken });
    } catch(e) {
        console.log("[Logout] Failed to logout", e)
    }
}

function* doRefreshTokenRequest() {
    try {
       const refreshToken = getToken('refreshToken');

       // Verify that refreshToken is not null
        if(!refreshToken){
            console.log("[Login] Refresh token not found. Skip renewval process.");
            return;
        }

       const response: IloginResponse = yield call(postRefreshTokenRequest, { RefreshToken: refreshToken })
       const { AccessToken, RefreshToken } = response;
        setToken('token', AccessToken.Token, 0);
        setToken('user', jwt_decode(AccessToken.Token), 0);
        setToken('refreshToken', RefreshToken, 0);    
        setToken('expire', response.AccessToken.ExpiresIn, 0);
        console.log('[Login] Refreshed tokens. New value:', getToken('token'));
    } catch(e) {
        console.log(e);
    }
}

function* watchLoginRequest() {
    yield takeLatest(ELoginActions.loginRequest, doLoginRequest);
}

function* watchLogoutRequest() {
    yield takeLatest(ELoginActions.logoutRequest, doLogoutRequest);
}

function* watchRefreshTokenRequest() {
    yield takeLatest(ELoginActions.refreshTokenRequest, doRefreshTokenRequest)
}


const loginSagas = [
    fork(watchLoginRequest),
    fork(watchLogoutRequest),
    fork(watchRefreshTokenRequest)
];

export default loginSagas;