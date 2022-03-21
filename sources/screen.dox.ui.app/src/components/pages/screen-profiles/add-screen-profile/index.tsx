import React from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { FormControl, Grid, Select, TextField } from '@material-ui/core';
import { TitleTextModal, ScreendoxTextInput, ScreendoxTextArea  } from '../../styledComponents';
import { getScreeningProfileListSelector } from '../../../../selectors/shared';
import { getScreeningProfileListRequest, TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../../../actions/shared';
import { isScreenProfilesListRequestLoadingSelector, screenProfileCreateScreenProfileDescriptionSelector, isNewScreenProfileLoadingSelector, screenProfileCreateScreenProfileNameSelector } from 'selectors/screen-profiles';
import { setCreateScreenProfileDescription, setCreateScreenProfileName } from 'actions/screen-profiles';


const AddScreenProfile = (): React.ReactElement => {

    const dispatch = useDispatch();
    const createName: string = useSelector(screenProfileCreateScreenProfileNameSelector);
    const createDescription: string = useSelector(screenProfileCreateScreenProfileDescriptionSelector);
    const isNewScreenProfileLoading: boolean = useSelector(isNewScreenProfileLoadingSelector);
    
    return (
        <Grid container spacing={1} >   
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Name*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <ScreendoxTextInput
                    type="text"
                    id="screenProfileName"
                    value={createName}
                    onChange={e => {
                        e.stopPropagation();
                        dispatch(setCreateScreenProfileName(e.target.value));
                    }}
                />
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '20px' }}>
                <TitleTextModal>
                    Description
                </TitleTextModal>
            </Grid>
            <ScreendoxTextArea
                value={createDescription}
                onChange={e => {
                    dispatch(setCreateScreenProfileDescription(`${e.target.value}`));
                }}
            >
                { createDescription }
            </ScreendoxTextArea>
        </Grid>
    )
}

export default AddScreenProfile;