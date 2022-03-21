import { call, fork, put, takeLatest, select } from 'redux-saga/effects';
import { EFollowUpRelatedReportActions, IActionPayload, TChoiceItem } from '../../../actions/index';
import { 
    postFollowUpsRequestStart, postFollowUpsRequestError, postFollowUpsRequestSuccess, 
    IFollowUpReportResponse, getFollowUpReportPatientAttendedVisitOptionsRequestStart, 
    getFollowUpReportPatientAttendedVisitOptionsRequestError, getFollowUpReportPatientAttendedVisitOptionsRequestSuccess, 
    getFollowUpReportFollowUpContactOutcomeRequestStart, getFollowUpReportFollowUpContactOutcomeRequestError, 
    getFollowUpReportFollowUpContactOutcomeRequestSuccess, getFollowUpReportNewVisitReferralRecommendationRequestStart,
    getFollowUpReportNewVisitReferralRecommendationRequestError, getFollowUpReportNewVisitReferralRecommendationRequestSuccess, 
    getFollowUpReporNewVisitReferralRecommendationAcceptedRequestStart, getFollowUpReporNewVisitReferralRecommendationAcceptedRequestError, 
    getFollowUpReporNewVisitReferralRecommendationAcceptedRequestSuccess, getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestStart, 
    getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestError, getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestSuccess, 
    getFollowUpReportDischargedRequestStart, getFollowUpReportDischargedRequestError, getFollowUpReportDischargedRequestSuccess, 
    updateFollowUpReportRequestStart, updateFollowUpReportRequestError, updateFollowUpReportRequestSuccess, setFollowUpDate, 
    setCurrentSelectedFollowUpReportPatientAttendedVisitOption, setFollowUpReportNewVisitDate, setFollowUpReportSelectedContactOutcomeOption, 
    setFollowUpReporNewVisitReferralRecommendationAcceptedOption, setFollowUpReponewVisitReferralRecommendationNotAcceptedOption, 
    setFollowUpReportSelectedNewVisitReferralRecommendationOption, setNewVisitReferralRecommendation, setVisitReportIsCompleted, setFollowUpCreate
} from '../../../actions/follow-up/report';
import { notifyError, notifySuccess } from '../../../actions/settings';
import getFollowUpReportById from '../../../api/calls/get-follow-up-report-by-id';
import getVisitDrugChoiceOptions, { EVisitReportOptions } from '../../../api/calls/get-visit-report-options';
import putFollowUpReport, { IFollowUpReportUpdateRequest } from '../../../api/calls/put-follow-up-report';
import { 
    getCurrentlySelectedDischargedOptionSelector, getCurrentlySelectedNewVisitReferralRecommendationAcceptedOptionSelector, 
    getCurrentlySelectedNewVisitReferralRecommendationNotAcceptedOptionSelector, getCurrentSelectedFollowUpContactOutcomeOption, 
    getcurrentSelectedNewVisitReferralRecommendationOption, getCurrentSelectedPatientAttendedVisitOptionSelector, 
    getFollowUpDateSelector, getFollowUpNoteSelector, getFollowUpSelector, getNewVisitDateSelector, getNewVisitReferralRecommendationAcceptedOptionsSelector 
} from '../../../selectors/follow-up/report';

function* doFollowUpReportRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        yield put(postFollowUpsRequestStart());
        const responseReport: IFollowUpReportResponse = yield call(getFollowUpReportById, id);
        yield put(setVisitReportIsCompleted(responseReport.IsCompleted));
        yield put(setFollowUpCreate(responseReport.ThirtyDatyFollowUpFlag));
        if (responseReport.FollowUpDate) {
            yield put(setFollowUpDate(responseReport.FollowUpDate))
        }
        if (responseReport.PatientAttendedVisit) {
            yield put(setCurrentSelectedFollowUpReportPatientAttendedVisitOption(responseReport.PatientAttendedVisit.Id));
        }
        if (responseReport.NewVisitDate) {
            yield put(setFollowUpReportNewVisitDate(responseReport.NewVisitDate));
        }
        if (responseReport.FollowUpContactOutcome) {
            yield put(setFollowUpReportSelectedContactOutcomeOption(responseReport.FollowUpContactOutcome.Id))
        }
        if (responseReport.NewVisitReferralRecommendation) {
            yield put(setFollowUpReportSelectedNewVisitReferralRecommendationOption(responseReport.NewVisitReferralRecommendation.Id))
        }
        if (responseReport.ReasonNewVisitReferralRecommendationNotAccepted) {
            yield put(setFollowUpReponewVisitReferralRecommendationNotAcceptedOption(responseReport.ReasonNewVisitReferralRecommendationNotAccepted.Id));
        }
        if (responseReport.NewVisitReferralRecommendationAccepted) {
            yield put(setFollowUpReporNewVisitReferralRecommendationAcceptedOption(responseReport.NewVisitReferralRecommendationAccepted.Id));
        }
        if (responseReport.VisitRefferalRecommendation) {
            yield put(setNewVisitReferralRecommendation(responseReport.VisitRefferalRecommendation));
        }
         // 
        yield put(postFollowUpsRequestSuccess(responseReport));
    } catch(e) {
        yield put(postFollowUpsRequestError());
        yield put(notifyError(`Cannot fetch report.`));
    }
}

function* doFollowUpReportUpdateFollowUpReportRequest(action: IActionPayload) {
    try {
        const { id } = action.payload || {};
        yield put(updateFollowUpReportRequestStart());
        const body: IFollowUpReportUpdateRequest = {};
        const discharged: number = yield select(getCurrentlySelectedDischargedOptionSelector);
        if (discharged) {
            body.Discharged = { Id: discharged };
        }
        const newVisitReferralRecommendation: number = yield select(getcurrentSelectedNewVisitReferralRecommendationOption);
        if (newVisitReferralRecommendation) {
            const nvrrOptions: Array<TChoiceItem> = yield select(getNewVisitReferralRecommendationAcceptedOptionsSelector)
            const foundRecom = nvrrOptions.find(d => d.Id === newVisitReferralRecommendation);
            if (foundRecom) {
                body.VisitRefferalRecommendation = foundRecom.Name;
            }
        }
        const patientAttendedVisit: number = yield select(getCurrentSelectedPatientAttendedVisitOptionSelector);
        if (patientAttendedVisit) {
            body.PatientAttendedVisit= { Id: patientAttendedVisit }
        }
        const followUpContactOutcome: number = yield select(getCurrentSelectedFollowUpContactOutcomeOption);
        if (followUpContactOutcome) {
            body.FollowUpContactOutcome = { Id: followUpContactOutcome };
        }
        const newVisitDate: string = yield select(getNewVisitDateSelector);
        if (newVisitDate) {
            body.NewVisitDate = newVisitDate;
        }
        const newVisitReferralRecommendationAccepted: number = yield select(getCurrentlySelectedNewVisitReferralRecommendationAcceptedOptionSelector);
        if (newVisitReferralRecommendationAccepted) {
            body.NewVisitReferralRecommendationAccepted = { Id: newVisitReferralRecommendationAccepted }
        }
        const newVisitReferralRecommendationNotAccepted: number = yield select(getCurrentlySelectedNewVisitReferralRecommendationNotAcceptedOptionSelector);
        if (newVisitReferralRecommendationNotAccepted) {
            body.ReasonNewVisitReferralRecommendationNotAccepted = { Id: newVisitReferralRecommendationNotAccepted }
        }

        const thirtyDatyFollowUpFlag: boolean = yield select(getFollowUpSelector);
        if (thirtyDatyFollowUpFlag) {
            body.ThirtyDatyFollowUpFlag = thirtyDatyFollowUpFlag;
        }
        const followUpDate: string = yield select(getFollowUpDateSelector);
        if (followUpDate) {
            body.FollowUpDate = followUpDate;
        }
        const notes: string = yield select(getFollowUpNoteSelector);
        if (notes) {
            body.Notes = notes;
        }

        const res: string = yield call(putFollowUpReport, id, body);
        if (res !== 'Follow-up has been updated successfully.') {
            yield put(updateFollowUpReportRequestError());
            yield put(notifyError(`Cannot update follow up report.`));  
        }
        yield put(updateFollowUpReportRequestSuccess());
        yield put(notifySuccess(`Saved.`));  
    } catch (e) {
        yield put(updateFollowUpReportRequestError());
        yield put(notifyError(`Cannot update follow up report.`));
    }
}

