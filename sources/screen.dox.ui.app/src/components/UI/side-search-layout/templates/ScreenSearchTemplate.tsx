import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,  ContainerBlock } from './styledComponents';
import CustomDateRange from '../../../UI/daterange/custom/index';
import GPRAReportingPeriod from '../../../UI/daterange/gpra-reporting-period';
import { 
    getScreenListFirstNameSelector, getScreenListLastNameSelector, 
    getScreenListScreendoxResultIdSelector, getScreenListStartDateSelector, 
    getScreenListEndDateSelector, getScreenListGrpaPeriodKey, isScreenListLoadingSelector, 
    getScreenListLocationList, getScreenListSelectedLocationId, getScreenListGrpaPeriodList
} from '../../../../selectors/screen';
import { setCreateDateCustomOrGPRASelector } from 'selectors/shared'; 
import { setCreateDateCustomOrGPRA } from 'actions/shared';
import { 
    setLastName, setFirstName, triggerSearchScreeningListSearchBarParameters, 
    triggerClearScreeningListSearchBarParameters, setCurrentGPRAPeriodRangeChange,
    setScreendoxId, setCurrentScreenListStartDate, setCurrentScreenListEndDate, 
    getLocationListAction, setLocationIdAction,  
} from '../../../../actions/screen';

import { 
    setVisitListFirstName, setVisitListLastName, setVisitListScreeningID, 
    setVisitSearchLocationsId, setVisitCurrentScreenListStartDate, setVisitCurrentScreenListEndDate,
    setVisitCurrentGPRAPeriodRangeChange, requestToCleanVisitSearchParameters
} from '../../../../actions/visit';

import { 
    setFollowUpFirstName, setFollowUpLastName, setFollowUpScreeningID, setFollowUpLocationsId, setFollowUpBSRReportType,
    setFollowUpCurrentStartDate, setFollowUpCurrentEndDate, setFollowUpCurrentGPRAPeriodRangeChange, postFilteredFollowUpsRequest, 
    resetFollowUpSearchParametersRequest
} from '../../../../actions/follow-up';


