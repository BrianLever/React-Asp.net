import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { ContentContainer, TitleText } from '../../styledComponents';
import { TitleTextModal, useStyles, CssrsDateInput, CssrsDateInputWrapper,CustomSelect,CustomMenuItem } from '../../styledComponents';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select, TextField, MenuItem } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import SuicidalIdeationRow from '../suicidal-ideation-row';
import AddIcon from '@material-ui/icons/Add';
import SuicideBehaviorRow from '../sucide-behavior-row';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { useState } from 'react';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';
import { convertDate } from 'helpers/dateHelper';
import ScreendoxSelect from 'components/UI/select';
import SuicideBehaviorDateRow from '../c-ssrs-new-report-component/suicide-behavior-date-row';
import SuicideBehaviorActualRow from '../c-ssrs-new-report-component/suicide-behavior-actual-row';


const SuicideBehavior = (): React.ReactElement => {
    const dispatch = useDispatch(); 
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    const classes2 = useStyles();
    const optionArray=[        
        {name:"1",value:1},
        {name:"2",value:2},
        {name:"3",value:3},
        {name:"4",value:4},
        {name:"5",value:5}
    ]
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
        <Table  style={{ marginBottom: 0}}>
            <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={6} style={{borderRight: "1px solid #e0e0e0"}}>
                    SUICIDE BEHAVIOR
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody className={classes2.tableStyleWithBorder}>
                <TableRow className={classes.secondHead}>
                    <TableCell colSpan={3} >(Check all that apply, so long as these are separate events; must ask about all types)</TableCell> 
                    <TableCell  className={classes.boldText} style={{ textAlign: 'center' }}>
                         Lifetime
                    </TableCell>
                    <TableCell className={classes.boldText} style={{ textAlign: 'center' }}>
                         Past 3 Months
                    </TableCell>
                </TableRow>
                <SuicideBehaviorRow
                    titleText={'Actual Attempt'}
                    descriptionText={'A potentially self-injurious act committed with at least some wish to die, as a result of act. Behavior was in part thought of as method to kill oneself. Intent does not have to be 100%. If there is any intent/desire to die associated with the act, then it can be considered an actual suicide attempt. There does not have to be any injury or harm, just the potential for injury or harm. If person pulls trigger while gun is in mouth but gun is broken so no injury results, this is considered an attempt.Inferring Intent: Even if an individual denies intent/wish to die, it may be inferred clinically from the behavior or circumstances. For example, a highly lethal act that is clearly not an accident so no other intent but suicide can be inferred (e.g., gunshot to head, jumping from window of a high floor/story). Also, if someone denies intent to die, but they thought that what they did could be lethal, intent may be inferred.'}
                    questionText={[`Have you made a suicide attempt?`,
                                    `Have you done anything to harm yourself?`,
                                    `Have you done anything dangerous where you could have died?`,
                                    `What did you do?`,
                                    `Did you______ as a way to end your life?`,
                                    `Did you want to die (even a little) when you_____?`,
                                    `Were you trying to end your life when you _____?`,
                                    `Or Did you think it was possible you could have died from_____?`,
                                    `Or did you do it purely for other reasons / without ANY intention of killing yourself (like to relieve stress, feel better, get sympathy, or get something else to happen)? (Self-Injurious Behavior without suicidal intent)`,
                                ]}
                    totalActivityText={'Total # of interrupted:'}
                    LifetimeLevel={cssrsReport?.ActualAttempt?.LifetimeLevel}
                    LifetimeCount={cssrsReport?.ActualAttempt?.LifetimeCount|| null}
                    PastThreeMonths={cssrsReport?.ActualAttempt?.PastThreeMonths}
                    PastThreeMonthsCount={cssrsReport?.ActualAttempt?.PastThreeMonthsCount || null}
                    Description={cssrsReport?.ActualAttempt?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            ActualAttempt: {
                                ...cssrsReport?.ActualAttempt,
                                Description: value
                            }
                        }))
                    }}  
                    handleChangeLifetimeLevel={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                ActualAttempt: {
                                    ...cssrsReport?.ActualAttempt,
                                    LifetimeLevel: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangeLifetimeCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                ActualAttempt: {
                                    ...cssrsReport?.ActualAttempt,
                                    LifetimeCount: value
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonths={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                ActualAttempt: {
                                    ...cssrsReport?.ActualAttempt,
                                    PastThreeMonths: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonthsCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                ActualAttempt: {
                                    ...cssrsReport?.ActualAttempt,
                                    PastThreeMonthsCount: value
                                }
                            }))                       
                    }}            
                />
                <SuicideBehaviorRow
                    titleText={'Has subject engaged in Non-Suicidal Self-Injurious Behavior?'}
                    descriptionText={''}
                    questionText={''}
                    totalActivityText={''}
                    LifetimeLevel={cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.LifetimeLevel}
                    LifetimeCount={cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.LifetimeCount|| null}
                    PastThreeMonths={cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.PastThreeMonths}
                    PastThreeMonthsCount={cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.PastThreeMonthsCount || null}
                    Description={cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior: {
                                ...cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                                Description: value
                            }
                        }))
                    }}  
                    handleChangeLifetimeLevel={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior: {
                                    ...cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                                    LifetimeLevel: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangeLifetimeCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior: {
                                    ...cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                                    LifetimeCount: value
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonths={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior: {
                                    ...cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                                    PastThreeMonths: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonthsCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior: {
                                    ...cssrsReport?.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                                    PastThreeMonthsCount: value
                                }
                            }))                       
                    }}  
                />
                <SuicideBehaviorRow
                    titleText={'Interrupted Attempt'}
                    descriptionText={'When the person is interrupted (by an outside circumstance) from starting the potentially self-injurious act (if not for that, actual attempt would have occurred). Overdose: Person has pills in hand but is stopped from ingesting. Once they ingest any pills, this becomes an attempt rather than an interrupted attempt. Shooting: Person has gun pointed toward self, gun is taken away by someone else, or is somehow prevented from pulling trigger. Once they pull the trigger, even if the gun fails to fire, it is an attempt. Jumping: Person is poised to jump, is grabbed and taken down from ledge. Hanging: Person has noose around neck but has not yet started to hang - is stopped from doing so.'}
                    questionText={'Has there been a time when you started to do something to end your life but someone or something stopped you before you actually did anything?'}
                    totalActivityText={'Total # of interrupted:'}
                    LifetimeLevel={cssrsReport?.InterruptedAttempt?.LifetimeLevel}
                    LifetimeCount={cssrsReport?.InterruptedAttempt?.LifetimeCount|| null}
                    PastThreeMonths={cssrsReport?.InterruptedAttempt?.PastThreeMonths}
                    PastThreeMonthsCount={cssrsReport?.InterruptedAttempt?.PastThreeMonthsCount || null}
                    Description={cssrsReport?.InterruptedAttempt?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            InterruptedAttempt: {
                                ...cssrsReport?.InterruptedAttempt,
                                Description: value
                            }
                        }))
                    }}  
                    handleChangeLifetimeLevel={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                InterruptedAttempt: {
                                    ...cssrsReport?.InterruptedAttempt,
                                    LifetimeLevel: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangeLifetimeCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                InterruptedAttempt: {
                                    ...cssrsReport?.InterruptedAttempt,
                                    LifetimeCount: value
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonths={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                InterruptedAttempt: {
                                    ...cssrsReport?.InterruptedAttempt,
                                    PastThreeMonths: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonthsCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                InterruptedAttempt: {
                                    ...cssrsReport?.InterruptedAttempt,
                                    PastThreeMonthsCount: value
                                }
                            }))                       
                    }}  
                />

                <SuicideBehaviorRow
                    titleText={'Aborted or Self-Interrupted Attempt'}
                    descriptionText={'When person begins to take steps toward making a suicide attempt, but stops themselves before they actually have engaged in any self-destructive behavior. Examples are similar to interrupted attempts, except that the individual stops him/herself, instead of being stopped by something else.'}
                    questionText={'Has there been a time when you started to do something to try to end your life but you stopped yourself before you actually did anything?'}
                    totalActivityText={'Total # of aborted or self-interrupted:'}
                    LifetimeLevel={cssrsReport?.AbortedAttempt?.LifetimeLevel}
                    LifetimeCount={cssrsReport?.AbortedAttempt?.LifetimeCount|| null}
                    PastThreeMonths={cssrsReport?.AbortedAttempt?.PastThreeMonths}
                    PastThreeMonthsCount={cssrsReport?.AbortedAttempt?.PastThreeMonthsCount || null}
                    Description={cssrsReport?.AbortedAttempt?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            AbortedAttempt: {
                                ...cssrsReport?.AbortedAttempt,
                                Description: value
                            }
                        }))
                    }}  
                    handleChangeLifetimeLevel={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                AbortedAttempt: {
                                    ...cssrsReport?.AbortedAttempt,
                                    LifetimeLevel: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangeLifetimeCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                AbortedAttempt: {
                                    ...cssrsReport?.AbortedAttempt,
                                    LifetimeCount: value
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonths={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                AbortedAttempt: {
                                    ...cssrsReport?.AbortedAttempt,
                                    PastThreeMonths: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonthsCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                AbortedAttempt: {
                                    ...cssrsReport?.AbortedAttempt,
                                    PastThreeMonthsCount: value
                                }
                            }))                       
                    }}  
                    
                />
                <SuicideBehaviorRow
                    titleText={'Preparatory Acts or Behavior'}
                    descriptionText={'Acts or preparation towards imminently making a suicide attempt. This can include anything beyond a verbalization or thought, such as assembling a specific method (e.g., buying pills, purchasing a gun) or preparing for one’s death by suicide (e.g., giving things away, writing a suicide note).'}
                    questionText={'Have you taken any steps towards making a suicide attempt or preparing to kill yourself (such as collecting pills, getting a gun, giving valuables away or writing a suicide note)?'}
                    totalActivityText={'Total # of preparatory acts:'}
                    LifetimeLevel={cssrsReport?.PreparatoryActs?.LifetimeLevel}
                    LifetimeCount={cssrsReport?.PreparatoryActs?.LifetimeCount|| null}
                    PastThreeMonths={cssrsReport?.PreparatoryActs?.PastThreeMonths}
                    PastThreeMonthsCount={cssrsReport?.PreparatoryActs?.PastThreeMonthsCount || null}
                    Description={cssrsReport?.PreparatoryActs?.Description}
                    handleChangeDescription={(value: string) => {
                        dispatch(setCssrsReport({
                            ...cssrsReport,
                            PreparatoryActs: {
                                ...cssrsReport?.PreparatoryActs,
                                Description: value
                            }
                        }))
                    }}  
                    handleChangeLifetimeLevel={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                PreparatoryActs: {
                                    ...cssrsReport?.PreparatoryActs,
                                    LifetimeLevel: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangeLifetimeCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                PreparatoryActs: {
                                    ...cssrsReport?.PreparatoryActs,
                                    LifetimeCount: value
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonths={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                PreparatoryActs: {
                                    ...cssrsReport?.PreparatoryActs,
                                    PastThreeMonths: valueToReturn(value)
                                }
                            }))                       
                    }}  
                    handleChangePastThreeMonthsCount={
                        (value: number | null) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                PreparatoryActs: {
                                    ...cssrsReport?.PreparatoryActs,
                                    PastThreeMonthsCount: value
                                }
                            }))                       
                    }}  
                /> 
            </TableBody>             
        </Table>                                                                   
        <SuicideBehaviorDateRow/>  
        <SuicideBehaviorActualRow/>                      
    </div>
    )
}

export default SuicideBehavior;