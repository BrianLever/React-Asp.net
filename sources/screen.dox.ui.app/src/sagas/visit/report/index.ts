import { call, fork, put, select, takeLatest } from 'redux-saga/effects';
import { 
    EVisitReportActions, IActionPayload, 
} from '../../../actions';
import { 
    getVisitReportRequestStart, getVisitReportRequestError, getVisitReportRequestSuccess,
    IVisitReportResponse, getVisitDrugChoiceRequestStart, getVisitDrugChoiceRequestError, 
    getVisitDrugChoiceRequestSuccess, TChoiceItem, initVisitReportOtherScreeningToolsItem, 
    getVisitTritmentOptionsRequestStart, getVisitTritmentOptionsRequestError, 
    getVisitTritmentOptionsRequestSuccess, getVisitNewReferalRecomendationRequestError, 
    getVisitNewReferalRecomendationRequestSuccess, getVisitNewReferalRecomendationRequestStart, 
    getVisitNewReferalRecomendationAcceptedRequestStart, getVisitNewReferalRecomendationAcceptedRequestError, 
    getVisitNewReferalRecomendationAcceptedRequestSuccess, getVisitNewReferalRecomendationNotAcceptedRequestStart,
    getVisitNewReferalRecomendationNotAcceptedRequestError, getVisitNewReferalRecomendationNotAcceptedRequestSuccess, 
    getVisitDischargedRequestStart, getVisitDischargedRequestError, getVisitDischargedRequestSuccess, 
    updateVisitReportRequestStart, updateVisitReportRequestError, updateVisitReportRequestSuccess, 
    TOtherScreeningToolsItem, initVisitReportTritmentActionToolsItem, setVisitReferralRecommendationDescription, 
    setVisitReferralRecommendationId, setVisitReferralRecommendationAccepted, setVisitReferralRecommendationNotAccepted, 
    setVisitDischarged, setVisitDate, setVisitFollowUpDate, setVisitFollowUp, setVisitNotes, setVisitDrugPrimaryItem, 
    setVisitDrugSecondaryItem, setVisitDrugTertiaryItem, setVisitReportIsCompleted, setFollowUPVisitArray
} from '../../../actions/visit/report';
import { 
    getOtherScreeningToolsSelector, getVisitReferralRecommendationDescriptionSelector, 
    getVisitReferralRecommendationIdSelector, getTritmentActionToolsSelector,
    getVisitReferralRecommendationAcceptedSelector, getVisitReferralRecommendationNotAcceptedSelector, 
    getVisitDateSelector, getVisitDischargedSelector, getVisitDrugPrimaryItemSelector, getVisitDrugSecondaryItemSelector, 
    getVisitDrugTertiaryItemSelector, getVisitFollowUpSelector, getVisitFollowUpDateSelector, getVisitFollowNoteSelector
} from '../../../selectors/visit/report';
import getVisitReportByID from '../../../api/calls/get-visit-by-id';
import getVisitDrugChoiceOptions, { EVisitReportOptions } from '../../../api/calls/get-visit-report-options';
import updateVisitReport, { TUpdateVisitReport } from '../../../api/calls/update-visit-report';
import getFollowUpVisitById from 'api/calls/get-followup-visit-by-id';
import { notifyError, notifySuccess } from '../../../actions/settings';
import { IFollowUpInnerInnerItem } from 'actions/follow-up';

