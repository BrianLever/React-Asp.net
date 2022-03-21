import { call, delay, fork, put, select, takeLatest } from 'redux-saga/effects';
import { EScreenProfileActions, IActionPayload } from '../../actions';
import { getScreenProfileListRequestStart, IScreenProfilesResponseItem, 
         setCreateScreenProfileDescription, setCreateScreenProfileName, 
         setNewScreenProfileLoading, setScreenProfileId, IScreeningProfileMinimunAgeResponseItem, 
         screenProfileMinimumAgeListRequestStart, screenProfileMinimumAgeListRequestSuccess, screenProfileMinimumAgeListRequestError,
         IScreenProfileFrequencyResponseItem,
         screenProfileFrequencyListRequestSuccess,
         screenProfileFrequencyListRequestStart,
         screenProfileFrequencyListRequestError,
         IScreenProfileFrequencyListItem,
         screenProfileDefaultFrequencyListSuccess,
         IScreenProfileAgeGroupsItem,
         screenProfileAgeGroupsListRequestSuccess
} from '../../actions/screen-profiles';
import { screenProfileFilterByNameSelector, screenProfileMaximumRowsSelector, 
         screenProfileSortKeySelector, screenProfileSortDirectionelector,
         screenProfileStartRowIndexSelector, screenProfilesCurrentPageSelector, 
         screenProfileCreateScreenProfileNameSelector, screenProfileCreateScreenProfileDescriptionSelector, 
         ScreenProfileIdSelector, 
         screeningMinimumAgeListSelector,
         screenProfileFrequencyListSelector
} from 'selectors/screen-profiles';
import postScreenProfilesList  from 'api/calls/post-screen-profiles-list';
import { IScreenProfilesResponse, getScreenProfileListRequestSuccess, setCurrentPage, getScreenProfileListRequest, changeActiveSortDirectionAction, changeActiveSortKeyAction } from 'actions/screen-profiles';
import { closeModalWindow, EModalWindowKeys, notifyError, notifySuccess, openModalWindow } from 'actions/settings';
import postCreateScreenProfile from 'api/calls/post-create-screen-profile';
import getScreenProfileById from 'api/calls/get-screen-profile-by-id';
import updateScreenProfile from 'api/calls/put-update-screen-profile';
import deleteScreenProfile from 'api/calls/delete-screen-profile-by-id';
import getScreeningMinumumList from 'api/calls/get-screening-age-list';
import updateScreenProfileMinimumAgeList from 'api/calls/post-screen-profile-minimum-age-update';
import getScreenProfileFrequencyList from 'api/calls/get-screen-profile-frequency-list';
import getScreenProfileFrequencyListById from 'api/calls/get-screen-profile-frequency-list-by-id';
import updateScreenProfileFrequencyList from 'api/calls/post-screen-profile-frequency-update-by-id';
import getScreenProfileAgeGroupsList from 'api/calls/get-screen-profile-age-groups-list';

const MAXIMUM_RECORDS_PER_PAGE = 20;

function* doScreenProfilesListRequest() {
    yield put(getScreenProfileListRequestStart());
    const sortKey: string = yield select(screenProfileSortKeySelector);
    const sortDirection: string = yield select(screenProfileSortDirectionelector);
    const StartRowIndex: number = yield  select(screenProfileStartRowIndexSelector);
    const MaximumRows: number = yield select(screenProfileMaximumRowsSelector);
    const OrderBy = (sortKey && sortDirection) ?  `${sortKey} ${sortDirection}` : 'Name ASC';
    const FilterByName: string = yield select(screenProfileFilterByNameSelector);
    const currentPage: number = yield select(screenProfilesCurrentPageSelector);

    const LastStartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
    const response: IScreenProfilesResponse = yield call(postScreenProfilesList, {
        OrderBy: OrderBy,
        MaximumRows: MaximumRows,
        StartRowIndex: LastStartRowIndex,
        FilterByName: FilterByName
    })
    yield put(getScreenProfileListRequestSuccess({
        Items: [...response.Items],
        TotalCount:  Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE)
    }));
}

function* doChangeCurrentPageRequest(action: IActionPayload) {
    const { page = 1 } = action.payload || {};
    yield put(setCurrentPage(page));
    yield put(getScreenProfileListRequest());
}

