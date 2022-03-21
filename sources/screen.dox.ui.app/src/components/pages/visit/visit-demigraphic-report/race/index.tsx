import React from 'react';
import styled from 'styled-components';
import { Grid, FormControl, Select, TextField } from '@material-ui/core';
import Autocomplete from '@material-ui/lab/Autocomplete';
import CircularProgress from '@material-ui/core/CircularProgress';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getSelectedVisitDemographicRaceSelector, 
    getSelectedVisitDemographicTribalAffiliationSelector, 
    getVisitDemographicRaceOptionsSelector,
    getVisitDemographicTribalAffiliationArraySelector,
    getVisitDemographicTribalAffiliationArrayLoadingSelector
} from '../../../../../selectors/visit/demographic-report';
import { getVisitDemographicRaceRequest, 
        setSelectedVisitDemographicRace, 
        getVisitDemographicTribalAffiliationRequest,
        setSelectedVisitDemographicTribalAffiliation
} from 'actions/visit/demographic-report';
import {EMPTY_LIST_VALUE} from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

export const RaceComponentContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const RaceComponentContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const RaceComponentContainerHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
    
`;

export const RaceComponentContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
    border-radius: 0 0 5px 5px;
`;

function sleep(delay = 0) {
    return new Promise((resolve) => {
      setTimeout(resolve, delay);
    });
}

const RaceComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const selectedRace = useSelector(getSelectedVisitDemographicRaceSelector);
    const raceOptions = useSelector(getVisitDemographicRaceOptionsSelector);
    const tribalAffiliation = useSelector(getSelectedVisitDemographicTribalAffiliationSelector);
    const tribalAffiliationArrary: Array<{id: number, value: string}> = useSelector(getVisitDemographicTribalAffiliationArraySelector);
    const isTribalAffiliationRequestLoading = useSelector(getVisitDemographicTribalAffiliationArrayLoadingSelector)
    const [open, setOpen] = React.useState(false);
    const [options, setOptions] = React.useState([]);
    const loading = open && options.length === 0;

    React.useEffect(() => {
        let active = true;

        if (!loading) {
        return undefined;
        }
    
        (async () => {
            dispatch(getVisitDemographicTribalAffiliationRequest(tribalAffiliation));
            await sleep(1e3); // For demo purposes.
        })();
    
        return () => {
            active = false;
        };
    }, [loading, tribalAffiliation]);
      


    React.useEffect(() => {
        if (!Array.isArray(raceOptions) || !raceOptions.length) {
            dispatch(getVisitDemographicRaceRequest());
        }
    }, [selectedRace, raceOptions, raceOptions.length, dispatch])

    return (
        <RaceComponentContainer>
            <RaceComponentContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <RaceComponentContainerHeaderTitle>
                            Race
                        </RaceComponentContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <RaceComponentContainerHeaderTitle>
                            Tribal Affiliation
                        </RaceComponentContainerHeaderTitle>
                    </Grid>
                </Grid>
            </RaceComponentContainerHeader>
            <RaceComponentContainerContent>
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
                            options={raceOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedRace ? selectedRace.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const race = raceOptions.find(d => `${d.Id}` === value);
                                if (race) {
                                    dispatch(setSelectedVisitDemographicRace(race));
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
                            getOptionLabel={(option) => option.value}
                            options={tribalAffiliationArrary}
                            inputValue={tribalAffiliation?tribalAffiliation:''}
                            loading={isTribalAffiliationRequestLoading}
                            onInputChange={(e: any) => {
                                if(!!e) {
                                    dispatch(setSelectedVisitDemographicTribalAffiliation(e.target['innerText']));
                                }
                            }}
                            renderInput={(params) => {
                                return(
                                <TextField
                                {...params}
                                variant="outlined"
                                value={tribalAffiliation?tribalAffiliation:''}
                                onChange={(e: any) => {
                                    e.stopPropagation();
                                    dispatch(setSelectedVisitDemographicTribalAffiliation(`${e.target.value}`));
                                }}
                                InputProps={{
                                    ...params.InputProps,
                                    endAdornment: (
                                    <React.Fragment>
                                        {isTribalAffiliationRequestLoading ? <CircularProgress color="inherit" size={20} /> : null}
                                        {params.InputProps.endAdornment}
                                    </React.Fragment>
                                    ),
                                }}
                                />
                            )}}
                        />
                    </Grid>
                </Grid>
            </RaceComponentContainerContent>
        </RaceComponentContainer>
    );
}

export default RaceComponent;