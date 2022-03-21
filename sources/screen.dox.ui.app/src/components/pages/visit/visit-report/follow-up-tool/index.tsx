import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import {
    KeyboardDatePicker,
    MuiPickersUtilsProvider
} from '@material-ui/pickers';
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import { 
    getVisitFollowUpSelector, getVisitFollowUpDateSelector, getVisitFollowNoteSelector,getVisitDateSelector,isCompletedSelector, getFollowUpVisitArraySelector
} from '../../../../../selectors/visit/report';
import EditorConvertToHTML from '../../../../../components/UI/editor';
import { setVisitFollowUp, setVisitFollowUpDate, setVisitNotes } from '../../../../../actions/visit/report';
import { Link } from 'react-router-dom';
import ScreendoxSelect from 'components/UI/select';

export const FollowUpToolContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const FollowUpToolContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const FollowUpToolHeaderTitle = styled.h1`
    font-size: 14px;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const FollowUpToolContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

export const ResetFollowUpDefaultDate = styled.p`
    span {
        &:hover {
            text-decoration: underline;
            cursor: pointer;
        }
    }
`;

const LinkStyle = {
    color: 'black', 
    fontSize: 15, 
}

const FollowUpToolComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const followUp = useSelector(getVisitFollowUpSelector);
    const followUpDate = useSelector(getVisitFollowUpDateSelector);
    const note = useSelector(getVisitFollowNoteSelector);
    const visitDate = useSelector(getVisitDateSelector);
    const isCompleted = useSelector(isCompletedSelector);
    const followUpVisitArray = useSelector(getFollowUpVisitArraySelector);
    console.log(note)
    const resetFollowUpDate = () => {
        if(visitDate) {
            var date: any = new Date(visitDate);
            date.setDate(date.getDate() + 30);
            dispatch(setVisitFollowUpDate(date));
        }
    }

    const convertDate = (date: string | null) => {
        if(date) {
            var dateFormat = new Date(date);
            return new Intl.DateTimeFormat('en-US', {year: 'numeric', month: '2-digit',day: '2-digit'}).format(dateFormat)
        }
    }
    return (
    <FollowUpToolContainer>
        <FollowUpToolContainerHeader>
            <Grid container spacing={1} >
                <Grid item xs={6}>
                    <FollowUpToolHeaderTitle>
                        Create Follow-Up?
                    </FollowUpToolHeaderTitle>
                </Grid>
               {followUp &&  <Grid item xs={6}>
                    <FollowUpToolHeaderTitle>
                        Follow-Up Date
                    </FollowUpToolHeaderTitle>
                </Grid>}
            </Grid>
        </FollowUpToolContainerHeader>
        <FollowUpToolContainerContent>
            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <ScreendoxSelect 
                            options={[
                                { name: 'Yes', value: 1},
                                { name: 'No', value: 0 }
                            ]}
                            defaultValue={followUp ? 1 : 0}
                            rootOption={{ name: "No", value: 0 }}
                            changeHandler={(value: any) => {
                                dispatch(setVisitFollowUp( Boolean(parseInt(`${value}`))));
                            }}
                            rootOptionDisabled
                            disabled={!visitDate || isCompleted}
                        />
                    </Grid>
                    {followUp && <Grid 
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
                                placeholder="mm/dd/yyyy"
                                id="date-picker-from"
                                value={followUpDate}
                                onChange={(date: MaterialUiPickersDate | any, value: string | null = '') => {
                                    if (date !== 'Invalid Date') {
                                        dispatch(setVisitFollowUpDate(date));
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
                    </Grid>}
                    <Grid item xs={6}>
                      
                    </Grid>
                    {followUp && <Grid item xs={6}>
                        <ResetFollowUpDefaultDate onClick={resetFollowUpDate}> <span className="reSetDeafultDate">follow-up date to default 30 days</span></ResetFollowUpDefaultDate>
                    </Grid>}
                    <Grid item xs={12}>
                        <FollowUpToolHeaderTitle style={{ padding: '1.8em 0 0 0'}}>
                            Notes
                        </FollowUpToolHeaderTitle>
                    </Grid>
                    <Grid item xs={12}>
                       <EditorConvertToHTML 
                            text={note || ''}
                            onChangeHandler={(value: any) => {
                                dispatch(setVisitNotes(value));
                            }}
                       />
                    </Grid>
                    {isCompleted && followUp && <div style={{ marginTop: 10}}><Grid item xs={12} >
                        <FollowUpToolHeaderTitle>
                            Follow-up Reports
                        </FollowUpToolHeaderTitle>
                    </Grid>
                    <Grid item xs={12}>
                        <ul>
                            {followUpVisitArray && followUpVisitArray.map((item, i) => (
                                <li key={i}><Link to={'/follow-up-report/'+item.ID} style={LinkStyle}>Follow-Up Report {convertDate(item.FollowUpDate)}, Status: {item.Status}</Link></li>
                            ))}
                        </ul>
                    </Grid></div>}
                </Grid>
            </MuiPickersUtilsProvider>
        </FollowUpToolContainerContent>
    </FollowUpToolContainer>
    )
}

export default FollowUpToolComponent;