function* doChangeDirectionSortRequest(action: IActionPayload) {
    const { direction, key } = action.payload || {};
    yield put(changeActiveSortDirectionAction(direction));
    yield put(changeActiveSortKeyAction(key));
    yield put(getScreenProfileListRequest());
}

function* doCreateNewScreenProfileRequest () {
    try {
        yield put(setNewScreenProfileLoading(true));
        const createName: string = yield select(screenProfileCreateScreenProfileNameSelector);
        const createDescription: string = yield select(screenProfileCreateScreenProfileDescriptionSelector);
        if (!createName) {
            yield put(notifyError(`Screen Profile Name is required.`));
            return;
        }
        const newScrenProfile: IScreenProfilesResponseItem = yield call(postCreateScreenProfile, {
            Name: createName, 
            Description: createDescription
        }) 
        
        yield put(notifySuccess(`${createName} created successfully.`));
        yield put(getScreenProfileListRequest());
        yield put(closeModalWindow(EModalWindowKeys.screenProfileAdd));
        yield put(setCreateScreenProfileName(''));
        yield put(setCreateScreenProfileDescription(''));

    } catch(e) {
        yield put(notifyError(e?.response.data.Errors[0]))
    }

}

function* doScreenProfileByIdRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        const screenProfile: IScreenProfilesResponseItem = yield call(getScreenProfileById, id);
        yield put(setCreateScreenProfileName(screenProfile.Name));
        yield put(setCreateScreenProfileDescription(screenProfile.Description));
        yield put(openModalWindow(EModalWindowKeys.screenProfileEdit));
        yield put(setScreenProfileId(id));
        
    } catch(e) {
        yield put(notifyError(`Please try again.`));
    }
}

function* doScreenProfileUpdateRequest() {
    try {
        const name: string = yield select(screenProfileCreateScreenProfileNameSelector);
        const description: string = yield select(screenProfileCreateScreenProfileDescriptionSelector);
        const id: number = yield select(ScreenProfileIdSelector);
        if(!name) {
            yield put(notifyError('Screen Profile Name is required.'));
            return;
        }
        const res: string = yield call(updateScreenProfile, id, {
            "Name": name,
            "Description": description
        })
 
        yield put(notifySuccess('Screen Profile has been updated successfully.'));
        yield put(closeModalWindow(EModalWindowKeys.screenProfileEdit));
        yield put(getScreenProfileListRequest())

    } catch (e) {console.log(e)
        if(e.response.data) {
            yield put(notifyError(e.response.data.Errors[0]));
        } else {
            yield put(notifyError('Unnown server error.'));
        }
    }
}

function* doScreenProfileDeleteRequest() {
    try {
        const id: number = yield select(ScreenProfileIdSelector);
        const res: string = yield call(deleteScreenProfile, id);
        yield put(notifySuccess(`Screen Profile has been deleted successfully.`));
        yield put(closeModalWindow(EModalWindowKeys.screenProfileEdit));
        yield put(getScreenProfileListRequest())
    } catch(e) {
        yield put(notifyError('Something went wrong, Please try again.'));
    }
}

function* doScreenProfileAgeListRequest() {
    try {
        yield put(screenProfileMinimumAgeListRequestStart());
        const screenProfileId: number = yield select(ScreenProfileIdSelector);
        const response: Array<IScreeningProfileMinimunAgeResponseItem> = yield call(getScreeningMinumumList, screenProfileId);
        yield put(screenProfileMinimumAgeListRequestSuccess(response));
        const list: Array<IScreenProfileAgeGroupsItem> = yield call(getScreenProfileAgeGroupsList);
        yield put(screenProfileAgeGroupsListRequestSuccess(list));
    } catch(e) {
        yield put(screenProfileMinimumAgeListRequestError());
    }
}

function* doScreenProfileMinimumUpdateRequest() {
    try {
         const screeningMinimumAgeList: Array<IScreeningProfileMinimunAgeResponseItem> = yield select(screeningMinimumAgeListSelector);
         const screenProfileId: number = yield select(ScreenProfileIdSelector);
         const response: string = yield call(updateScreenProfileMinimumAgeList, screenProfileId, {
             Items: [
                 ...screeningMinimumAgeList
             ]
         });

         yield put(notifySuccess("Success!"))
    } catch(e) {
        yield put(notifyError(e.response.data.Errors[0]));
    }
}

