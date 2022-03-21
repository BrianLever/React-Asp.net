import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { EBranchLocationsActions, IActionPayload } from '../../actions'
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from '../../actions/settings';
import { 
    getBranchLocationsRequestError, getBranchLocationsRequestStart, getBranchLocationsRequestSuccess, IBranchLocationResponse,
    changeActiveSortDirectionAction, changeActiveSortKeyAction, getBranchLocationsRequest, setCurrentPage, resetKioskFilter, 
    createNewBranchLocationRequestStart, createNewBranchLocationRequestError, createNewBranchLocationRequestSuccess, 
    ICreateUpdateBranchLocationResponse, getBranchLocationByIdRequestStart, getBranchLocationByIdRequestError, createBranchLocationName, 
    createBranchLocationDescription, createBranchLocationScreenProfile, setCurrentBranchLocation, updateBranchLocationRequestStart, 
    updateBranchLocationRequestError, updateBranchLocationRequestSuccess, deleteBranchLocationRequestStart, 
    deleteBranchLocationRequestSuccess, deleteBranchLocationRequestError, setBranchLocationAutoStatus, setBranchLocationDisabled
} from '../../actions/branch-locations';
import { 
    getBranchLocationsActiveDirectionSelector, getBranchLocationsActiveKeySelector, getBranchLocationsCurrentPageSelector,
    getSelectedFilterBranchLocationNameKeySelector, getSelectedFilterShowDisabledSelector, getSelectedFilterScreeningProfileIdSelector, 
    getCreateBranchLocationsNameSelector, getCreateBranchLocationsDescriptionSelector, getCreateBranchLocationsScreenProfileSelector, 
    getSelectedBranchLocationIdSelector 
} from '../../selectors/branch-locations';
import postBranchLocations from '../../api/calls/post-branch-locations';
import postCreateNewBranchLocation from '../../api/calls/post-create-new-branch';
import getBranchLocationById from '../../api/calls/get-branch-location-by-id';
import putUpdateBranchLocationById from '../../api/calls/put-branch-location-by-id';
import deleteBranchLocationId from '../../api/calls/delete-branch-location-by-id';

const MAXIMUM_RECORDS_PER_PAGE = 20;

function* doGetBranchLocationsRequest() {
    try {
        yield put(getBranchLocationsRequestStart());
        const locationNameKey: string = yield select(getSelectedFilterBranchLocationNameKeySelector);
        const selectedVisibility: number = yield select(getSelectedFilterShowDisabledSelector);
        const screeningProfileId: number = yield select(getSelectedFilterScreeningProfileIdSelector);
        const sortDirection: string = yield select(getBranchLocationsActiveDirectionSelector);
        const sortKey: string = yield select(getBranchLocationsActiveKeySelector);
        const currentPage: number = yield select(getBranchLocationsCurrentPageSelector);
        const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'Name ASC';
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const response: IBranchLocationResponse = yield call(postBranchLocations, {
            ShowDisabled: !!selectedVisibility,
            StartRowIndex,
            ScreeningProfileId: screeningProfileId,
            FilterByName: locationNameKey,
            OrderBy
        });
        if(!response || !response.Items || !response.Items.length) {
            yield put(notifyError(`Branch Locations was not found.`));
            return
        }
        yield put(getBranchLocationsRequestSuccess({
            Items: [...response.Items],
            TotalCount:  Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE)
        }));
    } catch(e) {
        yield put(notifyError(`Branch Locations not loaded.`));
        yield put(getBranchLocationsRequestError());
    }
}

function* doCnageDirectionSortRequest(action: IActionPayload) {
    const { direction, key } = action.payload || {};
    yield put(changeActiveSortDirectionAction(direction));
    yield put(changeActiveSortKeyAction(key));
    yield put(getBranchLocationsRequest());
}

function* doChangeCurrentPageRequest(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    yield put(setCurrentPage(page));
    yield put(getBranchLocationsRequest());
}

function* doResetBranchLocationsFilterRequest() {
    yield put(resetKioskFilter());
    yield put(getBranchLocationsRequest());
}

function* doCreateNewBranchLocationRequest() {
    try {
        yield put(createNewBranchLocationRequestStart());
        const createName: string = yield select(getCreateBranchLocationsNameSelector);
        const branchLocation: number = yield select(getCreateBranchLocationsScreenProfileSelector);
        
        if (!createName) {
            yield put(notifyError(`Branch Location Name is required.`));
            return;
        }

        if(branchLocation == 0) {
            yield put(notifyError(`Screen Profile is required.`));
            return;
        }
        
        const createDescription: string = yield select(getCreateBranchLocationsDescriptionSelector);
        const createScreenProfile: number = yield select(getCreateBranchLocationsScreenProfileSelector);
        const branch: ICreateUpdateBranchLocationResponse = yield call(postCreateNewBranchLocation, {
            Name: createName,
            Description: createDescription,
            ScreeningProfileID: createScreenProfile,
            Disabled: false,
        });
        yield put(notifySuccess(`Created.`));
        yield put(createNewBranchLocationRequestSuccess(branch));
        yield put(getBranchLocationsRequest());
        yield put(closeModalWindow(EModalWindowKeys.branchLocationsAddNewBranchLocation));
    } catch(e) {
        yield put(notifyError(`Not Saved.`));
        yield put(createNewBranchLocationRequestError());
    }
}

function* doBranchLocationByIdRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        yield put(getBranchLocationByIdRequestStart());
        const branch: ICreateUpdateBranchLocationResponse = yield call(getBranchLocationById, id);
        yield put(createBranchLocationName(branch.Name));
        yield put(createBranchLocationDescription(branch.Description));
        yield put(createBranchLocationScreenProfile(branch.ScreeningProfileID));
        yield put(setBranchLocationDisabled(branch.Disabled));
        yield put(openModalWindow(EModalWindowKeys.branchLocationsEditBranchLocation));
        yield put(setCurrentBranchLocation(id));
    } catch(e) {
        yield put(notifyError(`Cannot find Branch Location.`));
        yield put(getBranchLocationByIdRequestError());
    }
}

function* doUpdateBranchLocationRequest(action: IActionPayload) {
    try {
        const { disable = false } = action.payload || {};
        yield put(updateBranchLocationRequestStart());
        const selectedBranchId: number = yield select(getSelectedBranchLocationIdSelector);
        const createName: string = yield select(getCreateBranchLocationsNameSelector);
        if (!createName) {
            yield put(notifyError(`Branch Location Name is required.`));
            return;
        }
        const createDescription: string = yield select(getCreateBranchLocationsDescriptionSelector);
        const createScreenProfile: number = yield select(getCreateBranchLocationsScreenProfileSelector);
        // const disabled: number = yield select(getSelectedFilterShowDisabledSelector);
        const response: string = yield call(putUpdateBranchLocationById, selectedBranchId, {
            Description: createDescription,
            Name: createName,
            Disabled: disable || false,
            ScreeningProfileID: createScreenProfile,
        });
        if (response === 'Branch location has been updated successfully.') {
            yield put(updateBranchLocationRequestSuccess());
            yield put(closeModalWindow(EModalWindowKeys.branchLocationsEditBranchLocation));
            yield put(notifySuccess(`Updated.`));
            yield put(getBranchLocationsRequest());
        } else {
            yield put(notifyError(`Cannot update Branch Location.`));
            yield put(updateBranchLocationRequestError());
        }
    } catch (e) {
        yield put(notifyError(`Cannot update Branch Location.`));
        yield put(updateBranchLocationRequestError());
    }
}

function* doDeleteBranchLocationRequest() {
    try {
        const id: number = yield select(getSelectedBranchLocationIdSelector);
        yield put(deleteBranchLocationRequestStart());
        const deletedResponse: string = yield call(deleteBranchLocationId, id);
        if (deletedResponse === 'Branch location has been deleted successfully.') {
            yield put(deleteBranchLocationRequestSuccess());
            yield put(getBranchLocationsRequest());
            yield put(closeModalWindow(EModalWindowKeys.branchLocationsEditBranchLocation));
            yield put(notifySuccess(`Deleted.`));
        } else {
            yield put(notifyError(deletedResponse));
            yield put(deleteBranchLocationRequestError());
        }
    } catch (e) {
        yield put(notifyError('Cannot delete this branch location'));
        yield put(deleteBranchLocationRequestError());
    }
}

function* watchGetBranchLocationsRequest() {
    yield takeLatest(EBranchLocationsActions.getBranchLocationsListRequest, doGetBranchLocationsRequest);
}

function* watchCnageDirectionSortRequest() {
    yield takeLatest(EBranchLocationsActions.changeActiveSort, doCnageDirectionSortRequest);
}

function* watchChangeCurrentPageRequest() {
    yield takeLatest(EBranchLocationsActions.changeCurrentPageRequst, doChangeCurrentPageRequest);
}

function* watchResetBranchLocationsFilterRequest() {
    yield takeLatest(EBranchLocationsActions.resetBranchLocationsFilterRequest, doResetBranchLocationsFilterRequest);
}

function* watchCreateNewBranchLocationRequest() {
    yield takeLatest(EBranchLocationsActions.createNewBranchLocationRequest, doCreateNewBranchLocationRequest);
}

function* watchBranchLocationByIdRequest() {
    yield takeLatest(EBranchLocationsActions.getBranchLocationByIdRequest, doBranchLocationByIdRequest);
}

function* watchUpdateBranchLocationRequest() {
    yield takeLatest(EBranchLocationsActions.updateBranchLocationRequest, doUpdateBranchLocationRequest);
}

function* watchDeleteBranchLocationRequest() {
    yield takeLatest(EBranchLocationsActions.deleteBranchLocationRequest, doDeleteBranchLocationRequest);
}

const branchLocationsSagas = [
    fork(watchChangeCurrentPageRequest),
    fork(watchBranchLocationByIdRequest),
    fork(watchGetBranchLocationsRequest),
    fork(watchCnageDirectionSortRequest),
    fork(watchUpdateBranchLocationRequest),
    fork(watchDeleteBranchLocationRequest),
    fork(watchCreateNewBranchLocationRequest),
    fork(watchResetBranchLocationsFilterRequest)
];

export default branchLocationsSagas;