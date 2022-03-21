import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { CssrsTextArea, useStyles,CustomMenuItem,CustomSelect} from '../../styledComponents';
import { TableBody, TableCell, TableHead, TableRow, Table, Grid, FormControl, Select, MenuItem } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';
import ScreendoxSelect from 'components/UI/select';
import { notifyError } from 'actions/settings';
import { CSSRS_LIFETIME_MESSAGE } from 'helpers/general';

const IntensityOfIdeation = (): React.ReactElement => {
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

    return (
        <div style={{ borderRadius: 5, overflow: 'hidden'}}>
        <Table  style={{ marginBottom: 10 }}>
            <TableHead className={classes.tableHead}>
              <TableRow>
                <TableCell colSpan={3} style={{borderRight: "1px solid #e0e0e0"}}>
                    INTENSITY OF IDEATION
                </TableCell>
              </TableRow>
            </TableHead>
            <TableBody className={classes2.tableStyleWithBorder}>
                <TableRow className={classes.secondHead}>
                    <TableCell width={'60%'}></TableCell> 
                    <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center' }}>
                       Lifetime: Most Servere
                    </TableCell>
                    <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center' }}>
                        Recent: Most Severe
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Frequency</p>
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginTop: 10 }}>How many times Have you had these thoughts?</p>
                        <Grid container spacing={1} style={{ marginTop: 10 }}>
                            <Grid item xs={6} sm={6}>
                                <p>(1) Less than once a week</p>
                                <p>(2) Once a week</p>
                                <p>(3) 2-5 times in week</p>
                            </Grid>
                            <Grid item xs={6} sm={6}>
                                <p>(4) Daily or almost daily</p>
                                <p>(5) Many times each day</p>
                            </Grid>
                        </Grid>
                    </TableCell>
                    <TableCell>                       
                        <FormControl variant="outlined" fullWidth>                                           
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.Frequency?.LifetimeMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.Frequency?.LifetimeMostSevere && v < cssrsReport?.Frequency?.LifetimeMostSevere) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Frequency: {
                                                ...cssrsReport?.Frequency,
                                                LifetimeMostSevere: v
                                                }
                                         }))
                                    } catch(e) {}                                                                                               
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell>                       
                        <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            defaultValue={cssrsReport?.Frequency?.RecentMostSevere}
                            rootOption={{name:'Select',value:0}}
                            changeHandler={(value: any) => {                                                                
                                try {
                                    const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Frequency: {
                                                ...cssrsReport?.Frequency,
                                                RecentMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                  
                                                            
                            }}
                        /> 
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Duration</p>
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginTop: 10 }}>When you have the thoughts how long do they last?</p>
                        <Grid container spacing={1} style={{ marginTop: 10 }}>
                            <Grid item xs={6} sm={6}>
                                <p>(1) Fleeting- few seconds or minutes</p>
                                <p>(2) Less than 1hour/some of the time</p>
                                <p>(3) 1-4hours/a lot of time</p>
                            </Grid>
                            <Grid item xs={6} sm={6}>
                                <p>(4) 4-8hours/most of day</p>
                                <p>(5) More than 8hours/persistent or continous</p>
                            </Grid>
                        </Grid>
                    </TableCell>
                    <TableCell>                       
                        <FormControl variant="outlined" fullWidth>                                            
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.Duration?.LifetimeMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.Duration?.LifetimeMostSevere && v < cssrsReport?.Duration?.LifetimeMostSevere) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Duration: {
                                                ...cssrsReport?.Duration,
                                                LifetimeMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                   
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell>                   
                        <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            defaultValue={cssrsReport?.Duration?.RecentMostSevere}
                            rootOption={{name:'Select',value:0}}
                            changeHandler={(value: any) => {                                                                
                                try {
                                    const v = parseInt(`${value}`)
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        Duration: {
                                            ...cssrsReport?.Duration,
                                            RecentMostSevere: v
                                                }
                                            }))
                                } catch(e) {}                                     
                                                            
                            }}
                            /> 
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Controllability</p>
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginTop: 10 }}>Could/can you stop thinking about killing yourself or wanting to die if you want to?</p>
                        <Grid container spacing={1} style={{ marginTop: 10 }}>
                            <Grid item xs={6} sm={6}>
                                <p>(1)Easily able to control thoughts</p>
                                <p>(2)Can control thoughts with little difficulty</p>
                                <p>(3)Can control thoughts with some difficulty</p>
                            </Grid>
                            <Grid item xs={6} sm={6}>
                                <p>(4)Can control thoughts with lot of difficulty</p>
                                <p>(5)Unable to control thoughts</p>
                                <p>(6)Does not attempt to control thoughts</p>
                            </Grid>
                        </Grid>
                    </TableCell>
                    <TableCell>                      
                        <FormControl variant="outlined" fullWidth>                                              
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.Controllability?.LifetimeMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.Controllability?.LifetimeMostSevere && v < cssrsReport?.Controllability?.LifetimeMostSevere) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Controllability: {
                                                ...cssrsReport?.Controllability,
                                                LifetimeMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                     
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell>                      
                        <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            defaultValue={cssrsReport?.Controllability?.RecentMostSevere}
                            rootOption={{name:'Select',value:0}}
                            changeHandler={(value: any) => {                                                                
                                try {
                                    const v = parseInt(`${value}`)
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        Controllability: {
                                            ...cssrsReport?.Controllability,
                                            RecentMostSevere: v
                                                }
                                            }))
                                } catch(e) {}                                    
                                                            
                            }}
                            /> 
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Deterrents</p>
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginTop: 10 }}>Are there things - anyone or anything (e.g., family, religion, pain of death)-that stopped you from wanting to die or acting on thoughts of suicide?</p>
                        <Grid container spacing={1} style={{ marginTop: 10 }}>
                            <Grid item xs={6} sm={6}>
                                <p>(1)Deterrents definitely stopped you from attempting suicide</p>
                                <p>(2)Deterrents probably stopped you</p>
                                <p>(3)Uncertain that deterrents stopped you</p>
                            </Grid>
                            <Grid item xs={6} sm={6}>
                                <p>(4)Deterrents most likely did not stop you</p>
                                <p>(5)Deterrents definitely did not stop you</p>
                                <p>(6)Does not apply</p>
                            </Grid>
                        </Grid>
                    </TableCell>
                    <TableCell>                      
                        <FormControl variant="outlined" fullWidth>                                              
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.Deterrents?.LifetimeMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.Deterrents?.LifetimeMostSevere && v < cssrsReport?.Deterrents?.LifetimeMostSevere) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Deterrents: {
                                                ...cssrsReport?.Deterrents,
                                                LifetimeMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                      
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell>                       
                        <FormControl variant="outlined" fullWidth>                                           
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.Deterrents?.RecentMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            Deterrents: {
                                                ...cssrsReport?.Deterrents,
                                                RecentMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                      
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Reasons from Ideation</p>
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginTop: 10 }}>What sort of reasons did you have for thinking about wanting to die or killing yourself? Was it to end the pain or stop the way you were feeling (in other words you couldn’t go on living with this pain or how you were feeling) or was it to get attention, revenge or a reaction from others? Or both? </p>
                        <Grid container spacing={1} style={{ marginTop: 10 }}>
                            <Grid item xs={6} sm={6}>
                                <p>(1) Completely to get attention, revenge or a reaction from others.</p>
                                <p>(2) Mostly to get attention, revenge or a reaction from other</p>
                                <p>(3) Equally to get attention, revenge or a reaction from others and to end/stop the pain</p>
                            </Grid>
                            <Grid item xs={6} sm={6}>
                                <p>(4) Mostly to end or stop the pain (you couldn’t go on living with the pain or how you were feeling)</p>
                                <p>(5)Completely to end or stop the pain (you couldn’t go on living with the pain or how you were feeling)</p>
                                <p>(6)Does not apply</p>
                            </Grid>
                        </Grid>
                    </TableCell>
                    <TableCell>                       
                        <FormControl variant="outlined" fullWidth>                                            
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.ReasonsForIdeation?.LifetimeMostSevere}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.ReasonsForIdeation?.LifetimeMostSevere && v < cssrsReport?.ReasonsForIdeation?.LifetimeMostSevere) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            ReasonsForIdeation: {
                                                ...cssrsReport?.ReasonsForIdeation,
                                                LifetimeMostSevere: v
                                                    }
                                                }))
                                    } catch(e) {}                                      
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell>                                                             
                        <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            defaultValue={cssrsReport?.ReasonsForIdeation?.RecentMostSevere}
                            rootOption={{name:'Select',value:0}}
                            changeHandler={(value: any) => {                                                                
                                try {
                                    const v = parseInt(`${value}`)
                                    dispatch(setCssrsReport({
                                        ...cssrsReport,
                                        ReasonsForIdeation: {
                                            ...cssrsReport?.ReasonsForIdeation,
                                            RecentMostSevere: v
                                                }
                                            }))
                                } catch(e) {}                                     
                                                            
                            }}
                            /> 
                    </TableCell>
                </TableRow>
               
                <TableRow>
                    <TableCell colSpan={3} className={classes.tableHead} style={{ textAlign: 'left'}}>
                        The following features should be rated with respect to the most severe type of ideation (i.e., 1-5 from above, with 1 being the least severe and 5 being the most severe). Ask about time he/she was feeling the most suicidal.
                    </TableCell>
                </TableRow>
                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Lifetime - Most Severe Ideation</p>
                        <p>Select # (1-5)</p>                      
                        <FormControl variant="outlined" style={{minWidth:115}}>                                              
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.LifetimeMostSevereIdeationLevel}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        if(cssrsReport?.LifetimeMostSevereIdeationLevel && v < cssrsReport?.LifetimeMostSevereIdeationLevel) {
                                            dispatch(notifyError(CSSRS_LIFETIME_MESSAGE));
                                            return;
                                        }
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            LifetimeMostSevereIdeationLevel:v
                                                }))
                                    } catch(e) {}                                     
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell colSpan={2}>
                        <p>Describe ideation:</p>                        
                        <CssrsTextArea value={String(cssrsReport?.LifetimeMostSevereIdeationDescription)}
                            onChange={(e) => {
                                dispatch(setCssrsReport({
                                    ...cssrsReport,
                                    LifetimeMostSevereIdeationDescription: `${e.target.value}`
                                }))
                            }}>
                        </CssrsTextArea>                                                                                                              
                    </TableCell>
                </TableRow>

                <TableRow>
                    <TableCell>
                        <p className={classes.boldText}>Recent - Most Severe Ideation</p>
                        <p>Select # (1-5)</p>                     
                        <FormControl variant="outlined" style={{minWidth:115}}>                                              
                            <ScreendoxSelect
                                options={optionArray.map((option: any) => (
                                    { name: `${option.name}`, value: option.value}
                                ))}
                                defaultValue={cssrsReport?.RecentMostSevereIdeationLevel}
                                rootOption={{name:'Select',value:0}}
                                changeHandler={(value: any) => {                                                                
                                    try {
                                        const v = parseInt(`${value}`)
                                        dispatch(setCssrsReport({
                                            ...cssrsReport,
                                            RecentMostSevereIdeationLevel:v
                                                }))
                                    } catch(e) {}                                     
                                                                
                                }}
                             /> 
                        </FormControl>
                    </TableCell>
                    <TableCell colSpan={2}>
                        <p>Describe ideation:</p>                        
                        <CssrsTextArea value={String(cssrsReport?.RecentMostSevereIdeationDescription)}
                            onChange={(e) => {
                                dispatch(setCssrsReport({
                                    ...cssrsReport,
                                    RecentMostSevereIdeationDescription: `${e.target.value}`
                                }))
                            }}>
                        </CssrsTextArea>                                                                                                               
                    </TableCell>
                </TableRow>
            </TableBody>
        </Table>
    </div>
    )   
 }

export default IntensityOfIdeation;