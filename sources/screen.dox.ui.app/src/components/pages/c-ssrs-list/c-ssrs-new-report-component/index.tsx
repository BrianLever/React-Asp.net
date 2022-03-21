import React from 'react';
import momnet from 'moment';
import { Grid, TextField, CircularProgress, RadioGroup, Button  } from '@material-ui/core';
import CssrsPatientRecordsTable from 'components/UI/table/cssrs-patient-records-table';
import { ContentContainer, TitleText, DescriptionText } from '../../styledComponents';
import { useDispatch, useSelector } from 'react-redux';
import { cssrsReportScreendoxPatientRecordsSelector, cssrsReportEhrPatientRecordsSelector, cssrsReportEhrExportPatientRecordSelectedIdSelector, isCssrsReportPatientRecordLoadingSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { cssrsReportCreateRequest, ICssrsReportRequest, setCssrsReportEhrExportPatientRecordSelectedId } from 'actions/c-ssrs-list/c-ssrs-report';
import { EhrExportPatientRecordsRadioComponent } from 'components/UI/radio';
import { notifyError } from 'actions/settings';



const CssrsNewReportComponent = (): React.ReactElement => {
    const dispatch = useDispatch();
    const screendoxPatientRecords = useSelector(cssrsReportScreendoxPatientRecordsSelector);
    const ehrPatientRecords = useSelector(cssrsReportEhrPatientRecordsSelector);
    const isLoading: boolean = useSelector(isCssrsReportPatientRecordLoadingSelector);
    const selectedRecordId = useSelector(cssrsReportEhrExportPatientRecordSelectedIdSelector);
    
    const handleClick = () => {
        if(!selectedRecordId) {
            dispatch(notifyError('Please select patient record.'));
            return;
        }
        const requestBody = ehrPatientRecords.filter(record => record.ID === selectedRecordId);
        dispatch(cssrsReportCreateRequest({ ...requestBody[0]}));    
    }

    return (
        <ContentContainer>
             
            <Grid container>
                <Grid item sm={12} style={{ marginTop: 20, textAlign: 'right' }}>
                    <Button 
                            size="large"  
                            disabled={isLoading}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42',  }}
                            onClick={() => {
                                handleClick();
                            }}
                        >
                            <p style={{ color: '#fff' }}>
                                Next
                            </p>
                    </Button>
                </Grid>
                <Grid item sm={12} style={{ marginTop: 20 }}>
                    <TitleText>EHR Record</TitleText>
                    <DescriptionText>Select correct EHR record option, then click "Next"</DescriptionText>
                    {isLoading?<CircularProgress disableShrink={false} style={{ margin: '0 auto', color: 'rgb(46, 46, 66)' }}/>:
                    <RadioGroup aria-label="patient-record" name='patient-name' style={{ width: '100%' }}>
                        {ehrPatientRecords && ehrPatientRecords.map(record => (
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
                                    dispatch(setCssrsReportEhrExportPatientRecordSelectedId(record.ID));
                                }}
                            />
                        ))}  
                    </RadioGroup>}
                </Grid>
            </Grid>
        </ContentContainer>
    )
}

export default CssrsNewReportComponent;