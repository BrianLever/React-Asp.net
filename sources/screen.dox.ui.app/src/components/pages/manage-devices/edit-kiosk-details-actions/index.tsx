import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from '../../../../actions/settings';
import { deleteKioskByIdRequest, disbaleKioskByIdRequest, updateKioskByIdRequest } from '../../../../actions/manage-devices';
import { getEditKioskDetailsSelector } from 'selectors/manage-devices';
import { ButtonTextStyle } from 'components/pages/styledComponents';

const EditKioskDetailsActions = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const kioskDetails = useSelector(getEditKioskDetailsSelector);
    return (
        <Grid container spacing={1} style={{ fontSize: 16 }} >
            <Grid item xs={12} style={{ textAlign: 'center' }}>
                <Button 
                    size="large" 
                    fullWidth 
                    disabled={false}
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    onClick={() => {
                        dispatch(updateKioskByIdRequest());
                    }}
                >
                   <ButtonTextStyle>Save Changes</ButtonTextStyle>
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
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                    onClick={() => {
                        dispatch(deleteKioskByIdRequest());
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>Delete</ButtonTextStyle>
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
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                    onClick={() => {
                        dispatch(updateKioskByIdRequest(!kioskDetails.Disabled));
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>{!kioskDetails.Disabled?'Disable':'Enable'}</ButtonTextStyle>
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
                    style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42' }}
                    onClick={() => {
                        dispatch(closeModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
                    }}
                >
                    <ButtonTextStyle style={{ color: '#2e2e42' }}>Cancel</ButtonTextStyle>
                </Button>
            </Grid>
        </Grid>
    )
}

export default EditKioskDetailsActions;