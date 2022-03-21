
import React, { ChangeEvent } from 'react';
import { TitleTextModal, useStyles, DescriptionText, EhrExportDetailText, FindAddressEhrDescriptionText, FindAddressEhrFirstNameText, FindAddressEhrLastNameText, FindAddressEhrDescriptionRedText, FindAddressEhrItemContainer, FindAddressEhrBirthText, ButtonTextStyle } from '../../styledComponents';
import {  Button, Grid, TextField, FormControl, Select, RadioGroup, FormControlLabel, Radio, CircularProgress  } from '@material-ui/core';
import { TitleText } from 'components/UI/table/styledComponents';
import { usStates } from 'helpers/general';
import { useDispatch, useSelector } from 'react-redux';
import { getEhrExportPatientRecordSelectedIdSelector, getScreeningID, getScreenListEhrExportPatientInfoSelector,getEhrExportScreeningResultIdSelector, getScreenListEhrExportPatientRecordsSelector,getScreenListEhrExportPatientTotatlRecordsSelector, isEhrExportPatientInfoSavingLoadingSelector, isEhrExportPatientRecordsErrorSelector, isEhrExportPatientRecordsLoadingSelector, getScreenListEhrExportPatientRecordsCurrentPageSelector } from 'selectors/screen';
import { getScreenListEhrExportPatientInfoRequestSuccess,getScreenListEhrExportPatientInfoRequest, getScreenListEhrExportVisitRecordsRequest, postScreenListEhrExportPatientInfoRequest, setEhrExportPatientRecordSelectedId, setEhrExportPatientRecordsCurrentPage } from 'actions/screen';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { IEhrRecordPatientsItem } from 'actions/find-patient-address';
import { notifyError } from 'actions/settings';
import { EhrExportPatientRecordContainer } from '../styledComponents';
import { EhrExportPatientRecordsRadioComponent } from 'components/UI/radio';
import customClasss from  '../../pages.module.scss';
import ScreendoxInfoButton from 'components/UI/custom-buttons/infoButton';
import { EhrExportModal, EhrExportScreenDoxInformationModal } from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import Pagination from '@material-ui/lab/Pagination';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';

