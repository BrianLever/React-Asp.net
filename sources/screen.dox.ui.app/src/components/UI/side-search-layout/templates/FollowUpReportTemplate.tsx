import React from 'react';
import { useHistory } from 'react-router';
import { useDispatch } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import * as fileDownload from 'js-file-download';
import { ContainerBlock } from './styledComponents';
import { updateFollowUpReportRequest } from '../../../../actions/follow-up/report';
import getFollowUpReportPrint from '../../../../api/calls/get-follow-up-report-prent';
import classes from  '../templates.module.scss';
import { IPdfFileDownload } from 'api/axios';
import { ButtonText } from '../styledComponents';  


const FollowUpReportTemplate = (): React.ReactElement => {

    const history = useHistory();
    const dispatch = useDispatch();
    const reportId = `${history.location.pathname}`.replace('/follow-up-report/', '');

    return (
        <ContainerBlock>
            <Grid container spacing={1} style={{ marginTop: '30px' }}>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        disabled={false}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => {}}
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Find Address</ButtonText>
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
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                            if (reportId) {
                                dispatch(updateFollowUpReportRequest(reportId));
                            }
                        }}
                    >
                        <ButtonText >Save Changes</ButtonText>
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
                        <ButtonText style={{ color: '#2e2e42'}}>Cancel</ButtonText>
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
                        style={{ backgroundColor: '#2e2e42',  }}
                        onClick={() => {
                            try {
                                if (!!reportId) {
                                    getFollowUpReportPrint(reportId).then((data:IPdfFileDownload) => {
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
            </Grid>
        </ContainerBlock>
    )
}

export default FollowUpReportTemplate;