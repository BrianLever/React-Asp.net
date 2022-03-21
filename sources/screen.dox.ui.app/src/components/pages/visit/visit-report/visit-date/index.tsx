import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import {
    KeyboardDatePicker, MuiPickersUtilsProvider
} from '@material-ui/pickers';
import { 
    getVisitDischargedOptionsSelector, getVisitDateSelector, getVisitDischargedSelector,
} from '../../../../../selectors/visit/report';
import { 
    getVisitDischargedRequest, setVisitDischarged, setVisitDate, setVisitFollowUp, setVisitFollowUpDate
} from '../../../../../actions/visit/report';
import ScreendoxSelect from 'components/UI/select';
import { EMPTY_LIST_VALUE } from 'helpers/general';

export const VisitDateContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const VisitDateContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const VisitDateHeaderTitle = styled.h1`
    font-size: 14px;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const VisitDateContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const VisitDateComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const options = useSelector(getVisitDischargedOptionsSelector);
    const visitDate = useSelector(getVisitDateSelector);
    const discharged = useSelector(getVisitDischargedSelector);
    
    const resetFollowUpDate = (fromDate: string) => {
        if(fromDate) {
            var date: any = new Date(fromDate);
            date.setDate(date.getDate() + 30);
            dispatch(setVisitFollowUpDate(date));
        }
    }

    React.useEffect(() => {
        if (!options || !options.length) {
            dispatch(getVisitDischargedRequest());
        }
    }, [options, options.length, dispatch]);

    return (
    <VisitDateContainer>
        <VisitDateContainerHeader>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <VisitDateHeaderTitle>
                        New Visit Date
                    </VisitDateHeaderTitle>
                </Grid>
                <Grid item xs={6}>
                    <VisitDateHeaderTitle>
                        Discharged?
                    </VisitDateHeaderTitle>
                </Grid>
            </Grid>
        </VisitDateContainerHeader>
        <VisitDateContainerContent>
            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <Grid container spacing={1}>
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
                                value={visitDate}
                                placeholder="mm/dd/yyyy"
                                onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                                    if(!date) {
                                        dispatch(setVisitFollowUp(false))
                                        resetFollowUpDate(date);
                                    }
                                    if (date !== 'Invalid Date') {
                                        dispatch(setVisitDate(date));
                                        resetFollowUpDate(date);
                                    } else {
                                        dispatch(setVisitFollowUp(false));
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
                    <Grid item xs={6}>
                        <ScreendoxSelect 
                            options={
                                options.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={discharged}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                dispatch(setVisitDischarged(parseInt(value)));
                            }}
                            rootOptionDisabled
                        />
                    </Grid>
                </Grid>
                
            </MuiPickersUtilsProvider>
        </VisitDateContainerContent>
    </VisitDateContainer>
    )
}

export default VisitDateComponent;