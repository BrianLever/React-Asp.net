import React from 'react';
import { Button, Grid, RadioGroup, FormControlLabel, Radio, styled } from '@material-ui/core';
import SearchCheckbox from '../checkbox/SearchCheckbox';
import { EReportType } from '../../../actions/visit';
import classes from './radio.module.scss';
import IsNotMatchesFieldCheck from 'helpers/notMatchesFieldCheck';



const BpIcon = styled('span')(({ theme }) => ({
    borderRadius: '50%',
    width: 25,
    height: 25,
    backgroundColor: 'rgb(46, 46, 66)',
    backgroundImage: 'linear-gradient(180deg,hsla(0,0%,100%,.1),hsla(0,0%,100%,0))',
    '.Mui-focusVisible &': {
        outline: '2px auto rgba(19,124,189,.6)',
        outlineOffset: 2,
    },
    'input:hover ~ &': {
        // backgroundColor: theme.palette.mode === 'dark' ? '#30404d' : '#ebf1f5',
    },
    'input:disabled ~ &': {
        boxShadow: 'none',
        background:
        false ? 'rgba(57,75,89,.5)' : 'rgb(46, 46, 66)',
    },
    '&:before': {
        display: 'block',
        width: 25,
        height: 25,
        backgroundImage: 'radial-gradient(#fff,#fff 50%,transparent 32%)',
        content: '""',
    },
}));
  
const BpCheckedIcon = styled(BpIcon)({
    backgroundColor: 'rgb(46, 46, 66)',
    backgroundImage: 'linear-gradient(180deg,hsla(0,0%,100%,.1),hsla(0,0%,100%,0))',
    '&:before': {
      display: 'block',
      width: 25,
      height: 25,
      backgroundImage: 'radial-gradient(#fff,#fff 28%,transparent 32%)',
      content: '""',
    },
    'input:hover ~ &': {
      backgroundColor: 'rgb(46, 46, 66)',
    },
});
  
const BpRadio = (props: { isChecked: boolean }) => {
    return (
      <Radio
        disableRipple
        color="default"
        checkedIcon={<BpCheckedIcon />}
        icon={<BpIcon />}
        checked={props.isChecked}
      />
    );
}

export interface IScreendoxRadioComponentProps {
    selected: number;
    elementsRenderArray: Array<EReportType>;
    onSelectRadio: (k: number) => void; 
}

export interface IEhrExportPatientRecordsRadioComponentProps {
    selected: number | null;
    hrn: number;
    firstName?: string | null;
    lastName?: string | null;
    middleName?: string | null;
    birthday?: string | null;
    phone?: string | null;
    city?: string | null;
    zipCode?: string | null;
    stateName?: string | null;
    streetAddress?: string | null;
    NotMatchFields?: Array<string>;
    onSelectRadio: () => void; 
}

export const EhrExportPatientRecordsRadioComponent = (props: IEhrExportPatientRecordsRadioComponentProps): React.ReactElement => {
    
    return (
        <Grid container spacing={1}>
            {
                <Grid item xs={12} style={{ marginBottom: 10, background: props.selected === props.hrn?'rgb(237,237,242)':'transparent' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        endIcon={
                            <FormControlLabel  control={<BpRadio isChecked={props.selected === props.hrn } />} label="" />
                        }
                        onClick={event => {
                            event.stopPropagation();
                            props.onSelectRadio && props.onSelectRadio();
                        }}
                    >
                        <div style={{ width: '100%' }}>
                            <h1 className={classes.ehrExportRecordHrnText}>
                                HRN: {props.hrn}
                            </h1>
                            <h1 className={classes.ehrExportRecordNameText}>
                                <p className={classes.ehrExportRecordNameText} style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'FirstName' })?'#ef7753':'unset'}}>{props.firstName}</p>&nbsp;  
                                <p className={classes.ehrExportRecordNameText} style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'MiddleName' })?'#ef7753':'unset'}}>{props.middleName}</p> &nbsp; 
                                <p className={classes.ehrExportRecordNameText} style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'LastName' })?'#ef7753':'unset'}}>{props.lastName}</p>  
                            </h1>
                            <h1 className={classes.ehrExportRecordText} style={{ color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'Birthday' })?'#ef7753':'unset' }}>
                                {props.birthday}
                            </h1>
                            <h1 className={classes.ehrExportRecordText} style={{ color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'Phone' })?'#ef7753':'unset' }}>
                                {props.phone}
                            </h1>
                            <h1 className={classes.ehrExportRecordText} style={{ color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'StreetAddress' })?'#ef7753':'unset' }}>
                                {props.streetAddress}
                            </h1>
                            <h1 className={classes.ehrExportRecordText}>
                             <p  style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'City' })?'#ef7753':'unset'}}>{props.city}</p>,
                             <p  style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'StateName' })?'#ef7753':'unset'}}>{props.stateName}</p> ,
                             <p  style={{ display: 'inline', color: IsNotMatchesFieldCheck({ notMatchFields: props.NotMatchFields, field: 'ZipCode' })?'#ef7753':'unset'}}>{props.zipCode}</p> 
                            </h1>
                        </div>
                    </Button>
                </Grid>
            }
        </Grid>
    ) 
}


export interface IEhrExportVisitRecordsRadioComponentProps {
    selected: number | null;
    name: string | null;
    date?: string | null;
    id?: number | null;
    onSelectRadio: () => void; 
}


export const EhrExportVisitRecordsRadioComponent = (props: IEhrExportVisitRecordsRadioComponentProps): React.ReactElement => {
    
    return (
        <Grid container spacing={1}>
            {
                <Grid item xs={12} style={{ marginBottom: 10, background:props.id === props.selected?'rgb(237,237,242)':'transparent' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="default" 
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                        endIcon={
                            <FormControlLabel  control={<BpRadio isChecked={ props.id === props.selected } />} label="" />
                        }
                        onClick={event => {
                            event.stopPropagation();
                            props.onSelectRadio && props.onSelectRadio();
                        }}
                    >
                        <div style={{ width: '100%' }}>
                            <h1 className={classes.ehrExportRecordHrnText}>
                                {props.date}
                            </h1>
                            <h1 className={classes.ehrExportRecordHrnText}>
                                {props.name}
                            </h1>
                        </div>
                    </Button>
                </Grid>
            }
        </Grid>
    ) 
}


const ScreendoxRadioComponent = (props: IScreendoxRadioComponentProps): React.ReactElement => {
    const { elementsRenderArray, selected, onSelectRadio } = props;
    return (
        <Grid container spacing={1}>
            {
                elementsRenderArray.map((e: EReportType) => (
                    <Grid item xs={12} key={e.key}>
                        <Button 
                            size="large" 
                            fullWidth
                            className={classes.removeBoxShadow}
                            variant="contained" 
                            color="default" 
                            style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', paddingRight: 12 }}
                            endIcon={
                                <SearchCheckbox  
                                    name="custom-date-ranger-checkbox-name"
                                    id="custom-date-ranger-checkbox-id"
                                    isChecked={e.value === selected}
                                    changeHandler={event => {
                                        event.stopPropagation();
                                        event.preventDefault();
                                        onSelectRadio(e.value)
                                    }}
                                />
                            }
                            onClick={event => {
                                event.stopPropagation();
                                onSelectRadio(e.value)
                            }}
                        >
                            <p style={{ color: '#2e2e42', width: '100%', textTransform: 'none', textAlign: 'left' }}>
                                { e.name }
                            </p>
                        </Button>
                    </Grid>
                ))
            }
        </Grid>
    ) 
}

export default ScreendoxRadioComponent;