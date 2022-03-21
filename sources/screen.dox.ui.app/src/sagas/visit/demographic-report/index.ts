import { call, fork, put, select, takeLatest } from 'redux-saga/effects';
import getVisitDrugChoiceOptions, { EVisitReportOptions } from '../../../api/calls/get-visit-report-options';
import { 
    EVisitDemographicReportActions, IActionPayload, 
} from '../../../actions';
import { 
    getVisitDemographicReportRequestStart, getVisitDemographicReportRequestError, 
    IVisitDemographicReportResponse, getVisitDemographicReportRequestSuccess, TVisitDemographicItem,
    getVisitDemographicRaceRequestStart, getVisitDemographicRaceRequestError, getVisitDemographicRaceRequestSuccess, 
    setSelectedVisitDemographicRace,updateVisitDemographicReportRequestStart, updateVisitDemographicReportRequestError,
    updateVisitDemographicReportRequestSuccess, IVisitDemographicReportUpdateRequast, getVisitDemographicGenderRequestStart, 
    getVisitDemographicGenderRequestError, getVisitDemographicGenderRequestSuccess, getVisitDemographicSexualOrientationRequestStart, 
    getVisitDemographicSexualOrientationRequestError, getVisitDemographicSexualOrientationRequestSuccess, 
    setSelectedVisitDemographicSexualOrientation, setSelectedVisitDemographicGender, getVisitDemographicEducationLevelRequestStart, 
    getVisitDemographicEducationLevelRequestError, getVisitDemographicEducationLevelRequestSuccess, getVisitDemographicMeritalStatusRequestStart, 
    getVisitDemographicMeritalStatusRequestError, getVisitDemographicMeritalStatusRequestSuccess, 
    setSelectedVisitDemographicEducationLevel, setSelectedVisitDemographicMeritalStatus, getVisitDemographicLivingOnReservationRequestStart, 
    getVisitDemographicLivingOnReservationRequestError, getVisitDemographicLivingOnReservationRequestSuccess, setSelectedVisitDemographicLivingOnReservation, 
    setSelectedVisitDemographicCountyOfResidence, setSelectedVisitDemographicTribalAffiliation, changeVisitDemographicMilitaryExperience, 
    getVisitDemographicMilitaryExperienceRequestStart, getVisitDemographicMilitaryExperienceRequestError, getVisitDemographicMilitaryExperienceRequestSuccess,
    getVisitDemographicTribalAffiliationRequestStart, getVisitDemographicTribalAffiliationRequest, getVisitDemographicTribalAffiliationRequestSuccess,
    getVisitDemographicCountyOfResidenceArrayRequestStart, getVisitDemographicCountyOfResidenceArrayRequest, getVisitDemographicCountyOfResidenceArrayRequestSuccess, getVisitDemoGraphicCompletedDate, getVisitDemoGraphicStaffNameComplted
} from '../../../actions/visit/demographic-report';

import getDemographicById from '../../../api/calls/get-demographic';
import updateDemographicReportById from '../../../api/calls/update-demographic-report';
import getTypeheadTribe from '../../../api/calls/get-typehead-tribe';
import getTypeheadCountyOfResidence from '../../../api/calls/get-typehead-county-residence';

import { 
    getSelectedVisitDemographicGenderSelector, getSelectedVisitDemographicLivingOnReservationSelector, getSelectedVisitDemographicRaceSelector, 
    getSelectedVisitDemographicSexualOrientationSelector, getSelectedVisitDemographicTribalAffiliationSelector, 
    getSelectedVisitDemographicEducationLevelSelector, getSelectedVisitDemographicMaritalStatusSelector, getSelectedVisitDemographicCountyOfResidenceValueSelector, 
    geSelectedVisitDemographicMilitaryExperienceSelector
} from '../../../selectors/visit/demographic-report';
import { notifyError, notifySuccess } from 'actions/settings';

function* doVisitDemographicReportRequest(action: IActionPayload) {
    try {
        const { reportId } = action.payload || {};
        yield put(getVisitDemographicReportRequestStart());
        const result: IVisitDemographicReportResponse = yield call(getDemographicById, reportId);
        if (!!result) {
            yield put(getVisitDemographicReportRequestSuccess(result));
            const race = result.Race;
            yield put(setSelectedVisitDemographicRace(race));
            const orientation = result.SexualOrientation;
            yield put(setSelectedVisitDemographicSexualOrientation(orientation));
            const gender = result.Gender;
            yield put(setSelectedVisitDemographicGender(gender));
            const education = result.EducationLevel;
            yield put(setSelectedVisitDemographicEducationLevel(education));
            const maritalStatus = result.MaritalStatus;
            yield put(setSelectedVisitDemographicMeritalStatus(maritalStatus));
            const livingReservation = result.LivingOnReservation;
            yield put(setSelectedVisitDemographicLivingOnReservation(livingReservation));
            const countyOfResidence = result.CountyOfResidence;
            yield put(setSelectedVisitDemographicCountyOfResidence(countyOfResidence));
            const tribalAffiliation = result.TribalAffiliation;
            yield put(setSelectedVisitDemographicTribalAffiliation(tribalAffiliation));
            const militaryExperience = result.MilitaryExperience;
            yield put(changeVisitDemographicMilitaryExperience(militaryExperience));
            yield put(getVisitDemoGraphicCompletedDate(result.CompleteDate))
            yield put(getVisitDemoGraphicStaffNameComplted(result.StaffNameCompleted))
        } else {
            yield put(getVisitDemographicReportRequestError());
        }
    } catch (e) {
        yield put(getVisitDemographicReportRequestError());
    }
}

