import React, { useEffect } from 'react';
import { Grid, CircularProgress, TextField, Button,  RadioGroup } from '@material-ui/core';
import { useSelector, useDispatch  } from 'react-redux';
import {  FindAddressEhrDescriptionText, FindAddressEhrFirstNameText, FindAddressEhrLastNameText, FindAddressEhrDescriptionRedText, FindAddressEhrItemContainer, FindAddressEhrBirthText } from '../../styledComponents';
import { getFindPatientAddressEhrRecordPatientRequest, getFindPatientAddressEhrRecordPatientRequestError, IEhrRecordPatientsItem, setFindPatientAddressCity, setFindPatientAddressEhrExportPatientRecordSelectedId, setFindPatientAddressEhrExportRecordCurrentPage, setFindPatientAddressPhoneNumber, setFindPatientAddressState, setFindPatientAddressStreetAddress, setFindPatientAddressZipCode } from 'actions/find-patient-address';
import { findPatientAddressCurrentPageSelector, findPatientAddressEhrRecordPatientsSelector, findPatientAddressTotalSelector, getFindPatientAddressEhrExportPatientRecordSelectedIdSelector, isFindPatientAddressehrRecordPatientLoadingErrorSelector, isFindPatientAddressEhrRecordPatientLoadingSelector } from 'selectors/find-patient-address';
import customClasss from  '../../pages.module.scss';
import { EhrExportPatientRecordsRadioComponent } from 'components/UI/radio';
import { useParams } from "react-router-dom";
import Pagination from '@material-ui/lab/Pagination';


const EhrRecordPatientsList = (): React.ReactElement => {
    const dispatch = useDispatch();
    const ehrRecordPatiens: Array<IEhrRecordPatientsItem> = useSelector(findPatientAddressEhrRecordPatientsSelector);
    const isLoading: boolean = useSelector(isFindPatientAddressEhrRecordPatientLoadingSelector);
    const selectedRecordId: number | null = useSelector(getFindPatientAddressEhrExportPatientRecordSelectedIdSelector);
    const isError: boolean = useSelector(isFindPatientAddressehrRecordPatientLoadingErrorSelector);
    const currentPage: number = useSelector(findPatientAddressCurrentPageSelector);
    const totalRecords: number = useSelector(findPatientAddressTotalSelector);
    const { screeningResultId } = useParams<{ screeningResultId: string }>();


    useEffect(() => {

    })

    if(isLoading) {
        return <CircularProgress disableShrink={false} className={customClasss.circularLoadingStyle}/>;
    }

    return (<>{!isError?
            <Grid container spacing={1}>
                <RadioGroup aria-label="patient-record" name='patient-name' style={{ width: '100%' }}>
                    {ehrRecordPatiens && ehrRecordPatiens.map(record => (
                        <EhrExportPatientRecordsRadioComponent 
                            key={record.ID}
                            selected={selectedRecordId}
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
                                dispatch(setFindPatientAddressEhrExportPatientRecordSelectedId(record.ID));
                                dispatch(setFindPatientAddressPhoneNumber(`${record.Phone}`))
                                dispatch(setFindPatientAddressState(`${record.StateName}`));
                                dispatch(setFindPatientAddressStreetAddress(`${record.StreetAddress}`));
                                dispatch(setFindPatientAddressCity(`${record.City}`));
                                dispatch(setFindPatientAddressZipCode(`${record.ZipCode}`))
                            }}
                        />
                    ))}  
                </RadioGroup>
            </Grid>:<FindAddressEhrDescriptionRedText>Cannot search for patients. There is no connection to the EHR database.Cannot search for patients. There is no connection to the EHR database.</FindAddressEhrDescriptionRedText>}

            {!isError?
            <Grid item xs={12}>
                <Grid container justifyContent="center" alignItems="center">
                    <Grid item xs={2} />
                    <Grid item xs={8} >
                        <Grid container justifyContent="center" alignItems="center" >
                            <Pagination
                                page={currentPage}
                                count={totalRecords} 
                                shape="rounded"
                                disabled={isLoading}
                                defaultPage={1}
                                onChange={(event: React.ChangeEvent<unknown>, page: number) => {
                                    event.stopPropagation();
                                    if(screeningResultId) {
                                        dispatch(setFindPatientAddressEhrExportRecordCurrentPage(page));
                                        dispatch(getFindPatientAddressEhrRecordPatientRequest(Number(screeningResultId)));
                                    }
                                }}
                            />
                        </Grid>
                    </Grid>
                    <Grid item xs={2} />
                </Grid>
            </Grid>:null}
       </>
    )
}

export default EhrRecordPatientsList;