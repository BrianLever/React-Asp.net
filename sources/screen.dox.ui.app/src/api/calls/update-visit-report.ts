import axios from  '../axios';
import { TOtherScreeningToolsItem } from '../../actions/visit/report';

export type TActionsRequestItem = {
    Id: number;
    Description: string;  
}

export type TUpdateVisitReport = {
    OtherScreeningTools?: Array<TOtherScreeningToolsItem>,
    TreatmentActions?: Array<TActionsRequestItem>,
    NewVisitReferralRecommendation?: TActionsRequestItem,
    NewVisitReferralRecommendationAccepted?: { Id: number; },
    ReasonNewVisitReferralRecommendationNotAccepted?: { Id: number; },
    NewVisitDate?: string;
    Discharged?: { Id: number; };
    ThirtyDatyFollowUpFlag?: boolean;
    FollowUpDate?: string | null,
    Notes?: string | Array<any>;
    DrugOfChoice?: {
        ScreeningSectionID: "DOCH",
        Answers: Array<{ QuestionID: number; AnswerValue: number; }>
    },
}

const updateVisitReport = async (data: TUpdateVisitReport, currentReport: string): Promise<string> => {
   return await axios.instance.put(`visit/${currentReport}`, data);
}

export default updateVisitReport;