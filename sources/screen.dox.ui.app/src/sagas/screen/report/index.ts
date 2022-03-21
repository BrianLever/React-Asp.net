import { call, fork, put, takeLatest } from 'redux-saga/effects';
import { EScreenReportActions } from '../../../actions';
import { 
    deleteScreeningReportRequestError,
    deleteScreeningReportRequestStart, getScreenReportDefinitionError, getScreenReportDefinitionStart,
    getScreenReportDefinitionSuccess, IDefenitionResponse, IScreeningReportSections, setScreeningReportObejct,
} from '../../../actions/screen/report';
import {
    IScreeningReport, getScreeningReportRequestSuccess, getScreeningReportRequestStart, 
    getScreeningReportRequestError, getScreenReportVisitRequest, getScreenReportVisitRequestStart, getScreenReportVisitRequestSuccess
} from '../../../actions/screen/report';
import { IActionPayload } from '../../../actions';
import getScreenDefinition from '../../../api/calls/get-screen-definition';
import getScreeningReport from '../../../api/calls/get-screening-report';
import deleteScreeningReportById from '../../../api/calls/delete-screen-report-by-id';
import  getScreenReportVisitId from '../../../api/calls/get-screen-report-visit';
import { notifyError, notifySuccess } from '../../../actions/settings';
import { ERouterUrls } from '../../../router';

function* doScreenReportDefinition() {
    try {
        yield put(getScreenReportDefinitionStart());
        const definition: IDefenitionResponse = yield call(getScreenDefinition);
        yield put(getScreenReportDefinitionSuccess(definition));
    } catch (e) {
        yield put(getScreenReportDefinitionError());
    }
}

function* doGetScreeningReportRequest(action: IActionPayload) {
    try {
        const { reportId } = action.payload || {};
        yield put(getScreeningReportRequestStart());
        const report: IScreeningReport = yield call(getScreeningReport, reportId);
        yield put(getScreeningReportRequestSuccess(report));
        if (report && Array.isArray(report.Sections) && report.Sections.length) {
            const currentScreeningReportObject: {[k: string]: IScreeningReportSections} = {};
            for (const d of report.Sections) {
                currentScreeningReportObject[d.ScreeningSectionID] = d;
            }
            yield put(setScreeningReportObejct(currentScreeningReportObject));
        } else {
            // exception
        }
    } catch (e) {
        yield put(getScreeningReportRequestError());
        // TODO: error handler
    }
}

function* doScreenReportDeleteRequest(action: IActionPayload) {
    const { id, history } = action.payload || {};
    try {
        yield put(deleteScreeningReportRequestStart());
        yield call(deleteScreeningReportById, id);
        yield put(notifySuccess(`Deleted`));
        history && history.push(ERouterUrls.SCREEN_LIST);
    } catch(e) {
        yield put(deleteScreeningReportRequestError());
        yield put(notifyError(`Failed to load Visit by id ${id}`));
    }
}

function* doScreenReportVisitRequest(action: IActionPayload) {
    const { reportId } = action.payload || {};
    try {
        yield put(getScreenReportVisitRequestStart());
        const visitId: IScreeningReport = yield call(getScreenReportVisitId, reportId);
        yield put(getScreenReportVisitRequestSuccess(visitId));
    } catch(e) {
       console.log(e)
    }
}



function* watchGetScreeningReportRequest() {
    yield takeLatest(EScreenReportActions.getScreeningReportRequest, doGetScreeningReportRequest);
}

function* watchScreenReportDefinitionRequest() {
    yield takeLatest(EScreenReportActions.getScreenReportDefinitionRequest, doScreenReportDefinition);
}

function* watchScreenReportDeleteRequest() {
    yield takeLatest(EScreenReportActions.deleteScreeningReportRequest, doScreenReportDeleteRequest);
}

function* watchScreenReportVisitRequest() {
    yield takeLatest(EScreenReportActions.getScreenReportVisitId, doScreenReportVisitRequest);
}



const screenReportSagas = [
    fork(watchGetScreeningReportRequest),
    fork(watchScreenReportDeleteRequest),
    fork(watchScreenReportDefinitionRequest),
    fork(watchScreenReportVisitRequest)
];

export default screenReportSagas;