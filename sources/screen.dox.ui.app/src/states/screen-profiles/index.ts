import { 
    IActionPayload, EScreenProfileActions 
} from '../../actions';
import { IScreenProfilesResponseItem, IScreeningProfileMinimunAgeResponseItem, 
    IScreenProfileFrequencyResponseItem, IScreenProfileFrequencyListItem,
    IScreenProfileAgeGroupsItem
} from '../../actions/screen-profiles';

export interface IScreenProfilesState {
    screenProfileList: Array<IScreenProfilesResponseItem>;
    isScreenProfileListLoading: boolean;
    sortKey: string;
    sortDirection: string;
    StartRowIndex: number;
    MaximumRows: 25;
    FilterByName: string;
    totalScreenProfiles: number;
    currentPage: number;
    createScreenProfileName: string;
    createScreenProfileDescription: string;
    isCreateScreenProfileLoading: boolean;
    screenProfileId: number;
    screeningMinimumAgeList: Array<IScreeningProfileMinimunAgeResponseItem>;
    isScreeningMinimumAgeListLoading: boolean;
    selectedScreenProfileEditOption: number;
    isScreenProfileFrequencyListLoading: boolean;
    screenProfileFrequencyList: Array<IScreenProfileFrequencyResponseItem>;
    screenProfileDefaultFrequencyList: Array<IScreenProfileFrequencyListItem>;
    screenProfileAgeGroupsList: Array<IScreenProfileAgeGroupsItem>;
}

export const screenProfilesInitState: IScreenProfilesState = {
    screenProfileList: [],
    isScreenProfileListLoading: false,
    sortKey: 'Name',
    sortDirection: 'asc',
    StartRowIndex: 0,
    MaximumRows: 25,
    FilterByName: "",
    totalScreenProfiles: 0,
    currentPage: 1,
    createScreenProfileName: '',
    createScreenProfileDescription: '',
    isCreateScreenProfileLoading: false,
    screenProfileId: 0,
    screeningMinimumAgeList: [],
    isScreeningMinimumAgeListLoading: false,
    selectedScreenProfileEditOption: 0,
    isScreenProfileFrequencyListLoading: false,
    screenProfileFrequencyList: [],
    screenProfileDefaultFrequencyList: [],
    screenProfileAgeGroupsList: []
}

const screenProfilesReducer = (state: IScreenProfilesState = screenProfilesInitState, action: IActionPayload) => {console.log(action)
    switch(action.type) {
        case EScreenProfileActions.getScreenProfileListRequestStart:
            return {
                ...state,
                isScreenProfileListLoading: true,
            }
        case EScreenProfileActions.getScreenProfileListRequestSuccess:
            return {
                ...state,
                screenProfileList: action.payload.Items,
                isScreenProfileListLoading: false,
                totalScreenProfiles: action.payload.TotalCount
            }
        case EScreenProfileActions.setCurrentPage:
            return {
                ...state,
                currentPage: action.payload.page
            }
        case EScreenProfileActions.changeActiveKey:
            return {
                ...state,
                sortKey: action.payload.key,
            }
        case EScreenProfileActions.changeActiveDirection:
            return {
                ...state,
                sortDirection: action.payload.direction,
            }
        case EScreenProfileActions.setFilterByName:
            return {
                ...state,
                FilterByName: action.payload.name
            }
        case EScreenProfileActions.clearSearchParamsRequest:
            return {
                ...state,
                FilterByName: screenProfilesInitState.FilterByName,
                totalScreenProfiles: 0,
                currentPage: 1
            }
        case EScreenProfileActions.setCreateScreenProfileName:
            return {
                ...state,
                createScreenProfileName: action.payload.name
            }
        case EScreenProfileActions.setCreateScreenProfileDescription:
            return {
                ...state,
                createScreenProfileDescription: action.payload.description
            }
        case EScreenProfileActions.setNewScreenProfileLoading:
            return {
                ...state,
                isCreateScreenProfileLoading: action.payload.value
            }
        case EScreenProfileActions.setScreenProfileId:
            return {
                ...state, 
                screenProfileId: action.payload.id
            }
        case EScreenProfileActions.screenProfileMinimumAgeListRequestStart:
            return {
                ...state,
                isScreeningMinimumAgeListLoading: true,
            }
        case EScreenProfileActions.screenProfileMinimumAgeListRequestError:
            return {
                ...state,
                isScreeningMinimumAgeListLoading: false,
            }
        case EScreenProfileActions.screenProfileMinimumAgeListRequestSuccess:
            return {
                ...state,
                isScreeningMinimumAgeListLoading: false,
                screeningMinimumAgeList: action.payload.screeningMinimumAgeList
            }
        case EScreenProfileActions.selectedScreenProfileEditOption:
            return {
                ...state,
                selectedScreenProfileEditOption: action.payload.id
            }
        case EScreenProfileActions.screenProfileFrequencyListRequestStart:
            return {
                ...state,
                isScreenProfileFrequencyListLoading: true
            }
        case EScreenProfileActions.screenProfileFrequencyListRequestError:
            return {
                ...state,
                isScreenProfileFrequencyListLoading: false
            }
        case EScreenProfileActions.screenProfileFrequencyListRequestSuccess:
            return {
                ...state,
                isScreenProfileFrequencyListLoading: false,
                screenProfileFrequencyList: action.payload.screenProfileFrequencyList
            }
        case EScreenProfileActions.screenProfileDefaultFrequencyListSuccess:
            return {
                ...state,
                screenProfileDefaultFrequencyList: action.payload
            }
        case EScreenProfileActions.screenProfileAgeGroupsListRequestSuccess:
            return {
                ...state,
                screenProfileAgeGroupsList: action.payload
            }
        default: return state;
    }
}

export default screenProfilesReducer;