import { Action } from 'redux';
import { EVisitDemographicReportActions, IActionPayload } from '../../';

export type TVisitDemographicItem = {
    Id: number;
    Name: string;
    OrderIndex: number;
}

export type TVisitDemographicPatientInfo = {
    City: string;
    Phone: string;
    StateID: string;
    StateName: string;
    StreetAddress: string;
    ZipCode: string;
    Birthday: string;
    FirstName: string;
    LastName: string;
    MiddleName: string;
    FullName: string;
    Age: number;
}

export interface IVisitDemographicReportResponse {
    ScreeningDate: string;
    ScreeningResultID: number;
    LocationID: number;
    Race: TVisitDemographicItem;
    Gender: TVisitDemographicItem;
    SexualOrientation: TVisitDemographicItem;
    TribalAffiliation: string;
    MaritalStatus: TVisitDemographicItem;
    EducationLevel: TVisitDemographicItem;
    LivingOnReservation: TVisitDemographicItem;
    CountyOfResidence: string;
    MilitaryExperience: Array<TVisitDemographicItem>,
    LocationLabel: string;
    Result: {
        Sections: [],
        ID: number;
        PatientInfo: TVisitDemographicPatientInfo,
        ExportedToHRN: string;
        VisitID: number;
        CreatedDate: string;
        ExportDate: string;
        ExportedByFullName: string;
        ExportedToPatientID: string;
        ExportedToVisitDate: string;
        ExportedToVisitID: string;
        ExportedToVisitLocation: string;
        IsContactInfoEligableForExport: boolean;
        IsEligible4Export: boolean;
        IsPassedAnySection: boolean;
        KioskID: number;
        LocationID: number;
        LocationLabel: string;
        BhsVisitStatus: string;
        BhsVisitID: string;
        WithErrors: boolean;
        WithErrorsMessage: string;
        PatientDemographicsID: null
    },
    ID: number;
    StaffNameCompleted: string;
    CompleteDate: string;
    CreatedDate: string;
}

export const getVisitDemographicReportRequest = (reportId: number): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicReportByIdRequest,
    payload: { reportId }
});

export const getVisitDemographicReportRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicReportByIdRequestStart
});

export const getVisitDemographicReportRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicReportByIdRequestError
});

export const getVisitDemographicReportRequestSuccess = (report: IVisitDemographicReportResponse | null): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicReportByIdRequestSuccess,
    payload: { report }
});

export const getVisitDemographicRaceRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicRaceRequest
});

export const getVisitDemographicRaceRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicRaceRequestStart
});

export const getVisitDemographicRaceRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicRaceRequestError
});

export const getVisitDemographicRaceRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicRaceRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicRace = (race: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setVisitDemographicSelectedRace,
    payload: { race }
});

export interface IVisitDemographicReportUpdateRequast {
    Race?: {
        Id: number;
    },
    Gender?: {
        Id: number;
    },
    SexualOrientation?: {
        Id: number;
    },
    TribalAffiliation?: string;
    MaritalStatus?: {
        Id: number;
    },
    EducationLevel?: {
        Id: number;
    },
    LivingOnReservation?: {
        Id: number;
    },
    CountyOfResidence?: string;
    MilitaryExperience?: Array<{ Id: number; }>
}

export const updateVisitDemographicReportRequest = (reportId: string)
: IActionPayload => ({
    type: EVisitDemographicReportActions.updateVisitDemographicReportRequest,
    payload: { reportId }
});

export const updateVisitDemographicReportRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.updateVisitDemographicReportRequestStart
});

export const updateVisitDemographicReportRequestError = (): Action => ({
    type: EVisitDemographicReportActions.updateVisitDemographicReportRequestError
});

export const updateVisitDemographicReportRequestSuccess = (): Action => ({
    type: EVisitDemographicReportActions.updateVisitDemographicReportRequestSuccess
});


export const getVisitDemographicGenderRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicGenderRequest
});

export const getVisitDemographicGenderRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicGenderRequestStart
});

