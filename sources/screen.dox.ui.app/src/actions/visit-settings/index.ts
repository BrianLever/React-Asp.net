import { Action } from 'redux';
import { IActionPayload, EVisitSettingsActions } from '..';

export interface VisitSettingsResponseItem {
    Id: string,
    Name: string,
    CutScore: number,
    SectionScoreHint: string,
    IsEnabled: boolean,
    LastModifiedDateUTC: string
}

export const getVisitSettingsRequest = (): Action => ({ type: EVisitSettingsActions.getVisitSettingsRequest });
export const getVisitSettingsRequestStart = (): Action => ({ type: EVisitSettingsActions.getVisitSettingsRequestStart });
export const getVisitSettingsRequestError = (): Action => ({ type: EVisitSettingsActions.getVisitSettingsRequestError });
export const getVisitSettingsRequestSuccess = (payload: Array<VisitSettingsResponseItem>): IActionPayload => ({ type: EVisitSettingsActions.getVisitSettingsRequestSuccess, payload });
export const updateVisitSettingsRequest = (): Action => ({ type: EVisitSettingsActions.updateVisitSettingsRequest });
export const updateVisitSettingsRequestStart = (): Action => ({ type: EVisitSettingsActions.updateVisitSettingsRequestStart });
export const updateVisitSettingsRequestError = (): Action => ({ type: EVisitSettingsActions.updateVisitSettingsRequestError });
