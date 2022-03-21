import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import { Link } from 'react-router-dom';
import DateFnsUtils from "@date-io/date-fns";
import {
    KeyboardDatePicker,
    MuiPickersUtilsProvider
} from '@material-ui/pickers';
import { MaterialUiPickersDate } from "@material-ui/pickers/typings/date";
import EditorConvertToHTML from '../../../../../components/UI/editor';
import { setFollowUpCreate, setFollowUpDate, setFollowUpNote } from '../../../../../actions/follow-up/report';
import { 
    getFollowUpSelector, getFollowUpDateSelector, getFollowUpNoteSelector, getNewVisitDateSelector, isCompletedSelector
} from '../../../../../selectors/follow-up/report';
import ScreendoxSelect from 'components/UI/select';
import { EMPTY_LIST_VALUE } from 'helpers/general';


export const FollowUpToolContainer = styled.div`
    margin-top: 20px;
    font-size: 16px;;
    border: 1px solid #f5f6f8;
`;

export const FollowUpToolContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 16px;;
  background-color: #ededf2;
`;

export const FollowUpToolHeaderTitle = styled.h1`
    font-size: 16px;;
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

const FollowUpToolComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const followUp = useSelector(getFollowUpSelector);
    const followUpDate = useSelector(getFollowUpDateSelector);
    const note = useSelector(getFollowUpNoteSelector);
    const visitDate = useSelector(getNewVisitDateSelector);
    const isCompleted = useSelector(isCompletedSelector);

    const resetFollowUpDate = () => {
        if(visitDate) {
            var date: any = new Date(visitDate);
            date.setDate(date.getDate() + 30);
            dispatch(setFollowUpDate(date));
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
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <FollowUpToolHeaderTitle>
                        Create Follow-Up?
                    </FollowUpToolHeaderTitle>
                </Grid>
                {followUp && <Grid item xs={6}>
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
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                dispatch(setFollowUpCreate(Boolean(parseInt(`${value}`))));
                            }}
                            disabled={!visitDate || isCompleted}
                            rootOptionDisabled
                        />
                    </Grid>
                    {followUp &&  <Grid 
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
                                        dispatch(setFollowUpDate(date));
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
                        <FollowUpToolHeaderTitle>
                            Notes
                        </FollowUpToolHeaderTitle>
                    </Grid>
                    <Grid item xs={12}>
                       <EditorConvertToHTML 
                            text={note}
                            onChangeHandler={(value: string) => {
                                dispatch(setFollowUpNote(value));
                            }}
                       />
                    </Grid>
                    {/* {followUp && isCompleted && <div style={{ marginTop: 10}}><Grid item xs={12} >
                        <FollowUpToolHeaderTitle>
                            Follow-up Reports
                        </FollowUpToolHeaderTitle>
                    </Grid>
                    <Grid item xs={12}>
                        <Link to={'#'} style={{ color: 'black', fontSize: 15, textDecoration: 'none' }}>Follow-Up Report {convertDate(followUpDate)}, Status: Created</Link>
                    </Grid></div>} */}
                </Grid>
            </MuiPickersUtilsProvider>
        </FollowUpToolContainerContent>
    </FollowUpToolContainer>
    )
}

export default FollowUpToolComponent;