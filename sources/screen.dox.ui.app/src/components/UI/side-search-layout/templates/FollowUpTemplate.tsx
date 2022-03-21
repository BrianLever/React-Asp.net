import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import CustomDateRange from '../../../UI/daterange/custom/index';
import GPRAReportingPeriod from '../../../UI/daterange/gpra-reporting-period';
import { 
    setFollowUpFirstName, setFollowUpLastName, setFollowUpScreeningID, setFollowUpLocationsId, setFollowUpBSRReportType,
    setFollowUpCurrentStartDate, setFollowUpCurrentEndDate, setFollowUpCurrentGPRAPeriodRangeChange, postFilteredFollowUpsRequest, 
    resetFollowUpSearchParametersRequest, getLocationListAction
} from '../../../../actions/follow-up';

import { 
    setLastName, setFirstName, triggerSearchScreeningListSearchBarParameters, 
    triggerClearScreeningListSearchBarParameters, setCurrentGPRAPeriodRangeChange,
    setScreendoxId, setCurrentScreenListStartDate, setCurrentScreenListEndDate, 
    setLocationIdAction,  
} from '../../../../actions/screen';

import { 
    setVisitListFirstName, setVisitListLastName, setVisitListScreeningID, 
    setVisitSearchLocationsId, setVisitCurrentScreenListStartDate, setVisitCurrentScreenListEndDate,
    setVisitCurrentGPRAPeriodRangeChange, requestToCleanVisitSearchParameters
} from '../../../../actions/visit';

import { setCreateDateCustomOrGPRASelector } from 'selectors/shared';
import { setCreateDateCustomOrGPRA } from 'actions/shared';
import {ILocationItemResponse } from  '../../../../actions';
import ScreendoxRadioComponent from '../../../../components/UI/radio';
import { visitReportsArray } from '../../../../actions/visit';
import postAllFollowUpPrint from '../../../../api/calls/post-all-follow-up-print';
import { 
    isFollowUpLoadingSelector, getFollowUpScreeningResultIdSelector, getFollowUpEndDateSelector,
    getFollowUpLocationsSelector, getFollowUpSelectedLocationIdSelector, getFollowUpBsrReportTypeSelector,
    getFollowUpStartDateSelector, getFollowUpFirstNameSelector, getFollowUpLastNameSelector,
    getFollowUpGrpaPeriodsSelector,getFollowUpGrpaPeriodKeySelector,
} from '../../../../selectors/follow-up';
import classes from  '../templates.module.scss';
import { ButtonText } from '../styledComponents';  
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreenDoxSelect from 'components/UI/select';


const FollowUpTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const isLoading = useSelector(isFollowUpLoadingSelector);
    const screendoxResultId = useSelector(getFollowUpScreeningResultIdSelector);
    const endDate = useSelector(getFollowUpEndDateSelector);
    const gpraPeriod = useSelector(getFollowUpGrpaPeriodKeySelector);
    const periods = useSelector(getFollowUpGrpaPeriodsSelector)
    const lastName = useSelector(getFollowUpLastNameSelector);
    const firstName = useSelector(getFollowUpFirstNameSelector);
    const startDate = useSelector(getFollowUpStartDateSelector);
    const locations = useSelector(getFollowUpLocationsSelector);
    const locationId = useSelector(getFollowUpSelectedLocationIdSelector);
    const reportType = useSelector(getFollowUpBsrReportTypeSelector);
    const isCreateDateCustom: boolean = useSelector(setCreateDateCustomOrGPRASelector);
    const [ isExpandedCustom, setExpandedCustom ] = React.useState(isCreateDateCustom);
    const [ isExpandedGPRA, setExpandedGPRA ] = React.useState(!isCreateDateCustom);

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
            dispatch(setFollowUpCurrentGPRAPeriodRangeChange(''));
        } else if (isExpandedCustom) {
            dispatch(setFollowUpCurrentStartDate(null));
            dispatch(setFollowUpCurrentEndDate(null));
            if (periods[0]) {
                dispatch(setFollowUpCurrentGPRAPeriodRangeChange(periods[0].Label));
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
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: 30 }}>
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
                            dispatch(setFollowUpFirstName(e.target.value))
                            dispatch(setFirstName(e.target.value))
                            dispatch(setVisitListFirstName(e.target.value))
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(postFilteredFollowUpsRequest())
                             }
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
                            dispatch(setFollowUpLastName(e.target.value))
                            dispatch(setLastName(e.target.value))
                            dispatch(setVisitListLastName(e.target.value))
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(postFilteredFollowUpsRequest())
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
                            dispatch(setFollowUpScreeningID(parseInt(e.target.value)))
                            dispatch(setScreendoxId(parseInt(e.target.value)))
                            dispatch(setVisitListScreeningID(parseInt(e.target.value)))
                        }}
                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(postFilteredFollowUpsRequest())
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
                    <ScreenDoxSelect
                        options={locations.map((location: ILocationItemResponse) => (
                            { name: `${location.Name}`.slice(0, 20), value: location.ID}
                        ))}
                        defaultValue={locationId}
                        rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                        changeHandler={(value: any) => {
                            const v = parseInt(`${value}`);
                            dispatch(setFollowUpLocationsId(v))
                            dispatch(setLocationIdAction(v))
                            dispatch(setVisitSearchLocationsId(v))
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
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Report Type
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <ScreendoxRadioComponent
                        selected={reportType}
                        elementsRenderArray={visitReportsArray}
                        onSelectRadio={(v: number) => {
                            dispatch(setFollowUpBSRReportType(v));
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center', marginTop: 30  }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        className={classes.removeBoxShadow}
                        disabled={isLoading}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => dispatch(postFilteredFollowUpsRequest())}
                    >
                        <ButtonText>Search</ButtonText>
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
                            dispatch(triggerClearScreeningListSearchBarParameters());
                            dispatch(resetFollowUpSearchParametersRequest());
                            dispatch(requestToCleanVisitSearchParameters());
                            handleExpandGPRA(true);
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
                        style={{ backgroundColor: '#2e2e42', border: '1px solid #2e2e42' }}
                        onClick={() => {
                            try {
                                postAllFollowUpPrint().then(data => {
                                    fileDownload.default(data.Data, data.Filename);
                                });
                            } catch(e) {
                                console.log(':-)))');
                            }
                        }}
                    >
                        <ButtonText>Print</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default FollowUpTemplate;
