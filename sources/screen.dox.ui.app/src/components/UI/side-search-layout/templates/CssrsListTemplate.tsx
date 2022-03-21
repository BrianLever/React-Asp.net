import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, MenuItem, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2, ContainerBlock } from './styledComponents';
import CustomDateRange from '../../../UI/daterange/custom/index';
import GPRAReportingPeriod from '../../../UI/daterange/gpra-reporting-period';
import { getListBranchLocationsSelector, setCreateDateCustomOrGPRASelector } from '../../../../selectors/shared';
import { setCreateDateCustomOrGPRA  } from '../../../../actions/shared';
import { ILocationItemResponse } from  '../../../../actions';
import ScreendoxRadioComponent from '../../../../components/UI/radio';
import { reportsIndicateReportTypeArray } from '../../../../actions/reports';
import { getCssrsListEndDateSelector, getCssrsListLocationIdSelector, getCssrsListReportTypeSelector, getCssrsListFirstNameSelector, getCssrsListGpraPeriodKeySelector, getCssrsListGpraPeriodsSelector, getCssrsListLastNameSelector, getCssrsListLocationsSelector, getCssrsListStartDateSelector, isCssrsListLoadingSelector, getCssrsListDateofBirthSelector } from 'selectors/c-ssrs-list';
import { getCssrsListRequest, getCssrsLocationListActionRequest, resetCssrsListParameter, setCssrsListBsrReportType, setCssrsListDateofBirth, setCssrsListEndDate, setCssrsListFirstName, setCssrsListGpraPeriodKey, setCssrsListLastName, setCssrsListLocationId, setCssrsListScreeningResultId, setCssrsListStartDate } from 'actions/c-ssrs-list';
import { useHistory } from 'react-router';
import { ERouterUrls } from 'router';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { ButtonText } from '../styledComponents';
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const CssrsListTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const history = useHistory();
    const endDate = useSelector(getCssrsListEndDateSelector);
    const gpraPeriod = useSelector(getCssrsListGpraPeriodKeySelector);
    const periods = useSelector(getCssrsListGpraPeriodsSelector)
    const isLoading:boolean = useSelector(isCssrsListLoadingSelector);
    const lastName = useSelector(getCssrsListLastNameSelector);
    const firstName = useSelector(getCssrsListFirstNameSelector);
    const dateofBirth=useSelector(getCssrsListDateofBirthSelector);
    const startDate = useSelector(getCssrsListStartDateSelector); 
    const locations = useSelector(getCssrsListLocationsSelector);
    const locationId = useSelector(getCssrsListLocationIdSelector);    
    const [ isExpandedCustom, setExpandedCustom ] = React.useState(false);
    const [ isExpandedGPRA, setExpandedGPRA ] = React.useState(true);   
    React.useEffect(() => {
        if (!locations.length && (locationId === 0)) {
             dispatch(getCssrsLocationListActionRequest())
        }
    }, [locations.length, locationId, dispatch]);
  
    const cleanPeriods = () => {
        if (isExpandedGPRA) {                  
            dispatch(setCssrsListStartDate(null));
            dispatch(setCssrsListEndDate(null));
            dispatch(setCssrsListGpraPeriodKey(''));
        } else if (isExpandedCustom) {
            dispatch(setCssrsListStartDate(null));
            dispatch(setCssrsListEndDate(null));
            if (periods[0]) {
                dispatch(setCssrsListGpraPeriodKey(periods[0].Label));
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
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search  or Add New Patient
                    </TitleText>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: 30 }}>
                    <TitleTextH2>
                        Patient Information
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center'}}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label="First Name"
                        InputLabelProps={{style: {fontWeight:'bold'}}} 
                        inputProps={{style: {fontWeight: 'bold'}}}                                             
                        id="outlined-margin-none"
                        variant="outlined"
                        value={firstName || ''}
                        onChange={(e: any) => {
                            e.stopPropagation();
                            const value = `${e.target.value}`;
                            dispatch(setCssrsListFirstName(value));
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label="Last Name"
                        InputLabelProps={{style: {fontWeight:'bold'}}} 
                        inputProps={{style: {fontWeight: 'bold'}}}  
                        id="outlined-margin-none"
                        variant="outlined"
                        value={lastName || ''}
                        onChange={(e: any) => {
                            e.stopPropagation();
                            const value = `${e.target.value}`;
                            dispatch(setCssrsListLastName(value));
                        }}
                       
                    />
                </Grid>

                <Grid item sm={12} xs={12}>
                    <MuiPickersUtilsProvider utils={DateFnsUtils} >
                        <KeyboardDatePicker                                                     
                             format="MM/dd/yyyy"
                             margin="normal"                           
                             inputVariant="outlined"
                             id="date-picker-from"
                             label="Date of Birth"  
                             InputLabelProps={{style: {fontWeight:'bold'}}}    
                             inputProps={{style: {fontWeight: 'bold'}}}                           
                             fullWidth   
                             InputProps={{
                                style: {                                                                    
                                    height: 38                                                                   
                                }
                            }}                       
                             value={dateofBirth}
                             onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                                 if (`${date}` !== 'Invalid Date') {
                                     dispatch(setCssrsListDateofBirth(date));
                                 }
                             }}
                             style={{  padding: '7px 0 0 0' }}
                        />
                    </MuiPickersUtilsProvider>
                </Grid>            
               
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Location
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12}>
                    <FormControl fullWidth variant="outlined">                       
                        <ScreendoxSelect
                            options={locations.map((location: ILocationItemResponse) => (
                                { name: `${location.Name}`.slice(0, 20), value: location.ID}
                            ))}
                            defaultValue={locationId}
                            rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const v = parseInt(`${value}`);
                                dispatch(setCssrsListLocationId(v))                                
                            }}
                        />                      
                    </FormControl>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px'}}>
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
                           dispatch(setCssrsListStartDate(d && d.toISOString()))
                        }}
                        handleTillDateChange={(d: Date | null) => {
                            dispatch(setCssrsListEndDate(d && d.toISOString()))
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
                           dispatch(setCssrsListGpraPeriodKey(s));
                        }}
                    />
                </Grid>

                
                
                <Grid item xs={12} style={{ textAlign: 'center', marginTop: 30 }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        disabled={isLoading}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() =>
                            dispatch(getCssrsListRequest())
                        }
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
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                            history.push(ERouterUrls.CSSRS_LIFETIME_ADD_REPORT)
                        }}
                    >
                       
                        <ButtonText>Add New Patient</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        disabled={isLoading}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', marginTop: 30 }}
                        onClick={() => {
                            dispatch(resetCssrsListParameter())
                            dispatch(getCssrsListRequest())
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
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        onClick={() => {
                           
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Print</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default CssrsListTemplate;
