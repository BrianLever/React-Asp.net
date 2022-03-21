import { Action } from 'redux';
import { EBranchLocationsActions, IActionPayload } from '..';

export const getBranchLocationsRequest = (): Action => ({ type: EBranchLocationsActions.getBranchLocationsListRequest });
export const getBranchLocationsRequestStart = (): Action => ({ type: EBranchLocationsActions.getBranchLocationsListRequestStart });
export const getBranchLocationsRequestError = (): Action => ({ type: EBranchLocationsActions.getBranchLocationsListRequestError });

export interface IBranchLocationItemRequest {
    OrderBy?: string;
    StartRowIndex?: number;
    MaximumRows?: number;
    ScreeningProfileId?: number;
    FilterByName?: string;
    ShowDisabled?: boolean;
}

export interface IBranchLocationItemResponse {
    BranchLocationID: number;
    Name: string;
    Description: string;
    Disabled: boolean;
    ScreeningProfileID: number;
    ScreeningProfileName: string;
}

export interface IBranchLocationResponse {
    Items: Array<IBranchLocationItemResponse>;
    TotalCount: number;
}

export const getBranchLocationsRequestSuccess = (payload: IBranchLocationResponse): IActionPayload => 
({ type: EBranchLocationsActions.getBranchLocationsListRequestSuccess, payload });

export const changeActiveSortObjectAction = (key: string, direction: string): IActionPayload => {
    return ({
        type: EBranchLocationsActions.changeActiveSort, payload: { key, direction } 
    })
}

export const changeActiveSortDirectionAction = (direction: string): IActionPayload => {
    return ({
        type: EBranchLocationsActions.changeActiveDirection, payload: { direction } 
    })
}

export const changeActiveSortKeyAction = (key: string): IActionPayload => {
    return ({
        type: EBranchLocationsActions.changeActiveKey, payload: { key } 
    })
}

export const setCurrentPage = (page: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.setCurrentPage, payload: { page } 
    })
}  
export const changeCurrentPageRequest = (page: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.changeCurrentPageRequst, payload: { page } 
    })
}

export const setLocationNameKey = (name: string): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.setFilterLocationNameKey, payload: { name } 
    })
}

export const setScreeningProfileId = (id: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.setScreeningProfileId, payload: { id } 
    })
}

export const setFilterBranchLocationId = (id: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.setFilterBranchLocationId, payload: { id } 
    })
}

export const setShowDisabled = (value: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.setFilterShowDisabled, payload: { value } 
    })
}

export const resetBranchLocationsFilterRequest = (): Action => ({ type: EBranchLocationsActions.resetBranchLocationsFilterRequest });
export const resetKioskFilter = (): Action => ({ type: EBranchLocationsActions.resetBranchLocationsFilter });

// Create new branch actions
export const createBranchLocationName = (value: string): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.createBranchLocationName, payload: { value } 
    })
}

export const createBranchLocationScreenProfile = (value: number): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.createBranchLocationScreenProfile, payload: { value } 
    })
}

export const createBranchLocationDescription = (value: string): IActionPayload  => {
    return ({
        type: EBranchLocationsActions.createBranchLocationDescription, payload: { value } 
    })
}

export interface ICreateUpdateBranchLocationRequest {
    Name?: string;
    Description?: string;
    Disabled?: boolean;
    ScreeningProfileID?: number;
}

export interface ICreateUpdateBranchLocationResponse {
    BranchLocationID: number;
    Name: string;
    Description: string;
    Disabled: boolean;
    ScreeningProfileID: number;
    ScreeningProfileName: string;
}

export const createNewBranchLocationRequest = (): Action  => 
({ type: EBranchLocationsActions.createNewBranchLocationRequest });
export const createNewBranchLocationRequestStart = (): Action  => 
({ type: EBranchLocationsActions.createNewBranchLocationRequestStart });
export const createNewBranchLocationRequestError = (): Action  => 
({ type: EBranchLocationsActions.createNewBranchLocationRequestError });
export const createNewBranchLocationRequestSuccess = (branch: ICreateUpdateBranchLocationResponse): IActionPayload  => 
({ type: EBranchLocationsActions.createNewBranchLocationRequestSuccess, payload: { branch } });

export const getBranchLocationByIdRequest = (id: number): IActionPayload  => 
({ type: EBranchLocationsActions.getBranchLocationByIdRequest, payload: { id } });
export const getBranchLocationByIdRequestStart = (): Action  => 
({ type: EBranchLocationsActions.getBranchLocationByIdRequestStart });
export const getBranchLocationByIdRequestError = (): Action  => 
({ type: EBranchLocationsActions.getBranchLocationByIdRequestStart });
export const getBranchLocationByIdRequestSuccess = (branch: ICreateUpdateBranchLocationResponse): IActionPayload  => 
({ type: EBranchLocationsActions.getBranchLocationByIdRequestSuccess, payload: { branch } });

export const updateBranchLocationRequest = (disable?: boolean): IActionPayload  => 
({ type: EBranchLocationsActions.updateBranchLocationRequest, payload: { disable } });
export const updateBranchLocationRequestStart = (): Action  => 
({ type: EBranchLocationsActions.updateBranchLocationRequestStart });
export const updateBranchLocationRequestError = (): Action  => 
({ type: EBranchLocationsActions.updateBranchLocationRequestError });
export const updateBranchLocationRequestSuccess = (): Action  => 
({ type: EBranchLocationsActions.updateBranchLocationRequestSuccess });

export const deleteBranchLocationRequest = (): Action  => 
({ type: EBranchLocationsActions.deleteBranchLocationRequest });
export const deleteBranchLocationRequestStart = (): Action  => 
({ type: EBranchLocationsActions.deleteBranchLocationRequestStart });
export const deleteBranchLocationRequestError = (): Action  => 
({ type: EBranchLocationsActions.deleteBranchLocationRequestError });
export const deleteBranchLocationRequestSuccess = (): Action  => 
({ type: EBranchLocationsActions.deleteBranchLocationRequestSuccess });

export const setCurrentBranchLocation = (id: number): IActionPayload  => 
({ type: EBranchLocationsActions.setCurrentBranchLocation, payload: { id } });

export const setNewBranchLocationLoading = (value: boolean): IActionPayload => 
({ type: EBranchLocationsActions.setNewBranchLocationLoading, payload: { value }});

export const setBranchLocationDisabled = (value: boolean): IActionPayload => 
({ type: EBranchLocationsActions.setBranchLocationDisabled, payload: { value }});

export const setBranchLocationAutoStatus = (): Action => ({
    type: EBranchLocationsActions.setBranchLocationListAutoStatus
})

export const branchLocationAutoRequest = (): Action => ({
    type: EBranchLocationsActions.branchLocationListAutoRequest
})