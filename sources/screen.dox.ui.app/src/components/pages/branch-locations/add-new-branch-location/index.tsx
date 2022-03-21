import React from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { FormControl, Grid, Select, TextField } from '@material-ui/core';
import { TitleTextModal } from '../../styledComponents';
import { getScreeningProfileListSelector } from '../../../../selectors/shared';
import { getScreeningProfileListRequest, TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../../../actions/shared';
import { createBranchLocationName, createBranchLocationScreenProfile, createBranchLocationDescription } from '../../../../actions/branch-locations';
import { getCreateBranchLocationsNameSelector, getCreateBranchLocationsDescriptionSelector, getCreateBranchLocationsScreenProfileSelector, IsNewBranchLocationLoadingSelector, getBranchLocationArraySelector, setBranchLocationDisabledSelector } from '../../../../selectors/branch-locations';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';


export const AddNewDeviceContentTextarea = styled.textarea`
    font-size: 1.2em !important;
    border: 1px solid #2e2e42 !important;
    width: 100%;
    min-height: 120px;
    background: transparent;
    padding: 10px;
`;

const AddNewBranchLocation = (): React.ReactElement => {

    const dispatch = useDispatch();
    const createName: string = useSelector(getCreateBranchLocationsNameSelector);
    const createDescription: string = useSelector(getCreateBranchLocationsDescriptionSelector);
    const createScreenProfile: number = useSelector(getCreateBranchLocationsScreenProfileSelector);
    const screeningProfileList: Array<TScreeningProfileItemResponse> = useSelector(getScreeningProfileListSelector);
    const isNewbranchlocationLoading: boolean = useSelector(IsNewBranchLocationLoadingSelector);
    const branchLocationDisabled: boolean = useSelector(setBranchLocationDisabledSelector);
    React.useEffect(() => {
        if (!screeningProfileList.length) {
            dispatch(getScreeningProfileListRequest());
        }
    }, [dispatch, screeningProfileList, screeningProfileList.length]);


    return (
        <Grid container spacing={1} >   
            {branchLocationDisabled && <Grid item xs={12}>
                Branch Location is disabled
            </Grid>}
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Location Name*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <TextField
                    fullWidth
                    margin="dense"
                    label=""
                    error={!createName && isNewbranchlocationLoading}
                    id="deviceName"
                    variant="outlined"
                    value={createName}
                    onChange={e => {
                        e.stopPropagation();
                        dispatch(createBranchLocationName(e.target.value));
                    }}
                />
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Screen Profile*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12}>
                <ScreendoxSelect 
                    options={screeningProfileList.map((location: TScreeningProfileItemResponse) => (
                        { name: `${location.Name}`.slice(0, 20), value: location.ID}
                    ))}
                    defaultValue={createScreenProfile}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                    changeHandler={(value: any) => {
                        dispatch(createBranchLocationScreenProfile(parseInt(value)));
                    }}
                />
                {/* <FormControl fullWidth variant="outlined">
                    <Select
                        native
                        margin="dense"
                        error={!createScreenProfile && isNewbranchlocationLoading}
                        id="screen-profile"
                        value={createScreenProfile}
                        onChange={(event: React.ChangeEvent<{ name?: string; value: unknown }>) => {
                            if (event.target.value) {
                                try {
                                    const value = parseInt(`${event.target.value}`);
                                    dispatch(createBranchLocationScreenProfile(value));
                                } catch(e) {}
                            } else {
                                dispatch(createBranchLocationScreenProfile(0));
                            }
                        }}
                    >
                        <option key="screen-profile-default-location" value={0} >{EMPTY_LIST_VALUE}</option>
                        {
                            screeningProfileList.map((l: TScreeningProfileItemResponse) => (
                                <option key={l.ID} value={l.ID} >
                                    {`${l.Name}`.slice(0, 20)}
                                </option>
                            ))
                        }
                    </Select>
                </FormControl> */}
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Description
                </TitleTextModal>
            </Grid>
            <AddNewDeviceContentTextarea
                value={createDescription}
                onChange={e => {
                    dispatch(createBranchLocationDescription(`${e.target.value}`));
                }}
            >
                { createDescription }
            </AddNewDeviceContentTextarea>
        </Grid>
    )
}

export default AddNewBranchLocation;