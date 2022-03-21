import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import * as fileDownload from 'js-file-download';
import { 
    Button, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import { TitleText,TitleTextH2,ContainerBlock } from './styledComponents';
import printErrorLogs  from '../../../../api/calls/post-print-all-error-logs';
import classes from  '../templates.module.scss';
import { deleteErrorLogsRequest, getErrorLogRequest, printErrorLogsExcelRequest, setErrorLogEndDate, setErrorLogStartDate } from 'actions/error-log';
import { getErrorLogEndDateSelector, getErrorLogStartDateSelector, isErrorLogLoadingSelector } from 'selectors/error-log';
import { ButtonText , DateToTextStyle, TemplateDateInputWrapper, TemplateDateInput } from '../styledComponents';

const ErrorLogTemplate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const isLoading: boolean = useSelector(isErrorLogLoadingSelector);
    const startDate: string | null = useSelector(getErrorLogStartDateSelector);
    const endDate: string | null = useSelector(getErrorLogEndDateSelector);
    
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleText>
                        Search Error Log
                    </TitleText>
                </Grid>
            
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Date
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Grid container>
                        <Grid item xs={12}>
                            <TemplateDateInputWrapper>
                                <TemplateDateInput type="date" 
                                    onChange={(e) => {
                                        dispatch(setErrorLogStartDate(e.target.value));
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
                                        dispatch(setErrorLogEndDate(e.target.value));
                                    }}
                                />
                            </TemplateDateInputWrapper>
                        </Grid>
                    </Grid>
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
                        onClick={() => dispatch(getErrorLogRequest())}
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
                            dispatch(setErrorLogStartDate(null));
                            dispatch(setErrorLogEndDate(defaultDate));
                            dispatch(getErrorLogRequest())
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Clear Fields</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center', marginTop: 20 }}>
                    <Button 
                        size="large" 
                        fullWidth
                        disabled={isLoading}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', border: '1px solid #2e2e42' }}
                        onClick={() => {
                            try {
                                dispatch(printErrorLogsExcelRequest())
                            } catch(e) {
                                console.log(':-)))');
                            }
                        }}
                    >
                        <ButtonText>Export All to Excel</ButtonText>
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
                           dispatch(deleteErrorLogsRequest());
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42'}}>Clear Error Log</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default ErrorLogTemplate;