function* doVisitDemographicRaceRequest() {
    try {
        yield put(getVisitDemographicRaceRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.Race);
            if (!!options) {
                yield put(getVisitDemographicRaceRequestSuccess(options));
            } else {
                yield put(getVisitDemographicRaceRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicRaceRequestError());
    }
}

function* doVisitDemographicGenderRequest() {
    try {
        yield put(getVisitDemographicGenderRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.Gender);
            if (!!options) {
                yield put(getVisitDemographicGenderRequestSuccess(options));
            } else {
                yield put(getVisitDemographicGenderRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicGenderRequestError());
    }
}

function* doVisitDemographicSexualOrientationRequest() {
    try {
        yield put(getVisitDemographicSexualOrientationRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.SexualOrientation);
            if (!!options) {
                yield put(getVisitDemographicSexualOrientationRequestSuccess(options));
            } else {
                yield put(getVisitDemographicSexualOrientationRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicSexualOrientationRequestError());
    }
}

function* doVisitDemographicEducationLevelRequest() {
    try {
        yield put(getVisitDemographicEducationLevelRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.EducationLevel);
            if (!!options) {
                yield put(getVisitDemographicEducationLevelRequestSuccess(options));
            } else {
                yield put(getVisitDemographicEducationLevelRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicEducationLevelRequestError());
    }
}

function* doVisitDemographicMaritalStatusRequest() {
    try {
        yield put(getVisitDemographicMeritalStatusRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.MaritalStatus);
            if (!!options) {
                yield put(getVisitDemographicMeritalStatusRequestSuccess(options));
            } else {
                yield put(getVisitDemographicMeritalStatusRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicMeritalStatusRequestError());
    }
}

function* doVisitDemographicLivingOnReservationRequest() {
    try {
        yield put(getVisitDemographicLivingOnReservationRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.LivingOnReservation);
            if (!!options) {
                yield put(getVisitDemographicLivingOnReservationRequestSuccess(options));
            } else {
                yield put(getVisitDemographicLivingOnReservationRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicLivingOnReservationRequestError());
    }
}

function* doVisitDemographicMilitaryExperienceRequest() {
    try {
        yield put(getVisitDemographicMilitaryExperienceRequestStart());
        const options: Array<TVisitDemographicItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.MilitaryExperience);
            if (!!options) {
                yield put(getVisitDemographicMilitaryExperienceRequestSuccess(options));
            } else {
                yield put(getVisitDemographicMilitaryExperienceRequestError());
            }
    } catch (e) {
        yield put(getVisitDemographicMilitaryExperienceRequestError());
    }
}

function* doUpdateVisitDemographicReportRequest(action: IActionPayload) {
    try {
        const { reportId } = action.payload || {};
        yield put(updateVisitDemographicReportRequestStart());
        const requestBody: IVisitDemographicReportUpdateRequast = {};
        const selectedRace: TVisitDemographicItem = yield select(getSelectedVisitDemographicRaceSelector);
        const selectedGender:TVisitDemographicItem = yield select(getSelectedVisitDemographicGenderSelector);
        const selectedOrientation:TVisitDemographicItem = yield select(getSelectedVisitDemographicSexualOrientationSelector);
        const education:TVisitDemographicItem = yield select(getSelectedVisitDemographicEducationLevelSelector);
        const marital:TVisitDemographicItem = yield select(getSelectedVisitDemographicMaritalStatusSelector);
        const livingReservation:TVisitDemographicItem = yield select(getSelectedVisitDemographicLivingOnReservationSelector);
        const tribalAffiliation:string = yield select(getSelectedVisitDemographicTribalAffiliationSelector);
        const countyOfResidenceValue:string = yield select(getSelectedVisitDemographicCountyOfResidenceValueSelector);
        const militaryEperience: Array<TVisitDemographicItem> = yield select(geSelectedVisitDemographicMilitaryExperienceSelector);

        // validations of required properties
        if(!selectedRace || !selectedRace.Id) {
            yield put(notifyError(`"Race" is required.`));
            return;
        } else if(!selectedGender || !selectedGender.Id) {
            yield put(notifyError(`"Gender" is required.`));
            return;
        } else if(!selectedOrientation || !selectedOrientation.Id) {
            yield put(notifyError(`"Sexual Orientation" is required.`));
            return;
        } else if(!tribalAffiliation) {
            yield put(notifyError(`"Tribal affiliatio"n is required.`));
            return;
        } else if(!marital || !marital.Id) {
            yield put(notifyError(`"Marital Status" is required.`));
            return;
        } else if(!education || !education.Id) {
            yield put(notifyError(`"Education Level" is required.`));
            return;
        } else if(!livingReservation || !livingReservation.Id) {
            yield put(notifyError(`"Living "on" or "off" reservation" is required.`));
            return;
        } else if (!countyOfResidenceValue) {
            yield put(notifyError(`"County of residence" is required.`));
            return;
        } else if (!militaryEperience.length) {
            yield put(notifyError(`"Military experience" is required.`));
            return;
        }


        if (tribalAffiliation) {
            requestBody.TribalAffiliation = tribalAffiliation;
        }
        if (countyOfResidenceValue) {
            requestBody.CountyOfResidence = countyOfResidenceValue;
        }
        if (selectedRace && selectedRace.Id) {
            requestBody.Race = { Id: selectedRace.Id };
        }
        if (selectedGender && selectedGender.Id) {
            requestBody.Gender = { Id: selectedGender.Id };
        }
        if (selectedOrientation && selectedOrientation.Id) {
            requestBody.SexualOrientation = { Id: selectedOrientation.Id };
        }
        if (education && education.Id) {
            requestBody.EducationLevel = { Id: education.Id };
        }
        if (marital && marital.Id) {
            requestBody.MaritalStatus = { Id: marital.Id };
        }
        if (livingReservation && livingReservation.Id) {
            requestBody.LivingOnReservation = { Id: livingReservation.Id };
        }
        if (Array.isArray(militaryEperience)) {
            requestBody.MilitaryExperience = militaryEperience.map(d => ({ Id: d.Id }));
        }
        const result: IVisitDemographicReportResponse = yield call(updateDemographicReportById, reportId, requestBody);
        if (!!result) {
            yield put(notifySuccess(`Saved.`));
            yield put(updateVisitDemographicReportRequestSuccess());
        } else {
            yield put(notifyError(`Not Saved.`));
            yield put(updateVisitDemographicReportRequestError());
        }
    } catch (e) {
        yield put(updateVisitDemographicReportRequestError());
        yield put(notifyError(`Not Saved.`));
    }
}


function* doVisitDemographicTribalAffiliationRequest(action: IActionPayload) {
    try {
        yield put(getVisitDemographicTribalAffiliationRequestStart());
        const payload: Array<string> = yield call(getTypeheadTribe, action.payload.query);
        var data: Array<any> = payload.map((item, i) => ({ id: i, value: item }));
        yield put(getVisitDemographicTribalAffiliationRequestSuccess(data));
    } catch (e) {
        console.log(e)
    }
}

function* doVisitDemographicCountyOfResidenceArrayRequest(action: IActionPayload) {
    try {
        yield put(getVisitDemographicCountyOfResidenceArrayRequestStart());
        const countyOfResidencArray: Array<string> = yield call(getTypeheadCountyOfResidence, action.payload.countyQuery);
        yield put(getVisitDemographicCountyOfResidenceArrayRequestSuccess(countyOfResidencArray));
    } catch (e) {
        console.log(e)
    }
}

function* watchVisitDemographicReportRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicReportByIdRequest, doVisitDemographicReportRequest);
}

function* watchVisitDemographicRaceRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicRaceRequest, doVisitDemographicRaceRequest);
}

function* watchUpdateVisitDemographicReportRequest() {
    yield takeLatest(EVisitDemographicReportActions.updateVisitDemographicReportRequest, doUpdateVisitDemographicReportRequest);
}

function* watchVisitDemographicGenderRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicGenderRequest, doVisitDemographicGenderRequest);
}

function* watchVisitDemographicSexualOrientationRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequest, doVisitDemographicSexualOrientationRequest);
}

function* watchVisitDemographicEducationLevelRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicEducationLevelRequest, doVisitDemographicEducationLevelRequest);
}

function* watchVisitDemographicMaritalStatusRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequest, doVisitDemographicMaritalStatusRequest);
}

function* watchVisitDemographicLivingOnReservationRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequest, doVisitDemographicLivingOnReservationRequest);
}

function* watchVisitDemographicMilitaryExperienceRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequest, doVisitDemographicMilitaryExperienceRequest);
}

function* watchVisitDemographicTribalAffiliationRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequest, doVisitDemographicTribalAffiliationRequest);
}


function* watchVisitDemographicCountyOfResidenceArrayRequest() {
    yield takeLatest(EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequest, doVisitDemographicCountyOfResidenceArrayRequest);
}


const visitDemographicReportSagas = [ 
    fork(watchVisitDemographicCountyOfResidenceArrayRequest),
    fork(watchVisitDemographicRaceRequest),
    fork(watchVisitDemographicReportRequest),
    fork(watchVisitDemographicGenderRequest),
    fork(watchUpdateVisitDemographicReportRequest),
    fork(watchVisitDemographicMaritalStatusRequest),
    fork(watchVisitDemographicEducationLevelRequest),
    fork(watchVisitDemographicSexualOrientationRequest),
    fork(watchVisitDemographicMilitaryExperienceRequest),
    fork(watchVisitDemographicLivingOnReservationRequest),
    fork(watchVisitDemographicTribalAffiliationRequest),
];

export default visitDemographicReportSagas;
