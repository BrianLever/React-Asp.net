import React from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { 
    FormControl, Grid, Select, TextField, 
} from '@material-ui/core';
import { TitleTextModal } from '../../styledComponents';
import { EMPTY_LOCALTION_LIST_VALUE, EMPTY_LIST_VALUE }  from 'helpers/general';
import { 
    selectAddNewKioskDeviceName, selectAddNewKioskSecretKey, 
    changeEditKioskDetailsBranchLocation, changeEditKioskDetailsDescription
} from '../../../../actions/manage-devices';
import { 
    getEditKioskDetailsSelector, getEditKioskDetailsBranchLocationSelector, getEditKioskDetailsDescriptionSelector, getSelectedAddNewKioskSecretKeySelector
} from '../../../../selectors/manage-devices';
import { getListBranchLocationsRequest, TBranchLocationsItemResponse } from '../../../../actions/shared';
import { getListBranchLocationsSelector } from '../../../../selectors/shared';
import ScreendoxSelect from 'components/UI/select';


export const EditKioskDetailsContentTextarea = styled.textarea`
    font-size: 1.2em !important;
    border: 1px solid #2e2e42 !important;
    width: 100%;
    min-height: 120px;
    background: transparent;
    padding: 10px;
`;

export const EditKioskDetailsContentText = styled.h1`
    font-family: "hero-new";
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 1.4;
    letter-spacing: 0px;
    text-transform: none;
    color: #2e2e42;
    background-color: transparent;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

const EditKioskDetailsContent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const locationsList = useSelector(getListBranchLocationsSelector);
    const kioskDetails = useSelector(getEditKioskDetailsSelector);
    const location = useSelector(getEditKioskDetailsBranchLocationSelector);
    const description = useSelector(getEditKioskDetailsDescriptionSelector);
    const keycode = useSelector(getSelectedAddNewKioskSecretKeySelector);

    React.useEffect(() => {
        if (!locationsList.length) {
            dispatch(getListBranchLocationsRequest());
        }
    }, [dispatch, locationsList, locationsList.length])


    return (
        <Grid container spacing={1} >
            {kioskDetails.Disabled && <Grid item xs={12}>
                <EditKioskDetailsContentText>
                    Kiosk is disabled
                </EditKioskDetailsContentText>
            </Grid>}
            <Grid item xs={12} style={{ textAlign: 'left' }}>
                <TitleTextModal>
                    Device Key (Use this value during Screendox device application installation)
                </TitleTextModal>
                <EditKioskDetailsContentText>
                    { kioskDetails.KioskKey }
                </EditKioskDetailsContentText>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left' }}>
                <TitleTextModal>
                    Branch Location*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12}>
                <ScreendoxSelect 
                    options={locationsList.map((l: TBranchLocationsItemResponse) => (
                        { name: `${l.Name}`.slice(0, 20), value: l.ID}
                    ))}
                    defaultValue={location}
                    rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                    changeHandler={(value: any) => {
                        dispatch(changeEditKioskDetailsBranchLocation(parseInt(value)));
                    }}
                />

            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Device Name*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <TextField
                    fullWidth
                    margin="dense"
                    label=""
                    disabled={kioskDetails.Disabled}
                    id="deviceName"
                    variant="outlined"
                    value={kioskDetails.Name}
                    onChange={e => {
                        e.stopPropagation();
                        dispatch(selectAddNewKioskDeviceName(e.target.value));
                    }}
                />
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Screen Profile
                </TitleTextModal>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left' }}>
                <EditKioskDetailsContentText>
                    { kioskDetails.ScreeningProfileName }
                </EditKioskDetailsContentText>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Secret Key*
                </TitleTextModal>
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <TextField
                    fullWidth
                    margin="dense"
                    label=""
                    disabled={false}
                    id="secretKey"
                    variant="outlined"
                    value={kioskDetails.SecretKey}
                    onChange={e => {
                        e.stopPropagation();
                        dispatch(selectAddNewKioskSecretKey(e.target.value));
                    }}
                />
            </Grid>
            <Grid item xs={12} style={{ textAlign: 'left', marginTop: '10px' }}>
                <TitleTextModal>
                    Description
                </TitleTextModal>
            </Grid>
            <EditKioskDetailsContentTextarea
                value={description}
                onChange={e => {
                    dispatch(changeEditKioskDetailsDescription(`${e.target.value}`));
                }}
            />
        </Grid>
    )
}

export default EditKioskDetailsContent;