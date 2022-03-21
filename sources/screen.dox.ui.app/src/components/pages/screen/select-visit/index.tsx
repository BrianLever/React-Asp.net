
import React, { ChangeEvent } from 'react';
import { TitleTextModal, useStyles, DescriptionText, EhrExportDetailText, FindAddressEhrDescriptionText, FindAddressEhrFirstNameText, FindAddressEhrLastNameText, FindAddressEhrDescriptionRedText, FindAddressEhrItemContainer, FindAddressEhrBirthText, ButtonTextStyle } from '../../styledComponents';
import {  Button, Grid, TextField, FormControl, Select, RadioGroup, FormControlLabel, Radio, CircularProgress  } from '@material-ui/core';
import { TitleText } from 'components/UI/table/styledComponents';
import { usStates } from 'helpers/general';
import { useDispatch, useSelector } from 'react-redux';
import { getEhrExportPatientRecordSelectedIdSelector, getEhrExportScreeningResultIdSelector, getEhrExportVisitRecordSelectedIdSelector, getEhrExportVisitRecordsCurrentPageSelector,getEhrExportVisitTotalRecordsSelector,getEhrExportVisitRecordsSelector, getScreeningID, getScreenListEhrExportPatientInfoSelector, getScreenListEhrExportPatientRecordsSelector, isEhrExportPatientInfoSavingLoadingSelector, isEhrExportPatientRecordsErrorSelector, isEhrExportPatientRecordsLoadingSelector, isEhrExportVisitRecordsErrorSelector, isEhrExportVisitRecordsLoadingSelector, getEhrExportScreeningDateSelector, isEhrExportFinalResultLoadingSelector } from 'selectors/screen';
import { ehrExportFinalResultRequest, getScreenListEhrExportPatientInfoRequestSuccess,setEhrExportVisitRecordsCurrentPage, getScreenListEhrExportVisitRecordsRequest,IScreenListEhrExportVisitRecord, postScreenListEhrExportPatientInfoRequest, setEhrExportPatientRecordSelectedId, setEhrExportSelectedTab, setEhrExportVisitRecordSelectedId } from 'actions/screen';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { IEhrRecordPatientsItem } from 'actions/find-patient-address';
import { notifyError } from 'actions/settings';
import { EhrExportPatientRecordContainer } from '../styledComponents';
import { EhrExportVisitRecordsRadioComponent } from 'components/UI/radio';
import customClasss from  '../../pages.module.scss';
import Pagination from '@material-ui/lab/Pagination';
import { convertDate } from 'helpers/dateHelper';

