import React from 'react';
import { useSelector } from 'react-redux';
import { 
    TopSectionLabel, AnswerLabelSection, ScoreLabelSection, 
    ScoreLevel, ScoreIndicates, ScoreLevelInlineStypeRules, CopyrightSection
} from '../../styledComponents';
import { 
    TScreenReportSectionItem, getScreeningReportAnswersBySections, getISectionScore, TSectionScore
} from '../../../../../selectors/screen/report';
import { IRootState } from '../../../../../states';
import MultiFiveAnswerComponent  from '../../../../UI/report/five-simple-answers';
import TwoSimpleAnswerComponent from '../../../../UI/report/two-simple-answers';
import { AnswerSection } from '../../../../UI/report/styledComponents';
import {COPYRIGHT_HITS} from '../../../../../helpers/copyrights';


const SECTION_ID = 'BBGS';
const SECTIONS_TO_SHOW = {'BBGS': SECTION_ID};

const ProblemGamblingSection = (): React.ReactElement => {

    const section = useSelector((state: IRootState) => getScreeningReportAnswersBySections(state, SECTIONS_TO_SHOW));
    const caceScoreDetails: TSectionScore = useSelector((state: IRootState) => getISectionScore(state, SECTION_ID));
    
    const {
        Score, Indicates, ScoreLevelLabel, ScreeningSectionName, ScreeningSectionShortName
     } = caceScoreDetails;

    // Skip section rendering when empty
    if (!section 
        || !Array.isArray(section.mainQuestions) || !section.mainQuestions.length 
        || !Array.isArray(section.notMainQuestions) || !section.notMainQuestions.length) {
        return <></>;
    }


return (
    <div >
        <AnswerLabelSection>
            <TopSectionLabel>
            {ScreeningSectionName}  
            </TopSectionLabel>
        </AnswerLabelSection>
        {
            section.mainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                <TwoSimpleAnswerComponent
                key={key} 
                       
                answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                blure={s.answer === 'NO_CLIENT_ANSWER'}
                question={`${s.preamble} ${s.question}`}
                isMain={s.isMain}
                />
            ))
        }
         <AnswerSection>
            <TopSectionLabel>
            {section.allQuestions[1].preamble}
            </TopSectionLabel>
        </AnswerSection>
        {
            section.notMainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                <TwoSimpleAnswerComponent
                    key={s.questionId} 
                    order={`${(key + 1)}. `}
                    answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                    blure={s.answer === 'NO_CLIENT_ANSWER'}
                    question={s.question}
                    isMain={s.isMain}
                />
            ))
        }
            <ScoreLabelSection>
            <ScoreLevel>
                <div style={ScoreLevelInlineStypeRules}>
                    {ScreeningSectionShortName} Score
                </div>
                <div style={ScoreLevelInlineStypeRules}> { Score } </div>
            </ScoreLevel>
            <ScoreIndicates>
                <b>{ ScoreLevelLabel }:</b> {Indicates}
            </ScoreIndicates>
        </ScoreLabelSection>
        <CopyrightSection>{COPYRIGHT_HITS}</CopyrightSection>
    </div>
)
}

export default ProblemGamblingSection;
