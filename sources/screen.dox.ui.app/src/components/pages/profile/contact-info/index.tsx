import React from 'react';
import { Grid, TextField, Button, FormControl, Select } from '@material-ui/core';
import {  ProfileLargeBoldText, TextArea, useStyles } from '../styledComponent';
import { getProfileSelector } from 'selectors/profile';
import { setProfile, updateProfileRequest } from 'actions/profile';
import { EMPTY_LIST_VALUE, usStates } from 'helpers/general';
import { useDispatch, useSelector } from 'react-redux';
import ScreendoxSelect from 'components/UI/select';

const ContactInfo = (): React.ReactElement => {
    const dispatch = useDispatch();
    const classes = useStyles();
    const profileInfo = useSelector(getProfileSelector);
    return (<>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={4}>
                <ProfileLargeBoldText>First Name*</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="First Name"
                    variant="outlined"
                    value={profileInfo?.FirstName}
                    onChange={e => {console.log(e.target.value);
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, FirstName: value }))
                    }}
                />
           </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>Last Name*</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Last Name"
                    variant="outlined"
                    value={profileInfo?.LastName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, LastName: value }))
                    }}
                />
           </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>Middle Name</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Middle Name"
                    variant="outlined"
                    value={profileInfo?.MiddleName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, MiddleName: value }))
                    }}
                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>
            <Grid item sm={4}>
                <ProfileLargeBoldText>E-mail</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="E-mail"
                    variant="outlined"
                    value={profileInfo?.Email}
                    onChange={e => {console.log(e.target.value);
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, Email: value }))
                    }}
                />
            </Grid>
            <Grid item sm={4}>
                <ProfileLargeBoldText>Primary Office Phone</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Primany Office Phone"
                    variant="outlined"
                    value={profileInfo?.ContactPhone}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, ContactPhone: value }))
                    }}
                />
            </Grid>
            <Grid item sm={4}>
                <ProfileLargeBoldText>Mobile Phone</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Mobile Phone"
                    variant="outlined"
                    value={''}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        // dispatch(setProfile({ ...profileInfo, ContactPhone: value }))
                    }}
                />
            </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={12}>
                <ProfileLargeBoldText>Address</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Address"
                    variant="outlined"
                    value={profileInfo?.AddressLine1}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, AddressLine1: value }))
                    }}
                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }} spacing={2}>  
           <Grid item sm={4}>
                <ProfileLargeBoldText>City</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="City"
                    variant="outlined"
                    value={profileInfo?.City}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, City: value }))
                    }}

                />
           </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>State</ProfileLargeBoldText>
                <ScreendoxSelect
                    options={usStates.map((state:{ name: string, abbreviation: string }) => (
                        { name: state.name, value: state.abbreviation }
                    ))}
                    defaultValue={profileInfo?.StateCode}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: '' }}
                    changeHandler={(value: any) => {
                        
                    }}
                />
           </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>Postal Code</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Postal Code"
                    variant="outlined"
                    value={profileInfo?.PostalCode}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setProfile({ ...profileInfo, PostalCode: value }))
                    }}

                />
           </Grid>
        </Grid>
        <Grid container justifyContent="space-between" alignItems="center" style={{ marginTop: 20 }}>
            <ProfileLargeBoldText>Comments</ProfileLargeBoldText>
             <TextArea value={profileInfo?.Comments}
                onChange={e => {
                    e.stopPropagation();
                    const value = `${e.target.value}`;
                    dispatch(setProfile({ ...profileInfo, Comments: value }))
                }}
             />           
        </Grid>
        <Grid container>
            <Grid item sm={12} style={{ textAlign: "right", marginTop: 40 }}>
                <Button 
                    size="large" 
                    disabled={false}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: '#2e2e42' }}
                    className={classes.buttonStyle}
                    onClick={() => {
                        dispatch(updateProfileRequest());
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
                    className={classes.buttonStyle}
                    style={{ backgroundColor: '#2e2e42' }}
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

export default ContactInfo;