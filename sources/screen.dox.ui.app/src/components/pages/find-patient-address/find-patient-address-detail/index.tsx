import React, { useEffect } from 'react';
import { Grid, CircularProgress, TextField, FormControl,  Select, Button } from '@material-ui/core';
import { useSelector, useDispatch  } from 'react-redux';
import { getScreeningReportRequest } from 'actions/screen/report';
import { useParams, useLocation } from "react-router-dom";
import { getReportPatientDateOfBirth, getReportPatientFirstName, getReportPatientLastName, getReportPatientMiddleName } from 'selectors/screen/report';
import {  TitleText, ContentContainer, TitleTextModal, ScreendoxRecordContainer, FindAddressDescriptionText } from '../../styledComponents';
import { getFindPatientAddressEhrRecordPatientRequest, postFindPatientAddressRequest, setFindPatientAddressCity, setFindPatientAddressPhoneNumber, setFindPatientAddressState, setFindPatientAddressStreetAddress, setFindPatientAddressZipCode } from 'actions/find-patient-address';
import EhrRecordPatientsList from '../ehr-record-patients';
import { usStates, EMPTY_LIST_VALUE } from 'helpers/general';
import { findPatientAddressCitySelector, findPatientAddressPhoneNumberSelector, findPatientAddressStateSelector, findPatientAddressStreetAddressSelector, findPatientAddressZipcodeSelector } from 'selectors/find-patient-address';
import { convertDate } from 'helpers/dateHelper';
import { useHistory, Redirect } from 'react-router';

function useQuery() {
    return new URLSearchParams(useLocation().search);
}


const FindPatientAddressDetail = (): React.ReactElement => {
    const dispatch = useDispatch();
    const { screeningResultId } = useParams<{ screeningResultId: string }>();
    const  query = useQuery();
    const history = useHistory();
    const lastName = useSelector(getReportPatientLastName);
    const firstName = useSelector(getReportPatientFirstName);
    const middleName = useSelector(getReportPatientMiddleName);
    const birthDate = useSelector(getReportPatientDateOfBirth);
    const phoneNumber = useSelector(findPatientAddressPhoneNumberSelector);
    const streetAddress = useSelector(findPatientAddressStreetAddressSelector);
    const city = useSelector(findPatientAddressCitySelector);
    const state = useSelector(findPatientAddressStateSelector);
    const zipCode = useSelector(findPatientAddressZipcodeSelector);

    useEffect(() => {
        if(query.get('type') !== 'visit' && query.get('type') !== 'demographic' ) {
            history.push('/');
        } 
        dispatch(getScreeningReportRequest(Number(screeningResultId)));
        dispatch(getFindPatientAddressEhrRecordPatientRequest(Number(screeningResultId)));
    })

    return (
        <ContentContainer >
            <Grid item sm={12}>
                <TitleText>
                    Select Patient Address
                </TitleText>
            </Grid>
            <Grid container style={{ marginTop: 10 }}>
                <Grid item sm={7} xs={12} >
                    <TitleTextModal>
                        ScreenDox Record
                    </TitleTextModal>
                    <ScreendoxRecordContainer>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>First name:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    disabled={true}
                                    value={firstName}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>Last name:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    disabled={true}
                                    value={lastName}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>MiddleName name:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    disabled={true}
                                    value={middleName}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>Date of birth:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    disabled={true}
                                    value={convertDate(birthDate !== 'N/A'?birthDate:null)}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>PHone number*:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    value={phoneNumber}
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        if(value) {
                                            dispatch(setFindPatientAddressPhoneNumber(value));
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>Street address*:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    value={streetAddress}
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        if(value) {
                                            dispatch(setFindPatientAddressStreetAddress(value));
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>City*:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    value={city}
                                    onChange={(e) => {
                                        const value = e.target.value;
                                        if(value) {
                                            dispatch(setFindPatientAddressCity(value));
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>State*:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <FormControl fullWidth variant="outlined">
                                    <Select
                                        native
                                        margin="dense"
                                        labelId="state"
                                        id="state"
                                        value={state}
                                        onChange={(event: React.ChangeEvent<{ name?: string; value: unknown }>) => {
                                            const value = `${event.target.value}`;
                                            if(value) {
                                                dispatch(setFindPatientAddressState(value));
                                            }
                                        }}
                                    >
                                        <option key="default-location" value={''} >{EMPTY_LIST_VALUE}</option>
                                        { usStates && usStates.map(item => (
                                            <option  value={item.abbreviation} key={item.abbreviation}>{item.name}</option>
                                        ))}
                                    </Select>
                                </FormControl>
                            </Grid>
                        </Grid>
                        <Grid container>
                            <Grid item sm={3}><FindAddressDescriptionText>Zip code*:</FindAddressDescriptionText></Grid>
                            <Grid item sm={9}>
                                <TextField
                                    fullWidth
                                    margin="dense"
                                    id="outlined-margin-none"
                                    variant="outlined"
                                    value={zipCode}
                                    onChange={(e) => {
                                        const value = `${e.target.value}`;
                                        if(value) {
                                            dispatch(setFindPatientAddressZipCode(value));
                                        }
                                    }}
                                />
                            </Grid>
                        </Grid>
                        <Grid container>

                        </Grid>
                    </ScreendoxRecordContainer>
                    <Grid item sm={12} xs={12} style={{ textAlign: 'right'}}>
                        <Button 
                            size="large" 
                            disabled={false}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42', marginTop: 10 }}
                            onClick={() => {
                                dispatch(postFindPatientAddressRequest(
                                    query.get('type') === 'visit'?1:0
                                ))
                            }}
                        >
                            <p style={{ color: '#fff' }}>
                                Save to ScreenDox
                            </p>
                        </Button>
                    </Grid>
                </Grid>
                <Grid item sm={5} xs={12} style={{ textAlign: 'center' }}>
                    <TitleTextModal>
                        EHR Record
                    </TitleTextModal>
                    <EhrRecordPatientsList />
                </Grid>
            </Grid>
        </ContentContainer>
    )
}

export default FindPatientAddressDetail;