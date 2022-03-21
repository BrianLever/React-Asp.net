import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { 
    Button, FormControl, Grid, InputLabel, Select, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import CustomDateRange from '../../daterange/custom/index';
import GPRAReportingPeriod from '../../daterange/gpra-reporting-period';
import {ILocationItemResponse } from  '../../../../actions';
import ScreendoxRadioComponent from '../../radio';
import {getLocationListActionRequest, 
    setReportsLocationsId, setReportsCurrentGPRAPeriodRangeChange,
    setReportsCurrentStartDate, setReportsCurrentEndDate,reportsIndicateReportTypeArray, setReportBSRReportType, resetExportToExcelParametersRequest
} from '../../../../actions/reports';
import postExportToExcelPrint from '../../../../api/calls/post-export-to-excel';

import { 
    getReportLocationsSelector, getReportSelectedLocationIdSelector, 
    getReportGrpaPeriodKeySelector, getReportStartDateSelector, getReportEndDateSelector,
    getReportBsrReportTypeSelector,isReportLoadingSelector,getReportGrpaPeriodsSelector, getEarliestDateSelector, getIncludeScreeningsSelector, getIncludeDemographicsSelector, getIncludeVisitsSelector, getIncludeFollowUpsSelector, getIncludeDrugsOfChoiceSelector, getIncludeCombinedSelector
} from '../../../../selectors/reports';

import classes from  '../templates.module.scss';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Collapse from '@material-ui/core/Collapse';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import { Link } from 'react-router-dom';
import { convertDate } from 'helpers/dateHelper';
import fileDownload from 'js-file-download';
import { ButtonText } from '../styledComponents';
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const useStyles = makeStyles((theme) => ({
    root: {
      width: '100%',
      maxWidth: 360,
      backgroundColor: theme.palette.background.paper,
    },
    nested: {
      paddingLeft: theme.spacing(4),
    },
    textItem: {
        fontWeight: 'bold',
        color: 'black'
    }
}));
  

const ExportToExcelTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const isLoading = useSelector(isReportLoadingSelector);  
    const endDate = useSelector(getReportEndDateSelector);
    const gpraPeriod = useSelector(getReportGrpaPeriodKeySelector);   
    const periods = useSelector(getReportGrpaPeriodsSelector)     
    const startDate = useSelector(getReportStartDateSelector);
    const locations = useSelector(getReportLocationsSelector);
    const locationId = useSelector(getReportSelectedLocationIdSelector);
    const reportType = useSelector(getReportBsrReportTypeSelector);
    const [ isExpandedCustom, setExpandedCustom ] = React.useState(false);
    const [ isExpandedGPRA, setExpandedGPRA ] = React.useState(true);
    const earliestDate= useSelector(getEarliestDateSelector);   
    const IncludeScreenings=useSelector(getIncludeScreeningsSelector);
    const IncludeDemographics=useSelector(getIncludeDemographicsSelector);
    const IncludeVisits=useSelector(getIncludeVisitsSelector);
    const IncludeFollowUps=useSelector(getIncludeFollowUpsSelector);
    const IncludeDrugsOfChoice=useSelector(getIncludeDrugsOfChoiceSelector);
    const IncludeCombined=useSelector(getIncludeCombinedSelector);
    const endDateStat = new Date().toISOString();
    React.useEffect(() => {
        if (!locations.length && (locationId === 0)) {
            dispatch(getLocationListActionRequest())
        }
    }, [locations.length, locationId, dispatch]);

    const cleanPeriods = () => {
        if (isExpandedGPRA) {          
            dispatch(setReportsCurrentStartDate(earliestDate));
            dispatch(setReportsCurrentEndDate(endDateStat));
            dispatch(setReportsCurrentGPRAPeriodRangeChange(''));
        } else if (isExpandedCustom) {
            dispatch(setReportsCurrentStartDate(null));
            dispatch(setReportsCurrentEndDate(null));
            if (periods[0]) {
                dispatch(setReportsCurrentGPRAPeriodRangeChange(periods[0].Label));
            }
        }
    }

    const handleExpandGPRA = (v: boolean): void => {
        cleanPeriods();
        setExpandedGPRA(v);
        setExpandedCustom(!v);
    }

    const handleExpandCustom = (v: boolean): void => {
        cleanPeriods();
        setExpandedCustom(v);
        setExpandedGPRA(!v);
    }
    const prepareFilterProps = () =>  {
    
        const loc = locations.find(l => l.ID === locationId);
        let location = null;
        if (locationId && (locationId > 0) && loc) {
            location = loc.ID;
        }
        let startDateGPRAPeriod;
        let endDateGPRAPeriod;             
        if (gpraPeriod) {
            const periodObject = periods.find(p => p.Label === gpraPeriod);
            startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
            endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : "2021-9-30";
        }    
        return {        
            StartDate: startDate || startDateGPRAPeriod || "2020-10-01",
            EndDate: endDate || endDateGPRAPeriod || "2021-9-30",
            LocationId: location,
            UniquePatientMode: !reportType,   
            IncludeScreenings: IncludeScreenings,  
            IncludeDemographics: IncludeDemographics,
            IncludeVisits: IncludeVisits,
            IncludeFollowUps: IncludeFollowUps,
            IncludeDrugsOfChoice: IncludeDrugsOfChoice,
            IncludeCombined: IncludeCombined,
        };
    }

    const list_classes = useStyles();
    
    const [open, setOpen] = React.useState(false);
    const [hover, setHover] = React.useState(false);
    const handleClick = () => {
        setOpen(!open);
        if(open) {
            setHover(true);
        }
    };  
    
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                    Report Information
                    </TitleText>
                </Grid>               
                 <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Location
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12}>
                    <ScreendoxSelect
                        options={locations.map((location: ILocationItemResponse) => (
                            { name: `${location.Name}`.slice(0, 20), value: location.ID}
                        ))}
                        defaultValue={locationId}
                        rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                        changeHandler={(value: any) => {
                            const v = parseInt(`${value}`);
                            dispatch(setReportsLocationsId(v))
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Report Period
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <CustomDateRange  
                        selectedFromDate={startDate?startDate:convertDate(earliestDate)} 
                        selectedTillDate={endDate?endDate:endDateStat}
                        isExpanded={isExpandedCustom}
                        setExpanded={v => {
                            handleExpandCustom(v);
                        }}
                        handleFromDateChange={(d: Date | null) => {
                            dispatch(setReportsCurrentStartDate(d && d.toISOString()));
                        }}
                        handleTillDateChange={(d: Date | null) => {
                            dispatch(setReportsCurrentEndDate(d && d.toISOString()));
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <GPRAReportingPeriod 
                        isExpanded={isExpandedGPRA}
                        setExpanded={v => {
                            handleExpandGPRA(v)
                        }}
                        selectedDate={gpraPeriod} 
                        handleDateChange={(s: string) => {
                            dispatch(setReportsCurrentGPRAPeriodRangeChange(s));
                        }}
                    />
                </Grid>               
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Report Type
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <ScreendoxRadioComponent
                        selected={reportType}
                        elementsRenderArray={reportsIndicateReportTypeArray}
                        onSelectRadio={(v: number) => {
                            dispatch(setReportBSRReportType(v));
                        }}
                    />
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
                            try {
                                postExportToExcelPrint(prepareFilterProps()).then(data => {
                                    fileDownload(data.Data, data.Filename)});
                            
                            } catch(e) {
                                console.log(':-)))');
                            }
                        }}
                    >
                        <ButtonText>Export</ButtonText>
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
                        onClick={() => {
                            dispatch(resetExportToExcelParametersRequest());
                            setExpandedGPRA(true);
                            setExpandedCustom(false);
                            // handleExpandGPRA(true);
                        } }
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Clear</ButtonText>
                    </Button>
                </Grid>               
            </Grid>
        </ContainerBlock>
    )
}

export default ExportToExcelTemplate;



