import React from 'react';
import styled from 'styled-components';
import { Grid, FormControl, Select, TextField } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';
import CircularProgress from '@material-ui/core/CircularProgress';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getSelectedVisitDemographicCountyOfResidenceValueSelector, getSelectedVisitDemographicLivingOnReservationSelector, 
    getVisitDemographicLivingOnReservationSelector,getVisitDemographicCountyOfResidenceArraySelector,getVisitDemographicCountyOfResidenceArrayLoadingSelector
} from '../../../../../selectors/visit/demographic-report';
import { 
    getVisitDemographicLivingOnReservationRequest, setSelectedVisitDemographicCountyOfResidence, 
    setSelectedVisitDemographicLivingOnReservation, getVisitDemographicCountyOfResidenceArrayRequest 
} from '../../../../../actions/visit/demographic-report';
import {EMPTY_LIST_VALUE} from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const LivingReservationContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid rgb(237,237,242);
    margine-top: 20px;
    border-radius: 5px;
`;

const LivingReservationContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

const LivingReservationContainerHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

const LivingReservationContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

function sleep(delay = 0) {
    return new Promise((resolve) => {
      setTimeout(resolve, delay);
    });
}


const LivingReservationComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const livingReservationOptions = useSelector(getVisitDemographicLivingOnReservationSelector);
    const selectedLiving = useSelector(getSelectedVisitDemographicLivingOnReservationSelector);
    const countyOfResidence = useSelector(getSelectedVisitDemographicCountyOfResidenceValueSelector);
    const [open, setOpen] = React.useState(false);
    const countyOfResidenceArrary = useSelector(getVisitDemographicCountyOfResidenceArraySelector);
    const isCountyOfResidencArrayRequestLoading = useSelector(getVisitDemographicCountyOfResidenceArrayLoadingSelector)
    
    const loading = open;
    
    React.useEffect(() => {
        let active = true;

        if (!loading) {
        return undefined;
        }
    
        (async () => {
            dispatch(getVisitDemographicCountyOfResidenceArrayRequest(countyOfResidence));
            await sleep(1e3); // For demo purposes.
        })();
    
        return () => {
            active = false;
        };
    }, [loading, countyOfResidence]);

    React.useEffect(() => {
        if (!Array.isArray(livingReservationOptions) || !livingReservationOptions.length) {
            dispatch(getVisitDemographicLivingOnReservationRequest())
        }
    }, [dispatch, selectedLiving, livingReservationOptions, livingReservationOptions.length])

    

    return (
        <LivingReservationContainer>
            <LivingReservationContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <LivingReservationContainerHeaderTitle>
                            Living "On" or "Off" Reservation
                        </LivingReservationContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <LivingReservationContainerHeaderTitle>
                            County of Residence
                        </LivingReservationContainerHeaderTitle>
                    </Grid>
                </Grid>
            </LivingReservationContainerHeader>
            <LivingReservationContent>
                <Grid container spacing={1}>
                    <Grid 
                        item 
                        xs={6} 
                        style={{
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                        }}
                    >
                         <ScreendoxSelect 
                            options={livingReservationOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedLiving ? selectedLiving.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const living = livingReservationOptions.find(d => `${d.Id}` ===  value);
                                if (living) {
                                    dispatch(setSelectedVisitDemographicLivingOnReservation(living));
                                }
                            }}
                        />
                    </Grid>
                    <Grid 
                        item 
                        xs={6} 
                        style={{
                            alignItems: 'center',
                            justifyContent: 'center',
                        }}
                    >
                        
                        <Autocomplete
                            id="asynchronous"
                            open={open}
                            onOpen={() => {
                                setOpen(true);
                            }}
                            onClose={() => {
                                setOpen(false);
                            }}
                            size="small"
                            options={countyOfResidenceArrary}
                            inputValue={countyOfResidence?countyOfResidence:''}
                            loading={isCountyOfResidencArrayRequestLoading}
                            onInputChange={(e: any) => {
                                if(!!e) {
                                    dispatch(setSelectedVisitDemographicCountyOfResidence(e.target['innerText']));
                                }
                            }}
                            renderInput={(params) => {
                                return(
                                <TextField
                                {...params}
                                variant="outlined"
                                value={countyOfResidence}
                                onChange={(e: any) => {
                                    e.stopPropagation();
                                    dispatch(setSelectedVisitDemographicCountyOfResidence(`${e.target.value}`));
                                }}
                                InputProps={{
                                    ...params.InputProps,
                                    endAdornment: (
                                    <React.Fragment>
                                        {isCountyOfResidencArrayRequestLoading ? <CircularProgress color="inherit" size={20} /> : null}
                                        {params.InputProps.endAdornment}
                                    </React.Fragment>
                                    ),
                                }}
                                />
                            )}}
                        />

                    </Grid>
                </Grid>
            </LivingReservationContent>
        </LivingReservationContainer>
    );
}

export default LivingReservationComponent;