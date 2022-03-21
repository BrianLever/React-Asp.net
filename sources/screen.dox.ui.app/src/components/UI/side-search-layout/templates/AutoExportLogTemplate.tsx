import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import { TitleText,TitleTextH2,ContainerBlock } from './styledComponents';
import classes from  '../templates.module.scss';
import { deleteErrorLogsRequest, getErrorLogRequest, printErrorLogsExcelRequest, setErrorLogEndDate, setErrorLogStartDate } from 'actions/error-log';
import { getAutoExportLogsEndDateSelector, getAutoExportLogsFilterNameSelector, getAutoExportLogsStartDateSelector, isAutoExportLogsLoadingSelector } from 'selectors/auto-export-logs';
import { getAutoExportLogsRequest, setAutoExportLogsEndDate, setAutoExportLogsFilterName, setAutoExportLogsStartDate } from 'actions/auto-export-logs';
import { ButtonText , DateToTextStyle, TemplateDateInputWrapper, TemplateDateInput, TemplateTextInput } from '../styledComponents';

const AutoExportLogTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const isLoading: boolean = useSelector(isAutoExportLogsLoadingSelector);
    const startDate: string | null = useSelector(getAutoExportLogsStartDateSelector);
    const endDate: string | null = useSelector(getAutoExportLogsEndDateSelector);
    const filterName: string = useSelector(getAutoExportLogsFilterNameSelector);
   
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search Auto-Export Log
                    </TitleText>
                </Grid>
            
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Export Date
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Grid container>
                        <Grid item xs={12}>
                            <TemplateDateInputWrapper>
                                <TemplateDateInput type="date" 
                                    onChange={(e) => {
                                        dispatch(setAutoExportLogsStartDate(`${e.target.value}`));
                                    }}
                                />
                            </TemplateDateInputWrapper>
                        </Grid>
                        <Grid item xs={12}>
                            <DateToTextStyle>to</DateToTextStyle>
                        </Grid>
                        <Grid item xs={12}>
                            <TemplateDateInputWrapper>
                                <TemplateDateInput type="date" 
                                    onChange={(e) => {
                                        dispatch(setAutoExportLogsEndDate(`${e.target.value}`));
                                    }}
                                />
                            </TemplateDateInputWrapper>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Name
                    </TitleTextH2>
                </Grid> 
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TemplateTextInput 
                        type="text"
                        value={filterName || ''}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setAutoExportLogsFilterName(e.target.value))
                        }}
                    />
                    {/* <TextField
                        fullWidth
                        margin="dense"
                        label="Name"
                        id="outlined-margin-none"
                        variant="outlined"
                        value={filterName || ''}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setAutoExportLogsFilterName(e.target.value))
                        }}
                    /> */}
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
                        onClick={() => dispatch(getAutoExportLogsRequest())}
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
                            const defaultDate = new Date().toISOString();
                            dispatch(setAutoExportLogsStartDate(null));
                            dispatch(setAutoExportLogsEndDate(defaultDate));
                            dispatch(setAutoExportLogsFilterName(''));
                            dispatch(getAutoExportLogsRequest());
                        }}
                    >
                        <ButtonText style={{ color: "#2e2e42" }}>Clear Fields</ButtonText>
                    </Button>
                </Grid>                            
            </Grid>
        </ContainerBlock>
    )
}

export default AutoExportLogTemplate;
