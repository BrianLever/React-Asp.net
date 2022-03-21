import React from 'react';
import { useSelector } from 'react-redux';
import { 
    TopSectionLabel, AnswerLabelSection, ScoreLabelSection, 
    ScoreIndicates, ScoreLevel, ScoreLevelInlineStypeRules, CopyrightSection
} from '../../styledComponents';
import { 
    getScreeningReportAnswersBySections, TScreenReportSectionItem, getISectionScore, TSectionScore
} from '../../../../../selectors/screen/report';
import TwoSimpleAnswerComponent from '../../../../UI/report/two-simple-answers';
import { IRootState } from '../../../../../states';
import { AnswerSection } from '../../../../../components/UI/report/styledComponents';
import { COPYRIGHT_DAST10 } from 'helpers/copyrights';

const SECTION_ID = 'DAST';
const SECTIONS_TO_SHOW = {'DAST': SECTION_ID};

const DastSection = (): React.ReactElement => {

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

    let orderIndex = 1;

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
                section.mainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                    <TwoSimpleAnswerComponent
                        key={key} 
                        order={`${(orderIndex++)}. `}
                        answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                        blure={s.answer === 'NO_CLIENT_ANSWER'}
                        question={s.question}
                        isMain={false}
                    />
                ))
            }
            {
                section.notMainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                    <TwoSimpleAnswerComponent
                        key={key} 
                        order={`${(orderIndex++)}. `}
                        answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                        blure={s.answer === 'NO_CLIENT_ANSWER'}
                        question={s.question}
                        isMain={false}
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
        <CopyrightSection>{COPYRIGHT_DAST10}</CopyrightSection>
        </div>
    )
}

export default DastSection;