export const getVisitDemographicGenderRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicGenderRequestError
});

export const getVisitDemographicGenderRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicGenderRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicGender = (gender: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setSelectedVisitDemographicGender,
    payload: { gender }
});

export const getVisitDemographicSexualOrientationRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequest
});

export const getVisitDemographicSexualOrientationRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestStart
});

export const getVisitDemographicSexualOrientationRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestError
});

export const getVisitDemographicSexualOrientationRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicSexualOrientationRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicSexualOrientation = (orientation: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setSelectedVisitDemographicSexualOrientation,
    payload: { orientation }
});

export const getVisitDemographicEducationLevelRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicEducationLevelRequest
});

export const getVisitDemographicEducationLevelRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestStart
});

export const getVisitDemographicEducationLevelRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestError
});

export const getVisitDemographicEducationLevelRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicEducationLevelRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicEducationLevel = (education: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setSelectedVisitDemographicEducationLevel,
    payload: { education }
});

export const getVisitDemographicMeritalStatusRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequest
});

export const getVisitDemographicMeritalStatusRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestStart
});

export const getVisitDemographicMeritalStatusRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestError
});

export const getVisitDemographicMeritalStatusRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicMeritalStatusRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicMeritalStatus = (status: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setVisitDemographicMeritalStatus,
    payload: { status }
});

export const getVisitDemographicLivingOnReservationRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequest
});

export const getVisitDemographicLivingOnReservationRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestStart
});

export const getVisitDemographicLivingOnReservationRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestError
});

export const getVisitDemographicLivingOnReservationRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicLivingOnReservationRequestSuccess,
    payload: { options }
});

export const setSelectedVisitDemographicLivingOnReservation = (reservation: TVisitDemographicItem)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setVisitDemographicLivingOnReservation,
    payload: { reservation }
});

export const setSelectedVisitDemographicTribalAffiliation = (value: string)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setVisitDemographicTribalAffiliation,
    payload: { value }
});

export const setSelectedVisitDemographicCountyOfResidence = (value: string)
: IActionPayload => ({
    type: EVisitDemographicReportActions.setVisitDemographicCountyOfResidence,
    payload: { value }
});


export const getVisitDemographicMilitaryExperienceRequest = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequest
});

export const getVisitDemographicMilitaryExperienceRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestStart
});

export const getVisitDemographicMilitaryExperienceRequestError = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestError
});

export const getVisitDemographicMilitaryExperienceRequestSuccess = (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicMilitaryExperienceRequestSuccess,
    payload: { options }
});

export const changeVisitDemographicMilitaryExperience= (options: Array<TVisitDemographicItem>)
: IActionPayload => ({
    type: EVisitDemographicReportActions.changeVisitDemographicMilitaryExperience,
    payload: { options }
});

export const getVisitDemographicTribalAffiliationRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequestStart
})

export const getVisitDemographicTribalAffiliationRequestSuccess = (tribalAffiliationArray: Array<string>): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequestSuccess,
    payload: tribalAffiliationArray
})

export const getVisitDemographicTribalAffiliationRequest = (query: string): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicTribalAffiliationRequest,
    payload: { query }
})


export const getVisitDemographicCountyOfResidenceArrayRequestStart = (): Action => ({
    type: EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequestStart
})

export const getVisitDemographicCountyOfResidenceArrayRequestSuccess = (countyOfResidenceArray: Array<string>): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequestSuccess,
    payload: countyOfResidenceArray
})

export const getVisitDemographicCountyOfResidenceArrayRequest = (countyQuery: string): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemographicCountyOfResidenceArrayRequest,
    payload: { countyQuery }
})


export const getVisitDemoGraphicCompletedDate = (completeDate: string): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemoGraphicCompletedDate,
    payload: { completeDate }
})


export const getVisitDemoGraphicStaffNameComplted = (staffName: string): IActionPayload => ({
    type: EVisitDemographicReportActions.getVisitDemoGraphicStaffNameComplted,
    payload: { staffName }
})

