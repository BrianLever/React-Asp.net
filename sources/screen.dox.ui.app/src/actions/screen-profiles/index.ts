import { IActionPayload, EScreenProfileActions } from "..";
import { IScreenProfilesState } from "../../states/screen-profiles";
import { Action } from 'redux';

export interface IScreenProfilesResponseItem {
    ID: number;
    Name: string;
    Description: string;
}

export interface IScreenProfilesResponse {
    Items: Array<IScreenProfilesResponseItem>;
    TotalCount: number;
}

export interface IScreenProfilesRequestItem {
    OrderBy: string;
    StartRowIndex: number;
    MaximumRows: number;
    FilterByName: string;
}

export interface IScreenProfileCreateRequestItem {
    Name: string;
    Description: string;
}

export interface IScreeningProfileMinimunAgeResponseItem {
    Name: string;
    IsHidden: boolean;
    ScreeningSectionID: string;
    MinimalAge: number;
    IsEnabled: boolean;
    LastModifiedDateUTC: string;
}

export interface IScreenProfileFrequencyResponseItem {
    Name: string;
    Description: string | null;
    ID: string;
    Frequency: number;
    LastModifiedDateUTC: string;
}

export interface IScreenProfileFrequencyListItem {
    ID: number;
    Name: string;
}

export interface IScreenProfileAgeGroupsItem {
    PrimarySectionID: string;
    AlternativeSectionID: string;
    DependentSectionIDs: Array<string>;
}

export const getScreenProfileListRequestStart = (): Action => 
({ type: EScreenProfileActions.getScreenProfileListRequestStart })

export const getScreenProfileListRequest = (): Action => 
({ type: EScreenProfileActions.getScreenProfileListRequest })

export const getScreenProfileListRequestSuccess = (payload: IScreenProfilesResponse) => 
({ type: EScreenProfileActions.getScreenProfileListRequestSuccess, payload })

export const changeCurrentPageRequest = (page: number): IActionPayload  => {
    return ({
        type: EScreenProfileActions.changeCurrentPageRequst, payload: { page } 
    })
}

export const setCurrentPage = (page: number): IActionPayload => {
    return ({
        type: EScreenProfileActions.setCurrentPage, payload: { page }
    })
}

export const changeActiveSortObjectAction = (key: string, direction: string): IActionPayload => {
    return ({
        type: EScreenProfileActions.changeActiveSort, payload: { key, direction } 
    })
}

export const changeActiveSortDirectionAction = (direction: string): IActionPayload => {
    return ({
        type: EScreenProfileActions.changeActiveDirection, payload: { direction } 
    })
}

export const changeActiveSortKeyAction = (key: string): IActionPayload => {
    return ({
        type: EScreenProfileActions.changeActiveKey, payload: { key } 
    })
}

export const setFilterByNameAction = (name: string): IActionPayload => {
    return ({
        type: EScreenProfileActions.setFilterByName, payload: { name }
    })
}

export const clearSearchParamsRequest = (): Action => {
    return ({
        type: EScreenProfileActions.clearSearchParamsRequest
    })
}

export const setCreateScreenProfileName = (name: string) => {
    return ({
        type: EScreenProfileActions.setCreateScreenProfileName, payload: { name }
    })
}

export const setCreateScreenProfileDescription = (description: string) => {
    return ({
        type: EScreenProfileActions.setCreateScreenProfileDescription, payload: { description }
    })
}

export const createNewScreenProfileRequest = (): Action => ({
    type: EScreenProfileActions.createNewScreenProfileRequest
})

export const setNewScreenProfileLoading = (value: boolean): IActionPayload => 
({ type: EScreenProfileActions.setNewScreenProfileLoading, payload: { value }});

export const getScreenProfileByIdRequest = (id: number): IActionPayload  => 
({ type: EScreenProfileActions.getScreenProfileByIdRequest, payload: { id } });

export const setScreenProfileId = (id: number): IActionPayload => ({
    type: EScreenProfileActions.setScreenProfileId,  payload: { id }
})

export const updateScreenProfileRequest = (): Action => ({ type: EScreenProfileActions.updateScreenProfileRequest });

export const deleteScreenProfileRequest = (): Action => ({ type: EScreenProfileActions.deleteScreenProfileRequest });

export const screenProfileMinimumAgeListRequestStart = (): Action => ({ type: EScreenProfileActions.screenProfileMinimumAgeListRequestStart });

export const screenProfileMinimumAgeListRequestError = (): Action => ({ type: EScreenProfileActions.screenProfileMinimumAgeListRequestError });

export const screenProfileMinimumAgeListRequestSuccess = (screeningMinimumAgeList:  Array<IScreeningProfileMinimunAgeResponseItem>): IActionPayload => ({
    type: EScreenProfileActions.screenProfileMinimumAgeListRequestSuccess, payload: { screeningMinimumAgeList }
})

export const screenProfileMinimumAgeListRequest = (): Action => ({ type: EScreenProfileActions.screenProfileMinimumAgeListRequest });

export const setScreenProfileSelectedEditOption = (id: number): IActionPayload => ({
    type: EScreenProfileActions.selectedScreenProfileEditOption, payload: { id }
})

export const screenProfileMinimumAgeUpdateRequest = (): Action => ({
    type: EScreenProfileActions.screenProfileMinimumAgeUpdateRequest
})

export const screenProfileFrequencyListRequest = (): Action => ({
    type: EScreenProfileActions.screenProfileFrequencyListRequest
})

export const screenProfileFrequencyListRequestStart = (): Action => ({
    type: EScreenProfileActions.screenProfileFrequencyListRequestStart
})

export const screenProfileFrequencyListRequestSuccess = (screenProfileFrequencyList: Array<IScreenProfileFrequencyResponseItem>): IActionPayload => ({
    type: EScreenProfileActions.screenProfileFrequencyListRequestSuccess, payload: { screenProfileFrequencyList }
})

export const screenProfileFrequencyListRequestError= (): Action => ({
    type: EScreenProfileActions.screenProfileFrequencyListRequestError
})

export const screenProfileDefaultFrequencyListSuccess = (payload: Array<IScreenProfileFrequencyListItem>): IActionPayload => ({
    type: EScreenProfileActions.screenProfileDefaultFrequencyListSuccess, payload
})

export const screenProfileFrequencyUpdateRequest = (): Action => ({ type: EScreenProfileActions.screenProfileFrequencyUpdateRequest })

export const screenProfileAgeGroupsListRequestSuccess = (payload: Array<IScreenProfileAgeGroupsItem>): IActionPayload => ({
    type: EScreenProfileActions.screenProfileAgeGroupsListRequestSuccess, payload
})