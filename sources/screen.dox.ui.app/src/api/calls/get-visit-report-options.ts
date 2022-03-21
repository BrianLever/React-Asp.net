import { TChoiceItem } from '../../actions/visit/report';
import axios from  '../axios';

export enum EVisitReportOptions {
    Race = 'Race',
    Gender = 'Gender',
    Discharged = 'Discharged',
    DrugOfChoice = 'DrugOfChoice',
    MaritalStatus = 'MaritalStatus',
    EducationLevel = 'EducationLevel',
    TreatmentAction = 'TreatmentAction',
    SexualOrientation = 'SexualOrientation',
    MilitaryExperience = 'MilitaryExperience',
    LivingOnReservation = 'LivingOnReservation',
    PatientAttendedVisit = 'PatientAttendedVisit',
    FollowUpContactOutcome = 'FollowUpContactOutcome',
    NewVisitReferralRecommendation = 'NewVisitReferralRecommendation',
    NewVisitReferralRecommendationAccepted = 'NewVisitReferralRecommendationAccepted',
    ReasonNewVisitReferralRecommendationNotAccepted = 'ReasonNewVisitReferralRecommendationNotAccepted',
}


const getVisitDrugChoiceOptions = async (optionLabel: EVisitReportOptions)
: Promise<Array<TChoiceItem>> => {
   return await axios.instance.get(`lookupvisit/${optionLabel}`);
}

export default getVisitDrugChoiceOptions;