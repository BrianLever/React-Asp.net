import { Action } from 'redux';
import { ESharedActions, IActionPayload } from '..';

export const getListBranchLocationsRequest = (): Action => ({
    type: ESharedActions.getLocationsRequest,
});

export const getListBranchLocationsRequestStart = (): Action => ({
    type: ESharedActions.getLocationsRequestStart,
});

export const getListBranchLocationsRequestError = (): Action => ({
    type: ESharedActions.getLocationsRequestError,
});

export type TBranchLocationsItemResponse = {
    ID: number;
    Name: string;
};

export const getListBranchLocationsRequestSuccess = (list: Array<TBranchLocationsItemResponse>): IActionPayload  => {
    return ({
        type: ESharedActions.getLocationsRequestSuccess, payload: { list } 
    })
}

export const getScreeningProfileListRequest = (): Action => ({ type: ESharedActions.getScreeningProfileListRequest });
export const getScreeningProfileListRequestStart = (): Action => ({ type: ESharedActions.getScreeningProfileListRequestStart });
export const getScreeningProfileListRequestError = (): Action => ({ type: ESharedActions.getScreeningProfileListRequestError });

export type TScreeningProfileItemResponse = {
    ID: number;
    Name: string;
}

export const getScreeningProfileListRequestSuccess = (list: Array<TScreeningProfileItemResponse>): IActionPayload  => {
    return ({
        type: ESharedActions.getScreeningProfileListRequestSuccess, payload: { list } 
    })
}

export const setCreateDateCustomOrGPRA = (): Action => {
    return ({ type: ESharedActions.setCreateDateCustomOrGPRA })
}