const SelectEhrRecord = ({ ...props }): React.ReactElement => {
    const classes = useStyles();
    const dispatch = useDispatch();
    const patientInfo = useSelector(getScreenListEhrExportPatientInfoSelector);
    const isEhrExportPatientInfoSavingLoading: boolean = useSelector(isEhrExportPatientInfoSavingLoadingSelector);
    const screeningId: number | null = useSelector(getScreeningID);
    const ehrPatientRecords: Array<IEhrRecordPatientsItem> = useSelector(getScreenListEhrExportPatientRecordsSelector);
    const ehrRecordsError: boolean = useSelector(isEhrExportPatientRecordsErrorSelector);
    const ehrRecordsLoading: boolean = useSelector(isEhrExportPatientRecordsLoadingSelector);
    const ehrExportPatientRecordSelectedId: number | null = useSelector(getEhrExportPatientRecordSelectedIdSelector);
    const ehrExportPatientRecordsCurrentPage: number = useSelector(getScreenListEhrExportPatientRecordsCurrentPageSelector);
    const ehrExportPatientTotalRecords: number = useSelector(getScreenListEhrExportPatientTotatlRecordsSelector);
    const screeningResultId: number | null = useSelector(getEhrExportScreeningResultIdSelector);

    return (
        <div className={classes.root}>
           <Grid  style={{ textAlign: 'right' }}>
                <Button 
                    size="large"  
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {
                        if(!ehrExportPatientRecordSelectedId) {
                            dispatch(notifyError('Please select matched patient.'));return;
                        }
                        dispatch(getScreenListEhrExportVisitRecordsRequest(ehrExportPatientRecordSelectedId));
                        props.handleChange();
                    }}
                >
                    <ButtonTextStyle>
                       Next
                    </ButtonTextStyle>
                </Button>
           </Grid>
           <Grid container spacing={1}>
               <Grid item sm={6} xs={6}>
                    <Grid container spacing={2}>
                        <TitleText>Screendox Record</TitleText>
                        <ScreendoxInfoButton onClickHandler={() => dispatch(openModalWindow(EModalWindowKeys.screendoxEhrExportInformation))}/>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>First Name</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.FirstName}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, FirstName: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>Last Name</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.LastName}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, LastName: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>Middle Name</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.MiddleName}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, MiddleName: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>Date of Birth</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                                <KeyboardDatePicker
                                    format="MM/dd/yyyy"
                                    fullWidth
                                    margin="normal"
                                    id="date-picker-from"
                                    value={patientInfo?.Birthday}
                                    onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                                        if (`${date}` !== 'Invalid Date') {
                                            dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, Birthday: date }))
                                        }
                                    }}
                                    style={{ border: '1px solid #2e2e42', borderRadius: '4px', padding: '5px 5px 0 5px' }}
                                />
                            </MuiPickersUtilsProvider>
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>Phone Number</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.Phone}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, Phone: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>Street Address</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.StreetAddress}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, StreetAddress: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>City</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.City}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, City: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>State</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <ScreendoxSelect
                                options={usStates.map((l) => (
                                    { name: l.name, value: l.abbreviation }
                                ))}
                                defaultValue={patientInfo?.StateID}
                                rootOption={{ name: EMPTY_LIST_VALUE, value: '' }}
                                changeHandler={(value: any) => {
                                    const v = `${value}`;
                                    if(v) {
                                        dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, StateID: v }))
                                    }
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid container spacing={2}>
                        <Grid item sm={4} xs={4}><EhrExportDetailText>ZIP Code</EhrExportDetailText></Grid>
                        <Grid item sm={8} xs={8}>
                            <TextField
                                fullWidth
                                margin="dense"
                                id="outlined-margin-none"
                                variant="outlined"
                                value={patientInfo?.ZipCode}
                                onChange={(e) => {
                                    const value = `${e.target.value}`;
                                    dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ ...patientInfo, ZipCode: value }))
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid style={{ textAlign: 'right'}}>
                        <Button 
                            size="large"  
                            disabled={isEhrExportPatientInfoSavingLoading}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42', marginTop: 20 }}
                            onClick={() => {
                                if(screeningId) { 
                                    dispatch(postScreenListEhrExportPatientInfoRequest(screeningId))
                                }
                            }}
                        >
                            <ButtonTextStyle>
                            Save to Screendox
                            </ButtonTextStyle>
                        </Button>
                    </Grid>
               </Grid>
               <Grid item sm={6} xs={6} style={{ paddingLeft: '10%'}}>
                    <Grid container spacing={1}><TitleText>EHR Record</TitleText></Grid>
                    {ehrRecordsLoading?<Grid item sm={12} style={{ textAlign: 'center' }} ><CircularProgress disableShrink={false} className={customClasss.circularLoadingStyle}/></Grid>:
                        <>
                            {(!ehrRecordsError)?<>
                            <Grid container spacing={1} style={{ marginTop: 10 }}><DescriptionText>Select matching EHR record for the patient</DescriptionText></Grid>
                            <Grid container spacing={1}>
                                <RadioGroup aria-label="patient-record" name='patient-name' style={{ width: '100%' }}>
                                    {ehrPatientRecords && ehrPatientRecords.map(record => (
                                        <EhrExportPatientRecordsRadioComponent 
                                            key={record.ID}
                                            selected={ehrExportPatientRecordSelectedId}
                                            hrn={record.ID}
                                            firstName={record.FirstName}
                                            lastName={record.LastName}
                                            middleName={record.MiddleName}
                                            birthday={record.BirthdayFormatted}
                                            phone={record.Phone}
                                            city={record.City}
                                            zipCode={record.ZipCode}
                                            stateName={record.StateName}
                                            streetAddress={record.StreetAddress}
                                            NotMatchFields={record.NotMatchesFields}
                                            onSelectRadio={() => { 
                                                dispatch(setEhrExportPatientRecordSelectedId(record.ID));
                                                dispatch(getScreenListEhrExportPatientInfoRequestSuccess({ 
                                                    ...patientInfo,
                                                    ...record,
                                                }))
                                            }}
                                        />
                                    ))}  
                                </RadioGroup>
                            </Grid></> : <p style={{ color: 'red', marginTop: 10 }}>There is no matches in the EHR system</p>   } 
                        </>
                    }
                    {!ehrRecordsError?
                    <Grid item xs={12}>
                        <Grid container justifyContent="center" alignItems="center">
                            <Grid item xs={2} />
                            <Grid item xs={8} >
                                <Grid container justifyContent="center" alignItems="center" >
                                    <Pagination
                                        page={ehrExportPatientRecordsCurrentPage}
                                        count={ehrExportPatientTotalRecords} 
                                        shape="rounded"
                                        disabled={ehrRecordsLoading}
                                        defaultPage={1}
                                        onChange={(event: React.ChangeEvent<unknown>, page: number) => {
                                            event.stopPropagation();
                                            if(screeningResultId) {
                                                dispatch(setEhrExportPatientRecordsCurrentPage(page));
                                                dispatch(getScreenListEhrExportPatientInfoRequest(screeningResultId));
                                            }
                                        }}
                                    />
                                </Grid>
                            </Grid>
                            <Grid item xs={2} />
                        </Grid>
                    </Grid>:null}
               </Grid>
               
           </Grid>
           <EhrExportScreenDoxInformationModal
            uniqueKey={EModalWindowKeys.screendoxEhrExportInformation}
            onConfirm={() => {
                dispatch(closeModalWindow(EModalWindowKeys.screendoxEhrExportInformation));
            }}
        />
        </div>
    )
}


export default SelectEhrRecord;
