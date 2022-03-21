import React from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { 
    FormControl, Grid, Select, TextField, 
} from '@material-ui/core';
import { TitleTextModal } from '../../styledComponents';
// import classes from  '../../pages.module.scss';
import { 
    selectAddNewKioskDescription, selectAddNewKioskBranchLocation, 
    selectAddNewKioskDeviceName, selectAddNewKioskScreenProfile, selectAddNewKioskSecretKey,  
} from '../../../../actions/manage-devices';
import { 
    getSelectedAddNewKioskBranchLocationIdSelector, getSelectedAddNewKioskDeviceNameSelector,
    getSelectedAddNewKioskScreenProfileSelector, getSelectedAddNewKioskSecretKeySelector, getSelectedAddNewKioskDescriptionSelector,
    getInconsistencyInFieldsFlagSelector
} from '../../../../selectors/manage-devices';
import { getListBranchLocationsSelector, getScreeningProfileListSelector } from '../../../../selectors/shared';
import { getListBranchLocationsRequest, TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../../../actions/shared';
import { EMPTY_LIST_VALUE, EMPTY_LOCALTION_LIST_VALUE } from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';


export const AddNewDeviceContentTextarea = styled.textarea`
    font-size: 1.2em !important;
    border: 1px solid #2e2e42 !important;
    width: 100%;
    min-height: 120px;
    background: transparent;
    padding: 10px;
`;

const AddNewDeviceContent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const locationsList = useSelector(getListBranchLocationsSelector);
    const selectedLocationId: number = useSelector(getSelectedAddNewKioskBranchLocationIdSelector);
    const deviceName: string = useSelector(getSelectedAddNewKioskDeviceNameSelector);
    const screenProfile: string = useSelector(getSelectedAddNewKioskScreenProfileSelector);
    const screeningProfileList: Array<TScreeningProfileItemResponse> = useSelector(getScreeningProfileListSelector);
    const secretKey: string = useSelector(getSelectedAddNewKioskSecretKeySelector);
    const description: string = useSelector(getSelectedAddNewKioskDescriptionSelector);
    const inconsistencyInFieldsFlag: boolean = useSelector(getInconsistencyInFieldsFlagSelector);
    const isLocationIdError = (inconsistencyInFieldsFlag && !selectedLocationId);
    const isDeviceNameError = (inconsistencyInFieldsFlag && !deviceName);
    const isSecretKeyError = (inconsistencyInFieldsFlag && !secretKey);

    React.useEffect(() => {
        if (!locationsList.length) {
            dispatch(getListBranchLocationsRequest());
        }
    }, [dispatch, locationsList, locationsList.length]);


    return (
        <Grid container spacing={1} >
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
                defaultValue={selectedLocationId}
                rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                changeHandler={(value: any) => {
                    dispatch(selectAddNewKioskBranchLocation(parseInt(value)));
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
                error={isDeviceNameError}
                id="deviceName"
                variant="outlined"
                value={deviceName}
                onChange={e => {
                    e.stopPropagation();
                    dispatch(selectAddNewKioskDeviceName(e.target.value));
                }}
            />
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
                error={isSecretKeyError}
                id="secretKey"
                variant="outlined"
                value={secretKey}
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
        <AddNewDeviceContentTextarea
            value={description}
            onChange={e => {
                dispatch(selectAddNewKioskDescription(`${e.target.value}`))
            }}
         >
             { description }
         </AddNewDeviceContentTextarea>
    </Grid>
    )
}

export default AddNewDeviceContent;