function* doFollowUpReportPatientAttendedVisitOptionsRequest() {
    try {
        yield put(getFollowUpReportPatientAttendedVisitOptionsRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.PatientAttendedVisit);
        yield put(getFollowUpReportPatientAttendedVisitOptionsRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReportPatientAttendedVisitOptionsRequestError());
        yield put(notifyError(`Cannot fetch patient attended options.`));
    }
}

function* doFollowUpReporttFollowUpContactOutcomeRequest() {
    try {
        yield put(getFollowUpReportFollowUpContactOutcomeRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.FollowUpContactOutcome);
        yield put(getFollowUpReportFollowUpContactOutcomeRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReportFollowUpContactOutcomeRequestError());
        yield put(notifyError(`Cannot fetch follow up contact outcome options.`));
    }
}

function* doFollowUpReportNewVisitReferralRecommendationRequest() {
    try {
        yield put(getFollowUpReportNewVisitReferralRecommendationRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.NewVisitReferralRecommendation);
        yield put(getFollowUpReportNewVisitReferralRecommendationRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReportNewVisitReferralRecommendationRequestError());
        yield put(notifyError(`Cannot fetch new visit / referral recommendation options.`));
    }
}

function* doFollowUpReportNewVisitReferralRecommendationAcceptedRequest() {
    try {
        yield put(getFollowUpReporNewVisitReferralRecommendationAcceptedRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.NewVisitReferralRecommendationAccepted);
        yield put(getFollowUpReporNewVisitReferralRecommendationAcceptedRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReporNewVisitReferralRecommendationAcceptedRequestError());
        yield put(notifyError(`Cannot fetch new visit / referral recommendation accepted options.`));
    }
}

function* doFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest() {
    try {
        yield put(getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.ReasonNewVisitReferralRecommendationNotAccepted);
        yield put(getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReponewVisitReferralRecommendationNotAcceptedRequestError());
        yield put(notifyError(`Cannot fetch new visit / referral recommendation not accepted options.`));
    }
}

function* doFollowUpReportDischargedRequestOptionsRequest() {
    try {
        yield put(getFollowUpReportDischargedRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.Discharged);
        yield put(getFollowUpReportDischargedRequestSuccess(options));
    } catch (e) {
        yield put(getFollowUpReportDischargedRequestError());
        yield put(notifyError(`Cannot fetch new visit / referral recommendation not accepted options.`));
    }
}

function* watchFollowUpReportPatientAttendedVisitOptionsRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportPatientAttendedVisitOptionsRequest, doFollowUpReportPatientAttendedVisitOptionsRequest);
}

function* watchFollowUpReportDischargedRequestOptionsRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportDischargedRequest, doFollowUpReportDischargedRequestOptionsRequest);
}


function* watchFollowUpReportRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getReportRequest, doFollowUpReportRequest);
}

function* watchFollowUpReportFollowUpContactOutcomeRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportFollowUpContactOutcome, doFollowUpReporttFollowUpContactOutcomeRequest);
}

function* watchFollowUpReportNewVisitReferralRecommendationRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendation, doFollowUpReportNewVisitReferralRecommendationRequest);
}

function* watchFollowUpReportNewVisitReferralRecommendationAcceptedRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationAcceptedRequest, doFollowUpReportNewVisitReferralRecommendationAcceptedRequest);
}

function* watchFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.getFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest, doFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest);
}

function* watchFollowUpReportUpdateFollowUpReportRequest() {
    yield takeLatest(EFollowUpRelatedReportActions.updateFollowUpReportRequest, doFollowUpReportUpdateFollowUpReportRequest);
}

const followUpReportSagas = [
    fork(watchFollowUpReportRequest),
    fork(watchFollowUpReportUpdateFollowUpReportRequest),
    fork(watchFollowUpReportFollowUpContactOutcomeRequest),
    fork(watchFollowUpReportDischargedRequestOptionsRequest),
    fork(watchFollowUpReportPatientAttendedVisitOptionsRequest),
    fork(watchFollowUpReportNewVisitReferralRecommendationRequest),
    fork(watchFollowUpReportNewVisitReferralRecommendationAcceptedRequest),
    fork(watchFollowUpReportNewVisitReferralRecommendationNotAcceptedRequest),
];

export default followUpReportSagas;