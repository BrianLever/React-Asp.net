import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { EManageDevicesActions, IActionPayload } from '../../actions';
import { 
    closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow 
} from '../../actions/settings';
import { 
    changeActiveSortDirectionAction, changeActiveSortKeyAction, getManageDevicesListRequestSuccess,
    getManageDevicesListRequestError, getManageDevicesListRequestStart, IManegeDevicesListResponse, 
    getManageDevicesListRequest, createNewKioskRequestStart, updateKioskByIdRequest,
    createNewKioskRequestError, createNewKioskRequestSuccess, addNewKioskInconsistencyInFields, 
    setCurrentPage, resetKioskFilter,getEditKioskDetailsByIdRequestStart, 
    getEditKioskDetailsByIdRequestError, IKioskDetailsResponse, getEditKioskDetailsByIdRequestSuccess, 
    changeEditKioskDetailsBranchLocation, changeEditKioskDetailsDescription, updateKioskByIdRequestStart, 
    updateKioskByIdRequestSuccess, updateKioskByIdRequestError, deleteKioskByIdRequestStart, 
    deleteKioskByIdRequestError, deleteKioskByIdRequestSuccess, setManageDeviceAutoStatus, 
} from '../../actions/manage-devices';
import postManageDevicesSearch from '../../api/calls/post-kiosk-serach';
import { 
    getCurrentActiveDirectionSelector, getCurrentActiveKeySelector, getEditKioskDetailsBranchLocationSelector, 
    getEditKioskDetailsSelector, getManageDevicesCurrentPageSelector, getEditKioskDetailsDescriptionSelector,
    getSelectedAddNewKioskBranchLocationIdSelector, getSelectedAddNewKioskDescriptionSelector, getSelectedAddNewKioskDeviceNameSelector, 
    getSelectedAddNewKioskSecretKeySelector, getSelectedFilterBranchLocationIdSelector, 
    getSelectedFilterBranchLocationNameKeySelector, getSelectedFilterScreeningProfileIdSelector, getSelectedFilterShowDisabledSelector, 
    getEditKioskDetailsCurrentEditKioskIdSelector, // getSelectedAddNewKioskScreenProfileSelector
} from '../../selectors/manage-devices';
import postCreateNewKiosk from '../../api/calls/post-create-new-kiosk';
import getKioskById from '../../api/calls/get-kiosk-by-id';
import putUpdateKioskById from '../../api/calls/put-update-kiosk-by-id';
import deleteKioskById from '../../api/calls/delete-kiosk-by-id';

const MAXIMUM_RECORDS_PER_PAGE = 20;

function* doFetchManageDevicesRequest() {
    try {
        yield put(getManageDevicesListRequestStart());
        const locationId: number = yield select(getSelectedFilterBranchLocationIdSelector);
        const locationNameKey: string = yield select(getSelectedFilterBranchLocationNameKeySelector);
        const screeningProfileId: number = yield select(getSelectedFilterScreeningProfileIdSelector);
        const selectedVisibility: number = yield select(getSelectedFilterShowDisabledSelector);
        const sortKey: string = yield select(getCurrentActiveKeySelector);
        const sortDirection: string = yield select(getCurrentActiveDirectionSelector);
        const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'KioskID ASC';
        const currentPage: number = yield select(getManageDevicesCurrentPageSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const manageDevicesList:{
            Items: Array<IManegeDevicesListResponse>;
            TotalCount: number;
        } = yield call(postManageDevicesSearch, {
            OrderBy,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE,
            StartRowIndex,
            BranchLocationId: locationId || null,
            NameOrKey: locationNameKey || '',
            ShowDisabled: !!selectedVisibility,
            ScreeningProfileId: screeningProfileId || null,
        });
        yield put(getManageDevicesListRequestSuccess({ 
            items: manageDevicesList.Items, totalCount: Math.ceil(manageDevicesList.TotalCount / MAXIMUM_RECORDS_PER_PAGE),
        }));
    } catch (e) {
        yield put(getManageDevicesListRequestError());
        yield put(notifyError(`Failed to load manage devices list.`));
    }
}

function* doChangeCurrentPageRequest(action: IActionPayload) {
    const { page } = action.payload || {};
    yield put(setCurrentPage(page));
    yield put(getManageDevicesListRequest());
}

function* doManageDevicesListSortChangeRequest(action: IActionPayload) {
    const { direction, key } = action.payload || {}; 
    yield put(changeActiveSortDirectionAction(direction));
    yield put(changeActiveSortKeyAction(key));
    yield put(getManageDevicesListRequest());
}

function* doResetKioskFilterRequest() {
    yield put(resetKioskFilter());
    yield put(getManageDevicesListRequest());
}

function* doCreateNewKioskRequest() {
    try {
        yield put(createNewKioskRequestStart());
        yield put(addNewKioskInconsistencyInFields(false));
        // check valid requst
        const selectedLocationId: number = yield select(getSelectedAddNewKioskBranchLocationIdSelector);
        const deviceName: string = yield select(getSelectedAddNewKioskDeviceNameSelector);
        // TODO: ASK WHAT THE FUCK
        // const screenProfile: string = yield select(getSelectedAddNewKioskScreenProfileSelector);
        const secretKey: string = yield select(getSelectedAddNewKioskSecretKeySelector);
        const description: string = yield select(getSelectedAddNewKioskDescriptionSelector);

        if (selectedLocationId && secretKey && deviceName) {
            yield call(postCreateNewKiosk, {
                BranchLocationId: selectedLocationId,
                Name: deviceName,
                SecretKey: secretKey,
                Description: description,
            });
            yield put(notifySuccess('Created.'));
            yield put(createNewKioskRequestSuccess());
            yield put(closeModalWindow(EModalWindowKeys.manageDevicesAddNewDevice));
            yield put(getManageDevicesListRequest());
        } else {
            yield put(addNewKioskInconsistencyInFields(true));
            yield put(createNewKioskRequestError());
            if (!selectedLocationId) {
                yield put(notifyError('Branch Location required.'));
            } else if (!secretKey) {
                yield put(notifyError('Secret Key required.'));
            } else if (!deviceName) {
                yield put(notifyError('Device Name required.'));
            }
        }
    } catch(e) {
        yield put(createNewKioskRequestError());
        yield put(notifyError('Not Created.'));
    }
}

function* doGetEditKioskByIdRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        yield put(getEditKioskDetailsByIdRequestStart());
        const kioskDetails: IKioskDetailsResponse = yield call(getKioskById, id);
        yield put(getEditKioskDetailsByIdRequestSuccess(kioskDetails, id));
        yield put(changeEditKioskDetailsBranchLocation(kioskDetails.BranchLocationID));
        yield put(changeEditKioskDetailsDescription(kioskDetails.Description));
        yield put(openModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
    } catch (e) {
        yield put(getEditKioskDetailsByIdRequestError());
        yield put(notifyError('No kiosk data found.'));
    }
}