function* doVisitReportRequest(action: IActionPayload) {
    const { reportId } = action.payload || {};
    try {
        yield put(getVisitReportRequestStart());
        const report: IVisitReportResponse = yield call(getVisitReportByID, reportId);
        if (report) {
            yield put(getVisitReportRequestSuccess(report));
            yield put(setVisitReportIsCompleted(report.IsCompleted));
            if (Array.isArray(report.OtherScreeningTools) && report.OtherScreeningTools.length) {
                yield put(initVisitReportOtherScreeningToolsItem(report.OtherScreeningTools));
            }
            if (Array.isArray(report.TreatmentActions) && report.TreatmentActions.length) {
                yield put(initVisitReportTritmentActionToolsItem(report.TreatmentActions));
            }
            if (report.NewVisitReferralRecommendation) {
                yield put(setVisitReferralRecommendationDescription(report.NewVisitReferralRecommendation.Description));
                yield put(setVisitReferralRecommendationId(report.NewVisitReferralRecommendation.Id));
            }
            if (report.NewVisitReferralRecommendationAccepted) {
                yield put(setVisitReferralRecommendationAccepted(report.NewVisitReferralRecommendationAccepted.Id));
            }
            if (report.ReasonNewVisitReferralRecommendationNotAccepted) {
                yield put(setVisitReferralRecommendationNotAccepted(report.ReasonNewVisitReferralRecommendationNotAccepted.Id));
            }
            if (report.NewVisitDate) {
                yield put(setVisitDate(report.NewVisitDate));
            }
            if (report.Discharged) {
                yield put(setVisitDischarged(report.Discharged.Id));
            }
            if (report.FollowUpDate) {
                yield put(setVisitFollowUpDate(report.FollowUpDate));
            }
            if (report.ThirtyDatyFollowUpFlag) {
                yield put(setVisitFollowUp(report.ThirtyDatyFollowUpFlag));
            }
            if (report.Notes) {
                try {
                    var notes = JSON.parse(report.Notes)
                } catch (e) {
                    var notes = JSON.parse('');
                }
                yield put(setVisitNotes(notes['ops']));
            }

            if (report.Result && Array.isArray(report.Result.Sections)) {
                const DOCH = report.Result.Sections.find(d => d.ScreeningSectionID === 'DOCH');
                if (DOCH && Array.isArray(DOCH.Answers)) {
                    const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.DrugOfChoice);
                    const primary = DOCH.Answers[0];
                    const secondary = DOCH.Answers[1];
                    const ternery = DOCH.Answers[2];
                    if (Array.isArray(options)) {
                        if (primary) {
                            const primaryResult = options.find(c => c.Id === primary.AnswerValue);
                            if (primaryResult) {
                                yield put(setVisitDrugPrimaryItem(primaryResult))
                            } else {
                                yield put(setVisitDrugPrimaryItem(options[0]))
                            }
                        }
                        if (secondary) {
                            const secondaryResult = options.find(c => c.Id === secondary.AnswerValue);
                            if (secondaryResult) {
                                yield put(setVisitDrugSecondaryItem(secondaryResult))
                            } else {
                                yield put(setVisitDrugPrimaryItem(options[0]))
                            }
                        }
                        if (ternery) {
                            const ternaryResult = options.find(c => c.Id === ternery.AnswerValue);
                            if (ternaryResult) {
                                yield put(setVisitDrugTertiaryItem(ternaryResult))
                            } else {
                                yield put(setVisitDrugPrimaryItem(options[0]))
                            }
                        }
                    }
                }
            }
        }
    } catch (e) {
        console.log(e, 'error message');
        yield put(getVisitReportRequestError())
        yield put(notifyError(`Cannot load visit report ${reportId}`));
    }
}

function* doVisitDrugChoiceOptionsRequest() {
    try {
        yield put(getVisitDrugChoiceRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.DrugOfChoice);
        yield put(getVisitDrugChoiceRequestSuccess(options));
    } catch (e) {
        yield put(getVisitDrugChoiceRequestError());
        yield put(notifyError(`Cannot load drug report options`));
    }
}

function* doVisitTritmentOptionsRequest() {
    try {
        yield put(getVisitTritmentOptionsRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.TreatmentAction);
        yield put(getVisitTritmentOptionsRequestSuccess(options));
    } catch (e) {
        yield put(getVisitTritmentOptionsRequestError());
        yield put(notifyError(`Cannot load treatment report options`));
    }
}

function* doVisitNewReferalRecomendationRequest() {
    try {
        yield put(getVisitNewReferalRecomendationRequestStart());
        const options: Array<TChoiceItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.NewVisitReferralRecommendation);
        yield put(getVisitNewReferalRecomendationRequestSuccess(options));
    } catch (e) {
        yield put(getVisitNewReferalRecomendationRequestError());
        yield put(notifyError(`Cannot load "New Visit Referral Recommendation" report options`));
    }
}

