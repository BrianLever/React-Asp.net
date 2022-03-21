import { IRootState } from '../../../states';
import { IScreeningReportSections } from 'actions/screen/report';
import {ScreeningSectionDescriptor} from '../../../helpers/screeningSectionDescriptor';

const COMMON_EMPTY_RESPONSE = 'N/A';
const AUTOFILLED_BY_SCREENDOX = 'Autofilled by Screendox';

export const getCurrentScreeningReportSelector = (state: IRootState) => state.screenReport.currentScreeningReport;
export const getReportPatientLastName = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.LastName || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientFirstName = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.FirstName || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientMiddleName = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.MiddleName || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientDateOfBirth = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.Birthday || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientHRN = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.ExportedToHRN || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientMailingAddress = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.StreetAddress || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientCity = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.City || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientState = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.StateName || COMMON_EMPTY_RESPONSE;
}

export const getReportPatientStateId= (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.StateID || COMMON_EMPTY_RESPONSE;
}

export const getReportPatientZIP = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.ZipCode || COMMON_EMPTY_RESPONSE;
}
export const getReportPatientPhone = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.PatientInfo.Phone || COMMON_EMPTY_RESPONSE;
}
export const getReportLocation = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.LocationLabel || COMMON_EMPTY_RESPONSE;
}

export const getReportCreatedDateFormatted = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.CreatedDateLabel || COMMON_EMPTY_RESPONSE;
}

export const getReportExportDateFormatted = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.ExportDateLabel || COMMON_EMPTY_RESPONSE;
}
export const getReportCreatedByFullName = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.CreatedByFullName || AUTOFILLED_BY_SCREENDOX;
}

export const getReportExportedByFullName = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.ExportedByFullName || COMMON_EMPTY_RESPONSE;
}
export const getReportSRN = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report.ID.toString();
}

export const getScreenReportVisitID = (state: IRootState) => {
    const report = state.screenReport;
    if (!report) { return COMMON_EMPTY_RESPONSE; }
    return report;
}

type AnswerType = {
    text: string;
    value: boolean | 'NO_CLIENT_ANSWER';
}

export type TScreenMultiReportSection = { 
    mainQuestions: Array<TScreenMultiReportSectionItem>, 
    notMainQuestions: Array<TScreenMultiReportSectionItem>, 
    allQuestions: Array<TScreenMultiReportSectionItem>, 
    SectionHeader?: string,
    IsPhq2Mode?: boolean,
    IsGad2Mode?: boolean
};

export type TScreenMultiReportSectionItem = { 
    questionId: number,
    question: string, 
    preamble?: string,
    answers: Array<AnswerType>,
    isMain: boolean;
    score?: number;
};

export type TScreenReportSection = { 
    mainQuestions: Array<TScreenReportSectionItem>, 
    notMainQuestions: Array<TScreenReportSectionItem>, 
    allQuestions: Array<TScreenReportSectionItem>, 
};


export type TScreenReportSectionItem = { 
    questionId: number,
    question: string, 
    preamble?: string,
    answer: boolean | string | number | 'NO_CLIENT_ANSWER',
    answerText?: string;
    isMain: boolean;
    score?: number;
};

export type TSectionScore = {
    Score: number;
    ScoreLevel: number;
    ScoreLevelLabel: string;
    Indicates: string;
    ScreeningSectionName: string;
    ScreeningSectionShortName: string;   
}

export const getScreeningReportMultipleAnswersBySection = (state: IRootState, sectionsArray: {[k: string]: string }) : TScreenMultiReportSection => {
    const reportObject = state.screenReport.currentScreeningReportObject;
    const definition = state.screenReport.definition;

    let result: TScreenMultiReportSection = {
        mainQuestions: [],
        notMainQuestions: [],
        allQuestions:[]
    };

    if (!definition) { 
        console.warn("Definition object is not defined.");
        return result; 
    } // check logic

    const interestedDefinitions = state.screenReport.definition?.filter(d => !!sectionsArray && sectionsArray[d.ScreeningSectionID]);
    
    if (!Array.isArray(interestedDefinitions) || !interestedDefinitions.length) {
        return result;
    }


    result.mainQuestions = interestedDefinitions.reduce((acc: Array<TScreenMultiReportSectionItem>, def) => {
        const rep = reportObject && reportObject[def.ScreeningSectionID];
        if (!rep) {
            return acc;
        }
        def.MainSectionQuestions.forEach(q => {
            const answers: Array<AnswerType> = [];
            for(const option of q.AnswerOptions) {
                const found = rep.Answers.find(a => (a.QuestionID === q.QuestionID));
                const isBoolean = (!!found && 
                    (typeof found?.AnswerValue === 'boolean' || typeof found?.AnswerValue === 'number')
                );
                const answer = isBoolean ? (found?.AnswerValue === option.Value) : 'NO_CLIENT_ANSWER';
                answers.push({
                    text: option.Text,
                    value: answer,
                })
            }
            acc.push({
                questionId: q.QuestionID,
                question: q.QuestionText,
                preamble: q.PreambleText,
                isMain: true,
                answers: answers,
                score: rep.Score,
            })
        });

        return acc;
    }, []);

    result.notMainQuestions = interestedDefinitions.reduce((acc: Array<TScreenMultiReportSectionItem>, def) => {
        const rep = reportObject && reportObject[def.ScreeningSectionID];
        if (!rep) {
            return acc;
        }

        // process the case with PHQ-2/GAD-2 mode
        if(rep.Answers.length === 2){
            if(rep.ScreeningSectionID === ScreeningSectionDescriptor.Depression)
            {
                result.SectionHeader = ScreeningSectionDescriptor.DepressionPhq2Name;
                result.IsPhq2Mode = true;
                return acc; // skip rendering empty non-main questions.
            }
            else if(rep.ScreeningSectionID === ScreeningSectionDescriptor.Anxiety)
            {
                result.SectionHeader = ScreeningSectionDescriptor.AnxietyGad2Name;
                result.IsGad2Mode = true;
                return acc; // skip rendering empty non-main questions.
            }
        }
        else{
            result.SectionHeader = rep.ScreeningSectionName;
        }

        def.NotMainSectionQuestions.forEach(q => {
            const answers: Array<AnswerType> = [];
            for(const option of q.AnswerOptions) {
                const found = rep.Answers.find(a => (a.QuestionID === q.QuestionID));
                const isBoolean = (!!found && 
                    (typeof found?.AnswerValue === 'boolean' || typeof found?.AnswerValue === 'number')
                );
                const answer = isBoolean ? (found?.AnswerValue === option.Value) : 'NO_CLIENT_ANSWER';
                answers.push({
                    text: option.Text,
                    value: answer,
                })
            }
            acc.push({
                questionId: q.QuestionID,
                question: q.QuestionText,
                preamble: q.PreambleText,
                isMain: true,
                answers: answers,
                score: rep.Score,
            })
        });

        return acc;

    }, []);

    // copy all questions to AllQuestions collection
    if(result.notMainQuestions && result.notMainQuestions.length > 0){
        result.allQuestions = result.mainQuestions.concat(result.notMainQuestions);
    }
    else{
        result.allQuestions = result.mainQuestions;
    }
    
    return result;
}



