import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import * as fileDownload from 'js-file-download';
import { 
    Button, Checkbox, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import CustomDateRange from '../../../UI/daterange/custom/index';
import GPRAReportingPeriod from '../../../UI/daterange/gpra-reporting-period';
import {ILocationItemResponse } from  '../../../../actions';
import { resetReportsSearchParametersRequest,setReportsCurrentStartDate,setReportsCurrentEndDate,getScreeningResultReportsBySortRequest,
    setReportsCurrentGPRAPeriodRangeChange,setReportsLocationsId,getLocationListActionRequest, resetReportsByAgeSearchParametersRequest, FilterBySort, FilterBySortItem, setScreeningResultReportBySortFilterArray
 } from '../../../../actions/reports';

 import { 
    getReportLocationsSelector, getReportSelectedLocationIdSelector, getReportsBySortFilterArraySelector,
    getReportGrpaPeriodKeySelector, getReportStartDateSelector, getReportEndDateSelector,
    getReportBsrReportTypeSelector,isReportLoadingSelector,getReportGrpaPeriodsSelector, getEarliestDateSelector
} from '../../../../selectors/reports';

import classes from  '../templates.module.scss';
import { makeStyles } from '@material-ui/core/styles';
import { convertDate } from 'helpers/dateHelper';
import { reportSortByFilterArray } from 'actions/reports';
import FilterDropdownItem from 'components/UI/filter-dropdown-item';
import ReportDropDownMenu from '../../dropdown/report-dropdown-menu';
import { ButtonText } from '../styledComponents'; 
import ScreendoxSelect from 'components/UI/select';


const ReportBySortTemplate = (): React.ReactElement => {

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
    const filterArray = useSelector(getReportsBySortFilterArraySelector);
    const endDateStat = new Date().toISOString();
    const [filterSortArray, setFilterSortArray] = useState(reportSortByFilterArray);
    const [tcc, setTcc] = React.useState(false);
    const [tccItem, setTccItem] = React.useState(reportSortByFilterArray[0].items);
    const [cage, setCage] = useState(false);
    const [cageItem, setCageItem] = useState(reportSortByFilterArray[1].items);
    const [dast, setDast] = useState(false);
    const [dastItem, setDastItem] = useState(reportSortByFilterArray[2].items);
    const [doch, setDoch] = useState(false);
    const [dochItem, setDochItem] = useState(reportSortByFilterArray[3].items);
    const [gad7a, setGad7a] = useState(false);
    const [gad7aItem, setGad7aItem] = useState(reportSortByFilterArray[4].items);
    const [phq, setPhq] = useState(false);
    const [phqItem, setPhqItem] = useState(reportSortByFilterArray[5].items);
    const [phq2, setPhq2] = useState(false);
    const [phq2Item, setPhq2Item] = useState(reportSortByFilterArray[6].items);
    const [hits, setHits] = useState(false);
    const [hitsItem, setHitsItem] = useState(reportSortByFilterArray[7].items);
    const [bbgs, setBbgs] = useState(false);
    const [bbgsItem, setBbgsItem] = useState(reportSortByFilterArray[8].items);


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
        const endDateStat = new Date().toISOString();
        if (gpraPeriod) {
            const periodObject = periods.find(p => p.Label === gpraPeriod);
            startDateGPRAPeriod = !!periodObject ? periodObject.StartDate : "2020-10-01";
            endDateGPRAPeriod = !!periodObject ? periodObject.EndDate : endDateStat;
        }    
        return {        
            StartDate: startDate || startDateGPRAPeriod || "2019-10-01",
            EndDate: endDate || endDateGPRAPeriod || endDateStat,
            Location: location,
            RenderUniquePatientsReportType: !reportType,      
        };
    }

    const handleCheck = (item: FilterBySortItem, value: boolean, index: number) => {
        
        var items = tccItem;
        var setOptions = null;
        switch(index) {
            case 0:
                items =  tccItem;
                setOptions = setTccItem;
                break;
            case 1:
                items = cageItem;
                setOptions = setCageItem;
                break;
            case 2:
                items = dastItem;
                setOptions = setDastItem;
                break;
            case 3:
                items = dochItem;
                setOptions = setDochItem;
                break;
            case 4:
                items = gad7aItem;
                setOptions = setGad7aItem;
                break;
            case 5:
                items = phqItem;
                setOptions = setPhqItem;
                break;
            case 6:
                items = phq2Item;
                setOptions = setPhq2Item;
                break;
            case 7:
                items = hitsItem;
                setOptions = setHitsItem;
                break;
            case 8:
                items = bbgsItem;
                setOptions = setBbgsItem;
                break;
            default:
                break;
        }

        items =  items.map(a => {
            if(a.name === item.name && a.value === item.value) {
                a.checked = value;
            }
            return a;
        })

        if(setOptions){
            setOptions(items);
        }
        setFilterArray()
        
    }

    const unHandleCheck = (index: number, value: boolean) => {
        if(value) {
            var setOptions = null;
            var items = tccItem;
            switch(index) {
                case 0:
                    items =  tccItem;
                    setOptions = setTccItem;
                    break;
                case 1:
                    items = cageItem;
                    setOptions = setCageItem;
                    break;
                case 2:
                    items = dastItem;
                    setOptions = setDastItem;
                    break;
                case 3:
                    items = dochItem;
                    setOptions = setDochItem;
                    break;
                case 4:
                    items = gad7aItem;
                    setOptions = setGad7aItem;
                    break;
                case 5:
                    items = phqItem;
                    setOptions = setPhqItem;
                    break;
                case 6:
                    items = phq2Item;
                    setOptions = setPhq2Item;
                    break;
                case 7:
                    items = hitsItem;
                    setOptions = setHitsItem;
                    break;
                case 8:
                    items = bbgsItem;
                    setOptions = setBbgsItem;
                    break;
                default:
                    break;
            }

            items =  items.map(a => {
                a.checked = false;
                return a;
            })

            if(setOptions){
                setOptions(items);
            }
            setFilterArray();
        }
    }
     
    const setFilterArray = () => {
        var data = [
            ...tccItem,
            ...cageItem,
            ...dastItem,
            ...gad7aItem,
            ...dochItem,
            ...phqItem,
            ...phq2Item,
            ...hitsItem,
            ...bbgsItem,
        ];
        var el = data.filter(elem => elem.checked === true);
        var totalArray: { ScreeningSection: string, MinScoreLevel: number }[] = [];
        el.map(item => {
            totalArray.push({
                "ScreeningSection": item.name,
                "MinScoreLevel": item.value
            });
        })

        dispatch(setScreeningResultReportBySortFilterArray(totalArray));
    }

    React.useEffect(() => {
        if (!locations.length && (locationId === 0)) {
             dispatch(getLocationListActionRequest())
        }
    }, [locations.length, locationId, dispatch]);
    
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
                <ReportDropDownMenu name={'Screening Results by Sort'}/>
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
                        selectedTillDate={endDate}
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
                        Sort By
                    </TitleTextH2>
                </Grid>
                
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <FilterDropdownItem data={tccItem} name={filterSortArray[0].name} changeHandler={handleCheck} index={0} clickHandler={() => setTcc(!tcc)} open={tcc} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={cageItem} name={filterSortArray[1].name} changeHandler={handleCheck} index={1} clickHandler={() => setCage(!cage)} open={cage} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={dastItem}  name={filterSortArray[2].name} changeHandler={handleCheck} index={2} clickHandler={() => setDast(!dast)} open={dast} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={dochItem}  name={filterSortArray[3].name} changeHandler={handleCheck} index={3} clickHandler={() => setDoch(!doch)} open={doch} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={gad7aItem}  name={filterSortArray[4].name} changeHandler={handleCheck} index={4} clickHandler={() => setGad7a(!gad7a)} open={gad7a} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={phqItem}  name={filterSortArray[5].name} changeHandler={handleCheck} index={5} clickHandler={() => setPhq(!phq)} open={phq} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={phq2Item}  name={filterSortArray[6].name} changeHandler={handleCheck} index={6} clickHandler={() => setPhq2(!phq2)} open={phq2} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={hitsItem}  name={filterSortArray[7].name} changeHandler={handleCheck} index={7} clickHandler={() => setHits(!hits)} open={hits} unHandleCheck={unHandleCheck}/>
                    <FilterDropdownItem data={bbgsItem}  name={filterSortArray[8].name} changeHandler={handleCheck} index={8} clickHandler={() => setBbgs(!bbgs)} open={bbgs} unHandleCheck={unHandleCheck}/>
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
                        onClick={() => dispatch(getScreeningResultReportsBySortRequest())}
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
                            setExpandedGPRA(true);
                            setExpandedCustom(false);
                            // handleExpandGPRA(true);
                        } }
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Clear</ButtonText>
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
                                // postReportByAgePrint(prepareFilterProps()).then(data => {
                                //     fileDownload.default(data.Data, data.Filename)});
                            
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

export default ReportBySortTemplate;
