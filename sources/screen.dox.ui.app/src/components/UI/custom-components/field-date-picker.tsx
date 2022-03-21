import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import { KeyboardDatePicker, MuiPickersUtilsProvider } from '@material-ui/pickers';
import { IActionPayload, TChoiceItem } from '../../../actions';
import { IRootState } from '../../../states';
import ScreendoxSelect from '../select';
import { EMPTY_LIST_VALUE } from 'helpers/general';

export const ScreendoxFieldDatePickerContainer = styled.div`
    margin-top: 10px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const ScreendoxFieldDatePickerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: #ededf2;
  border-radius: 5px 5px 0 0;
`;

export const ScreendoxFieldDatePickerHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const ScreendoxFieldDatePickerContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

export enum ETScreendoxFieldDatePicker {
    selectorDate, dateSelector
}

export type TScreendoxFieldDatePickerProps = {
    type: ETScreendoxFieldDatePicker
    // Text to reneder for Titles
    selectorTitle: string;
    datePickerTitle: string;
    // Actions to proccess a new data 
    onOptionChangeAction: (i: number) => IActionPayload;
    onDatePickerChangeAction: (i: string) => IActionPayload;
    // Selector from state to retrieve all options
    optionsSelector: (state: IRootState) => Array<TChoiceItem>;
    // Selectros to fetched currently selected data
    selectedOptionsValueSelector: (state: IRootState) => number;
    selectedDatePickerValueSelector: (state: IRootState) => string | null;
    
}

const ScreendoxFieldDatePicker = (props: TScreendoxFieldDatePickerProps): React.ReactElement => {
    const dispatch = useDispatch();
    const { 
        type, selectorTitle, datePickerTitle, 
        optionsSelector, selectedDatePickerValueSelector, selectedOptionsValueSelector,
        onOptionChangeAction, onDatePickerChangeAction
    } = props;
    const options = useSelector(optionsSelector);
    const datePicked = useSelector(selectedDatePickerValueSelector);
    const selectedOption = useSelector(selectedOptionsValueSelector);

    const datePickerComponent: React.ReactElement = (
        <Grid 
            item 
            xs={6} 
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                margin: '0px',
            }}
        >
            <KeyboardDatePicker
                format="MM/dd/yyyy"
                margin="normal"
                id="date-picker-from"
                value={datePicked}
                placeholder="mm/dd/yyyy"
                onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                    if (date !== 'Invalid Date') {
                        dispatch(onDatePickerChangeAction(date));
                    }
                }}
                style={{ 
                    border: '1px solid #2e2e42', 
                    borderRadius: '4px', 
                    padding: '5px 5px 0 5px',
                    width: '100%', 
                    margin: '0px',
                }}
            />
        </Grid>
    )

    const selectorComponent: React.ReactElement = (
        <Grid item xs={6}>
            <ScreendoxSelect
                options={options.map(l =>
                   ( { name: l.Name, value: l.Id } )
                )}
                defaultValue={selectedOption}
                rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                changeHandler={(value: any) => {
                    const v  = parseInt(`${value}`);
                    dispatch(onOptionChangeAction(v));
                }}
                rootOptionDisabled
            />
        </Grid>
    )
    
    return (    
    <ScreendoxFieldDatePickerContainer>
        <ScreendoxFieldDatePickerHeader>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <ScreendoxFieldDatePickerHeaderTitle>
                        {
                           (type === ETScreendoxFieldDatePicker.selectorDate) ? selectorTitle : datePickerTitle
                        }
                    </ScreendoxFieldDatePickerHeaderTitle>
                </Grid>
                <Grid item xs={6}>
                    <ScreendoxFieldDatePickerHeaderTitle>
                        {
                           (type === ETScreendoxFieldDatePicker.selectorDate) ?  datePickerTitle : selectorTitle
                        }
                    </ScreendoxFieldDatePickerHeaderTitle>
                </Grid>
            </Grid>
        </ScreendoxFieldDatePickerHeader>
        <ScreendoxFieldDatePickerContainerContent>
            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <Grid container spacing={1}>
                    {
                        (type === ETScreendoxFieldDatePicker.selectorDate) ? (
                            <>
                                {selectorComponent}
                                {datePickerComponent}
                            </>
                        ) : ( <>
                            {datePickerComponent}
                            {selectorComponent}
                        </>)
                    }
                </Grid>
            </MuiPickersUtilsProvider>
        </ScreendoxFieldDatePickerContainerContent>
    </ScreendoxFieldDatePickerContainer>)
}

export default ScreendoxFieldDatePicker;