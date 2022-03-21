import { notifyError, notifySuccess } from 'actions/settings';
import { call, fork, put, select, takeLatest, delay, all } from 'redux-saga/effects';
import { ECssrsReportActions, IActionPayload, ILocationItemResponse } from '../../../actions';
import { cssrsReportCreateRequest, cssrsReportCreateRequestStart, cssrsReportPatientRecordsRequestError,cssrsReportCreateRequestError, cssrsReportPatientRecordsRequestStart, cssrsReportPatientRecordsRequestSuccess, ICssrsReportPatientRecordsResponse, ICssrsReportRequest, cssrsReportCreateRequestSuccess, setCssrsReportLocations, cssrsReportDetailRequestStart, cssrsReportDetailRequestError, ICssrsEditableReportItem, cssrsReportDetailRequestSuccess, cssrsReportUpdateRequest, cssrsReportUpdateRequestStart, cssrsReportUpdateRequestError, cssrsReportUpdateRequestSuccess, cssrsReportCopyRequestStart, cssrsReportCopyRequestSuccess, cssrsReportCopyRequestError } from '../../../actions/c-ssrs-list/c-ssrs-report';
import postPatientRecords from '../../../api/calls/post-cssrs-patient-record';
import { cssrsReportBirthdaySelector, cssrsReportDetailSelector, cssrsReportFirstNameSelector, cssrsReportLastNameSelector, cssrsReportLocationIdSelector, cssrsReportMiddleNameSelector } from '../../../selectors/c-ssrs-list/c-ssrs-report';
import postColumbiasuicide from 'api/calls/post-columbiasuicide';
import getListBranchLocations from 'api/calls/get-list-branch-locations';
import getColumbiasuicide from 'api/calls/get-columbiasuicide-by-id';
import putCssrsReportUpdate from 'api/calls/put-columbiasuicide-update-by-id';
import * as config from 'config/app.json';
import { ERouterUrls } from 'router';
import postColumbiasuicideCopy from 'api/calls/post-columbiasuicide-copy';

function* doCssrsReportPatientRecordsRequest() {
    try {
        yield put(cssrsReportPatientRecordsRequestStart());
        const firstName: string | null = yield select(cssrsReportFirstNameSelector);
        const lastName: string | null = yield select(cssrsReportLastNameSelector);
        const middleName: string | null = yield select(cssrsReportMiddleNameSelector);
        const birthday: string | null = yield select(cssrsReportBirthdaySelector);
 
        if(!lastName || !birthday) {
            yield put(notifyError('Last name and Birthday field must not be empty.'));
            return;
        }

        const response: ICssrsReportPatientRecordsResponse = yield call(postPatientRecords, {
            "Birthday": birthday,
            "LastName": lastName,
            "FirstName": firstName,
            "MiddleName": middleName
        })

        const branchLocationsResponse: Array<ILocationItemResponse> = yield call(getListBranchLocations)
    
        yield put(cssrsReportPatientRecordsRequestSuccess({
            screendox: response.Screendox.Items,
            ehr: response.Ehr.Items
        }))

        yield put(setCssrsReportLocations(branchLocationsResponse));

    } catch (e) {
       yield put(cssrsReportPatientRecordsRequestError());
       if(!e.response.data) {
           yield e.response.data.Errors.map((x: string) => {
               put(notifyError(x))
           })
       } else {
           yield put(notifyError('EHR service is unavailable. Please try again later or contact Administrator..'))
       }
    }
}

function* doCssrsReportCreateRequest(action: IActionPayload) {
    const { requestBody } = action.payload;
    const BranchLocationID: number = yield select(cssrsReportLocationIdSelector);
    if(BranchLocationID === 0) {
        yield put(notifyError('Branch Location Id must be greater than 0.'))
        return;
    }
    try {
        yield put(cssrsReportCreateRequestStart());
        const response: number = yield call(postColumbiasuicide, { ...requestBody, BranchLocationID: BranchLocationID });
        yield put(notifySuccess('Report has been updated'));
        yield put(cssrsReportCreateRequestSuccess());
        window.location.href = config.BASEURL+ERouterUrls.CSSRS_LIFETIME_RECENT_REPORT.replace(':reportId', String(response));
    } catch(e) {
        yield put(cssrsReportCreateRequestError());
        yield put(notifyError('Something went wrong.'))
    }
}

function* doCssrsReportDetailRequest(action: IActionPayload) {
    const { reportId } = action.payload;
    try {
        yield put(cssrsReportDetailRequestStart());
        const response: ICssrsEditableReportItem = yield call(getColumbiasuicide, reportId);
        console.log(response, "repsonse");
        yield put(cssrsReportDetailRequestSuccess(response));
    } catch(e) {
        yield put(cssrsReportDetailRequestError());
    }
}

function* doCssrsReportUpdateRequest(action: IActionPayload) {
    try {
        const { id } = action.payload;
        yield put(cssrsReportUpdateRequestStart());
        const cssrsReport: ICssrsEditableReportItem = yield select(cssrsReportDetailSelector);
        const response: string = yield call(putCssrsReportUpdate, id, cssrsReport);
        yield put(cssrsReportUpdateRequestSuccess());
        yield put(notifySuccess('success.'))
    } catch(e) {
        yield put(cssrsReportUpdateRequestError());
    }
}

function* doCssrsReportCopyRequest(action: IActionPayload) {
    try {
        yield put(cssrsReportCopyRequestStart())
        const { id } = action.payload;
        const response: number = yield call(postColumbiasuicideCopy, { ID: id });
        yield put(cssrsReportCopyRequestSuccess())
        if(response) {
            window.location.href = config.BASEURL+ERouterUrls.CSSRS_LIFETIME_RECENT_REPORT.replace(':reportId', `${response}`);
        }
    } catch(e) {
        yield put(cssrsReportCopyRequestError())
        if(!e.response.data) {
            yield e.response.data.Errors.map((x: string) => {
                put(notifyError(x))
            })
        } 
    }
}

function* watchCssrsReportPatientRecordsRequest() {
    yield takeLatest(ECssrsReportActions.cssrsReportPatientRecordsRequest, doCssrsReportPatientRecordsRequest)
}

function* watchCssrsReportCreateRequest() {
    yield takeLatest(ECssrsReportActions.cssrsReportCreateRequest, doCssrsReportCreateRequest);
}

function* watchCssrsReportDetailRequest() {
    yield takeLatest(ECssrsReportActions.cssrsReportDetailRequest, doCssrsReportDetailRequest);
}

function* watchCssrsReportUpdateRequest() {
    yield takeLatest(ECssrsReportActions.cssrsReportUpdateRequest, doCssrsReportUpdateRequest);
}

function* watchCssrsReportCopyRequest() {
    yield takeLatest(ECssrsReportActions.cssrsReportCopyRequest, doCssrsReportCopyRequest);
}

const cssrsReportSagas = [
    fork(watchCssrsReportPatientRecordsRequest),
    fork(watchCssrsReportCreateRequest),
    fork(watchCssrsReportDetailRequest),
    fork(watchCssrsReportUpdateRequest),
    fork(watchCssrsReportCopyRequest),
];

export default cssrsReportSagas;
