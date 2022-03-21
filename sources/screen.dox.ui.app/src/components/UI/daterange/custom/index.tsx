import React from 'react';
import { Accordion, AccordionSummary, AccordionDetails, Grid } from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import SearchCheckbox from '../../checkbox/SearchCheckbox';
import { createTheme, ThemeProvider } from "@material-ui/core/styles";


const materialTheme = createTheme({
    overrides: {
    //   MuiPickersToolbar: {
    //     toolbar: {
    //       backgroundColor: lightBlue.A200,
    //     },
    //   },
    //   MuiPickersCalendarHeader: {
    //     switchHeader: {
    //       // backgroundColor: lightBlue.A200,
    //       // color: "white",
    //     },
    //   },
    //   MuiPickersDay: {
    //     day: {
    //       color: lightBlue.A700,
    //     },
    //     daySelected: {
    //       backgroundColor: lightBlue["400"],
    //     },
    //     dayDisabled: {
    //       color: lightBlue["100"],
    //     },
    //     current: {
    //       color: lightBlue["900"],
    //     },
    //   },
     
    },
  });


export type TCustomDateRangeProps = {
    isExpanded: boolean;
    selectedFromDate: string | null | undefined;
    selectedTillDate: string | null;
    setExpanded: (v: boolean) => void;
    handleFromDateChange: (d: Date | null) => void;
    handleTillDateChange: (d: Date | null) => void;
}

const CustomDateRange = (props: TCustomDateRangeProps): React.ReactElement => {

    const { 
        selectedFromDate, selectedTillDate, isExpanded, 
        handleFromDateChange,  handleTillDateChange, setExpanded 
    } = props;

    return (
        <Accordion 
            expanded={isExpanded}
            onChange={(e: any) => {
                e.stopPropagation();
                setExpanded(!isExpanded);
            } }
            style={{ backgroundColor: props.isExpanded ? '#f5f6f8' : 'transparent', border: '1px solid rgb(46,46,66)' }}
        >
            <AccordionSummary
                aria-controls="custom-date-ranger"
                id="custom-date-ranger"
            >
                <Grid container justifyContent="space-between">
                    <Grid item xs={10} style={{ textAlign: 'left' }}>
                        Custom
                    </Grid>
                    <Grid item xs={2}>
                        <SearchCheckbox  
                            name={`custom-date-ranger-checkbox-name-${selectedFromDate}`}
                            id={`custom-date-ranger-checkbox-name-${selectedFromDate}`}
                            isChecked={isExpanded}
                            changeHandler={e => {
                                e.stopPropagation();
                                e.preventDefault();
                                setExpanded(!isExpanded);
                            }}
                        />
                    </Grid>
                </Grid>
            </AccordionSummary>
            <AccordionDetails>
                <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <ThemeProvider theme={materialTheme}>
                    <Grid container>
                        <Grid item xs={12}>
                            <KeyboardDatePicker
                                format="MM/dd/yyyy"
                                margin="normal"
                                id="date-picker-from"
                                value={selectedFromDate}
                                onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                                    if (`${date}` !== 'Invalid Date') {
                                        handleFromDateChange(date);
                                    }
                                }}
                                style={{ border: '1px solid #2e2e42', borderRadius: '4px', padding: '5px 5px 0 5px' }}
                            />
                        </Grid>
                        <Grid item xs={12}>
                            TO
                        </Grid>
                        <Grid item xs={12}>
                            <KeyboardDatePicker
                                format="MM/dd/yyyy"
                                margin="normal"
                                id="date-picker-till"
                                value={selectedTillDate}
                                onChange={(date: MaterialUiPickersDate | any) => { 
                                    if (`${date}` !== 'Invalid Date') {
                                        handleTillDateChange(date)
                                    }
                                }}
                                style={{ border: '1px solid #2e2e42', borderRadius: '4px', padding: '5px 5px 0 5px' }}
                            />
                        </Grid>
                    </Grid>
                </ThemeProvider>
                </MuiPickersUtilsProvider>
            </AccordionDetails>
        </Accordion>
    );
}

export default CustomDateRange;