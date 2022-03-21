import React from 'react';
import { useSelector } from 'react-redux';
import { 
    TopSectionLabel, AnswerLabelSection, ScoreLabelSection, 
    ScoreLevel, ScoreIndicates, ScoreLevelInlineStypeRules, CopyrightSection
} from '../../styledComponents';
import { 
    getScreeningReportMultipleAnswersBySection, TScreenMultiReportSectionItem, getISectionScore, TSectionScore
} from '../../../../../selectors/screen/report';
import { IRootState } from '../../../../../states';
import MultiFiveAnswerComponent  from '../../../../../components/UI/report/five-simple-answers';
import TwoSimpleAnswerComponent from '../../../../UI/report/two-simple-answers';
import { AnswerSection } from '../../../../../components/UI/report/styledComponents';
import {COPYRIGHT_HITS} from '../../../../../helpers/copyrights';


const SECTION_ID = 'HITS';
const SECTIONS_TO_SHOW = {'HITS': SECTION_ID};

const HitsSection = (): React.ReactElement => {

    const section = useSelector((state: IRootState) => getScreeningReportMultipleAnswersBySection(state, SECTIONS_TO_SHOW));
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
            section.mainQuestions.map((s: TScreenMultiReportSectionItem, key: number) => (
                <TwoSimpleAnswerComponent
                key={key} 
                       
                        answer={ s.answers[0].value !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answers[0].value) : false }
                        blure={s.answers[0].value === 'NO_CLIENT_ANSWER'}
                        question={`${s.preamble} ${s.question}`}
                    isMain={s.isMain}
                />
            ))
        }
         <AnswerSection>
            <TopSectionLabel>
            {section.allQuestions[0].preamble}
            </TopSectionLabel>
        </AnswerSection>
        {
            section.notMainQuestions.map((s: TScreenMultiReportSectionItem, key: number) => (
                <MultiFiveAnswerComponent
                    key={s.questionId} 
                    order={`${(key + 1)}. `}
                    answers={s.answers.map(a => {
                        return {
                            text: a.text,
                            value: a.value !== 'NO_CLIENT_ANSWER' ?  Boolean(a.value) : false
                        }
                    })}
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

export default HitsSection;
