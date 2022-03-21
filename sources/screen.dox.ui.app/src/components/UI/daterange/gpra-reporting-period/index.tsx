import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Select, FormControl, MenuItem } from '@material-ui/core';
import { Accordion, AccordionSummary, AccordionDetails, Grid } from '@material-ui/core';
import SearchCheckbox from '../../checkbox/SearchCheckbox';
import { getScreenListGrpaPeriodList } from '../../../../selectors/screen';
import { getGPRAPeriodsAction } from '../../../../actions/screen';
import { TGPRAPeriodResponseItem } from '../../../../actions';
import ScreendoxSelect from 'components/UI/select';

export type TCustomDateRangeProps = {
    isExpanded: boolean;
    selectedDate: string | null;
    setExpanded: (v: boolean) => void;
    handleDateChange: (d: string) => void;
}

const GPRAReportingPeriod = (props: TCustomDateRangeProps): React.ReactElement => {

    const dispatch = useDispatch();
    const { selectedDate, isExpanded, handleDateChange, setExpanded } = props;
    const periods = useSelector(getScreenListGrpaPeriodList);
    const selected = periods.find(d => d.Label === selectedDate);

    React.useEffect(() => {
        if (!periods.length) {
            dispatch(getGPRAPeriodsAction())
        }
    }, [periods.length, periods, dispatch])

    return (
        <Accordion 
            expanded={isExpanded}
            onChange={e => {
                e.stopPropagation();
                setExpanded(!isExpanded)
            }}
            style={{ backgroundColor: isExpanded ? '#f5f6f8' : 'transparent', border: '1px solid rgb(46,46,66)' }}
        >
            <AccordionSummary
                aria-controls="grpa-date-ranger"
                id="grpa-date-ranger"
            >
                <Grid container justifyContent="space-between">
                    <Grid item xs={10} style={{ textAlign: 'left' }}>
                        GPRA Reporting Period
                    </Grid>
                    <Grid item xs={2}>
                        <SearchCheckbox 
                            name="grpa-date-ranger-name"
                            id="grpa-date-ranger-id"
                            isChecked={props.isExpanded}
                            changeHandler={e => {
                                e.stopPropagation();
                                e.preventDefault();
                                props.setExpanded(!props.isExpanded);
                            }}
                        />
                    </Grid>
                </Grid>
            </AccordionSummary>
            <AccordionDetails>
                <ScreendoxSelect 
                    options={periods.map((l: TGPRAPeriodResponseItem) => (
                        { name: `${l.Label}`.slice(0, 35), value: l.Label}
                    ))}
                    defaultValue={selected ? selected.Label : ''}
                    rootOption={{ name: '', value: 0 }}
                    changeHandler={(value: any) => {
                        handleDateChange(`${value}` || '');
                    }}
                    rootOptionDisabled
                />
            </AccordionDetails>
    </Accordion>
    );
}

export default GPRAReportingPeriod;