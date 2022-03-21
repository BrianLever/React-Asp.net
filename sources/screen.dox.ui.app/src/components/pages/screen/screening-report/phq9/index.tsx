import React from 'react';
import { useSelector } from 'react-redux';
import { 
    TopSectionLabel, AnswerLabelSection, ScoreLabelSection, 
    ScoreIndicates, ScoreLevel, ScoreLevelInlineStypeRules, CopyrightSection
} from '../../styledComponents';
import { 
    getScreeningReportMultipleAnswersBySection, TScreenMultiReportSectionItem, getISectionScore, TSectionScore
} from '../../../../../selectors/screen/report';
import { IRootState } from '../../../../../states';
import MultiAnswerComponent from '../../../../../components/UI/report/four-simple-answers';
import { AnswerSection } from '../../../../../components/UI/report/styledComponents';
import {COPYRIGHT_PHQ9} from '../../../../../helpers/copyrights';

const SECTION_ID = 'PHQ-9';

const SECTIONS_TO_SHOW = {'PHQ-9': SECTION_ID};

const PHQ9Section = (): React.ReactElement => {

    const section = useSelector((state: IRootState) => getScreeningReportMultipleAnswersBySection(state, SECTIONS_TO_SHOW));
    const scoreDetails: TSectionScore = useSelector((state: IRootState) => getISectionScore(state, SECTION_ID));
    
    let {
        Score, Indicates, ScoreLevelLabel, ScreeningSectionName, ScreeningSectionShortName
     } = scoreDetails;

    // Skip section rendering when empty
    if (!section 
        || !Array.isArray(section.allQuestions) || !section.allQuestions.length) {
        return <></>;
    }

    if(section.SectionHeader)
    {
        ScreeningSectionName = section.SectionHeader;
    }
    

    return (
        <div>
            <AnswerLabelSection>
                <TopSectionLabel>
                {ScreeningSectionName}
                </TopSectionLabel>
            </AnswerLabelSection>
            <AnswerSection>
                <TopSectionLabel>
                {section.allQuestions[0].preamble}
                </TopSectionLabel>
            </AnswerSection>
            {
                section.allQuestions.map((s: TScreenMultiReportSectionItem, key: number) => (
                    <MultiAnswerComponent
                        key={s.question} 
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
                       { ScreeningSectionShortName } Score
                   </div>
                   <div style={ScoreLevelInlineStypeRules}> { Score } </div>
                </ScoreLevel>
                <ScoreIndicates>
                    <b>{ ScoreLevelLabel }:</b> {Indicates}
                </ScoreIndicates>
            </ScoreLabelSection>
            <CopyrightSection>{COPYRIGHT_PHQ9}</CopyrightSection>
        </div>
    )
}

export default PHQ9Section;
