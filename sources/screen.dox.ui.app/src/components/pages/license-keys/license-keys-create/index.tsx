import React, { ChangeEvent } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getLicenseKeySelector, isLicenseKeyCreateLoadingSelector } from 'selectors/license-keys';
import { Grid, Button, TextField } from '@material-ui/core';
import { LicenseTitleText, ButtonTextStyle, TitleText, useStyles } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from 'actions/settings';
import { createLicenseKeyRequest, setLicenseKey } from 'actions/license-keys';
import CloseIcon from '@material-ui/icons/Close';

const CreateLicenseKey = (): React.ReactElement => {
    const customClasses = useStyles();
    const dispatch = useDispatch();
    const licenseKey = useSelector(getLicenseKeySelector);
    const isLoading = useSelector(isLicenseKeyCreateLoadingSelector);

    return (
        <div>
            <Grid item sm={12}>
                <CloseIcon className={customClasses.closeIcon} 
                    style={{ top: 10, right: 10 }}
                    onClick={() =>{
                        dispatch(closeModalWindow(EModalWindowKeys.licenseKeysCreate));
                    }
                }/>
            </Grid>
            <Grid item sm={12}>
                <TitleText>License Key</TitleText>
            </Grid>
            <Grid container >
                <Grid item sm={9}>
                    <TextField
                        fullWidth
                        margin="dense"
                        label=""
                        error={!licenseKey && isLoading}
                        id="licenseKey"
                        variant="outlined"
                        value={licenseKey}
                        onChange={e => {
                            e.stopPropagation();
                            dispatch(setLicenseKey(e.target.value));
                        }}
                    />
                </Grid>
                <Grid item sm={3} style={{ textAlign: 'center' }}>
                    <Button 
                        size="large" 
                        disabled={isLoading}
                        className={classes.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42', marginTop: 5 }}
                        onClick={() => {
                            dispatch(createLicenseKeyRequest())
                        }}
                    >
                        <ButtonTextStyle style={{ color: '#fff' }}>
                            Register License
                        </ButtonTextStyle>
                    </Button>
                </Grid>
            </Grid>
        </div>  
    )
}

export default CreateLicenseKey;
