import { IBranchLocationItemResponse } from 'actions/branch-locations';
import { 
    EManageUsersActions,
    IActionPayload
} from '../../actions';
import { IManageUsersResponseItem, IManageUsersUser } from '../../actions/manage-users';
import { TBranchLocationsItemResponse } from '../../actions/shared';


export interface IManageUsersState {
    users: IManageUsersResponseItem[];
    isUsersLoading: boolean;
    currentPage: number;
    totals: number;
    orderKey: string;
    orderDirection: string;
    selectedLocationId: number | null;
    locations: Array<TBranchLocationsItemResponse>;
    user: IManageUsersUser;
    isUserCreateLoading: boolean;
    userGroups: Array<string>;
    isUserDetailsLoading: boolean;
    selectedUserId: number | null;
}   

export const IManageUsersInitState: IManageUsersState = {
    users: [],
    isUsersLoading: false,
    currentPage: 1,
    totals: 0,
    orderKey: 'FirstName',
    orderDirection: 'ASC',
    selectedLocationId: null,
    locations: [],
    user: {
        UserName: '',
        Password: '',
        FirstName: '',
        LastName: '',
        MiddleName: '',
        ContactPhone: '',
        StateCode: '',
        City: '',
        AddressLine1: '',
        AddressLine2: '',
        PostalCode: '',
        RoleName: '',
        BranchLocationID: null,
        Comments: '',
        Email: '',
        ConfirmPassword: '',
        IsBlock: false,
    },
    isUserCreateLoading: false,
    userGroups: [],
    selectedUserId: null,
    isUserDetailsLoading: false,
}



const manageUsersReducer = (state: IManageUsersState = IManageUsersInitState, action: IActionPayload) => {
    switch(action.type) {
        case EManageUsersActions.getManageUsersListRequestStart:
            return {
                ...state,
                isUsersLoading: true
            }
        case EManageUsersActions.getManageUsersListRequestError:
            return {
                ...state,
                isUsersLoading: false
            }
        case EManageUsersActions.getManageUsersListRequestSuccess:
            return {
                ...state,
                isUsersLoading: false,
                users: action.payload.Items,
                totals: action.payload.TotalCount
            }
        case EManageUsersActions.setManageUsersLocations:
            return {
                ...state,
                locations: action.payload,
            }
        case EManageUsersActions.setManageUsersCurrentPage:
            return {
                ...state,
                currentPage: action.payload.currentPage,
            }
        case EManageUsersActions.setManageUsersOrderDirection:
            return {
                ...state,
                orderDirection: action.payload.orderDirection,
            }
        case EManageUsersActions.setManageUsersOrderKey:
            return {
                ...state,
                orderKey: action.payload.orderKey,
            }
        case EManageUsersActions.setManageUsrsSelectedLocationId:
            return {    
                ...state,
                selectedLocationId: action.payload.selectedLocationId
            }
        case EManageUsersActions.setManageUsersUser:
            return {
                ...state,
                user: action.payload
            }
        case EManageUsersActions.manageUsersCreateRequestStart:
            return {
                ...state,
                isUserCreateLoading: true
            }
        case EManageUsersActions.manageUsersCreateRequestError:
            return {
                ...state,
                isUserCreateLoading:  false
            }
        case EManageUsersActions.manageUsersCreateRequestSuccess:
            return {
                ...state,
                isUserCreateLoading: false,
            }
        case EManageUsersActions.setManageUsersGroups:
            return {
                ...state,
                userGroups: action.payload
            }
        case EManageUsersActions.manageUsersDetailRequestStart:
            return {
                ...state,
                isUserDetailsLoading: true
            }
        case EManageUsersActions.manageUsersDetailRequestError:
            return {
                ...state,
                isUserDetailsLoading: false,
            }
        case EManageUsersActions.manageUsersDetailRequestSuccess:
            return {
                ...state,
                isUserDetailsLoading: false,
                user: action.payload
            }
        case EManageUsersActions.setManageUsersSelectedUserId:
            return {
                ...state,
                selectedUserId: action.payload.selectedUserId
            }
        default: return state;
    }
}

export default manageUsersReducer;