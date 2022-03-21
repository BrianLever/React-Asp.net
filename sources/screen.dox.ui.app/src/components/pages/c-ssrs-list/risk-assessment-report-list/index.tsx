import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { ContentContainer, TitleText } from '../../styledComponents';
import { TitleTextModal, useStyles, ButtonTextStyle } from '../../styledComponents';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select, FormGroup, FormControlLabel, Checkbox, Button } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import SuicidalIdeationRow from '../suicidal-ideation-row';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import RiskAssessmentCheckboxItem from '../risk-assesment-checkbox-item';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';



const RiskAssessmentList = (): React.ReactElement => {
    const dispatch = useDispatch();
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    const classes2 = useStyles();

    return (<div>
        <div style={{ borderRadius: 5, overflow: 'hidden'}}>
        <Table  style={{ marginBottom: 20 ,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
            <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={2}>
                    SUICIDAL AND SELF-INJURY BEHAVIOR (PAST WEEK)
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px'}}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Actual suicide attempt"
                                    name="Actual suicide attempt"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.ActualSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                ActualSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.ActualSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Actual suicide attempt'}
                                />
                            </Grid>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Lifetime"
                                    name="Lifetime"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.LifetimeActualSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                LifetimeActualSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.LifetimeActualSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Lifetime'}
                                />
                            </Grid>
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Interrupted suicide attempt"
                                    name="Interrupted suicide attempt"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.InterruptedSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                InterruptedSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.InterruptedSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Interrupted suicide attempt'}
                                />                                
                            </Grid>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Lifetime"
                                    name="Lifetime"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.LifetimeInterruptedSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                LifetimeInterruptedSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.LifetimeInterruptedSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Lifetime'}
                                />
                            </Grid>
                        </Grid>
                    </TableCell>
                </TableRow> 
                 <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Aborted suicide attempt"
                                    name="Aborted suicide attempt"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.AbortedSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                AbortedSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.AbortedSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Aborted suicide attempt'}
                                /> 
                            </Grid>
                            <Grid item sm={6}>
                                 <RiskAssessmentCheckboxItem 
                                    id="Lifetime"
                                    name="Lifetime"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.LifetimeAbortedSuicideAttempt}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                LifetimeAbortedSuicideAttempt: !cssrsReport?.RiskAssessmentReport?.LifetimeAbortedSuicideAttempt
                                            }
                                        }))
                                    }}
                                    description={'Lifetime'}
                                />
                            </Grid>
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Other preparatory acts to kill self"
                                    name="Other preparatory acts to kill self"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.OtherPreparatoryActsToKillSelf}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                OtherPreparatoryActsToKillSelf: !cssrsReport?.RiskAssessmentReport?.OtherPreparatoryActsToKillSelf
                                            }
                                        }))
                                    }}
                                    description={'Other preparatory acts to kill self'}
                                /> 
                            </Grid>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Lifetime"
                                    name="Lifetime"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.LifetimeOtherPreparatoryActsToKillSelf}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                LifetimeOtherPreparatoryActsToKillSelf: !cssrsReport?.RiskAssessmentReport?.LifetimeOtherPreparatoryActsToKillSelf
                                            }
                                        }))
                                    }}
                                    description={'Lifetime'}
                                />  
                            </Grid>
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow>
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>
                                 <RiskAssessmentCheckboxItem 
                                    id="Self-injury behavior w/o suicide intent"
                                    name="Self-injury behavior w/o suicide intent"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.ActualSelfInjuryBehaviorWithoutSuicideIntent}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                ActualSelfInjuryBehaviorWithoutSuicideIntent: !cssrsReport?.RiskAssessmentReport?.ActualSelfInjuryBehaviorWithoutSuicideIntent
                                            }
                                        }))
                                    }}
                                    description={'Self-injury behavior w/o suicide intent'}
                                /> 
                            </Grid>
                            <Grid item sm={6}>
                                <RiskAssessmentCheckboxItem 
                                    id="Lifetime"
                                    name="Lifetime"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent: !cssrsReport?.RiskAssessmentReport?.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent
                                            }
                                        }))
                                    }}
                                    description={'Lifetime'}
                                />  
                            </Grid>
                        </Grid>
                    </TableCell>
                </TableRow>   
            </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}> 
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>      
            <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={2}>
                    SUICIDAL IDEATION (MOST SEVERE IN PAST WEEK)
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody>               
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                 <RiskAssessmentCheckboxItem 
                                    id="Wish to be dead"
                                    name="Wish to be dead"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.WishToBeDead}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                WishToBeDead: !cssrsReport?.RiskAssessmentReport?.WishToBeDead
                                            }
                                        }))
                                    }}
                                    description={'Wish to be dead'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Suicidal thoughts"
                                    name="Suicidal thoughts"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SuicidalThoughts}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SuicidalThoughts: !cssrsReport?.RiskAssessmentReport?.SuicidalThoughts
                                            }
                                        }))
                                    }}
                                    description={'Suicidal thoughts'}
                                />                               
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={12}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Suicidal thoughts with method (but without specific plan or intent to act)"
                                    name="Suicidal thoughts with method (but without specific plan or intent to act)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SuicidalThoughtsWithMethod}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SuicidalThoughtsWithMethod: !cssrsReport?.RiskAssessmentReport?.SuicidalThoughtsWithMethod
                                            }
                                        }))
                                    }}
                                    description={'Suicidal thoughts with method (but without specific plan or intent to act)'}
                                />                                 
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}>  
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Suicidal intent (without specific plan)"
                                    name="Suicidal intent (without specific plan)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SuicidalIntent}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SuicidalIntent: !cssrsReport?.RiskAssessmentReport?.SuicidalIntent
                                            }
                                        }))
                                    }}
                                    description={'Suicidal intent (without specific plan)'}
                                />                                
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Suicidal intent with specific plan"
                                    name="Suicidal intent with specific plan"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SuicidalIntentWithSpecificPlan}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SuicidalIntentWithSpecificPlan: !cssrsReport?.RiskAssessmentReport?.SuicidalIntentWithSpecificPlan
                                            }
                                        }))
                                    }}
                                    description={'Suicidal intent with specific plan'}
                                />                                 
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
           </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={2}>
                    ACTIVATING EVENTS (RECENT)
                </TableCell>
              </TableRow>
            </TableHead>
        <TableBody>              
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={12}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Recent loss(es) or other significant negative event(s) (legal, financial, relationship, etc.)"
                                    name="Recent loss(es) or other significant negative event(s) (legal, financial, relationship, etc.)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.RecentLoss}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                RecentLoss: !cssrsReport?.RiskAssessmentReport?.RecentLoss
                                            }
                                        }))
                                    }}
                                    description={'Recent loss(es) or other significant negative event(s) (legal, financial, relationship, etc.)'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>     
                <TableRow >
                    <TableCell> 
                        <Grid container>
                            <Grid item sm={12}>                                
                            <p>Describe:</p>
                            <p>
                                <textarea style={{ width: '100%', height: 120 }}>
                                </textarea>
                            </p>                           
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                 <RiskAssessmentCheckboxItem 
                                    id="Pending incarceration or homelessness"
                                    name="Pending incarceration or homelessness"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.PendingIncarceration}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                PendingIncarceration: !cssrsReport?.RiskAssessmentReport?.PendingIncarceration
                                            }
                                        }))
                                    }}
                                    description={'Pending incarceration or homelessness'}
                                />                          
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>   
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Current or pending isolation or feeling alone"
                                    name="Current or pending isolation or feeling alone"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.CurrentOrPendingIsolation}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                CurrentOrPendingIsolation: !cssrsReport?.RiskAssessmentReport?.CurrentOrPendingIsolation
                                            }
                                        }))
                                    }}
                                    description={'Current or pending isolation or feeling alone'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>      
           </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={2}>
                    CLINICAL STATUS (RECENT)
                </TableCell>
              </TableRow>
            </TableHead>
        <TableBody>     

                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                 <RiskAssessmentCheckboxItem 
                                    id="Hopelessness"
                                    name="Hopelessness"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.Hopelessness}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                Hopelessness: !cssrsReport?.RiskAssessmentReport?.Hopelessness
                                            }
                                        }))
                                    }}
                                    description={'Hopelessness'}
                                />                         
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>     
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Helplessness"
                                    name="Helplessness"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.Helplessness}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                Helplessness: !cssrsReport?.RiskAssessmentReport?.Helplessness
                                            }
                                        }))
                                    }}
                                    description={'Helplessness'}
                                />                           
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>   
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Feeling trapped"
                                    name="Feeling trapped"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.FeelingTrapped}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                FeelingTrapped: !cssrsReport?.RiskAssessmentReport?.FeelingTrapped
                                            }
                                        }))
                                    }}
                                    description={'Feeling trapped'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Major depressive episode"
                                    name="Major depressive episode"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.MajorDepressiveEpisode}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                MajorDepressiveEpisode: !cssrsReport?.RiskAssessmentReport?.MajorDepressiveEpisode
                                            }
                                        }))
                                    }}
                                    description={'Major depressive episode'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                 <RiskAssessmentCheckboxItem 
                                    id="Mixed affective episode"
                                    name="Mixed affective episode"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.MixedAffectiveEpisode}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                MixedAffectiveEpisode: !cssrsReport?.RiskAssessmentReport?.MixedAffectiveEpisode
                                            }
                                        }))
                                    }}
                                    description={'Mixed affective episode'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Command hallucinations to hurt self"
                                    name="Command hallucinations to hurt self"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.CommandHallucinationsToHurtSelf}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                CommandHallucinationsToHurtSelf: !cssrsReport?.RiskAssessmentReport?.CommandHallucinationsToHurtSelf
                                            }
                                        }))
                                    }}
                                    description={'Command hallucinations to hurt self'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Highly impulsive behavior"
                                    name="Highly impulsive behavior"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.HighlyImpulsiveBehavior}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                HighlyImpulsiveBehavior: !cssrsReport?.RiskAssessmentReport?.HighlyImpulsiveBehavior
                                            }
                                        }))
                                    }}
                                    description={'Highly impulsive behavior'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Substance abuse or dependence"
                                    name="Substance abuse or dependence"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SubstanceAbuseOrDependence}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SubstanceAbuseOrDependence: !cssrsReport?.RiskAssessmentReport?.SubstanceAbuseOrDependence
                                            }
                                        }))
                                    }}
                                    description={'Substance abuse or dependence'}
                                />                           
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                 <RiskAssessmentCheckboxItem 
                                    id="Agitation or severe anxiety"
                                    name="Agitation or severe anxiety"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.AgitationOrSevereAnxiety}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                AgitationOrSevereAnxiety: !cssrsReport?.RiskAssessmentReport?.AgitationOrSevereAnxiety
                                            }
                                        }))
                                    }}
                                    description={'Agitation or severe anxiety'}
                                />                              
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                
                                <RiskAssessmentCheckboxItem 
                                    id="Perceived burden on family or others"
                                    name="Perceived burden on family or others"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.PerceivedBurdenOnFamilyOrOthers}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                PerceivedBurdenOnFamilyOrOthers: !cssrsReport?.RiskAssessmentReport?.PerceivedBurdenOnFamilyOrOthers
                                            }
                                        }))
                                    }}
                                    description={'Perceived burden on family or others'}
                                />                                  
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={12}>                                                              
                                 <RiskAssessmentCheckboxItem 
                                    id="Chronic physical pain or other acute medical problem (HIV/AIDS, COPD, cancer, etc.)"
                                    name="Chronic physical pain or other acute medical problem (HIV/AIDS, COPD, cancer, etc.)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.ChronicPhysicalPain}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                ChronicPhysicalPain: !cssrsReport?.RiskAssessmentReport?.ChronicPhysicalPain
                                            }
                                        }))
                                    }}
                                    description={'Chronic physical pain or other acute medical problem (HIV/AIDS, COPD, cancer, etc.)'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                    id="Homicidal ideation"
                                    name="Homicidal ideation"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.HomicidalIdeation}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                HomicidalIdeation: !cssrsReport?.RiskAssessmentReport?.HomicidalIdeation
                                            }
                                        }))
                                    }}
                                    description={'Homicidal ideation'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                 <RiskAssessmentCheckboxItem 
                                    id="Aggressive behavior towards others"
                                    name="Aggressive behavior towards others"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.AggressiveBehaviorTowardsOthers}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                AggressiveBehaviorTowardsOthers: !cssrsReport?.RiskAssessmentReport?.AggressiveBehaviorTowardsOthers
                                            }
                                        }))
                                    }}
                                    description={'Aggressive behavior towards others'}
                                />                           
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                    id="Method for suicide available (gun, pills, etc.)"
                                    name="Method for suicide available (gun, pills, etc.)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.MethodForSuicideAvailable}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                MethodForSuicideAvailable: !cssrsReport?.RiskAssessmentReport?.MethodForSuicideAvailable
                                            }
                                        }))
                                    }}
                                    description={'Method for suicide available (gun, pills, etc.)'}
                                />                         
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>  
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                    id="Refuses or feels unable to agree to safety plan"
                                    name="Refuses or feels unable to agree to safety plan"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.RefusesOrFeelsUnableToAgreeToSafetyPlan}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                RefusesOrFeelsUnableToAgreeToSafetyPlan: !cssrsReport?.RiskAssessmentReport?.RefusesOrFeelsUnableToAgreeToSafetyPlan
                                            }
                                        }))
                                    }}
                                    description={'Refuses or feels unable to agree to safety plan'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow> 
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                    id="Sexual abuse (lifetime)"
                                    name="Sexual abuse (lifetime)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.SexualAbuseLifetime}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                SexualAbuseLifetime: !cssrsReport?.RiskAssessmentReport?.SexualAbuseLifetime
                                            }
                                        }))
                                    }}
                                    description={'Sexual abuse (lifetime)'}
                                />                             
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>
                <TableRow >
                    <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                        <Grid container>
                            <Grid item sm={6}>                                                              
                                 <RiskAssessmentCheckboxItem 
                                    id="Family history of suicide (lifetime)"
                                    name="Family history of suicide (lifetime)"
                                    isChecked={cssrsReport?.RiskAssessmentReport?.FamilyHistoryOfSuicideLifetime}
                                    changeHandler={() => {
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RiskAssessmentReport: {
                                                ...cssrsReport?.RiskAssessmentReport,
                                                FamilyHistoryOfSuicideLifetime: !cssrsReport?.RiskAssessmentReport?.FamilyHistoryOfSuicideLifetime
                                            }
                                        }))
                                    }}
                                    description={'Family history of suicide (lifetime)'}
                                />                            
                            </Grid>                           
                        </Grid>
                    </TableCell>
                </TableRow>          
           </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
            <TableRow>
            <TableCell colSpan={2}>
                    TREATMENT HISTORY
            </TableCell>
            </TableRow>
        </TableHead>
        <TableBody> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                id="Previous psychiatric diagnoses and treatments"
                                name="Previous psychiatric diagnoses and treatments"
                                isChecked={cssrsReport?.RiskAssessmentReport?.PreviousPsychiatricDiagnosesAndTreatments}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            PreviousPsychiatricDiagnosesAndTreatments: !cssrsReport?.RiskAssessmentReport?.PreviousPsychiatricDiagnosesAndTreatments
                                        }
                                    }))
                                }}
                                description={'Previous psychiatric diagnoses and treatments'}
                            />                          
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow>
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Hopeless or dissatisfied with treatment"
                                name="Hopeless or dissatisfied with treatment"
                                isChecked={cssrsReport?.RiskAssessmentReport?.HopelessOrDissatisfiedWithTreatment}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            HopelessOrDissatisfiedWithTreatment: !cssrsReport?.RiskAssessmentReport?.HopelessOrDissatisfiedWithTreatment
                                        }
                                    }))
                                }}
                                description={'Hopeless or dissatisfied with treatment'}
                            />                             
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Non-compliant with treatment"
                                name="Non-compliant with treatment"
                                isChecked={cssrsReport?.RiskAssessmentReport?.NonCompliantWithTreatment}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            NonCompliantWithTreatment: !cssrsReport?.RiskAssessmentReport?.NonCompliantWithTreatment
                                        }
                                    }))
                                }}
                                description={'Non-compliant with treatment'}
                            />                              
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                id="Not receiving treatment"
                                name="Not receiving treatment"
                                isChecked={cssrsReport?.RiskAssessmentReport?.NotReceivingTreatment}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            NotReceivingTreatment: !cssrsReport?.RiskAssessmentReport?.NotReceivingTreatment
                                        }
                                    }))
                                }}
                                description={'Not receiving treatment'}
                            />                            
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow>                  
        </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
            <TableRow>
            <TableCell colSpan={2}>
                    PROTECTIVE FACTORS (RECENT)
            </TableCell>
            </TableRow>
        </TableHead>
        <TableBody>         

            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Identifies reasons for living"
                                name="Identifies reasons for living"
                                isChecked={cssrsReport?.RiskAssessmentReport?.IdentifiesReasonsForLiving}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            IdentifiesReasonsForLiving: !cssrsReport?.RiskAssessmentReport?.IdentifiesReasonsForLiving
                                        }
                                    }))
                                }}
                                description={'Identifies reasons for living'}
                            />                           
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                id="Responsibility to family or others; living with family"
                                name="Responsibility to family or others; living with family"
                                isChecked={cssrsReport?.RiskAssessmentReport?.ResponsibilityToFamilyOrOthers}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            ResponsibilityToFamilyOrOthers: !cssrsReport?.RiskAssessmentReport?.ResponsibilityToFamilyOrOthers
                                        }
                                    }))
                                }}
                                description={'Responsibility to family or others; living with family'}
                            />                             
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow>  
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Supportive social network or family"
                                name="Supportive social network or family"
                                isChecked={cssrsReport?.RiskAssessmentReport?.SupportiveSocialNetworkOrFamily}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            SupportiveSocialNetworkOrFamily: !cssrsReport?.RiskAssessmentReport?.SupportiveSocialNetworkOrFamily
                                        }
                                    }))
                                }}
                                description={'Supportive social network or family'}
                            />                            
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                id="Fear of death or dying due to pain and suffering"
                                name="Fear of death or dying due to pain and suffering"
                                isChecked={cssrsReport?.RiskAssessmentReport?.FearOfDeathOrDyingDueToPainAndSuffering}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            FearOfDeathOrDyingDueToPainAndSuffering: !cssrsReport?.RiskAssessmentReport?.FearOfDeathOrDyingDueToPainAndSuffering
                                        }
                                    }))
                                }}
                                description={'Fear of death or dying due to pain and suffering'}
                            />                               
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                                <RiskAssessmentCheckboxItem 
                                id="Belief that suicide is immoral; high spirituality"
                                name="Belief that suicide is immoral; high spirituality"
                                isChecked={cssrsReport?.RiskAssessmentReport?.BeliefThatSuicideIsImmoral}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            BeliefThatSuicideIsImmoral: !cssrsReport?.RiskAssessmentReport?.BeliefThatSuicideIsImmoral
                                        }
                                    }))
                                }}
                                description={'Belief that suicide is immoral; high spirituality'}
                            />                              
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Engaged in work or school"
                                name="Engaged in work or school"
                                isChecked={cssrsReport?.RiskAssessmentReport?.EngagedInWorkOrSchool}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            EngagedInWorkOrSchool: !cssrsReport?.RiskAssessmentReport?.EngagedInWorkOrSchool
                                        }
                                    }))
                                }}
                                description={'Engaged in work or school'}
                            />                             
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow> 
            <TableRow >
                <TableCell style={{ padding: '10px 20px 10px 20px' }}> 
                    <Grid container>
                        <Grid item sm={6}>                                                              
                            <RiskAssessmentCheckboxItem 
                                id="Engaged with phone worker"
                                name="Engaged with phone worker"
                                isChecked={cssrsReport?.RiskAssessmentReport?.EngagedWithPhoneWorker}
                                changeHandler={() => {
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        RiskAssessmentReport: {
                                            ...cssrsReport?.RiskAssessmentReport,
                                            EngagedWithPhoneWorker: !cssrsReport?.RiskAssessmentReport?.EngagedWithPhoneWorker
                                        }
                                    }))
                                }}
                                description={'Engaged with phone worker'}
                            />                              
                        </Grid>                           
                    </Grid>
                </TableCell>
            </TableRow>          
        </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
            <TableRow>
            <TableCell colSpan={2}>
                    OTHER PROTECTIVE FACTORS
            </TableCell>
            </TableRow>
        </TableHead>
        <TableBody>
            <TableRow >
                <TableCell colSpan={2}>                       
                    <p>
                            <textarea style={{ width: '100%', height: 120 }}>
                        </textarea>
                    </p>                                    
                </TableCell>                                      
            </TableRow>  
            <TableRow>
                <TableCell colSpan={2} style={{backgroundColor: '#f3f3f9'}}>                     
                        <Button 
                            size="large"                         
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42' }}
                            >                    
                            <ButtonTextStyle>Add Protective Factor</ButtonTextStyle>
                        </Button>                    
                </TableCell>                    
            </TableRow>             
        </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
            <TableRow>
            <TableCell colSpan={2}>
                    OTHER RISK FACTORS
            </TableCell>
            </TableRow>
        </TableHead>
        <TableBody>

            <TableRow >
                <TableCell colSpan={2}>                       
                    <p>
                        <textarea style={{ width: '100%', height: 120 }}>
                        </textarea>
                    </p>                                    
                </TableCell>                                      
            </TableRow>  
            <TableRow>
                <TableCell colSpan={2} style={{backgroundColor: '#f3f3f9'}}>
                    <Button 
                         size="large"                         
                         variant="contained" 
                         color="primary" 
                         style={{ backgroundColor: '#2e2e42' }}
                         >                    
                        <ButtonTextStyle>Add Risk Factor</ButtonTextStyle>
                     </Button> 
                </TableCell>                
            </TableRow>             
        </TableBody>
        </Table>
        </div>
        <div style={{ borderTopRightRadius: 5, overflow: 'hidden',borderTopLeftRadius:5}}>
        <Table style={{marginBottom: 20,borderRight: "1px solid #e0e0e0",borderLeft: "1px solid #e0e0e0"}}>
        <TableHead className={classes.tableHead}>
            <TableRow>
            <TableCell colSpan={2}>
                    DESCRIBE ANY SUICIDAL, SELF-INJURIOUS OR AGGRESSIVE BEHAVIOR (INCLUDE DATES):
            </TableCell>
            </TableRow>
        </TableHead>
        <TableBody>
                <TableRow >
                    <TableCell colSpan={2}>                       
                        <p>
                             <textarea style={{ width: '100%', height: 120 }}>
                            </textarea>
                        </p>                                    
                    </TableCell>                                      
                </TableRow>                            
           </TableBody>
        </Table>
        </div>
        </div>    
    )
}

export default RiskAssessmentList;