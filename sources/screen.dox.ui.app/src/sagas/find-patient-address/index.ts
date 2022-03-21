import { notifyError, notifySuccess } from 'actions/settings';
import { call, fork, put, takeLatest, select, delay } from 'redux-saga/effects';
import { EFindPatientAddressActions, IActionPayload } from '../../actions';
import { getFindPatientAddressEhrRecordPatientRequestError, getFindPatientAddressEhrRecordPatientRequestStart, getFindPatientAddressEhrRecordPatientRequestSuccess, IEhrRecordPatientsItem, postFindPatientAddressRequestError, postFindPatientAddressRequestStart } from '../../actions/find-patient-address';
import postEhrRecordPatients from '../../api/calls/post-ehr-record-patients';
import { findPatientAddressCitySelector, findPatientAddressCurrentPageSelector, findPatientAddressPatientIdSelector, findPatientAddressPhoneNumberSelector, findPatientAddressStateSelector, findPatientAddressStreetAddressSelector, findPatientAddressZipcodeSelector, getFindPatientAddressEhrExportPatientRecordSelectedIdSelector } from '../../selectors/find-patient-address';
import postFindPatientAddress from 'api/calls/post-find-patient-address';
import { getCurrentScreeningReportSelector } from 'selectors/screen/report';
import { IScreeningReport } from 'actions/screen/report';
import { ERouterUrls } from 'router';
import * as config from 'config/app.json';



export const MAXIMUM_RECORDS_PER_PAGE = 10;

function* doGetFindPatientAddressEhrRecordPatientRequest(action: IActionPayload) {
    try {
        yield put(getFindPatientAddressEhrRecordPatientRequestStart());
        const { screeningResultId }  = action.payload;
        const currentPage: number = yield select(findPatientAddressCurrentPageSelector);
        const StartRowIndex = ((currentPage * MAXIMUM_RECORDS_PER_PAGE) - MAXIMUM_RECORDS_PER_PAGE);
        const response: { Items: Array<IEhrRecordPatientsItem>, TotalCount: number } = yield call(postEhrRecordPatients, screeningResultId, {
            StartRowIndex,
            MaximumRows: MAXIMUM_RECORDS_PER_PAGE     
        })
       
        yield put(getFindPatientAddressEhrRecordPatientRequestSuccess({ ...response, TotalCount: Math.ceil(response.TotalCount / MAXIMUM_RECORDS_PER_PAGE)}));
    } catch(e) {
        yield put(getFindPatientAddressEhrRecordPatientRequestError());
        yield put(notifyError('EHR service is unavailable. Please try again later or contact Administrator.'))
    }
}

function* watchGetFindPatientAddressEhrRecordPatientRequest() {
    yield takeLatest(EFindPatientAddressActions.getFindPatientAddressEhrRecordPatientsRequest, doGetFindPatientAddressEhrRecordPatientRequest);
}

function* doPostFindPatientAddressRequest(action: IActionPayload) {
    try {
        yield put(postFindPatientAddressRequestStart());
        const phoneNumber: string = yield select(findPatientAddressPhoneNumberSelector);
        const streetAddress: string = yield select(findPatientAddressStreetAddressSelector);
        const city: string = yield select(findPatientAddressCitySelector);
        const state: string = yield select(findPatientAddressStateSelector);
        const zipCode: string = yield select(findPatientAddressZipcodeSelector);
        const patientId: number = yield select(findPatientAddressPatientIdSelector);
        const ehrExportPatientRecordSelectedId: number = yield select(getFindPatientAddressEhrExportPatientRecordSelectedIdSelector)
        const screenReport: IScreeningReport = yield select(getCurrentScreeningReportSelector);
        const { id } = action.payload;
        if(!ehrExportPatientRecordSelectedId) {
            yield put(notifyError('Please select EHR record.'));
            return;
        }
        const response: string = yield call(postFindPatientAddress, id === 1?screenReport.BhsVisitID:screenReport.PatientDemographicsID, {
            PatientID: ehrExportPatientRecordSelectedId,
            City: city,
            Phone: phoneNumber,
            StateID: state,
            StreetAddress: streetAddress,
            ZipCode: zipCode
        })
        
        yield put(notifySuccess('Address has been updated'));
        if(id === 0) {
            window.location.href = config.BASEURL + ERouterUrls.VISIT_DEMOGRAPHIC_REPORT.replace(':reportId', String(screenReport.PatientDemographicsID));
        } else {
            window.location.href = config.BASEURL + ERouterUrls.VISIT_REPORTS.replace(':reportId', String(screenReport.BhsVisitID));
        }
    }  catch(e) {
        yield put(postFindPatientAddressRequestError());
        yield put(notifyError('Please try again.'))
       
    }

}

function* watchPostFindPatientAddressRequest() {
    yield takeLatest(EFindPatientAddressActions.postFindPatientAddressRequest, doPostFindPatientAddressRequest);
}


const findPatientAddressSagas = [
    fork(watchGetFindPatientAddressEhrRecordPatientRequest),
    fork(watchPostFindPatientAddressRequest)
];

export default findPatientAddressSagas;