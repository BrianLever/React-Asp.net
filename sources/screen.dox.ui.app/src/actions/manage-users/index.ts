import { TBranchLocationsItemResponse } from 'actions/shared';
import { Action } from 'redux';
import { EManageUsersActions, IActionPayload } from '..';


export interface IManageUsersResponseItem {
    UserID: number;
    FirstName: string | null;
    LastName: string | null;
    MiddleName: string | null;
    ContactPhone: string | null;
    RoleName: string | null;
    BranchLocationID: number | null;
    BranchLocationName: string | null;
    Email: string | null;
    UserName: string | null;
    IsBlock: boolean;
}

export interface IManageUsersRequestItem {
    BranchLocationID: number | null;
    OrderBy: string | null;
    StartRowIndex: number | null;
    MaximumRows: number;
}

export interface IManageUsersUser {
    UserName: string;
    Password: string;
    FirstName: string;
    LastName: string;
    MiddleName: string | null;
    ContactPhone: string | null;
    StateCode: string;
    City: string | null;
    AddressLine1: string | null;
    AddressLine2: string | null;
    PostalCode: string | null;
    RoleName: string;
    BranchLocationID: number | null;
    Comments: string | null;
    Email: string | null;
    ConfirmPassword: string;
    IsBlock: boolean;
}

export const getManageUsersListRequest = (): Action => ({
    type: EManageUsersActions.getManageUsersListRequest,
});

export const getManageUsersListRequestStart = (): Action => ({
    type: EManageUsersActions.getManageUsersListRequestStart,
});

export const getManageUsersListRequestError = (): Action => ({
    type: EManageUsersActions.getManageUsersListRequestError,
});

export const getManageUsersListRequestSuccess = (payload: { Items: IManageUsersResponseItem[], TotalCount: number }): IActionPayload => ({
    type: EManageUsersActions.getManageUsersListRequestSuccess, payload
});

export const setManageUsersLocations = (payload: TBranchLocationsItemResponse[]): IActionPayload => ({
    type: EManageUsersActions.setManageUsersLocations, payload
})

export const setManageUsersCurrentPage = (currentPage: number): IActionPayload => ({
    type: EManageUsersActions.setManageUsersCurrentPage, payload: { currentPage }
})

export const setManageUsersOrderDirection = (orderDirection: string): IActionPayload => ({
    type: EManageUsersActions.setManageUsersOrderDirection, payload: { orderDirection }
})

export const setManageUsersOrderKey = (orderKey: string): IActionPayload => ({
    type: EManageUsersActions.setManageUsersOrderKey, payload: { orderKey }
})

export const setManageUsersSelectedLocationId = (selectedLocationId: number | null) => ({
    type: EManageUsersActions.setManageUsrsSelectedLocationId, payload: { selectedLocationId }
})

export const manageUsersChangeActiveSortObjectAction = (key: string, direction: string): IActionPayload => {
    return ({
        type: EManageUsersActions.manageUsersChangeActiveSort, payload: { key, direction } 
    })
}

export const setManageUsersUser = (payload: IManageUsersUser): IActionPayload => ({
    type: EManageUsersActions.setManageUsersUser, payload
})

export const manageUsersCreateRequest = ():Action => ({
    type: EManageUsersActions.manageUsersCreateRequest
})

export const manageUsersCreateRequestStart = ():Action => ({
    type: EManageUsersActions.manageUsersCreateRequestStart
})

export const manageUsersCreateRequestError = ():Action => ({
    type: EManageUsersActions.manageUsersCreateRequestError
})

export const manageUsersCreateRequestSuccess = ():Action => ({
    type: EManageUsersActions.manageUsersCreateRequestSuccess
})

export const setManageUsersGroups = (payload: string[]) => ({
    type:EManageUsersActions.setManageUsersGroups, payload
})

export const manageUsersDetailRequest = (userId: number): IActionPayload => ({
    type: EManageUsersActions.manageUsersDetailRequest, payload: { userId }
})

export const manageUsersDetailRequestStart = ():Action => ({
    type: EManageUsersActions.manageUsersDetailRequestStart
})

export const manageUsersDetailRequestError = ():Action => ({
    type: EManageUsersActions.manageUsersDetailRequestError
})

export const manageUsersDetailRequestSuccess = (payload: IManageUsersUser) => ({
    type: EManageUsersActions.manageUsersDetailRequestSuccess, payload
})

export const setManageUsersSelectedUserId = (selectedUserId: number | null):IActionPayload => ({
    type: EManageUsersActions.setManageUsersSelectedUserId, payload: { selectedUserId }
})

export const manageUsersUpdateRequest = (userId: number):IActionPayload => ({
    type: EManageUsersActions.manageUsersUpdateRequest, payload: { userId }
})

export const manageUsersUpdateRequestStart = ():Action => ({
    type: EManageUsersActions.manageUsersUpdateRequestStart
})

export const manageUsersUpdateRequestError = ():Action => ({
    type: EManageUsersActions.manageUsersUpdateRequestError
})

export const manageUsersUpdateRequestSuccess = ():Action => ({
    type: EManageUsersActions.manageUsersUpdateRequestSuccess
})

export const manageUsersBlockRequest = (userId: number):IActionPayload => ({
    type: EManageUsersActions.manageUsersBlockRequest, payload: { userId }
})

export const manageUsersUnBlockRequest = (userId: number):IActionPayload => ({
    type: EManageUsersActions.manageUsersUnBlockRequest, payload: { userId }
})

export const manageUsersDeleteRequest = (userId: number):IActionPayload => ({
    type: EManageUsersActions.manageUsersDeleteRequest, payload: { userId }
})