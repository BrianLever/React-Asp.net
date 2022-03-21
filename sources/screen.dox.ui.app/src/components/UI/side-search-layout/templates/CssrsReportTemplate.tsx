import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { TitleText, TitleTextH2,ContainerBlock } from './styledComponents';
import { getCssrsListRequest } from 'actions/c-ssrs-list';
import { EAssessmentRouterKeys, ERouterUrls } from 'router';
import { useHistory } from 'react-router';
import { cssrsReportBirthdaySelector, cssrsReportFirstNameSelector, cssrsReportLastNameSelector, cssrsReportLocationsSelector,cssrsReportLocationIdSelector, cssrsReportMiddleNameSelector, isCssrsReportPatientRecordLoadingSelector } from 'selectors/c-ssrs-list/c-ssrs-report';
import { cssrsReportCreateRequest, cssrsReportPatientRecordsRequest, resetCssrsReportParameter, setCssrsReportBirthday, setCssrsReportFirstName, setCssrsReportLastName, setCssrsReportLocationId, setCssrsReportMiddleName } from 'actions/c-ssrs-list/c-ssrs-report';
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { IBranchLocationItemResponse } from 'actions/branch-locations';
import { ECssrsListActions, ILocationItemResponse } from 'actions';
import { ButtonText } from '../styledComponents';
import { EMPTY_LOCALTION_LIST_VALUE } from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const CssrsReportTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const history = useHistory();
    const firstName = useSelector(cssrsReportFirstNameSelector);
    const lastName = useSelector(cssrsReportLastNameSelector);
    const middleName = useSelector(cssrsReportMiddleNameSelector);
    const birthday = useSelector(cssrsReportBirthdaySelector);
    const isLoading = useSelector(isCssrsReportPatientRecordLoadingSelector);
    const locations = useSelector(cssrsReportLocationsSelector);
    const locationId = useSelector(cssrsReportLocationIdSelector);

    const handleClick = () => {
        dispatch(cssrsReportCreateRequest({
            FirstName: firstName,
            LastName: lastName,
            MiddleName: middleName,
            Birthday: birthday,
            BranchLocationID: locationId
        }))
    }
    
    return (
        <ContainerBlock>
        <Grid container spacing={1} >
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
                    InputLabelProps={{style: {fontWeight:'bold'}}} 
                    inputProps={{style: {fontWeight: 'bold'}}} 
                    id="outlined-margin-none"
                    variant="outlined"
                    value={firstName || ''}
                    onChange={(e: any) => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setCssrsReportFirstName(value));
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
                        dispatch(setCssrsReportLastName(value));
                    }}
                />
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <TextField
                    fullWidth
                    margin="dense"
                    label="Middle Name"
                    InputLabelProps={{style: {fontWeight:'bold'}}} 
                    inputProps={{style: {fontWeight: 'bold'}}} 
                    id="outlined-margin-none"
                    variant="outlined"
                    value={middleName || ''}
                    onChange={(e: any) => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setCssrsReportMiddleName(value));
                    }}
                />
            </Grid>       
            <Grid item sm={12} xs={12}>
                <MuiPickersUtilsProvider utils={DateFnsUtils}>
                    <KeyboardDatePicker
                         format="MM/dd/yyyy"
                         margin="normal"                           
                         inputVariant="outlined"
                         id="date-picker-from"
                         label="Date of Birth"  
                         InputLabelProps={{style: {fontWeight:'bold'}}}    
                         inputProps={{style: {fontWeight: 'bold'}}}
                         fullWidth                          
                         value={birthday}
                         onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                             if (`${date}` !== 'Invalid Date') {
                                 dispatch(setCssrsReportBirthday(date));
                             }
                         }}
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
                                dispatch(setCssrsReportLocationId(v))                                
                            }}
                        />  
                </FormControl>
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
                        dispatch(cssrsReportPatientRecordsRequest())
                    }
                >
                    <ButtonText>
                        Search
                    </ButtonText>
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
                        handleClick();
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
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42'}}
                    onClick={() => {
                        dispatch(resetCssrsReportParameter())
                        dispatch(cssrsReportPatientRecordsRequest())
                    }}
                >
                    <ButtonText style={{ color: '#2e2e42' }}>Clear</ButtonText>
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
                    onClick={() =>
                        history.push(ERouterUrls.CSSRS_LIST)
                    }
                >
                    <ButtonText>Return To CSSRS-List</ButtonText>
                </Button>
            </Grid>
        </Grid>
    </ContainerBlock>
    )
}

export default CssrsReportTemplate;
