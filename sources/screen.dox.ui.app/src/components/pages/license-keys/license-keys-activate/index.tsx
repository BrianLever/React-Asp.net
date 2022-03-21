import React, { ChangeEvent } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getLicenseActivationKeySelector, getLicenseKeySelector, isLicenseKeyCreateLoadingSelector } from 'selectors/license-keys';
import { Grid, Button, TextField } from '@material-ui/core';
import { LicenseTitleText, ButtonTextStyle, TitleText, DescriptionText } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys, notifySuccess } from 'actions/settings';
import { activeLicenseKeyRequest, createLicenseKeyRequest, setLicenseKey } from 'actions/license-keys';
import { useState } from 'react';
import RegularButton from 'components/UI/button/RegularButton';

const ActivateLicenseKey = (): React.ReactElement => {

    const dispatch = useDispatch();
    const licenseKey = useSelector(getLicenseKeySelector);
    const isLoading = useSelector(isLicenseKeyCreateLoadingSelector);
    const licenseActivationKey = useSelector(getLicenseActivationKeySelector);
    const [activateKey, setActivateKey] = useState('');

    const copyCodeToClipboard = (text: string | null) => {
        var textField = document.createElement('textarea')
        if(text) {
            textField.innerText = text
        }
        document.body.appendChild(textField)
        textField.select()
        document.execCommand('copy')
        textField.remove();
        dispatch(notifySuccess('Copied.'));
    }

    return (
        <Grid container spacing={1}>
            <Grid item sm={12}>
                <TitleText>License Key:</TitleText>
            </Grid>
            <Grid container spacing={1}>
                <Grid item sm={6}>
                    <DescriptionText>{ licenseKey }</DescriptionText>
                </Grid>
                <Grid item sm={6}>
                    <RegularButton type={'regular'} color={'regular'} onClick={() => copyCodeToClipboard(licenseKey)}>Copy</RegularButton>
                </Grid>
            </Grid>
            <Grid item sm={12}>
                <TitleText>Activation Request Key:</TitleText>
            </Grid>
            <Grid container spacing={1}>
                <Grid item sm={6}>
                    <DescriptionText>{ licenseActivationKey }</DescriptionText>
                </Grid>
                <Grid item sm={6}>
                    <RegularButton type={'button'} color={'regular'} onClick={() => copyCodeToClipboard(licenseActivationKey)}>Copy</RegularButton>
                </Grid>
            </Grid>
            <Grid item sm={12}>
                <TitleText>Enter Activation Key:</TitleText>
            </Grid>
            <Grid item sm={12}>
                <TextField
                    fullWidth
                    margin="dense"
                    label=""
                    error={activateKey === ''}
                    id="activateKey"
                    variant="outlined"
                    value={activateKey}
                    onChange={e => {
                        e.stopPropagation();
                        setActivateKey(e.target.value);
                    }}
                />
            </Grid>
            <Grid item sm={12}>
                <Button 
                    size="large" 
                    disabled={isLoading}
                    className={classes.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42', marginTop: 5 }}
                    onClick={() => {
                        dispatch(activeLicenseKeyRequest(activateKey))
                    }}
                >
                    <ButtonTextStyle style={{ color: '#fff' }}>
                        Activate
                    </ButtonTextStyle>
                </Button>
            </Grid>
        </Grid> 
    )
}

export default ActivateLicenseKey;