import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select, TextField, MenuItem } from '@material-ui/core';
import classes from  '../../../pages.module.scss';
import { setCssrsReport } from 'actions/c-ssrs-list/c-ssrs-report';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { CssrsDateInput, CssrsDateInputWrapper } from 'components/pages/styledComponents';


const SuicideBehaviorDateRow = (): React.ReactElement => {
    const dispatch = useDispatch();
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    return ( 
        <Table>            
            <TableRow style={{backgroundColor:'#ededf2',height:'130px'}}>
                <TableCell width={'40%'} style={{borderRight: "1px solid black"}}>                        
                </TableCell>
                <TableCell width={'20%'}style={{borderRight: "1px solid black"}}>
                 <Grid item sm={12} className={classes.boldText} style={{textAlign:'center',marginBottom: '10px'}}>
                Most Recent Attempt Date:        
            </Grid>  
                 <Grid item sm={12} >
                <CssrsDateInputWrapper>
                    <CssrsDateInput 
                        type="date" 
                        value={cssrsReport?.SuicideMostRecentAttemptDate || ""} 
                        onChange={(e) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                SuicideMostRecentAttemptDate: `${e.target.value}`
                            }))
                        }}
                    />
                </CssrsDateInputWrapper>
            </Grid>
                 </TableCell>
                <TableCell width={'20%'} style={{borderRight: "1px solid black"}}>
            <Grid item sm={12} className={classes.boldText} style={{textAlign:'center',marginBottom: '10px'}}>
                    Most Lethal Attempt Date:        
            </Grid>  
            <Grid item sm={12}>
                <CssrsDateInputWrapper>
                    <CssrsDateInput 
                        type="date" 
                        value={cssrsReport?.SuicideMostLethalRecentAttemptDate|| ""} 
                        onChange={(e) => {
                            dispatch(setCssrsReport({
                                ...cssrsReport,
                                SuicideMostLethalRecentAttemptDate: `${e.target.value}`
                            }))
                        }}
                    />
                </CssrsDateInputWrapper>
            </Grid>            
        </TableCell>
                <TableCell>
                <Grid item sm={12} className={classes.boldText} style={{textAlign:'center', marginBottom: '10px'}}>
                    Initial/First Attempt Date:       
                </Grid>  
                <Grid item sm={12}>
                    <CssrsDateInputWrapper>
                        <CssrsDateInput type="date" 
                            value={cssrsReport?.SuicideFirstAttemptDate || ""}
                            onChange={(e) => {
                                dispatch(setCssrsReport({
                                    ...cssrsReport,
                                    SuicideFirstAttemptDate: `${e.target.value}`
                                }))
                            }}
                        />
                    </CssrsDateInputWrapper>
                </Grid>           
        </TableCell>                 
            </TableRow> 
    </Table>
    )
}

export default SuicideBehaviorDateRow;


