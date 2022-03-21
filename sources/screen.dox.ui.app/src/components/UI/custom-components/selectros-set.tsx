import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import DateFnsUtils from "@date-io/date-fns";
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import { IActionPayload, TChoiceItem } from '../../../actions';
import { IRootState } from '../../../states';
import ScreendoxSelect from '../select';
import { EMPTY_LIST_VALUE } from 'helpers/general';

export const ScreendoxSelectorsSetContainer = styled.div`
    margin-top: 10px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const ScreendoxSelectorsSetHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: #ededf2;
  border-radius: 5px 5px 0 0;
`;

export const ScreendoxSelectorsSetHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const ScreendoxSelectorsSetContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

export type TScreendoxSelectorsSetProps = {
    // Text to reneder for Titles
    aSelectorTitle: string;
    bSelectorTitle: string;
    // Actions to proccess a new data 
    onAOptionChangeAction: (i: number) => IActionPayload;
    onBOptionChangeAction: (i: number) => IActionPayload;
    // Selector from state to retrieve all options
    optionsASelector: (state: IRootState) => Array<TChoiceItem>;
    optionsBSelector: (state: IRootState) => Array<TChoiceItem>;
    // Selectros to fetched currently selected data
    selectedAOptionsValueSelector: (state: IRootState) => number;
    selectedBOptionsValueSelector: (state: IRootState) => number;
    
}

const ScreendoxSelectorsSet = (props: TScreendoxSelectorsSetProps): React.ReactElement => {
    const dispatch = useDispatch();
    const { 
        aSelectorTitle, bSelectorTitle,
        optionsASelector, optionsBSelector, selectedAOptionsValueSelector, 
        selectedBOptionsValueSelector, onAOptionChangeAction, onBOptionChangeAction 
    } = props;
    const optionsA = useSelector(optionsASelector);
    const optionsB = useSelector(optionsBSelector);

    const selectedAOption = useSelector(selectedAOptionsValueSelector);
    const selectedBOption = useSelector(selectedBOptionsValueSelector);

    const changeSelectorHandler = (e: any, action: (i: number) => IActionPayload): void => {
        try {
            e.stopPropagation()
            const v  = parseInt(`${e.target.value}`);
            dispatch(action(v));
        } catch (e) {}
    }
    
    return (    
    <ScreendoxSelectorsSetContainer>
        <ScreendoxSelectorsSetHeader>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <ScreendoxSelectorsSetHeaderTitle>
                        { aSelectorTitle }
                    </ScreendoxSelectorsSetHeaderTitle>
                </Grid>
                <Grid item xs={6}>
                    <ScreendoxSelectorsSetHeaderTitle>
                        { bSelectorTitle }
                    </ScreendoxSelectorsSetHeaderTitle>
                </Grid>
            </Grid>
        </ScreendoxSelectorsSetHeader>
        <ScreendoxSelectorsSetContainerContent>
            <MuiPickersUtilsProvider utils={DateFnsUtils}>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <ScreendoxSelect
                            options={optionsA.map(l =>
                            ( { name: l.Name, value: l.Id } )
                            )}
                            defaultValue={selectedAOption}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any, e) => {
                                changeSelectorHandler(e, onAOptionChangeAction)
                            }}
                            rootOptionDisabled
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <ScreendoxSelect
                            options={optionsB.map(l =>
                            ( { name: l.Name, value: l.Id } )
                            )}
                            defaultValue={selectedBOption}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any, e) => {
                                changeSelectorHandler(e, onBOptionChangeAction)
                            }}
                            rootOptionDisabled
                        />
                    </Grid>
                </Grid>
            </MuiPickersUtilsProvider>
        </ScreendoxSelectorsSetContainerContent>
    </ScreendoxSelectorsSetContainer>)
}

export default ScreendoxSelectorsSet;