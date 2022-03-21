import axios from  '../axios';

export interface IFollowUpReportUpdateRequest {
    VisitRefferalRecommendation?: string;
    PatientAttendedVisit?: {
        Id: number;
    },
    FollowUpContactDate?: string;
    FollowUpContactOutcome?: {
        Id: number;
    },
    Discharged?: {
        Id: number;
    },
    NewVisitDate?:string;
    NewVisitReferralRecommendation?: {
        Description: string;
        Id: number;
    },
    NewVisitReferralRecommendationAccepted?: {
        Id: number;
    },
    ReasonNewVisitReferralRecommendationNotAccepted?: {
        Id: number;
    },
    ThirtyDatyFollowUpFlag?: boolean;
    FollowUpDate?: string;
    Notes?: string;
}

const putFollowUpReport = async (id: number, params: IFollowUpReportUpdateRequest): Promise<string> => {
   return await axios.instance.put(`followup/${id}`, params);
}

export default putFollowUpReport;