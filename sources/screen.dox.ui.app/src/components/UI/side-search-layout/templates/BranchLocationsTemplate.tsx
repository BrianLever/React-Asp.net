import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, FormControl, Grid, Select, TextField } from '@material-ui/core';
import { TitleText, TitleTextH2, ContainerBlock } from './styledComponents';
import classes from  '../templates.module.scss';
import { 
    getBranchLocationsRequest,
    resetBranchLocationsFilterRequest,
    setFilterBranchLocationId, setLocationNameKey, setScreeningProfileId, setShowDisabled }
     from '../../../../actions/branch-locations';
import { getListBranchLocationsSelector, getScreeningProfileListSelector } from '../../../../selectors/shared';
import { 
    getListBranchLocationsRequest, getScreeningProfileListRequest, TBranchLocationsItemResponse, TScreeningProfileItemResponse,
} from '../../../../actions/shared';
import { 
    getSelectedFilterBranchLocationIdSelector, getSelectedFilterBranchLocationNameKeySelector, getSelectedFilterScreeningProfileIdSelector, 
    getSelectedFilterShowDisabledSelector 
} from '../../../../selectors/branch-locations';
import { ButtonText } from '../styledComponents';
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

const BranchLocationsTemplate = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const locationsList:  Array<TBranchLocationsItemResponse> = useSelector(getListBranchLocationsSelector);
    const locationId: number = useSelector(getSelectedFilterBranchLocationIdSelector);
    const locationNameKey: string = useSelector(getSelectedFilterBranchLocationNameKeySelector);
    const selectedVisibility: number = useSelector(getSelectedFilterShowDisabledSelector);
    const screeningProfileList: Array<TScreeningProfileItemResponse> = useSelector(getScreeningProfileListSelector);
    const screeningProfileId: number = useSelector(getSelectedFilterScreeningProfileIdSelector);

    React.useEffect(() => {
        if (!locationsList.length) {
            dispatch(getListBranchLocationsRequest());
        }
        if (!screeningProfileList.length) {
            dispatch(getScreeningProfileListRequest());
        }
    }, [dispatch, locationsList, locationsList.length, screeningProfileList, screeningProfileList.length])


    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Search Branch Location
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Location Name
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <TextField
                        fullWidth
                        margin="dense"
                        id="search-branch-location"
                        variant="outlined"
                        style={{ margin: 0 }}
                        value={locationNameKey}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setLocationNameKey(e.target.value));
                        }}

                        onKeyDown={(e) => {
                            if(e.keyCode == 13){
                                dispatch(getBranchLocationsRequest());
                             }
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Screen Profile
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12}>
                    <ScreendoxSelect 
                        options={screeningProfileList.map((location: TScreeningProfileItemResponse) => (
                            { name: `${location.Name}`.slice(0, 20), value: location.ID}
                        ))}
                        defaultValue={screeningProfileId}
                        rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                        changeHandler={(value: any) => {
                            dispatch(setScreeningProfileId(parseInt(value)));
                        }}
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left' }}>
                    <TitleTextH2>
                        Branch Location
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12}>
                <ScreendoxSelect 
                    options={locationsList.map((location) => (
                        { name: `${location.Name}`.slice(0, 20), value: location.ID}
                    ))}
                    defaultValue={locationId}
                    rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                    changeHandler={(value: any) => {
                        dispatch(setFilterBranchLocationId(parseInt(value)));
                    }}
                />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left'}}>
                    <TitleTextH2>
                        Visibility
                    </TitleTextH2>
                </Grid>
                <Grid item xs={12}>
                    <ScreendoxSelect 
                        options={[{name: 'Show Active', value: 0}, { name: 'Show All', value: 1 }]}
                        defaultValue={selectedVisibility}
                        rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                        changeHandler={(value: any) => {
                            dispatch(setShowDisabled(parseInt(value)));
                        }}
                        rootOptionDisabled
                    />
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth 
                        disabled={false}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', textTransform: 'none', marginTop: 30}}
                        onClick={() => {
                            dispatch(getBranchLocationsRequest());
                        }}
                    >
                        <ButtonText>Search</ButtonText>
                    </Button>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        fullWidth
                        disabled={false}
                        variant="contained" 
                        color="default"
                        className={classes.removeBoxShadow}
                        style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', textTransform: 'none', }}
                        onClick={() => {
                            dispatch(resetBranchLocationsFilterRequest());
                        }}
                    >
                        <ButtonText style={{ color: '#2e2e42' }}>Cancel</ButtonText>
                    </Button>
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default BranchLocationsTemplate;