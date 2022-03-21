import React from 'react';
import { useSelector } from 'react-redux';
import { 
    TopSectionLabel, AnswerLabelSection
} from '../../styledComponents';

import { AnswerSection, TopSectionText } from '../../../../../components/UI/report/styledComponents';

import { 
    getScreeningReportAnswersBySections, TScreenReportSectionItem, getISectionScore, TSectionScore
} from '../../../../../selectors/screen/report';
import { Grid } from '@material-ui/core';
import { IRootState } from '../../../../../states';


const SECTION_ID = 'DOCH';
const SECTIONS_TO_SHOW = {'DOCH': SECTION_ID};

const DochSection = (): React.ReactElement => {

    const section = useSelector((state: IRootState) => getScreeningReportAnswersBySections(state, SECTIONS_TO_SHOW));
    console.log(section);
    const caceScoreDetails: TSectionScore = useSelector((state: IRootState) => getISectionScore(state, SECTION_ID));
    const {
        ScreeningSectionName
     } = caceScoreDetails;
     
   // Skip section rendering when empty
   if (!section 
        || !Array.isArray(section.mainQuestions) || !section.mainQuestions.length 
        || !Array.isArray(section.notMainQuestions) || !section.notMainQuestions.length) {
        return <></>;
    }

    return (
     
        <div>
            <AnswerLabelSection>
                <TopSectionLabel>   
                    {ScreeningSectionName }
                </TopSectionLabel>
            </AnswerLabelSection>
          
            {
                section.allQuestions.map((s: TScreenReportSectionItem, key: number) => (
                    <AnswerSection>
                        <TopSectionText>
                        <Grid container justifyContent="flex-end" alignItems="center" md={12} spacing={1}>
                            <Grid item xs={1} md={1} >{key + 1}.</Grid>
                            <Grid item xs={3} md={3}>{s.question}</Grid>
                            <Grid item xs={8} md={8}>{s.answerText}</Grid>
                        </Grid>
                        </TopSectionText>
                    </AnswerSection>
                    
                ))
            }

        </div> 

    )
}

export default DochSection;
