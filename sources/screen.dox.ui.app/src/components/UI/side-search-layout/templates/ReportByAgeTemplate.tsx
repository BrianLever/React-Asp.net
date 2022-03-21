import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import CustomDateRange from '../../../UI/daterange/custom/index';
import GPRAReportingPeriod from '../../../UI/daterange/gpra-reporting-period';
import {ILocationItemResponse } from  '../../../../actions';
import ScreendoxRadioComponent from '../../../../components/UI/radio';
import { reportsIndicateReportTypeArray,resetReportsSearchParametersRequest,setReportsCurrentStartDate,setReportsCurrentEndDate,
    setReportsCurrentGPRAPeriodRangeChange,setReportBSRReportType,setReportsLocationsId,postFilteredReportsByAgeRequest,getLocationListActionRequest, resetReportsByAgeSearchParametersRequest
 } from '../../../../actions/reports';
import postReportByAgePrint from '../../../../api/calls/post-report-by-age-print';
import { 
    getReportLocationsSelector, getReportSelectedLocationIdSelector, 
    getReportGrpaPeriodKeySelector, getReportStartDateSelector, getReportEndDateSelector,
    getReportBsrReportTypeSelector,isReportLoadingSelector,getReportGrpaPeriodsSelector, getEarliestDateSelector
} from '../../../../selectors/reports';
import classes from  '../templates.module.scss';
import { convertDate } from 'helpers/dateHelper';
import ReportDropDownMenu from '../../dropdown/report-dropdown-menu';
import { ButtonText } from '../styledComponents'; 
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const ReportByAgeTemplate = (): React.ReactElement => {

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
            Location: location,
            RenderUniquePatientsReportType: !reportType,      
        };
    }

  
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search Report
                    </TitleText>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Select Indicator Report
                    </TitleTextH2>
                </Grid>
                <ReportDropDownMenu name={'Screening Results by Age'}/>
                
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
                        disabled={isLoading}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => dispatch(postFilteredReportsByAgeRequest())}
                    >
                        <ButtonText>Apply</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        disabled={isLoading}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => {
                            dispatch(resetReportsByAgeSearchParametersRequest());
                            setExpandedGPRA(true);
                            setExpandedCustom(false);
                            // handleExpandGPRA(true);
                        } }
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Clear</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        disabled={isLoading}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => {
                            try {
                                postReportByAgePrint(prepareFilterProps()).then(data => {
                                    fileDownload.default(data.Data, data.Filename)});
                            
                            } catch(e) {
                                console.log(':-)))');
                            }
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Print</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default ReportByAgeTemplate;
