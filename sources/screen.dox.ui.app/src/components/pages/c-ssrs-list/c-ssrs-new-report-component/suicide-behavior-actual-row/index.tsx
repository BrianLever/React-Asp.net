import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select, TextField, MenuItem } from '@material-ui/core';
import classes from  '../../../pages.module.scss';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { useState } from 'react';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';
import { convertDate } from 'helpers/dateHelper';
import ScreendoxSelect from 'components/UI/select';

const SuicideBehaviorActualRow = (): React.ReactElement => {
    const dispatch = useDispatch();
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    const optionArray=[        
        {name:"1",value:1},
        {name:"2",value:2},
        {name:"3",value:3},
        {name:"4",value:4},
        {name:"5",value:5}
    ]  
    return ( 
        <Table style={{borderLeft: "1px solid #e0e0e0"}}>            
            <TableRow>
                    <TableCell colSpan={2} style={{borderRight: "1px solid #e0e0e0"}} width={'40%'}>
                        <Grid item sm={12} >
                           <p className={classes.boldText}>Actual Lethality/Medical Damage</p>            
                        </Grid>
                        <Grid item sm={12}>
                           <p>
                           (0) No physical damage or very minor physical damage (e.g., surface scratches).
                           </p>
                           <p>
                           (1) Minor physical damage (e.g., lethargic speech; first-degree burns; mild bleeding; sprains).
                           </p>
                           <p>
                           (2) Moderate physical damage; medical attention needed (e.g., conscious but sleepy, somewhat responsive; second-degree burns; bleeding of major vessel).
                           </p>
                           <p>
                           (3) Moderately severe physical damage; medical hospitalization and likely intensive care required (e.g., comatose with reflexes intact; third-degree burns less than 20% of body; extensive blood loss but can recover; major fractures).
                           </p>
                           <p>
                           (4) Severe physical damage; medical hospitalization with intensive care required (e.g., comatose without reflexes; third-degree burns over 20% of body; extensive blood loss with unstable vital signs; major damage to a vital area).
                           </p>
                           <p>
                           (5) Death
                           </p>
                        </Grid>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}} width={'20%'}>
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                      
                        <FormControl variant="outlined" fullWidth>                                        
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.ActualLethality?.InitialAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            ActualLethality: {
                                                ...cssrsReport?.ActualLethality,
                                                InitialAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                                 
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}} width={'20%'}>
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                      
                        <FormControl variant="outlined" fullWidth >                                            
                            <ScreendoxSelect                                                          
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.ActualLethality?.MostLethalAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            ActualLethality: {
                                                ...cssrsReport?.ActualLethality,
                                                MostLethalAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                                
                                }}                             
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}} width={'20%'}>
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                      
                        <FormControl variant="outlined" fullWidth>                                            
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.ActualLethality?.MostRecentAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            ActualLethality: {
                                                ...cssrsReport?.ActualLethality,
                                                MostRecentAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                               
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                </TableRow>
             <TableRow>
                    <TableCell colSpan={2} style={{borderRight: "1px solid #e0e0e0"}} width={'20%'}>
                        <Grid item sm={12}>
                           <p className={classes.boldText}>Potential Lethality: Only Answer if Actual Lethality=0</p>            
                        </Grid>
                        <Grid item sm={12}>
                           <p>
                           (0) Behavior not likely to result in injury
                           </p>
                           <p>
                           (1) Behavior likely to result in injury but not likely to cause death
                           </p>
                           <p>
                           (2) Behavior likely to result in death despite available medical care
                           </p>                       
                           
                        </Grid>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}} width={'20%'}>
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                        
                        <FormControl variant="outlined" fullWidth>                                              
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.PotentialLethality?.InitialAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            PotentialLethality: {
                                                ...cssrsReport?.PotentialLethality,
                                                InitialAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                               
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}}>
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                       
                        <FormControl variant="outlined" fullWidth>                                             
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.PotentialLethality?.MostLethalAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            PotentialLethality: {
                                                ...cssrsReport?.PotentialLethality,
                                                MostLethalAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                               
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell style={{borderRight: "1px solid #e0e0e0"}}> 
                        <p className={classes.boldText} style={{ marginBottom: 10, textAlign: 'center' }}>Select Code</p>                        
                        <FormControl variant="outlined" fullWidth>                                            
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.PotentialLethality?.MostRecentAttemptCode}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            PotentialLethality: {
                                                ...cssrsReport?.PotentialLethality,
                                                MostRecentAttemptCode: v
                                            }
                                                }))
                                    } catch(e) {}                                                                                              
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                </TableRow>   
    </Table>
    )
}

export default SuicideBehaviorActualRow;


