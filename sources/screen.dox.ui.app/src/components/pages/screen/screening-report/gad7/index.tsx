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
import MultiAnswerComponent from '../../../../UI/report/four-simple-answers';
import { AnswerSection } from '../../../../UI/report/styledComponents';
import { COPYRIGHT_GAD7 } from 'helpers/copyrights';

const SECTION_ID = 'GAD-7';
const SECTIONS_TO_SHOW = {'GAD-7': SECTION_ID};

const GAD7Section = (): React.ReactElement => {

    const section = useSelector((state: IRootState) => getScreeningReportMultipleAnswersBySection(state, SECTIONS_TO_SHOW));
    const caceScoreDetails: TSectionScore = useSelector((state: IRootState) => getISectionScore(state, SECTION_ID));
    
    let {
        Score, Indicates, ScoreLevelLabel, ScreeningSectionName, ScreeningSectionShortName
     } = caceScoreDetails;

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
        <div >
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
                       {ScreeningSectionShortName} Score
                   </div>
                   <div style={ScoreLevelInlineStypeRules}> { Score } </div>
                </ScoreLevel>
                <ScoreIndicates>
                    <b>{ ScoreLevelLabel }:</b> {Indicates}
                </ScoreIndicates>
            </ScoreLabelSection>
            <CopyrightSection>{COPYRIGHT_GAD7}</CopyrightSection>
        </div>
    )
}

export default GAD7Section;
