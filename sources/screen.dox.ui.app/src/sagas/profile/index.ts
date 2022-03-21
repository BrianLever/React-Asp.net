import { call, fork, put, takeLatest, select } from 'redux-saga/effects';
import { getProfileSelector } from 'selectors/profile';
import { EUserProfileActionType, IActionPayload } from '../../actions';
import { 
    getProfileRequestError, getProfileRequestStart, getProfileRequestSuccess, IProfileResponse, updateProfileRequest, updateProfileRequestStart,updateProfileRequestError,
} from '../../actions/profile';
import getProfile from '../../api/calls/get-profile';
import userProfileUpdate from 'api/calls/put-profile-update';
import { notifySuccess, notifyError  } from 'actions/settings';

function* doFetchProfile(action: IActionPayload) {
    try {
        const { clientCode } = action.payload || {};
        yield put(getProfileRequestStart());
        const profile: IProfileResponse = yield call(getProfile, clientCode);
        yield put(getProfileRequestSuccess({ profile }));
    } catch (e) {
        yield put(getProfileRequestError());
    }
}

function* doUpdateProfileRequest() {
    try {
         yield put(updateProfileRequestStart());
         const profileInfo: IProfileResponse = yield select(getProfileSelector);
         if(profileInfo) {
            const response: string = yield call(userProfileUpdate, { 
                FirstName: profileInfo?.FirstName,
                LastName: profileInfo?.LastName,
                MiddleName: profileInfo?.MiddleName,
                ContactPhone: profileInfo?.ContactPhone,
                StateCode: profileInfo?.StateCode,
                City: profileInfo?.City,
                AddressLine1: profileInfo?.AddressLine1,
                AddressLine2: profileInfo?.AddressLine2,
                PostalCode: profileInfo?.PostalCode,
                RoleName: profileInfo?.RoleName,
                BranchLocationID: profileInfo?.BranchLocationID,
                IsBlock: profileInfo?.IsBlock,
                Comments: profileInfo?.Comments,
                Email: profileInfo?.Email,
                UserName: profileInfo?.UserName
            })
            yield put(notifySuccess('Success.'));
            window.location.reload();
        } else {

        }
    } catch(e) {
        yield put(updateProfileRequestError());
        yield put(notifyError('Something weng wrong'))
    }
}

function* watchFetchProfile() {
    yield takeLatest(EUserProfileActionType.getProfileRequest, doFetchProfile);
}

function* watchUpdateProfileRequest() {
    yield takeLatest(EUserProfileActionType.updateProfileRequest, doUpdateProfileRequest)
}

const profileSagas = [
    fork(watchFetchProfile),
    fork(watchUpdateProfileRequest)
];

export default profileSagas;