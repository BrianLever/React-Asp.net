import React, { useEffect, ChangeEvent } from 'react';
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
import ReportHeader from '../../common/reportHeader';
import CssrsNewReportHeader from '../c-ssrs-new-report-header';
import SuicidalIdeation from '../suicidal-ideation';
import IntensityOfIdeation from '../intensity-of-ideation';
import SuicideBehavior from '../suicide-behavior';
import RiskAssessmentList from '../risk-assessment-report-list'


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

const RiskAssessmentReportComponent = (props: { handleChange?: (event: ChangeEvent<{}>, newValue: number) => void, value?: number }): React.ReactElement => {
    
    const dispatch = useDispatch();
    const classes = useStyles();
    const [value, setValue] = React.useState(0);
    const handleChange = (event: ChangeEvent<{}>, newValue: number) => {
        setValue(newValue);
    };

    return (
        <div style={{ marginTop: 60 }}>   
            <Grid item xs={12} sm={12}>
                <AppBar position="static" className={classes.appBar}>
                    <Tabs value={props.value}  onChange={props.handleChange}  className={classes.tabs}>
                        <Tab label="LIFETIME/RECENT REPORT" {...a11yProps(0)} className={classes.tab} />
                        <Tab label="RISK ASSESSMENT REPORT" {...a11yProps(1)} className={classes.tab} />
                    </Tabs>
                </AppBar>
            </Grid>
            <Grid item xs={12} sm={12} style={{ textAlign: "center"}}>
                <TitleText style={{ marginTop: '30px', marginBottom: 20 }}>
                    Risk Assessment Report
                </TitleText>
            </Grid>             
            <Grid item xs={12} sm={12}>
                <RiskAssessmentList />
            </Grid>              
        </div>
    )
}

export default RiskAssessmentReportComponent;