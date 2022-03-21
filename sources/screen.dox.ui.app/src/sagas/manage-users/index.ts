import { call, fork, put, takeLatest, select, delay, all } from 'redux-saga/effects';
import { EManageUsersActions, IActionPayload } from '../../actions';
import { changeActiveSortKeyAction, IBranchLocationResponse } from '../../actions/branch-locations';
import { getManageUsersListRequest, getManageUsersListRequestError, getManageUsersListRequestStart, getManageUsersListRequestSuccess, IManageUsersResponseItem, IManageUsersUser, manageUsersCreateRequestError, manageUsersCreateRequestStart, manageUsersCreateRequestSuccess, manageUsersDetailRequestError, manageUsersDetailRequestStart, manageUsersDetailRequestSuccess, manageUsersUpdateRequestStart, manageUsersUpdateRequestError, setManageUsersGroups, setManageUsersLocations, setManageUsersOrderDirection, setManageUsersOrderKey, setManageUsersSelectedUserId } from '../../actions/manage-users';
import postUserList from '../../api/calls/post-get-user-list';
import { manageUsersCurrentPageSelector, manageUsersLocationIdSelector, manageUsersOrderDirectionSelector, manageUsersOrderKeySelector, manageUsersUserSelector } from '../../selectors/manage-users';
import postBranchLocations from '../../api/calls/post-branch-locations';
import getListBranchLocations from '../../api/calls/get-list-branch-locations'
import { TBranchLocationsItemResponse } from '../../actions/shared';
import getUserGroups from 'api/calls/get-users-groups';
import { useSelector } from 'react-redux';
import postCreateUser from 'api/calls/post-create-user';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from 'actions/settings';
import getUserDetail from 'api/calls/get-user-detail-by-id';
import putUser from 'api/calls/put-user-update';
import BlockUser from 'api/calls/post-user-block';
import UnBlockUser from 'api/calls/post-unblock-user';
import DeleteUser from 'api/calls/delete-user-by-id';

export const MAXIMUM_RECORDS_PER_PAGE = 20;


function* doGetUserListRequest() {
    try {
        yield put(getManageUsersListRequestStart());
        const locationId: number | null = yield select(manageUsersLocationIdSelector);
        const orderKey: string = yield select(manageUsersOrderKeySelector);
        const orderDirection: string = yield select(manageUsersOrderDirectionSelector);
        const currentPage: number = yield select(manageUsersCurrentPageSelector);
        const OrderBy = (orderKey && orderDirection) ?  `${orderKey} ${orderDirection}` : 'FirstName ASC';
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const userGroups: string[] = yield call(getUserGroups);
        yield put(setManageUsersGroups(userGroups));
        const response: { Items: Array<IManageUsersResponseItem>, TotalCount: number } = yield call(postUserList, {
            BranchLocationID: locationId,
            OrderBy,
            StartRowIndex: StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE
        })
        yield put(getManageUsersListRequestSuccess({
            Items: [...response.Items],
            TotalCount:  Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE)
        }))
        const locations: Array<TBranchLocationsItemResponse> = yield call(getListBranchLocations);
        yield put(setManageUsersLocations(locations));
    } catch (error) {
        yield put(getManageUsersListRequestError());
        yield put(notifyError(error?.response.data.Errors[0]))
    }   
}

function* doChangeDirectionSortRequest(action: IActionPayload) {
    const { direction, key } = action.payload || {};
    yield put(setManageUsersOrderKey(key));
    yield put(setManageUsersOrderDirection(direction));
    yield put(getManageUsersListRequest());
}

function* doCreateUserRequest() {
    try {
        yield put(manageUsersCreateRequestStart())
        const user: IManageUsersUser = yield select(manageUsersUserSelector);
        if(user.ConfirmPassword === '') {
            yield put(notifyError('User confirm password is required.'));
            return;
        }
        if(user.ConfirmPassword !== user.Password) {
            yield put(notifyError('Password and Confirmation not equal.'));
            return;
        }
        const response: string = yield call(postCreateUser, { ...user });
        yield put(manageUsersCreateRequestSuccess());
        yield put(getManageUsersListRequest())
        yield put(notifySuccess(`${user.UserName} created successfully.`));
        yield put(closeModalWindow(EModalWindowKeys.manageUsersAddUser));
    } catch(e) {
        yield put(manageUsersCreateRequestError());
        const errors = e.response.data.Errors;
        yield all(errors.map((error: string) => { return put(notifyError(error))}))
    }
}

