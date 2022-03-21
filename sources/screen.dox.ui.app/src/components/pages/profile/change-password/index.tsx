import React, { useEffect, useState, ChangeEvent } from 'react';
import { Grid, TextField, Button } from '@material-ui/core';
import RectangleCheckbox from 'components/UI/checkbox/RectangleCheckbox';
import { useDispatch, useSelector } from 'react-redux';
import { ProfileTitle, ProfileLargeBoldText, ProfileLargeText, ProfileSmallText, TextArea, useStyles } from '../styledComponent';
import { getProfileSelector } from 'selectors/profile';
import { notifyError } from 'actions/settings';
import { changePasswordRequest } from 'actions/change-password';

const ChangePassword = (): React.ReactElement => {
    const dispatch = useDispatch();
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const classes = useStyles();
    const currentPasswordHangleChange = (value: any) => {
        setCurrentPassword(value);
    }

    const newPasswordHandleChange = (value: any) => {
        setNewPassword(value);
    }

    const confirmPasswordHandleChange = (value: any) => {
        setConfirmPassword(value);
    }

    const handleSubmit = () => {
        
        if(!currentPassword) {
            dispatch(notifyError('Password is required.'));
            return;
        }
        if(!newPassword) {
            dispatch(notifyError('New Password is required.'))
            return;
        }
        if(newPassword !== confirmPassword) {
            dispatch(notifyError('New Password and Password confirmation must match'));
            return;
        }

        dispatch(changePasswordRequest({CurrentPassword: currentPassword, NewPassword: newPassword}));
    }

    return (<>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 40 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Current Password*</ProfileLargeBoldText>
                <TextField
                        fullWidth
                        margin="dense"
                        id="First Name"
                        type={'password'}
                        variant="outlined"
                        value={currentPassword}
                        onChange={e => {
                            e.stopPropagation();
                            currentPasswordHangleChange(`${e.target.value}`)
                        }}

                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>New Password*</ProfileLargeBoldText>
                <TextField
                        fullWidth
                        margin="dense"
                        id="First Name"
                        type={'password'}
                        variant="outlined"
                        value={newPassword}
                        onChange={e => {
                            e.stopPropagation();
                            newPasswordHandleChange(`${e.target.value}`)
                        }}

                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Confirm New Password*</ProfileLargeBoldText>
                <TextField
                        fullWidth
                        margin="dense"
                        id="First Name"
                        type={'password'}
                        variant="outlined"
                        value={confirmPassword}
                        onChange={e => {
                            e.stopPropagation();
                            confirmPasswordHandleChange(`${e.target.value}`)
                        }}

                />
           </Grid>
        </Grid>                
        <Grid container>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 40 }}>
                <Button 
                    size="large" 
                    disabled={isLoading}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {
                        handleSubmit();
                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Save Changes
                    </p>
                </Button>
            </Grid>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 10}}>
                <Button 
                    size="large" 
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {

                    }}
                >
                    <p className={classes.buttonTextStyle}>
                        Cancel
                    </p>
                </Button>
            </Grid>
        </Grid></>
    )
}

export default ChangePassword;