import React from 'react';
import { useSelector } from 'react-redux';
import { TopSectionLabel, AnswerLabelSection } from '../../styledComponents';
import { 
    getScreeningReportAnswersBySections, TScreenReportSectionItem 
} from '../../../../../selectors/screen/report';
import TwoSimpleAnswerComponent from '../../../../UI/report/two-simple-answers';
import { IRootState } from '../../../../../states';

const  SECTIONS_TO_SHOW = {'SIH': 'SIH', 'TCC': 'TCC'};

const TobaccoSection = (): React.ReactElement => {

    const tobaccoSection = useSelector((state: IRootState) => getScreeningReportAnswersBySections(state, SECTIONS_TO_SHOW));
    // Skip section rendering when empty
    if (!tobaccoSection 
        || !Array.isArray(tobaccoSection.mainQuestions) || !tobaccoSection.mainQuestions.length 
        || !Array.isArray(tobaccoSection.notMainQuestions) || !tobaccoSection.notMainQuestions.length) {
        return <></>;
    }

    return (
        <div>
            <AnswerLabelSection>
                <TopSectionLabel>
                    Tobacco Use
                </TopSectionLabel>
            </AnswerLabelSection>
            {
                tobaccoSection.mainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                    <TwoSimpleAnswerComponent
                        key={key} 
                        answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                        blure={s.answer === 'NO_CLIENT_ANSWER'}
                        question={s.question}
                        isMain={s.isMain}
                    />
                ))
            }
            {
                tobaccoSection.notMainQuestions.map((s: TScreenReportSectionItem, key: number) => (
                    <TwoSimpleAnswerComponent
                        key={key} 
                        order={`${(key + 1)}. `}
                        answer={ s.answer !== 'NO_CLIENT_ANSWER' ?  Boolean(s.answer) : false }
                        blure={s.answer === 'NO_CLIENT_ANSWER'}
                        question={s.question}
                        isMain={s.isMain}
                    />
                ))
            }
        </div>
    )
}

export default TobaccoSection;
