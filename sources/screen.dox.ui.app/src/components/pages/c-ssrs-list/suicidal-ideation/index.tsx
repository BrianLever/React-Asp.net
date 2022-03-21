import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { ContentContainer, TitleText } from '../../styledComponents';
import { TitleTextModal, useStyles } from '../../styledComponents';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import SuicidalIdeationRow from '../suicidal-ideation-row';
import { useParams } from "react-router-dom";
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';


const SuicidalIdeation = (): React.ReactElement => {
    const dispatch = useDispatch();
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    const classes2 = useStyles();
    const valueToReturn = (value: number | null) => {console.log(value)
        let v = null;
            if(value === 1) {
                 v = true;
            } else if(value === 2) {
                 v = false;
            }
            return v
    }
    return (
        <div style={{ borderRadius: 5, overflow: 'hidden'}}>
            <Table style={{ marginBottom: 10 }}>
            <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={3} style={{borderRight: "1px solid #e0e0e0",borderLeft:"1px solid #e0e0e0",borderBottom:"1px solid #e0e0e0"}}>
                    SUICIDAL IDEATION
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody >
                <TableRow className={classes.secondHead}>
                    <TableCell width={'60%'}  style={{ textAlign: 'center',borderRight: "1px solid #e0e0e0",borderLeft:"1px solid #e0e0e0"}}>
                    Ask questions 1 and 2. If both are negative, proceed to “Suicidal Behavior” section. If the answer to question 2 is “yes”, ask questions 3, 4, and 5. If the answer to question 1 and/or 2 is “yes”, complete “Intensity of Ideation” section below.
                    </TableCell> 
                    <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center',borderRight: "1px solid #e0e0e0"}}>
                        Lifetime: Time He/She Felt Most Suicidal
                    </TableCell>
                    <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center',borderRight: "1px solid #e0e0e0"}}>
                        Past 1 Month
                    </TableCell>
                </TableRow>
                <SuicidalIdeationRow 
                    titleText={'1. Wish to be Dead'}
                    descriptionText={'Subject endorses thoughts about a wish to be dead or not alive anymore, or wish to fall asleep and not wake up.'}
                    questionText={'Have you wished you were dead or wished you could go to sleep and not wake up?'}
                    LifetimeMostSucidal={cssrsReport?.WishToDead?.LifetimeMostSucidal}
                    PastLastMonth={cssrsReport?.WishToDead?.PastLastMonth}
                    Description={cssrsReport?.WishToDead?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            WishToDead: {
                                ...cssrsReport?.WishToDead,
                                Description: value
                            }
                        }))
                    }}
                    handleChangeLifeTimeMostSucidal={(value: number | null) => {     
                        console.log(valueToReturn(value))                   
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            WishToDead: {
                                ...cssrsReport?.WishToDead,
                                LifetimeMostSucidal: valueToReturn(value)
                            }
                        }))
                    }}
                    handleChangePastLastMonth={(value: number | null) => {                       
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            WishToDead: {
                                ...cssrsReport?.WishToDead,
                                PastLastMonth: valueToReturn(value)
                            }
                        }))
                    }}
                />
                <SuicidalIdeationRow 
                    titleText={'2. Non-Specific Active Suicidal Thoughts'}
                    descriptionText={'Non-Specific Active Suicidal Thoughts General non-specific thoughts of wanting to end one’s life/die by suicide (e.g., “I’ve thought about killing myself”) without thoughts of ways to kill oneself/associated methods, intent, or plan during the assessment period.'}
                    questionText={'Have you actually had any thoughts of killing yourself?'}
                    LifetimeMostSucidal={cssrsReport?.NonSpecificActiveSuicidalThoughts?.LifetimeMostSucidal}
                    PastLastMonth={cssrsReport?.NonSpecificActiveSuicidalThoughts?.PastLastMonth}
                    Description={cssrsReport?.NonSpecificActiveSuicidalThoughts?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            NonSpecificActiveSuicidalThoughts: {
                                ...cssrsReport?.NonSpecificActiveSuicidalThoughts,
                                Description: value
                            }
                        }))
                    }}
                    handleChangeLifeTimeMostSucidal={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            NonSpecificActiveSuicidalThoughts: {
                                ...cssrsReport?.NonSpecificActiveSuicidalThoughts,
                                LifetimeMostSucidal: valueToReturn(value)
                            }
                        }))
                    }}
                    handleChangePastLastMonth={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            NonSpecificActiveSuicidalThoughts: {
                                ...cssrsReport?.NonSpecificActiveSuicidalThoughts,
                                PastLastMonth: valueToReturn(value)
                            }
                        }))
                    }}
                />
                <SuicidalIdeationRow 
                    titleText={'3. Active Suicidal Ideation with Any Methods (Not Plan) without Intent to Act'}
                    descriptionText={'Subject endorses thoughts of suicide and has thought of at least one method during the assessment period. This is different than a specific plan with time, place or method details worked out (e.g., thought of method to kill self but not a specific plan). Includes person who would say, “I thought about taking an overdose but I never made a specific plan as to when, where or how I would actually do it…and I would never go through with it.'}
                    questionText={'Have you been thinking about how you might do this?'}
                    LifetimeMostSucidal={cssrsReport?.ActiveSuicidalIdeationWithAnyMethods?.LifetimeMostSucidal}
                    PastLastMonth={cssrsReport?.ActiveSuicidalIdeationWithAnyMethods?.PastLastMonth}
                    Description={cssrsReport?.ActiveSuicidalIdeationWithAnyMethods?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithAnyMethods: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithAnyMethods,
                                Description: value
                            }
                        }))
                    }}
                    handleChangeLifeTimeMostSucidal={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithAnyMethods: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithAnyMethods,
                                LifetimeMostSucidal:valueToReturn(value)
                            }
                        }))
                    }}
                    handleChangePastLastMonth={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithAnyMethods: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithAnyMethods,
                                PastLastMonth: valueToReturn(value)
                            }
                        }))
                    }}
                />
                <SuicidalIdeationRow 
                    titleText={'4. Active Suicidal Ideation with Some Intent to Act, without Specific Plan'}
                    descriptionText={'Active suicidal thoughts of killing oneself and subject reports having some intent to act on such thoughts, as opposed to “I have the thoughts but I definitely will not do anything about them.'}
                    questionText={'Have you had these thoughts and had some intention of acting on them?'}
                    LifetimeMostSucidal={cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct?.LifetimeMostSucidal}
                    PastLastMonth={cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct?.PastLastMonth}
                    Description={cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSomeIntentToAct: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct,
                                Description: value
                            }
                        }))
                    }}
                    handleChangeLifeTimeMostSucidal={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSomeIntentToAct: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct,
                                LifetimeMostSucidal: valueToReturn(value)
                            }
                        }))
                    }}
                    handleChangePastLastMonth={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSomeIntentToAct: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSomeIntentToAct,
                                PastLastMonth: valueToReturn(value)
                            }
                        }))
                    }}
                />
                <SuicidalIdeationRow 
                    titleText={'5. Active Suicidal Ideation with Specific Plan and Intent'}
                    descriptionText={'Thoughts of killing oneself with details of plan fully or partially worked out and subject has some intent to carry it out.'}
                    questionText={'Have you started to work out or worked out the details of how to kill yourself? Do you intend to carry out this plan?'}
                    LifetimeMostSucidal={cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent?.LifetimeMostSucidal}
                    PastLastMonth={cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent?.PastLastMonth}
                    Description={cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSpecificPlanAndIntent: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent,
                                Description: value
                            }
                        }))
                    }}
                    handleChangeLifeTimeMostSucidal={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSpecificPlanAndIntent: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent,
                                LifetimeMostSucidal: valueToReturn(value)
                            }
                        }))
                    }}
                    handleChangePastLastMonth={(value: number | null) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActiveSuicidalIdeationWithSpecificPlanAndIntent: {
                                ...cssrsReport?.ActiveSuicidalIdeationWithSpecificPlanAndIntent,
                                PastLastMonth:valueToReturn(value)
                            }
                        }))
                    }}
                />
                </TableBody>
        </Table>
        </div>
    )
}

export default SuicidalIdeation;