function* doUserDetailRequest(action: IActionPayload) {
    try {
        const { userId } = action.payload;
        yield put(manageUsersDetailRequestStart());
        const response: IManageUsersUser = yield call(getUserDetail, userId);
        yield put(manageUsersDetailRequestSuccess(response));
        yield put(getManageUsersListRequest())
        yield put(openModalWindow(EModalWindowKeys.manageUsersEditUser));
        yield put(setManageUsersSelectedUserId(userId));
    } catch(e) {
        yield put(manageUsersDetailRequestError());
    }
}

function* doUserUpdateRequest(action: IActionPayload) {
    try {
        const { userId } = action.payload;
        yield put(manageUsersUpdateRequestStart());
        const user: IManageUsersUser = yield select(manageUsersUserSelector);
        const response: string = yield call(putUser, userId, user);
        yield put(getManageUsersListRequest())
        yield put(closeModalWindow(EModalWindowKeys.manageUsersEditUser));
        yield put(notifySuccess(`${user.UserName} updated successfully.`));
        
    } catch(e) {
        yield  put(manageUsersUpdateRequestError());
        const errors = e.response.data.Errors;
        yield all(errors.map((error: string) => { return put(notifyError(error))}))
    }
}

function* doUserBlockRequest(action: IActionPayload) {
    try {
        const { userId } = action.payload;
        const response: string = yield call(BlockUser, userId);
        const user: IManageUsersUser = yield select(manageUsersUserSelector);
        yield put(getManageUsersListRequest())
        yield put(notifySuccess(`${user.UserName} blocked successfully.`));
        yield put(closeModalWindow(EModalWindowKeys.manageUsersEditUser));
    } catch(e) {
        const errors = e.response.data.Errors;
        yield all(errors.map((error: string) => { return put(notifyError(error))}))
    }
}

function* doUserDeleteRequest(action: IActionPayload) {
    try {
        const { userId } = action.payload;
        const response: string = yield call(DeleteUser, userId);
        const user: IManageUsersUser = yield select(manageUsersUserSelector);
        yield put(getManageUsersListRequest())
        yield put(notifySuccess(`${user.UserName} account has been deleted successfully.`));
        yield put(closeModalWindow(EModalWindowKeys.manageUsersEditUser));
    } catch(e) {
        const errors = e.response.data.Errors;
        yield all(errors.map((error: string) => { return put(notifyError(error))}))
    }
}

function* doUserUnBlockRequest(action: IActionPayload) {
    try {
        const { userId } = action.payload;
        const response: string = yield call(UnBlockUser, userId);
        const user: IManageUsersUser = yield select(manageUsersUserSelector);
        yield put(getManageUsersListRequest())
        yield put(notifySuccess(`${user.UserName} unblocked successfully.`));
        yield put(closeModalWindow(EModalWindowKeys.manageUsersEditUser));
    } catch(e) {
        const errors = e.response.data.Errors;
        yield all(errors.map((error: string) => { return put(notifyError(error))}))
    }
}

function* watchGetUserListRequest() {
   yield takeLatest(EManageUsersActions.getManageUsersListRequest, doGetUserListRequest)
}

function* watchChangeDirectionSortRequest() {
    yield takeLatest(EManageUsersActions.manageUsersChangeActiveSort, doChangeDirectionSortRequest)
}

function* watchCreateUserRequest() {
    yield takeLatest(EManageUsersActions.manageUsersCreateRequest, doCreateUserRequest)
}

function* watchUserDetailRequest() {
    yield takeLatest(EManageUsersActions.manageUsersDetailRequest, doUserDetailRequest);
}

function* watchUserUpdateRequest() {
    yield takeLatest(EManageUsersActions.manageUsersUpdateRequest, doUserUpdateRequest);
}

function* watchBlockUserRequest() {
    yield takeLatest(EManageUsersActions.manageUsersBlockRequest, doUserBlockRequest);
}

function* watchUnBlockUserRequest() {
    yield takeLatest(EManageUsersActions.manageUsersUnBlockRequest, doUserUnBlockRequest)
}

function* watchDeleteUserRequest() {
    yield takeLatest(EManageUsersActions.manageUsersDeleteRequest, doUserDeleteRequest)
}

const manageUsersSagas = [
    fork(watchGetUserListRequest),
    fork(watchChangeDirectionSortRequest),
    fork(watchCreateUserRequest),
    fork(watchUserDetailRequest),
    fork(watchUserUpdateRequest),
    fork(watchBlockUserRequest),
    fork(watchUnBlockUserRequest),
    fork(watchDeleteUserRequest),
];

export default manageUsersSagas;