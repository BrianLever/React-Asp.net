import React, { ChangeEvent } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid, Button, TextField } from '@material-ui/core';
import { TitleText, ButtonTextStyle, useStyles , EhrLoginDateInput } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { closeModalWindow, EModalWindowKeys } from 'actions/settings';
import { ProfileLargeBoldText } from '../../profile/styledComponent';
import { setEhrLoginAccessCode, setEhrLoginExpireOn, setEhrLoginVerifyCode } from 'actions/ehr-login';
import { getEhrLoginAccessCodeSelector, getEhrLoginExpireOnSelector, getEhrLoginVerifyCodeSelector } from 'selectors/ehr-login';


const EhrLoginCreate = (): React.ReactElement => {

    const dispatch = useDispatch();
    const AccessCode = useSelector(getEhrLoginAccessCodeSelector);
    const VerifyCode = useSelector(getEhrLoginVerifyCodeSelector);
    const ExpireOn = useSelector(getEhrLoginExpireOnSelector);


    return (
        <div style={{ fontSize: 16 }}>
            <Grid container >
                <Grid item sm={12} style={{ marginBottom: 15 }}>
                    <ProfileLargeBoldText>Access Code (User Name)*</ProfileLargeBoldText>
                    <TextField
                        fullWidth
                        margin="dense"
                        label=""
                        id="access_code"
                        variant="outlined"
                        value={AccessCode}
                        onChange={e => {
                            dispatch(setEhrLoginAccessCode(`${e.target.value}`))
                        }}
                    />
                </Grid>
                <Grid item sm={12} style={{ marginBottom: 15 }}>
                    <ProfileLargeBoldText>Verify Code (Password)*</ProfileLargeBoldText>
                    <TextField
                        fullWidth
                        margin="dense"
                        label=""
                        id="verify_code"
                        variant="outlined"
                        value={VerifyCode}
                        onChange={e => {
                            dispatch(setEhrLoginVerifyCode(`${e.target.value}`))
                        }}
                    />
                </Grid>
                <Grid item sm={12} style={{ marginBottom: 15 }}>
                    <ProfileLargeBoldText>Expire On*</ProfileLargeBoldText>
                    <EhrLoginDateInput type="date" value={ExpireOn} onChange={e => dispatch(setEhrLoginExpireOn(`${e.target.value}`))}/>
                </Grid>
            </Grid>
        </div>  
    )
}

export default EhrLoginCreate;