const SelectVisit = ({ ...props }): React.ReactElement => {
    const classes = useStyles();
    const dispatch = useDispatch();
    const patientInfo = useSelector(getScreenListEhrExportPatientInfoSelector);
    const screeningId: number | null = useSelector(getScreeningID);
    const ehrPatientRecords: Array<IEhrRecordPatientsItem> = useSelector(getScreenListEhrExportPatientRecordsSelector);
    const ehrRecordsError: boolean = useSelector(isEhrExportPatientRecordsErrorSelector);
    const ehrRecordsLoading: boolean = useSelector(isEhrExportPatientRecordsLoadingSelector);
    const ehrExportPatientRecordSelectedId: number | null = useSelector(getEhrExportPatientRecordSelectedIdSelector);
    const ehrExportVisitRecords: Array<IScreenListEhrExportVisitRecord> = useSelector(getEhrExportVisitRecordsSelector);
    const isEhrExportVisitRecordsLoading: boolean = useSelector(isEhrExportVisitRecordsLoadingSelector);
    const isEhrExportvisitRecordsError: boolean = useSelector(isEhrExportVisitRecordsErrorSelector);
    const ehrExportRecordSelectedId: number | null = useSelector(getEhrExportVisitRecordSelectedIdSelector);
    const ehrExportScreeningResultId: number | null = useSelector(getEhrExportScreeningResultIdSelector);
    const ehrExportScreeningDate = useSelector(getEhrExportScreeningDateSelector);
    const ehrExportVisitRecordsCurrentPage: number = useSelector(getEhrExportVisitRecordsCurrentPageSelector);
    const ehrExportVisitTotalRecords: number = useSelector(getEhrExportVisitTotalRecordsSelector);
    const isEhrExportFinalResultLoading: boolean = useSelector(isEhrExportFinalResultLoadingSelector);
    return (
        <div className={classes.root}>
           <Grid  style={{ textAlign: 'right' }}>
                <Button 
                    size="large" 
                    variant="contained" 
                    color="default" 
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', marginRight: 10 }}
                    onClick={() => {
                       dispatch(setEhrExportSelectedTab(0));
                    } }
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>
                        Previous
                    </ButtonTextStyle>
                </Button>
                <Button 
                    size="large"  
                    disabled={isEhrExportFinalResultLoading}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {
                        if(ehrExportScreeningResultId) {
                            dispatch(ehrExportFinalResultRequest(ehrExportScreeningResultId));
                        }
                    }}
                >
                    {isEhrExportFinalResultLoading && <CircularProgress  disableShrink={false} style={{ color: '#fff', height: 30 }}/>}
                    {!isEhrExportFinalResultLoading && <ButtonTextStyle>Start Export</ButtonTextStyle>}
                </Button>
           </Grid>
           <Grid container spacing={1} style={{ marginTop: 25 }}>
               <Grid item sm={6} xs={6}>
                    <Grid item sm={12} xs={12}><DescriptionText>Select the visit for the location, date, and time of the screen being exported</DescriptionText></Grid>
                    <Grid item sm={12} xs={12} style={{ marginTop: 30, marginLeft: 0, marginBottom: 15 }}><TitleText>EHR patient record</TitleText></Grid>
                    <Grid item sm={12} xs={12}>
                        <p>HRN: {ehrExportPatientRecordSelectedId}</p>
                    </Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.FirstName} {patientInfo.MiddleName} {patientInfo.LastName}</p></Grid>
                    <Grid item sm={12} xs={12}><p>{convertDate(patientInfo.Birthday)} </p></Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.StreetAddress} </p></Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.City}, {patientInfo.StateName}, {patientInfo.ZipCode} </p></Grid>
                    <Grid item sm={12} xs={12} style={{ marginTop: 30, marginLeft: 0, marginBottom: 15 }}><TitleText>Screendox patient record</TitleText></Grid>
                    <Grid item sm={12} xs={12}>
                        <p>Record No: {ehrExportScreeningResultId}</p>
                    </Grid>
                    <Grid item sm={12} xs={12}>
                        <p>Screened on: {ehrExportScreeningDate}</p>
                    </Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.FirstName} {patientInfo.MiddleName} {patientInfo.LastName}</p></Grid>
                    <Grid item sm={12} xs={12}><p>{convertDate(patientInfo.Birthday)} </p></Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.StreetAddress} </p></Grid>
                    <Grid item sm={12} xs={12}><p>{patientInfo.City}, {patientInfo.StateName}, {patientInfo.ZipCode} </p></Grid>
               </Grid>
               <Grid item sm={6} xs={6} style={{ paddingLeft: '10%'}}>
                {isEhrExportVisitRecordsLoading?<Grid item sm={12} style={{ textAlign: 'center' }} ><CircularProgress disableShrink={false} className={customClasss.circularLoadingStyle}/></Grid>:
                    <>
                        {(!isEhrExportvisitRecordsError)?<>
                        <Grid container spacing={1}>
                            <RadioGroup aria-label="patient-record" name='patient-name' style={{ width: '100%' }}>
                                {ehrExportVisitRecords && ehrExportVisitRecords.map(record => (
                                    <EhrExportVisitRecordsRadioComponent 
                                        key={record.ID}
                                        selected={ehrExportRecordSelectedId}
                                        name={record.Location.Name}
                                        date={record.DateFormatted}
                                        id={record.ID}
                                        onSelectRadio={() => { 
                                            dispatch(setEhrExportVisitRecordSelectedId(record.ID));
                                        }}
                                    />
                                ))}  
                            </RadioGroup>
                        </Grid></> : <p style={{ color: 'red', marginTop: 10 }}>There is no matches in the EHR system</p>   } 
                    </>
                }
                <Grid item xs={12} style={{ marginTop: 15 }}>
                    <Grid container justifyContent="center" alignItems="center">
                        <Grid item xs={2} />
                        <Grid item xs={8} >
                            <Grid container justifyContent="center" alignItems="center" >
                                <Pagination
                                    page={ehrExportVisitRecordsCurrentPage}
                                    count={ehrExportVisitTotalRecords} 
                                    shape="rounded"
                                    disabled={isEhrExportVisitRecordsLoading}
                                    defaultPage={1}
                                    onChange={(event: React.ChangeEvent<unknown>, page: number) => {
                                        event.stopPropagation();
                                        if(ehrExportPatientRecordSelectedId) {
                                            dispatch(setEhrExportVisitRecordsCurrentPage(page));
                                            dispatch(getScreenListEhrExportVisitRecordsRequest(ehrExportPatientRecordSelectedId));
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid item xs={2} />
                    </Grid>
                </Grid>
               </Grid>
           </Grid>
        </div>
    )
}


export default SelectVisit;
