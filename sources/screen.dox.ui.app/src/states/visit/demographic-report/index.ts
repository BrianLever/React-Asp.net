import { 
    IActionPayload, EVisitDemographicReportActions,
} from '../../../actions';
import { 
    IVisitDemographicReportResponse, TVisitDemographicItem 
} from '../../../actions/visit/demographic-report';

export interface IVisitDemographicReportState {
    isVisitDemographicLoading: boolean;
    demographicReport: IVisitDemographicReportResponse | null;
    raceOptions: Array<TVisitDemographicItem>; 
    genderOptions: Array<TVisitDemographicItem>; 
    sexualOrientationOptions: Array<TVisitDemographicItem>; 
    educationLevelOptions: Array<TVisitDemographicItem>; 
    maritalStatusOptions: Array<TVisitDemographicItem>;
    livingOnReservationOptions: Array<TVisitDemographicItem>;
    militaryExperienceOptions: Array<TVisitDemographicItem>;
    selectedMilitaryExperienceOptions: Array<TVisitDemographicItem>;
    selectedRace: TVisitDemographicItem | null;
    selectedGender: TVisitDemographicItem | null;
    selectedSexualOrientation: TVisitDemographicItem | null;
    selectedEducationLevel: TVisitDemographicItem | null;
    selectedMeritalStatus: TVisitDemographicItem | null;
    selecteLivingOnReservation: TVisitDemographicItem | null;
    tribalAffiliationValue: string;
    countyOfResidenceValue: string;
    tribalAffiliationArrayValues: Array<{id: number, value: string}>;
    isTribalAffiliationLoading: boolean;
    countyOfResidenceArrayValues: Array<string>;
    isCountyOfResidenceArrayLoading: boolean;
    completeDate: string;
    staffNameCompleted: string;
    
}

const visitDemographicReportInitState: IVisitDemographicReportState =  {
    isVisitDemographicLoading: false,
    demographicReport: null,
    raceOptions: [],
    genderOptions: [],
    sexualOrientationOptions: [],
    educationLevelOptions: [],
    maritalStatusOptions: [],
    livingOnReservationOptions: [],
    selectedRace: null,
    selectedGender: null,
    selectedSexualOrientation: null,
    selectedEducationLevel: null,
    selectedMeritalStatus: null,
    selecteLivingOnReservation: null,
    tribalAffiliationValue: '',
    countyOfResidenceValue: '',
    militaryExperienceOptions:  [],
    selectedMilitaryExperienceOptions:  [],
    tribalAffiliationArrayValues: [],
    isTribalAffiliationLoading: false,
    countyOfResidenceArrayValues: [],
    isCountyOfResidenceArrayLoading: false,
    completeDate: '',
    staffNameCompleted: '',
};

const visitDemographicReportReducer = (
    state: IVisitDemographicReportState = visitDemographicReportInitState, action: IActionPayload
) => {
    switch(action.type) {
        case EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestError:
        case EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestError:
        case EVisitDemographicReportActions.getVisitDemographicRaceRequestError:
        case EVisitDemographicReportActions.getVisitDemographicGenderRequestError:
        case EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestError:
        case EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestError:
        case EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestError:
            return {
                ...state,
                isVisitDemographicLoading: false,
            }
        case EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicRaceRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicGenderRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestStart:
        case EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestStart:
            return {
                ...state,
                isVisitDemographicLoading: true,
            }
        case EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestSuccess:
            return {
                ...state,
                militaryExperienceOptions: action.payload.options
            }
        case EVisitDemographicReportActions.changeVisitDemographicMilitaryExperience:
            return {
                ...state,
                selectedMilitaryExperienceOptions: action.payload.options
            }
        case EVisitDemographicReportActions.setVisitDemographicTribalAffiliation:
            return {
                ...state,
                tribalAffiliationValue: action.payload.value,
            }
        case EVisitDemographicReportActions.setVisitDemographicCountyOfResidence:
            return {
                ...state,
                countyOfResidenceValue: action.payload.value,
            }
        case EVisitDemographicReportActions.setVisitDemographicLivingOnReservation:
            return {
                ...state,
                selecteLivingOnReservation: action.payload.reservation,
            }
        case EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestSuccess:
            return {
                ...state,
                livingOnReservationOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.setSelectedVisitDemographicEducationLevel:
            return {
                ...state,
                selectedEducationLevel: action.payload.education,
            }
        case EVisitDemographicReportActions.setVisitDemographicMeritalStatus:
            return {
                ...state,
                selectedMeritalStatus: action.payload.status,
            }
        case EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestSuccess:
            return {
                ...state,
                maritalStatusOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestSuccess:
            return {
                ...state,
                educationLevelOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.setSelectedVisitDemographicGender:
            return {
                ...state,
                selectedGender: action.payload.gender,
            }
        case EVisitDemographicReportActions.setSelectedVisitDemographicSexualOrientation:
            return {
                ...state,
                selectedSexualOrientation: action.payload.orientation,
            }
        case EVisitDemographicReportActions.getVisitDemographicGenderRequestSuccess:
            return {
                ...state,
                isVisitDemographicLoading: false,
                genderOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestSuccess:
            return {
                ...state,
                isVisitDemographicLoading: false,
                sexualOrientationOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.getVisitDemographicReportByIdRequestStart:
            return {
                ...state,
                isVisitDemographicLoading: true,
                demographicReport: null,
            }
        case EVisitDemographicReportActions.getVisitDemographicReportByIdRequestError:
            return {
                ...state,
                isVisitDemographicLoading: false,
            }
        case EVisitDemographicReportActions.setVisitDemographicSelectedRace:
            return {
                ...state,
                selectedRace: action.payload.race,
            }
        case EVisitDemographicReportActions.getVisitDemographicRaceRequestSuccess:
            return {
                ...state,
                isVisitDemographicLoading: false,
                raceOptions: action.payload.options,
            }
        case EVisitDemographicReportActions.getVisitDemographicReportByIdRequestSuccess:
            return {
                ...state,
                isVisitDemographicLoading: false,
                demographicReport: action.payload.report,
            }
        case EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequestSuccess:
            return {
                ...state,
                isTribalAffiliationLoading: false,
                tribalAffiliationArrayValues: action.payload
            }
        case EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequestStart:
            return {
                ...state,
                isTribalAffiliationLoading: true
            }

        case EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequestSuccess:
            return {
                ...state,
                isCountyOfResidenceArrayLoading: false,
                countyOfResidenceArrayValues: action.payload
            }
        case EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequestStart:
            return {
                ...state,
                isCountyOfResidenceArrayLoading: true
            }
        case EVisitDemographicReportActions.getVisitDemoGraphicCompletedDate: 
            return {
                ...state,
                completeDate: action.payload.completeDate
            }
        case EVisitDemographicReportActions.getVisitDemoGraphicStaffNameComplted:
            return {
                ...state,
                staffNameCompleted: action.payload.staffName
            }
        default: return state;
    }
}

export default visitDemographicReportReducer;