function* doVisitNewReferalRecomendationAcceptedRequest() {
    try {
        yield put(getVisitNewReferalRecomendationAcceptedRequestStart());
        const options: Array<TChoiceItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.NewVisitReferralRecommendationAccepted);
        yield put(getVisitNewReferalRecomendationAcceptedRequestSuccess(options));
    } catch (e) {
        yield put(getVisitNewReferalRecomendationAcceptedRequestError());
        yield put(notifyError(`Cannot load "New Visit Referral Recommendation Accepted" report options`));
    }
}

function* doVisitNewReferalRecomendationNotAcceptedRequest() {
    try {
        yield put(getVisitNewReferalRecomendationNotAcceptedRequestStart());
        const options: Array<TChoiceItem> = yield call(
            getVisitDrugChoiceOptions, EVisitReportOptions.ReasonNewVisitReferralRecommendationNotAccepted);
        yield put(getVisitNewReferalRecomendationNotAcceptedRequestSuccess(options));
    } catch (e) {
        yield put(getVisitNewReferalRecomendationNotAcceptedRequestError());
        yield put(notifyError(`Cannot load "Reason New Visit Referral Recommendation Not Accepted" report options`));
    }
}

function* doVisitDischargedRequest() {
    try {
        yield put(getVisitDischargedRequestStart());
        const options: Array<TChoiceItem> = yield call(getVisitDrugChoiceOptions, EVisitReportOptions.Discharged);
        yield put(getVisitDischargedRequestSuccess(options));
    } catch (e) {
        yield put(getVisitDischargedRequestError());
    }
}


function* doVisitSaveReportChangesRequest(action: IActionPayload) {
    try {
        const { reportId } = action.payload || {};
        yield put(updateVisitReportRequestStart());
        // TODO: check 
        const otherEvoluation: Array<TOtherScreeningToolsItem> = yield select(getOtherScreeningToolsSelector);
        const tritments: Array<TChoiceItem> =  yield select(getTritmentActionToolsSelector);
        const referalDiscription: string = yield select(getVisitReferralRecommendationDescriptionSelector);
        const referalId: number = yield select(getVisitReferralRecommendationIdSelector);
        const referralAccepted: number = yield select(getVisitReferralRecommendationAcceptedSelector);
        const referralNotAccepted: number = yield select(getVisitReferralRecommendationNotAcceptedSelector);
        const visitDate: string = yield select(getVisitDateSelector);
        const discharged: number = yield select(getVisitDischargedSelector);
        const primaryOption: TChoiceItem = yield select(getVisitDrugPrimaryItemSelector);
        const secondaryOption: TChoiceItem = yield select(getVisitDrugSecondaryItemSelector);
        const ternaryOption: TChoiceItem = yield select(getVisitDrugTertiaryItemSelector);
        const followUp: boolean = yield select(getVisitFollowUpSelector);
        const followUpDate: string = yield select(getVisitFollowUpDateSelector);
        const note: Array<any> = yield select(getVisitFollowNoteSelector);
       
        const dataToUpdate: TUpdateVisitReport = {};
        dataToUpdate.ThirtyDatyFollowUpFlag = followUp;
        if (followUpDate) {
            dataToUpdate.FollowUpDate = followUpDate;
        }
        if (note) {
            var noteData = {
                ops: note
            }
            dataToUpdate.Notes = JSON.stringify(noteData);
        }
        if (Array.isArray(otherEvoluation) && otherEvoluation.length) {
            dataToUpdate.OtherScreeningTools = otherEvoluation;
        }

      
        const isTreatmentDesc = !!tritments[0] && tritments[0].Description;
        
        if (Array.isArray(tritments) && tritments.length) {
            dataToUpdate.TreatmentActions = tritments.map(d => ({ "Id": d.Id+1, "Description": d.Description || '' }))
        }


        if (referalId) {
            dataToUpdate.NewVisitReferralRecommendation = {
                Id: referalId, Description: referalDiscription,
            }
        }
        if (referralAccepted) {
            dataToUpdate.NewVisitReferralRecommendationAccepted = { Id: referralAccepted };
        }
        if (!isTreatmentDesc) {
            yield put(notifyError(`Description is required when Treatment action is selected.`));
            return;
        }
        if (!Array.isArray(tritments) || !tritments[0].Description) {
            yield put(notifyError('"Treatment Action(s) Delivered*" cannot be empty'));
            return;
        }
        if (!referralAccepted) {
            yield put(notifyError('"New Visit/Referral Recommendation*" cannot be empty'));
            return;
        }
        if (referralNotAccepted === -1) {
            yield put(notifyError('"New Visit/Referral Recommendation Accepted*" cannot be empty'));
            return;
        }
        if (referralNotAccepted) {
            dataToUpdate.ReasonNewVisitReferralRecommendationNotAccepted = { Id: referralNotAccepted }
        }
        if (visitDate) {
            dataToUpdate.NewVisitDate = visitDate;
        }
        if (discharged) {
            dataToUpdate.Discharged = { Id: discharged };
        }
        if (primaryOption || secondaryOption || ternaryOption) {
            dataToUpdate.DrugOfChoice = {
                ScreeningSectionID: 'DOCH',
                Answers: [
                    {
                        AnswerValue: primaryOption.Id,
                        QuestionID: 1,
                    },
                    {
                        AnswerValue: secondaryOption.Id,
                        QuestionID: 2,
                    },
                    {
                        AnswerValue: ternaryOption.Id,
                        QuestionID: 3,
                    }
                ]
            }
        }
        const options: string = yield call(updateVisitReport, dataToUpdate, reportId);
        if (options !== 'Visit has been updated successfully.') {
            yield put(updateVisitReportRequestError());
            yield put(notifyError('Not Saved.'));
        } else {
            yield put(updateVisitReportRequestSuccess());
            yield put(notifySuccess('Saved.'));
        }
    } catch (e) {
        yield put(updateVisitReportRequestError());
        yield put(notifyError(`Report was not saved, try to change conditions`));
    }
}

