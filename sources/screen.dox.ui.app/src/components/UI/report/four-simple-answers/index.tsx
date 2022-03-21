import React from 'react';
import { Grid } from '@material-ui/core';
import { 
    AnswerSection, TopSectionText, TopSectionAdditionalText
} from '../styledComponents';
import classes from '../Report.module.scss';

export interface ITwoSimpleAnswerComponentProps {
    question: string;
    answers: Array<{ text: string; value: boolean; }>
    isMain: boolean;
    order?: string;
}

const MultiAnswerComponent = (props: ITwoSimpleAnswerComponentProps)
: React.ReactElement => {
    const { question, answers, isMain, order } = props;
    return (
        <AnswerSection>
            <Grid container justifyContent="flex-end" alignItems="center">
                <Grid item xs={4}>
                    {
                        isMain ? (
                            <TopSectionText>
                                <Grid container>
                                    {
                                        order ? (
                                            <Grid item xs={1}>
                                                { order }
                                            </Grid>
                                        ) : null
                                    }
                                    <Grid item xs={order ? 11 : 1}>
                                        { question }
                                    </Grid>
                                </Grid>
                            </TopSectionText>
                        ) : (
                            <TopSectionAdditionalText>
                                <Grid container>
                                    {
                                        order ? (
                                            <Grid item xs={1}>
                                                { order }
                                            </Grid>
                                        ) : null
                                    }
                                    <Grid item xs={order ? 11 : 1}>
                                        { question }
                                    </Grid>
                                </Grid>
                            </TopSectionAdditionalText>
                        )
                    }
                </Grid>
                {
                    answers.map(d => (
                        <Grid item xs={2}>
                            <label className={classes.container}>
                                <span className={classes.checkboxText} style={{ color : d.value ? '' : '#ededf2' }}>{d.text}</span>
                                <input type="checkbox" checked={d.value} />
                                <span className={classes.checkmark}></span>
                            </label>
                        </Grid>
                    ))
                }
            </Grid>   
        </AnswerSection>
    )
}

export default MultiAnswerComponent;