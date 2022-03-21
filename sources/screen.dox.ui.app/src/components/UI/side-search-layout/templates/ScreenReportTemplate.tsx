import React from 'react';
import { useHistory } from 'react-router';
import { Button,  Grid } from '@material-ui/core';
import fileDownload from 'js-file-download';
import { ContainerBlock } from './styledComponents';
import getScreenPrint from '../../../../api/calls/get-screen-print';
import { notifyError } from '../../../../actions/settings';
import { deleteScreeningReportRequest, getScreenReportVisitRequest } from '../../../../actions/screen/report';
import classes from  '../templates.module.scss';
import { Link } from 'react-router-dom';
import { ERouterUrls, EScreenRouterKeys } from '../../../../router';
import { setCurrentPage } from '../../../../actions/settings';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getScreenReportVisitID
} from '../../../../selectors/screen/report';
import getScreeningReportVisit from '../../../../api/calls/get-screen-report-visit';
import { ButtonText } from '../styledComponents';

const ScreenReportTemplate = (props: any): React.ReactElement => {
   
    const history = useHistory();
    const dispatch = useDispatch();
    const visitData : any = useSelector(getScreenReportVisitID);

    React.useEffect(() => {
        if(!props.visitStatus && (props.visitID === null)) {
            dispatch(getScreenReportVisitRequest(props.screenReportId));
        }
    }, [])

    const creatVisitReport = () => {
       getScreeningReportVisit(props.screenReportId).then(res => {
            history.push('/visit-report/'+res);
       }).catch(e => {
           console.log(e)
       }) 
       
    }

    return (
    <ContainerBlock style={{ fontSize: 14 }}>
        <Grid container spacing={1} >
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="default" 
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                >
                    <ButtonText style={{ color: '#2e2e42' }}>Find Address</ButtonText>
                </Button>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth
                    className={classes.removeBoxShadow}
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
                    variant="contained" 
                    color="default" 
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                    onClick={() => {
                        try {
                            const id = `${history.location.pathname}`.replace('/screening-report/', '');
                            dispatch(deleteScreeningReportRequest(parseInt(id), history))
                        } catch(e) {
                            dispatch(notifyError(`Unexpected problem, please try later`));
                        }
                    }}
                >
                   <ButtonText style={{ color: '#2e2e42' }}>Delete Report</ButtonText>
                </Button>
            </Grid>
        </Grid>
        <Grid container spacing={1} style={{ marginTop: '26px' }}>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    
                >   
                
                    {props.visitStatus && 
                        <Link to={'/visit-report/'+props.visitID} style={{ color: '#fff', textDecoration: 'none' }} >
                            <ButtonText >Open Visit Report </ButtonText>
                        </Link>
                    }
                    {!props.visitStatus &&
                        <Link to={'#'} onClick={creatVisitReport} style={{ color: '#fff', textDecoration: 'none' }} >
                            <ButtonText>Create Visit Report</ButtonText>
                        </Link>
                    }
                   
                </Button>
            </Grid>
        </Grid>
        <Grid container spacing={1} style={{ marginTop: '26px' }}>
            <Grid item xs={12} style={{ textAlign: 'center' }}>    
                <Button 
                    size="large" 
                    fullWidth
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {
                        try {
                            const id = `${history.location.pathname}`.replace('/screening-report/', '');
                            if (id) {
                                getScreenPrint(parseInt(id))
                                .then(data => {
                                    fileDownload(data.Data, data.Filename);
                                });
                            }
                        } catch(e) {
                            console.log(':-)))');
                        }
                    }}
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
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={e => {
                        e.stopPropagation();
                        history.goBack();
                    }}
                >
                    <ButtonText >Return to Screen List</ButtonText>
                </Button>
            </Grid>
        </Grid>
    </ContainerBlock>
    )
}

export default ScreenReportTemplate;