function* doFollowUpVisitArrayRequest(action: IActionPayload) {
    const { reportId }  = action.payload || {};
    const response: Array<IFollowUpInnerInnerItem> = yield call(getFollowUpVisitById, reportId);
    yield put(setFollowUPVisitArray(response));
}

function* watchVisitReportRequest() {
    yield takeLatest(EVisitReportActions.getVisitReportByIdRequest, doVisitReportRequest);
}

function* watchVisitDrugChoiceOptionsRequest() {
    yield takeLatest(EVisitReportActions.getDrugChoiceOptionRequest, doVisitDrugChoiceOptionsRequest);
}

function* watchVisitTritmentOptionsRequest() {
    yield takeLatest(EVisitReportActions.getDrugChoiceOptionRequest, doVisitTritmentOptionsRequest);
}

function* watchVisitReferalRecomendationRequest() {
    yield takeLatest(EVisitReportActions.getVisitNewReferalRecomendationRequest, doVisitNewReferalRecomendationRequest);
}

function* watchVisitReferalRecomendationAcceptedRequest() {
    yield takeLatest(EVisitReportActions.getVisitNewReferalRecomendationAcceptedRequest, doVisitNewReferalRecomendationAcceptedRequest);
}

function* watchVisitReferalRecomendationNotAcceptedRequest() {
    yield takeLatest(EVisitReportActions.getVisitNewReferalRecomendationNotAcceptedRequest, doVisitNewReferalRecomendationNotAcceptedRequest);
}

function* watchVisitDischargedRequest() {
    yield takeLatest(EVisitReportActions.getVisitDischargedOptionsRequest, doVisitDischargedRequest);
}

function* watchVisitSaveReportChangesRequest() {
    yield takeLatest(EVisitReportActions.updateVisitReportRequest, doVisitSaveReportChangesRequest);
}

function* watchFollowUpVisitByIdRequest() {
    yield takeLatest(EVisitReportActions.getFollowUPVisitArrayRequest, doFollowUpVisitArrayRequest);
}


const visitReportSagas = [
    fork(watchVisitReportRequest),
    fork(watchVisitDischargedRequest),
    fork(watchVisitTritmentOptionsRequest),
    fork(watchVisitSaveReportChangesRequest),
    fork(watchVisitDrugChoiceOptionsRequest),
    fork(watchVisitReferalRecomendationRequest),
    fork(watchVisitReferalRecomendationAcceptedRequest),
    fork(watchVisitReferalRecomendationNotAcceptedRequest),
    fork(watchFollowUpVisitByIdRequest)
];

export default visitReportSagas;
