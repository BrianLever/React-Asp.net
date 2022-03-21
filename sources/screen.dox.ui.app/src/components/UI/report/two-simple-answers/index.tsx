import React from 'react';
import { Grid } from '@material-ui/core';
import { 
    AnswerSection, TopSectionText, TopSectionAdditionalText, disabledAnswerCheckboxLabel, enabledAnswerCheckboxLabel
} from '../styledComponents';
import classes from '../Report.module.scss';

export interface ITwoSimpleAnswerComponentProps {
    question: string;
    answer: boolean;
    isMain: boolean;
    blure?: boolean;
    order?: string;
}

const TwoSimpleAnswerComponent = (props: ITwoSimpleAnswerComponentProps)
: React.ReactElement => {
    const { question, answer, isMain, blure = true, order } = props;


    return (
        <AnswerSection>
            <Grid container justifyContent="flex-end" alignItems="center" md={12} spacing={1}>
                <Grid item md={10} xs={10}>
                    {
                        isMain ? (
                            <TopSectionText>
                                <Grid container>
                                    {
                                        order ? (
                                            <Grid item xs={1} md={1}>
                                                { order }
                                            </Grid>
                                        ) : null
                                    }
                                    <Grid item >
                                        { question }
                                    </Grid>
                                </Grid>
                            </TopSectionText>
                        ) : (
                            <TopSectionAdditionalText>
                                <Grid container>
                                    {
                                        order ? (
                                            <Grid item xs={1} md={1}>
                                                { order }
                                            </Grid>
                                        ) : null
                                    }
                                    <Grid item xs={order ? 11 : 12}>
                                        { question }
                                    </Grid>
                                </Grid>
                            </TopSectionAdditionalText>
                        )
                    }
                </Grid>
                <Grid item md={2} xs={2}>
                    <Grid container justifyContent="space-between" alignItems="center">    
                        <label className={classes.container}>
                            <span className={classes.checkboxText} style = {blure || !answer ? disabledAnswerCheckboxLabel: enabledAnswerCheckboxLabel} >Yes</span>
                            <input type="checkbox" checked={!blure && answer} readOnly />
                            <span className={classes.checkmark}></span>
                        </label>
                        <label className={classes.container}>
                            <span className={classes.checkboxText} style = {blure || answer ? disabledAnswerCheckboxLabel: enabledAnswerCheckboxLabel} >No</span>
                            <input type="checkbox" checked={!blure && !answer} readOnly />
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                </Grid>
            </Grid>   
        </AnswerSection>
    )
}

export default TwoSimpleAnswerComponent;