function* doScreenProfileFrequencyListRequest() {
    try {
        yield put(screenProfileFrequencyListRequestStart());
        const screenProfileId: number = yield select(ScreenProfileIdSelector);
        const response: Array<IScreenProfileFrequencyResponseItem> = yield call(getScreenProfileFrequencyListById, screenProfileId);
        const list: Array<IScreenProfileFrequencyListItem> = yield call(getScreenProfileFrequencyList);
        yield put(screenProfileFrequencyListRequestSuccess(response));
        yield put(screenProfileDefaultFrequencyListSuccess(list));
    } catch (e) {
        yield put(screenProfileFrequencyListRequestError());
    }
}

function* doScreenProfileFrequencyUpdateRequest() {
    try {
        const screenProfileId: number = yield select(ScreenProfileIdSelector);
        const screenProfileFrequencyList: Array<IScreenProfileFrequencyResponseItem> = yield select(screenProfileFrequencyListSelector);
        const requestData: Array<any> = screenProfileFrequencyList.map(item =>  
             ({ ID: item.ID, Frequency: item.Frequency }))
        const response: string = yield call(updateScreenProfileFrequencyList, screenProfileId, { Items: [ ...requestData ]})
        yield put(notifySuccess("Success!"))
    } catch (e) {
        yield put(notifyError(e.response.data.Errors[0]));
    }
}

function* watchScreenProfilesListRequest() {
    yield takeLatest(EScreenProfileActions.getScreenProfileListRequest, doScreenProfilesListRequest);
}

function* watchChangeCurrentPageRequest() {
    yield takeLatest(EScreenProfileActions.changeCurrentPageRequst, doChangeCurrentPageRequest);
}

function* watchChangeDirectionSortRequest() {
    yield takeLatest(EScreenProfileActions.changeActiveSort, doChangeDirectionSortRequest);
}

function* watchCreateNewScreenProfileRequest() {
    yield takeLatest(EScreenProfileActions.createNewScreenProfileRequest,  doCreateNewScreenProfileRequest)
}

function* watchScreenProfileByIdRequest() {
    yield takeLatest(EScreenProfileActions.getScreenProfileByIdRequest, doScreenProfileByIdRequest);
}

function* watchScreenProfileUpdateRequest() {
    yield takeLatest(EScreenProfileActions.updateScreenProfileRequest, doScreenProfileUpdateRequest);
}

function* watchScreenProfileDeleteRequest() {
    yield takeLatest(EScreenProfileActions.deleteScreenProfileRequest, doScreenProfileDeleteRequest);
}

function* watchScreenProfileAgeListRequest() {
    yield takeLatest(EScreenProfileActions.screenProfileMinimumAgeListRequest, doScreenProfileAgeListRequest);
}

function* watchScreenProfileMinimumUpdateRequest() {
    yield takeLatest(EScreenProfileActions.screenProfileMinimumAgeUpdateRequest, doScreenProfileMinimumUpdateRequest);
}

function* watchScreenProfileFrequencyListRequest() {
    yield takeLatest(EScreenProfileActions.screenProfileFrequencyListRequest, doScreenProfileFrequencyListRequest);
}

function* watchScreenProfileFrequencyUpdateRequest() {
    yield takeLatest(EScreenProfileActions.screenProfileFrequencyUpdateRequest, doScreenProfileFrequencyUpdateRequest);
}

const screenProfilesSagas = [
  fork(watchScreenProfilesListRequest),
  fork(watchChangeCurrentPageRequest),
  fork(watchChangeDirectionSortRequest),
  fork(watchCreateNewScreenProfileRequest),
  fork(watchScreenProfileByIdRequest),
  fork(watchScreenProfileUpdateRequest),
  fork(watchScreenProfileDeleteRequest),
  fork(watchScreenProfileAgeListRequest),
  fork(watchScreenProfileMinimumUpdateRequest),
  fork(watchScreenProfileFrequencyListRequest),
  fork(watchScreenProfileFrequencyUpdateRequest)
];

export default screenProfilesSagas;