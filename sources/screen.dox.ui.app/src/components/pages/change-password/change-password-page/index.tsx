import React, { useEffect, useState } from 'react';
import { Grid, TextField, Button } from '@material-ui/core';
import {  ChangePasswordLabelText } from '../../styledComponents';
import { useSelector, useDispatch  } from 'react-redux';
import classes from '../../pages.module.scss';
import { notifyError } from 'actions/settings';
import { changePasswordRequest } from 'actions/change-password';

const ChangePasswordPage = (): React.ReactElement => {
    const dispatch = useDispatch();
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [isLoading, setIsLoading] = useState(false);


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
        setIsLoading(true);
        if(!currentPassword) {
            dispatch(notifyError('Password is required!'));
            return;
        }
        if(!newPassword) {
            dispatch(notifyError('New Password is required!'))
            return;
        }
        if(newPassword !== confirmPassword) {
            dispatch(notifyError('New Password and Password confirmation must match'));
            return;
        }

        dispatch(changePasswordRequest({CurrentPassword: currentPassword, NewPassword: newPassword}));
    }


    return (
        <div className={classes.centerContainer}>
            <Grid container>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            Current Password*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'center' }}>
                        <TextField
                            margin="dense"
                            label=""
                            error={!currentPassword && isLoading}
                            id="current_password"
                            type="password"
                            variant="outlined"
                            value={currentPassword}
                            onChange={e => {
                                e.stopPropagation();
                                currentPasswordHangleChange(e.target.value);
                            }}
                        />
                    </Grid>
                </Grid>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            New Password*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'center' }}>
                        <TextField
                            margin="dense"
                            label=""
                            error={!newPassword && isLoading}
                            id="new_password"
                            type="password"
                            variant="outlined"
                            value={newPassword}
                            onChange={e => {
                                e.stopPropagation();
                                newPasswordHandleChange(e.target.value);
                            }}
                        />
                    </Grid>         
                </Grid>
                <Grid container>
                    <Grid item sm={6} style={{ textAlign: 'center' }}>
                        <ChangePasswordLabelText>
                            Confirm Password*
                        </ChangePasswordLabelText>
                    </Grid>
                    <Grid item xs={6} style={{ textAlign: 'center' }}>
                        <TextField
                            margin="dense"
                            label=""
                            error={!confirmPassword && isLoading}
                            id="confirm_password"
                            variant="outlined"
                            value={confirmPassword}
                            type="password"
                            onChange={e => {
                                e.stopPropagation();
                                confirmPasswordHandleChange(e.target.value);
                            }}
                        />
                    </Grid>         
                </Grid>
                
                <Grid item xs={12} sm={12} style={{ textAlign: 'center', marginTop: 20 }}>
                    <Grid container >
                        <Grid item sm={6} style={{ textAlign: 'right' }}>
                            <Button 
                            size="large"  
                            disabled={false}
                            variant="contained" 
                            color="primary" 
                            style={{ backgroundColor: '#2e2e42', textAlign: 'right' }}
                            onClick={() => {
                                handleSubmit();
                            }}
                            >
                                <p style={{ color: '#fff' }}>
                                    Save Changes
                                </p>
                            </Button>
                        </Grid>
                        <Grid item sm={6} >
                            <Button 
                            size="large" 
                            disabled={false}
                            variant="contained" 
                            color="default"
                            style={{ backgroundColor: 'transparent', border: '1px solid #2e2e42', textAlign: 'left' }}
                            onClick={() => {
                                
                            }}
                            >
                                <p style={{ color: '#2e2e42' }}>
                                    Cancel
                                </p>
                            </Button>
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </div>
    )
}

export default ChangePasswordPage;