import { ILocationItemResponse } from '../../../../actions';
import classes from  '../templates.module.scss';
import { useStyles } from '../styledComponents';
import { ButtonText } from '../styledComponents'; 
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const ScreenSearchTemplate = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const endDate = useSelector(getScreenListEndDateSelector);
    const gpraPeriod = useSelector(getScreenListGrpaPeriodKey);
    const periods = useSelector(getScreenListGrpaPeriodList)
    const isLoading = useSelector(isScreenListLoadingSelector);
    const lastName = useSelector(getScreenListLastNameSelector);
    const firstName = useSelector(getScreenListFirstNameSelector);
    const startDate = useSelector(getScreenListStartDateSelector);
    const screendoxResultId = useSelector(getScreenListScreendoxResultIdSelector);
    const locations = useSelector(getScreenListLocationList);
    const locationId = useSelector(getScreenListSelectedLocationId);
    const isCreateDateCustom = useSelector(setCreateDateCustomOrGPRASelector);
    const [ isExpandedCustom, setExpandedCustom ] = React.useState(isCreateDateCustom);
    const [ isExpandedGPRA, setExpandedGPRA ] = React.useState(!isCreateDateCustom);
    const customClasses = useStyles();

    React.useEffect(() => {
        if (!locations.length && (locationId === 0)) {
            dispatch(getLocationListAction())
        }
    }, [locations.length, locationId, dispatch]);

    const cleanPeriods = () => {
        if (isExpandedGPRA) {
            const defaultDate = new Date().toISOString();
            dispatch(setVisitCurrentScreenListStartDate(defaultDate));
            dispatch(setVisitCurrentScreenListEndDate(defaultDate));
            dispatch(setCurrentScreenListStartDate(defaultDate));
            dispatch(setFollowUpCurrentStartDate(defaultDate));
            dispatch(setCurrentScreenListEndDate(defaultDate));
            dispatch(setFollowUpCurrentEndDate(defaultDate));
            dispatch(setCurrentGPRAPeriodRangeChange(''));
        } else if (isExpandedCustom) {
            dispatch(setCurrentScreenListStartDate(null));
            dispatch(setCurrentScreenListEndDate(null));
            if (periods[0]) {
                dispatch(setCurrentGPRAPeriodRangeChange(periods[0].Label));
            }
        }
    }

    const handleExpandGPRA = (v: boolean): void => {
        dispatch(setCreateDateCustomOrGPRA())
        cleanPeriods();
        setExpandedGPRA(v);
        setExpandedCustom(!v);
    }

    const handleExpandCustom = (v: boolean): void => {
        dispatch(setCreateDateCustomOrGPRA())
        cleanPeriods();
        setExpandedCustom(v);
        setExpandedGPRA(!v);
    }

    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search Patient
                    </TitleText>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Patient Information
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label="First Name"
                        id="outlined-margin-none"
                        variant="outlined"
                        value={firstName || ''}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setFirstName(e.target.value))
                            dispatch(setVisitListFirstName(e.target.value))
                            dispatch(setFollowUpFirstName(e.target.value))
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(triggerSearchScreeningListSearchBarParameters())
                             }
                        }}
                        InputLabelProps={{
                            classes: {
                              root: customClasses.cssLabel,
                              focused: customClasses.cssFocused,
                            },
                        }}
                        InputProps={{
                            classes: {
                              root: classes.cssOutlinedInput,
                              focused: classes.cssFocused,
                              notchedOutline: customClasses.notchedOutline
                            },
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label="Last Name"
                        id="outlined-margin-none"
                        variant="outlined"
                        value={lastName || ''}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setLastName(e.target.value))
                            dispatch(setVisitListLastName(e.target.value))
                            dispatch(setFollowUpLastName(e.target.value))
                        }}
                        InputLabelProps={{
                            classes: {
                              root: customClasses.cssLabel,
                              focused: customClasses.cssFocused,
                            },
                        }}
                        InputProps={{
                            classes: {
                              root: classes.cssOutlinedInput,
                              focused: classes.cssFocused,
                              notchedOutline: customClasses.notchedOutline
                            },
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(triggerSearchScreeningListSearchBarParameters())
                             }
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label="Screendox ID"
                        id="outlined-margin-none"
                        variant="outlined"
                        value={screendoxResultId || ''}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setScreendoxId(parseInt(e.target.value)))
                            dispatch(setVisitListScreeningID(parseInt(e.target.value)))
                            dispatch(setFollowUpScreeningID(parseInt(e.target.value)))
                        }}
                        InputLabelProps={{
                            classes: {
                              root: customClasses.cssLabel,
                              focused: customClasses.cssFocused,
                            },
                        }}
                        InputProps={{
                            classes: {
                              root: classes.cssOutlinedInput,
                              focused: classes.cssFocused,
                              notchedOutline: customClasses.notchedOutline
                            },
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(triggerSearchScreeningListSearchBarParameters())
                             }
                        }}
                    />
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
                            dispatch(setLocationIdAction(v))
                            dispatch(setVisitSearchLocationsId(v))
                            dispatch(setFollowUpLocationsId(v))
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Created Date
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <CustomDateRange  
                        selectedFromDate={startDate} 
                        selectedTillDate={endDate}
                        isExpanded={isExpandedCustom}
                        setExpanded={v => {
                            handleExpandCustom(v);
                        }}
                        handleFromDateChange={(d: Date | null) => {
                            dispatch(setCurrentScreenListStartDate(d && d.toISOString()));
                            dispatch(setVisitCurrentScreenListStartDate(d && d.toISOString()));
                            dispatch(setFollowUpCurrentStartDate(d && d.toISOString()));
                            
                        }}
                        handleTillDateChange={(d: Date | null) => {
                            dispatch(setCurrentScreenListEndDate(d && d.toISOString()));
                            dispatch(setVisitCurrentScreenListEndDate(d && d.toISOString()));
                            dispatch(setFollowUpCurrentEndDate(d && d.toISOString()));
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
                            dispatch(setCurrentGPRAPeriodRangeChange(s));
                            dispatch(setVisitCurrentGPRAPeriodRangeChange(s));
                            dispatch(setFollowUpCurrentGPRAPeriodRangeChange(s));
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        disabled={isLoading}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', textTransform: 'none', marginTop: 30}}
                        onClick={() => dispatch(triggerSearchScreeningListSearchBarParameters()) }
                    >
                        <ButtonText>Search</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        disabled={isLoading}
                        variant="contained" 
                        color="default"
                        className={classes.removeBoxShadow}
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', textTransform: 'none',}}
                        onClick={() => {
                            dispatch(triggerClearScreeningListSearchBarParameters());
                            dispatch(resetFollowUpSearchParametersRequest());
                            dispatch(requestToCleanVisitSearchParameters());
                            handleExpandGPRA(true);
                        } }
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Clear</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default ScreenSearchTemplate;