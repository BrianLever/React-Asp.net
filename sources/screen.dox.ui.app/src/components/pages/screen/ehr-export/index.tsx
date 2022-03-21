
import React, { ChangeEvent } from 'react';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import { TitleTextModal, useStyles, EhrExportDetailText, DescriptionText } from '../../styledComponents';
import { 
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, 
    Grid, TableSortLabel, Box, CircularProgress, Input, Switch } 
from '@material-ui/core';
import PropTypes from 'prop-types';
import SelectEhrRecord from '../select-ehr-record';
import { useDispatch, useSelector } from 'react-redux';
import { closeModalWindow, EModalWindowKeys, notifyError } from 'actions/settings';
import { getEhrExportFinalResultsSelector, getEhrExportPatientRecordSelectedIdSelector, getEhrExportSelectedTabSelector, getEhrExportVisitRecordSelectedIdSelector } from 'selectors/screen';
import SelectVisit from '../select-visit';
import { getScreenListEhrExportVisitRecordsRequest, setEhrExportPatientRecordSelectedId, setEhrExportSelectedTab, setEhrExportVisitRecordSelectedId } from 'actions/screen';
import { EhrExportFinalResultText, EhrExportFinalResultList } from '../styledComponents';
import CloseIcon from '@material-ui/icons/Close';
import { TabBar, TabPanel } from 'components/UI/tab';


const EhrExport = (): React.ReactElement => {
    const classes = useStyles();
    const selectedTab: number = useSelector(getEhrExportSelectedTabSelector);
    const dispatch  = useDispatch();
    const ehrExportPatientRecordSelectedId: number | null = useSelector(getEhrExportPatientRecordSelectedIdSelector);
    const ehrExportVisitRecordSelectedId: number | null = useSelector(getEhrExportVisitRecordSelectedIdSelector);
    const screeningResults = useSelector(getEhrExportFinalResultsSelector);

    const handleChange = (event: ChangeEvent<{}>, newValue: number) => {
        if(!ehrExportPatientRecordSelectedId) {
            dispatch(notifyError('Please select matched patient.'))
            return
        }
        dispatch(getScreenListEhrExportVisitRecordsRequest(ehrExportPatientRecordSelectedId));
        if(newValue === 2) {
            if(!ehrExportVisitRecordSelectedId) {
                dispatch(notifyError('Please select matched visit.'));
                return 
            }
        }
        dispatch(setEhrExportSelectedTab(newValue));
        if(newValue === 2) {
            setTimeout(() => {
                dispatch(closeModalWindow(EModalWindowKeys.screenListSelectEHRRecord));
                dispatch(setEhrExportPatientRecordSelectedId(null));
                dispatch(setEhrExportVisitRecordSelectedId(null));
                dispatch(setEhrExportSelectedTab(0));
            }, 10000)
        }
    };

    return (
        <div className={classes.root}>
            <CloseIcon className={classes.closeIcon} onClick={() =>{
                    dispatch(closeModalWindow(EModalWindowKeys.screenListSelectEHRRecord));
                    dispatch(setEhrExportPatientRecordSelectedId(null));
                    dispatch(setEhrExportVisitRecordSelectedId(null));
                    dispatch(setEhrExportSelectedTab(0));
                }
            }/>
            <TabBar 
                 tabArray={['SELECT EHR RECORD', 'SELECT VISIT', 'EXPORT CONFIRMATION']}
                 handleChange={handleChange}
                 selectedTab={selectedTab}
            />
            
            <TabPanel value={selectedTab} index={0}>
                <Grid spacing={1} >   
                    <SelectEhrRecord handleChange={() => dispatch(setEhrExportSelectedTab(1))}/>
                </Grid> 
            </TabPanel>
            <TabPanel value={selectedTab} index={1}>
                <Grid spacing={1}>
                    <SelectVisit />
                </Grid>
            </TabPanel>
            <TabPanel value={selectedTab} index={2}>
                <Grid item sm={12} style={{ textAlign: 'center', marginBottom: 20 }}>
                    <EhrExportFinalResultText>Export operation was completed successfully!</EhrExportFinalResultText>
                </Grid>
                <EhrExportFinalResultList>
                    {screeningResults?.ExportResults && screeningResults?.ExportResults.map((result, index) => (
                        <li key={index}>
                            <Grid container> 
                            <Grid item sm={4} xs={4}><DescriptionText>{ result.ActionName }</DescriptionText></Grid>
                            <Grid item sm={4} xs={4}><DescriptionText>{ result.IsSuccessful?'OK':result.Fault }</DescriptionText></Grid>
                            </Grid> 
                        </li>
                    ))}
                </EhrExportFinalResultList>
                <Grid item sm={12}> 
                    <EhrExportDetailText>Screening Results</EhrExportDetailText>
                </Grid>
                <Grid item sm={12}>
                    <EhrExportDetailText>Health Factors:</EhrExportDetailText>
                </Grid>
                <Grid item sm={12} style={{ padding: 20 }}>
                    <EhrExportFinalResultList>
                        {screeningResults?.ExportScope.HealthFactors && screeningResults?.ExportScope.HealthFactors.map((result: { Factor: string, Comment: string }, index) => (
                            <li key={index}>
                                <Grid container> 
                                    <Grid item sm={4} xs={4}><DescriptionText>{ result?.Factor }</DescriptionText></Grid>
                                    <Grid item sm={4} xs={4}><DescriptionText>Comments: {result?.Comment }</DescriptionText></Grid>
                                </Grid> 
                            </li>
                        ))}
                    </EhrExportFinalResultList>
                </Grid>
                <Grid item sm={12}>
                    <EhrExportDetailText>Exams:</EhrExportDetailText>
                </Grid>
                <Grid item sm={12} style={{ padding: 20 }}>
                    <EhrExportFinalResultList>
                        {screeningResults?.ExportScope.Exams && screeningResults?.ExportScope.Exams.map((result, index) => (
                            <li key={index}>
                                <Grid container> 
                                    <Grid item sm={4} xs={4}><DescriptionText>{ result?.ExamName }</DescriptionText></Grid>
                                    <Grid item sm={4} xs={4}><DescriptionText>Result: { result?.ResultLabel }</DescriptionText></Grid>
                                    <Grid item sm={4} xs={4}><DescriptionText>Comments: {result?.Comment }</DescriptionText></Grid>
                                </Grid> 
                            </li>
                        ))}
                    </EhrExportFinalResultList>
                </Grid>
            </TabPanel>
            
        </div>
    )
}


export default EhrExport;
