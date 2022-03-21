import React from 'react';
import { useHistory } from 'react-router';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import * as fileDownload from 'js-file-download';
import { ContainerBlock } from './styledComponents';
import { updateVisitDemographicReportRequest } from '../../../../actions/visit/demographic-report';
import postPrintAllVisit from '../../../../api/calls/get-print-all-visit';
import postPrintAllDemographics from '../../../../api/calls/post-print-all-demographics';
import classes from  '../templates.module.scss';
import { COMMON_EMPTY_RESPONSE } from 'helpers/general';
import { getVisitDemographicReportPatientStreetAddress, getVisitReportScreeningResultId } from 'selectors/visit/demographic-report';
import { ButtonText } from '../styledComponents'; 


const VisitDemographicReportTemplate = (): React.ReactElement => {

    const history = useHistory();
    const dispatch = useDispatch();
    const reportId = `${history.location.pathname}`.replace('/patient-demographics-report/', '');
    const mailingAddress = useSelector(getVisitDemographicReportPatientStreetAddress);
    const srn = useSelector(getVisitReportScreeningResultId);

    return (
        <ContainerBlock>
            <Grid container spacing={1} style={{ marginTop: '30px', fontSize: 14 }}>
                { mailingAddress === COMMON_EMPTY_RESPONSE && 
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        disabled={false}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => { 
                            if(srn)
                                history.push('/find-patient-address/'+srn+'?type=demographic') 
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Find Address</ButtonText>
                    </Button>
                </Grid>}
                
               
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        className={classes.removeBoxShadow}
                        disabled={false}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                            if (reportId) {
                               dispatch(updateVisitDemographicReportRequest(reportId))
                            }
                        }}
                    >
                        <ButtonText>Save Changes</ButtonText>
                    </Button>
                </Grid>

                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        disabled={false}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={e => {
                            e.stopPropagation();
                            history.goBack();
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Cancel</ButtonText>
                    </Button>
                </Grid>

                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        className={classes.removeBoxShadow}
                        disabled={false}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', marginTop: '2em'  }}
                        onClick={() => {
                            try {
                                if (!!reportId) {
                                    postPrintAllDemographics(reportId).then(data => {
                                        fileDownload.default(data.Data, data.Filename);
                                    });
                                }
                            } catch(e) {
                                console.log(':-)))');
                            }
                        } }
                    >
                        <ButtonText>Print</ButtonText>
                    </Button>
                </Grid>

                <Grid item xs={12} style={{ textAlign: 'center' }}>
                        <Button 
                            size="large" 
                            fullWidth
                            className={classes.removeBoxShadow}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42', }}
                            onClick={e => {
                                e.stopPropagation();
                                history.goBack();
                            }}
                        >
                            <ButtonText >Return to Visit List</ButtonText>
                        </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default VisitDemographicReportTemplate;