function* doUpdateKioskByIdRequest(action: IActionPayload) {
    try {
        const { disable = false } = action.payload || {}; 
        yield put(updateKioskByIdRequestStart());
        const kioskDetails: IKioskDetailsResponse = yield select(getEditKioskDetailsSelector);
        const location: number = yield select(getEditKioskDetailsBranchLocationSelector);
        const description: string = yield select(getEditKioskDetailsDescriptionSelector);
        const id: number = yield select(getEditKioskDetailsCurrentEditKioskIdSelector);
        if (!location) {
            yield put(notifyError(`Branch Location is required.`));
        } else {
            const body: IKioskDetailsResponse = {
                ...kioskDetails,
                BranchLocationID: location,
                Description: description,
                Disabled: disable,
            };
            const response: string = yield call(putUpdateKioskById, id, body);
            if (response === 'Kiosk has been updated successfully.') {
                yield put(closeModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
                yield put(updateKioskByIdRequestSuccess());
                yield put(notifySuccess('Updated.'));
                yield put(getManageDevicesListRequest())
            } else {
                yield put(notifyError(`Kiosk details wasn't updated.`));
                yield put(updateKioskByIdRequestError());
            }
        }
    } catch (e) {
        yield put(notifyError(`Kiosk details wasn't updated.`));
        yield put(updateKioskByIdRequestError());
    }
}

function* doDisbaleKioskByIdRequest() {
    yield put(updateKioskByIdRequest(true));
}

function* doDeleteKioskByIdRequest() {
    try {
        yield put(deleteKioskByIdRequestStart());
        const id: number = yield select(getEditKioskDetailsCurrentEditKioskIdSelector);
        const response: string = yield call(deleteKioskById, id);
        if (response === 'Kiosk has been deleted successfully.') {
            yield put(closeModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
            yield put(deleteKioskByIdRequestSuccess());
            yield put(notifySuccess('Deleted.'));
            yield put(getManageDevicesListRequest())
        } else {
            yield put(notifyError(`Kiosk details wasn't deleted.`));
            yield put(deleteKioskByIdRequestError());
        }
    } catch (e) {
        yield put(notifyError(`Kiosk details wasn't deleted.`));
        yield put(deleteKioskByIdRequestError());
        yield put(closeModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
    }
}

function* watchFetchManageDevicesRequest() {
    yield takeLatest(EManageDevicesActions.getManagedDevicesListRequest, doFetchManageDevicesRequest);
}

function* watchManageDevicesListSortChangeRequest() {
    yield takeLatest(EManageDevicesActions.changeActiveSort, doManageDevicesListSortChangeRequest);
}

function* watchCreateNewKioskRequest() {
    yield takeLatest(EManageDevicesActions.createNewKioskRequest, doCreateNewKioskRequest);
}

function* watchChangeCurrentPageRequest() {
    yield takeLatest(EManageDevicesActions.changeCurrentPageRequst, doChangeCurrentPageRequest);
}

function* watchResetKioskFilterRequest() {
    yield takeLatest(EManageDevicesActions.resetKioskFilterRequest, doResetKioskFilterRequest);
}

function* watchGetEditKioskByIdRequest() {
    yield takeLatest(EManageDevicesActions.getEditKioskDetailsByIdRequest, doGetEditKioskByIdRequest);
}

function* watchUpdateKioskByIdRequest() {
    yield takeLatest(EManageDevicesActions.updateKioskByIdRequest, doUpdateKioskByIdRequest);
}

function* watchDeleteKioskByIdRequest() {
    yield takeLatest(EManageDevicesActions.deleteKioskByIdRequest, doDeleteKioskByIdRequest);
}

function* watchDisbaleKioskByIdRequest() {
    yield takeLatest(EManageDevicesActions.disableKioskByIdRequest, doDisbaleKioskByIdRequest);
}


const manageDevicesSagas = [
    fork(watchCreateNewKioskRequest),
    fork(watchDeleteKioskByIdRequest),
    fork(watchUpdateKioskByIdRequest),
    fork(watchDisbaleKioskByIdRequest),
    fork(watchResetKioskFilterRequest),
    fork(watchGetEditKioskByIdRequest),
    fork(watchChangeCurrentPageRequest),
    fork(watchFetchManageDevicesRequest),
    fork(watchManageDevicesListSortChangeRequest),
];

export default manageDevicesSagas;