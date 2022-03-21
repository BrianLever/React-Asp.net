import { call, fork, put, takeLatest } from 'redux-saga/effects';
import { ESharedActions } from '../../actions';
import { 
    getListBranchLocationsRequestError, getListBranchLocationsRequestStart, getListBranchLocationsRequestSuccess, 
    getScreeningProfileListRequestError, getScreeningProfileListRequestStart, getScreeningProfileListRequestSuccess, 
    TBranchLocationsItemResponse, TScreeningProfileItemResponse
} from '../../actions/shared';
import getListBranchLocations from '../../api/calls/get-list-branch-locations';
import getScreeningProfileList from '../../api/calls/get-screening-profile-list';


function* doListBranchLocationsRequest() {
    try {
        yield put(getListBranchLocationsRequestStart());
        const branchLocationsArray: Array<TBranchLocationsItemResponse> = yield call(getListBranchLocations);
        yield put(getListBranchLocationsRequestSuccess(branchLocationsArray));
    } catch (e) {
        yield put(getListBranchLocationsRequestError());
    }
}

function* doGetScreeningProfileListRequest() {
    try {
        yield put(getScreeningProfileListRequestStart());
        const screeningProfileList: Array<TScreeningProfileItemResponse> = yield call(getScreeningProfileList);
        yield put(getScreeningProfileListRequestSuccess(screeningProfileList)); 
    } catch(e) {
        yield put(getScreeningProfileListRequestError());
    }
}


function* watchListBranchLocationsRequest() {
    yield takeLatest(ESharedActions.getLocationsRequest, doListBranchLocationsRequest);
}

function* watchGetScreeningProfileListRequest() {
    yield takeLatest(ESharedActions.getScreeningProfileListRequest, doGetScreeningProfileListRequest);
}

const sharedSagas = [
    fork(watchListBranchLocationsRequest),
    fork(watchGetScreeningProfileListRequest),
];

export default sharedSagas;