import { Action } from 'redux';
import { IActionPayload, EAgeGroupsActions } from '..';


export interface ageGroupValueItem {
    Key: string,
    Value: string,
    Name: string,
    Description: string,
    RegExp: string,
    IsExposed: boolean,
}

export interface ageGroupsResponseItem {
    Value: ageGroupValueItem;
    DefaultValue: string;
    Labels: Array<string>;
}

export const getAgeGroupRequest = (): Action => ({ type: EAgeGroupsActions.getAgeGroupsRequest });
export const getAgeGroupRequestStart = (): Action => ({ type: EAgeGroupsActions.getAgeGroupsRequestStart });
export const getAgeGroupRequestError = (): Action => ({ type: EAgeGroupsActions.getAgeGroupsRequestError });
export const getAgeGroupRequestSuccess = (payload: ageGroupsResponseItem): IActionPayload => ({ type: EAgeGroupsActions.getAgeGroupsRequestSuccess, payload });
export const setAgeGroupValue = (value: string): IActionPayload => ({ type: EAgeGroupsActions.setAgeGroupsValue, payload: { value } });
export const updateAgeGroupRequest = (): Action => ({ type: EAgeGroupsActions.updateAgeGroupRequest });
export const updateAgeGroupRequestStart = (): Action => ({ type: EAgeGroupsActions.updateAgeGroupRequestStart });
export const updateAgeGroupRequestSuccess = (payload: ageGroupsResponseItem ): IActionPayload => ({ type: EAgeGroupsActions.updateAgeGroupRequestSuccess, payload });
export const updateAgeGroupRequestError = (): Action => ({ type: EAgeGroupsActions.updateAgeGroupRequestError });