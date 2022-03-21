import React, { useEffect, ChangeEvent, useRef } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ContentContainer, TitleText } from '../../styledComponents';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import { TitleTextModal, useStyles } from '../../styledComponents';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Input, Switch, Button } from '@material-ui/core';
import PropTypes from 'prop-types';
import ReportHeader from '../../../../components/pages/common/reportHeader';
import CssrsNewReportHeader from '../c-ssrs-new-report-header';
import SuicidalIdeation from '../suicidal-ideation';
import IntensityOfIdeation from '../intensity-of-ideation';
import SuicideBehavior from '../suicide-behavior';
import RiskAssessmentReportComponent from '../risk-assessment-report-component';
import { useParams } from "react-router-dom";
import { cssrsReportDetailRequest } from 'actions/c-ssrs-list/c-ssrs-report';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';

function TabPanel(props: any) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`simple-tabpanel-${index}`}
            aria-labelledby={`simple-tab-${index}`}
            {...other}
        >
        {value === index && (
            <Box p={3}>
                <Typography>{children}</Typography>
            </Box>
        )}
        </div>
    );
}
  
TabPanel.propTypes = {
    children: PropTypes.node,
    index: PropTypes.any.isRequired,
    value: PropTypes.any.isRequired,
};
  
function a11yProps(index: number) {
    return {
        id: `simple-tab-${index}`,
        'aria-controls': `simple-tabpanel-${index}`,
    };
}


const CssrsReportComponent = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const { reportId } = useParams<{ reportId: string }>();
    const classes = useStyles();
    const [value, setValue] = React.useState(0);
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    const lifeTimeRef = useRef<null | HTMLParagraphElement>(null);
    const riskRef = useRef<null | HTMLParagraphElement>(null);

    const handleChange = (event: ChangeEvent<{}>, newValue: number) => {
        setValue(newValue);
        if(newValue === 0) {
            lifeTimeScroll();
        } else {
            riskScroll();
        }
    };
    
    const lifeTimeScroll = () => {
        if(lifeTimeRef) {
            lifeTimeRef?.current?.scrollIntoView({behavior: 'smooth'})
        }
    }

    const riskScroll = () => {
        if(riskRef) {
            riskRef?.current?.scrollIntoView({behavior: 'smooth'})
        }
    }
    
    React.useEffect(() => {
        if(reportId) {
            dispatch(cssrsReportDetailRequest(Number(reportId)))
        }
    }, [reportId])

    
    return (
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={12} style={{ textAlign: 'center', fontSize: 14, marginBottom: 30 }}>
                    <TitleText style={{ fontSize: '1.8em' }}>
                        Columbia-Suicide Serverity Rating Scale (C-SSRS)
                    </TitleText>
                </Grid>
                <Grid item xs={12} sm={12} ref={lifeTimeRef}>
                    <AppBar position="static" className={classes.appBar}>
                        <Tabs value={value}  onChange={handleChange}  className={classes.tabs}>
                            <Tab label="LIFETIME/RECENT REPORT" {...a11yProps(0)} className={classes.tab} />
                            <Tab label="RISK ASSESSMENT REPORT" {...a11yProps(1)} className={classes.tab} />
                        </Tabs>
                    </AppBar>
                </Grid>
                <Grid item xs={12} sm={12}   style={{ textAlign: "center", marginTop: 20 }}>
                    <TitleText>
                        Lifetime/Recent Report
                    </TitleText>
                </Grid>
                <Grid item xs={12} sm={12}>
                    <CssrsNewReportHeader 
                        lastName={cssrsReport?cssrsReport.LastName:''}
                        firstName={cssrsReport?cssrsReport.FirstName:''}
                        middleName={cssrsReport?cssrsReport.MiddleName:''}
                        birthDate={cssrsReport?cssrsReport.Birthday:''}
                        patientHRN={cssrsReport?cssrsReport.EhrPatientHRN:''}
                        mailingAddress={''}
                        city={cssrsReport?cssrsReport.City:''}
                        state={cssrsReport?cssrsReport.StateID:''}
                        zip={cssrsReport?cssrsReport.ZipCode:''}
                        phone={cssrsReport?cssrsReport.Phone:''}
                        location={cssrsReport?cssrsReport.BranchLocationName:''}
                        createdDate={cssrsReport?cssrsReport.CreatedDateFormatted:''}
                        createdBy={cssrsReport?cssrsReport.StaffNameCompleted:''}
                        srn={''}
                        showSrnAsLink={false}
                    />
                </Grid>
                <Grid item xs={12} sm={12}>
                    <SuicidalIdeation />
                </Grid>
                <Grid item xs={12} sm={12}>
                    <IntensityOfIdeation />
                </Grid>
                <Grid item xs={12} sm={12}>
                    <SuicideBehavior />
                </Grid>
                <Grid item xs={12} sm={12} ref={riskRef}>
                    <RiskAssessmentReportComponent handleChange={handleChange} value={value}/>
                </Grid>
            </Grid>
        </ContentContainer>
    )
}

export default CssrsReportComponent;