export const getISectionScore = (state: IRootState, key: string): TSectionScore => {
    const reportObject: {[k: string]: IScreeningReportSections}  = state.screenReport.currentScreeningReportObject;
    const screenReport: IScreeningReportSections = reportObject[`${key}`];
    if (!screenReport) {
        const dumpResp = '';
        return {
            Score: 0,
            ScoreLevel: 0,
            ScoreLevelLabel: dumpResp,
            Indicates: dumpResp,
            ScreeningSectionName: dumpResp,
            ScreeningSectionShortName: dumpResp
        }
    }
    return {
        Score: screenReport.Score,
        ScoreLevel: screenReport.ScoreLevel,
        ScoreLevelLabel: screenReport.ScoreLevelLabel,
        Indicates: screenReport.Indicates,
        ScreeningSectionName: screenReport.ScreeningSectionName,
        ScreeningSectionShortName: screenReport.ScreeningSectionShortName
    }
}

export const getReportAnswers = (state: IRootState) => {
    const report = state.screenReport.currentScreeningReport;
    if (!report) { return []; }
    return report.Sections;
}


export const getScreeningReportAnswersBySections = (
    state: IRootState, sectionsArray: {[k: string]: string }
): TScreenReportSection => {
    
    const reportObject = state.screenReport.currentScreeningReportObject;
    const definition = state.screenReport.definition;

    let result: TScreenReportSection = {
        mainQuestions: [],
        notMainQuestions: [],
        allQuestions:[]
    };

    
    if (!definition) { 
        console.warn("Definition object is not defined.");
        return result; 
    } // check logic

    const interestedDefinitions = state.screenReport.definition?.filter(d => !!sectionsArray && sectionsArray[d.ScreeningSectionID]);
    
    if (!Array.isArray(interestedDefinitions) || !interestedDefinitions.length) {
        return result;
    }


    result.mainQuestions = interestedDefinitions.reduce((acc: Array<TScreenReportSectionItem>, def) => {
        const rep = reportObject && reportObject[def.ScreeningSectionID];
        if (!rep) {
            return acc;
        }
        def.MainSectionQuestions.forEach(q => {
            const a = rep.Answers.find(a => a.QuestionID === q.QuestionID);
            const isBoolean = (!!a && (typeof a?.AnswerValue === 'boolean' || typeof a?.AnswerValue === 'number'));
            const answer = isBoolean ? !!a?.AnswerValue : 'NO_CLIENT_ANSWER';
            acc.push({
                questionId: q.QuestionID,
                question: q.QuestionText,
                preamble: q.PreambleText,
                answer: answer,
                answerText: a?.AnswerText,
                isMain: true,
                score: rep.Score,
            });
        });
      
        return acc;
    }, []);


    result.notMainQuestions = interestedDefinitions.reduce((acc: Array<TScreenReportSectionItem>, def) => {
        const rep = reportObject && reportObject[def.ScreeningSectionID];
        if (!rep) {
            return acc;
        }
       
        def.NotMainSectionQuestions.forEach(q => {
            const a = rep.Answers.find(a => a.QuestionID === q.QuestionID);
            const isBoolean = (!!a && (typeof a?.AnswerValue === 'boolean' || typeof a?.AnswerValue === 'number'));
            const answer = isBoolean ? !!a?.AnswerValue : 'NO_CLIENT_ANSWER';
            acc.push({
                questionId: q.QuestionID,
                question: q.QuestionText,
                preamble: q.PreambleText,
                answer: answer,
                answerText: a?.AnswerText,
                isMain: false,
                score: rep.Score,
            });
        });
        return acc;
    }, []);

    // copy all questions to AllQuestions collection
    result.allQuestions = result.mainQuestions.concat(result.notMainQuestions);

    return result;
}



export const getScreenDefinition = (state: IRootState) => state.screenReport.definition;