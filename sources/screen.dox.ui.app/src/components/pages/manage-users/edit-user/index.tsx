import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid , Button, TextField, Select, FormControl  } from '@material-ui/core';
import { ERouterUrls } from '../../../../router';
import { useHistory } from 'react-router';
import { ProfileLargeBoldText } from '../../profile/styledComponent';
import { TBranchLocationsItemResponse } from 'actions/shared';
import { manageUsersLocationsSelector , manageUsersLocationIdSelector, manageUsersUserSelector , manageUsersGroupsSelector, manageUsersSelectedUserIdSelector } from '../../../../selectors/manage-users';
import { ManageUserTextArea } from '../../styledComponents';
import { usStates, EMPTY_LIST_VALUE } from 'helpers/general';
import { setManageUsersUser } from 'actions/manage-users';
import ScreendoxSelect from '../../../UI/select';

const EditUser = (): React.ReactElement => {

  const dispatch = useDispatch();
  const locationId = useSelector(manageUsersLocationIdSelector);
  const locations = useSelector(manageUsersLocationsSelector);
  const user = useSelector(manageUsersUserSelector);
  const userGroups = useSelector(manageUsersGroupsSelector);

  React.useEffect(() => {

  }, [])

  return (
      <div className={'edit-user'} style={{ fontSize: 16 }}>
           <Grid container spacing={2}>
            <Grid item sm={4}>
                <ProfileLargeBoldText>User Name*</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Username"
                    variant="outlined"
                    value={user.UserName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            UserName: value
                        }))
                    }}
                />
           </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>Group*</ProfileLargeBoldText>
                <ScreendoxSelect
                    options={userGroups.map((l) => (
                        { name: l, value: l }
                    ))}
                    defaultValue={user.RoleName}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: '' }}
                    changeHandler={(value: any) => {
                        dispatch(setManageUsersUser({
                            ...user,
                            RoleName: `${value}`
                        }))
                    }}
                />
            </Grid>
           <Grid item sm={4}>
                <ProfileLargeBoldText>Branch Location*</ProfileLargeBoldText>
                <ScreendoxSelect
                    options={locations.map((l) => (
                        { name: l.Name, value: l.ID }
                    ))}
                    defaultValue={locationId}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                    changeHandler={(value: any) => {
                        dispatch(setManageUsersUser({
                            ...user,
                            BranchLocationID: Number(value)
                        }))
                    }}
                />
           </Grid>
        </Grid> 
       
        <Grid container spacing={2} justifyContent="space-between" alignItems="center">
            <Grid item sm={4}>
                <ProfileLargeBoldText>First Name*</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="First Name"
                    variant="outlined"
                    value={user.FirstName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            FirstName: value
                        }))
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
                    value={user.LastName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            LastName: value
                        }))
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
                    value={user.MiddleName}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            MiddleName: value
                        }))
                    }}
                />
            </Grid>
        </Grid>
        <Grid container spacing={2} justifyContent="space-between" alignItems="center">
            <Grid item sm={6}>
                <ProfileLargeBoldText>E-mail</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="email"
                    variant="outlined"
                    value={user.Email}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            Email: value
                        }))
                    }}
                />
           </Grid>
           <Grid item sm={3}>
                <ProfileLargeBoldText>Office Phone</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Primany Phone"
                    variant="outlined"
                    value={user.ContactPhone}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            ContactPhone: value
                        }))
                    }}
                />
           </Grid>
           <Grid item sm={3}>
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
                    }}
                />
           </Grid>
        </Grid>
        <Grid container spacing={2} justifyContent="space-between" alignItems="center">
            <Grid item sm={12}>
                <ProfileLargeBoldText>Address</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="Address"
                    variant="outlined"
                    value={user.AddressLine1}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            AddressLine1: value
                        }))
                    }}
                />
            </Grid>
        </Grid>
        <Grid container spacing={2} justifyContent="space-between" alignItems="center">
            <Grid item sm={4}>
                <ProfileLargeBoldText>City</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="city"
                    variant="outlined"
                    value={user.City}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            City: value
                        }))
                    }}
                />
            </Grid>
            <Grid item sm={4}>
                <ProfileLargeBoldText>State</ProfileLargeBoldText>
                <ScreendoxSelect
                    options={usStates.map((state:{ name: string, abbreviation: string }) => (
                        { name: state.name, value: state.abbreviation }
                    ))}
                    defaultValue={user.StateCode}
                    rootOption={{ name: EMPTY_LIST_VALUE, value: '' }}
                    changeHandler={(value: any) => {
                        dispatch(setManageUsersUser({
                            ...user,
                            StateCode: `${value}`
                        }))
                    }}
                />
            </Grid>
            <Grid item sm={4}>
                <ProfileLargeBoldText>Postal Code</ProfileLargeBoldText>
                <TextField
                    fullWidth
                    margin="dense"
                    id="postal code"
                    variant="outlined"
                    value={user.PostalCode}
                    onChange={e => {
                        e.stopPropagation();
                        const value = `${e.target.value}`;
                        dispatch(setManageUsersUser({
                            ...user,
                            PostalCode: value
                        }))
                    }}
                />
            </Grid>
        </Grid>
        <Grid container spacing={2} justifyContent="space-between" alignItems="center">
            <Grid item sm={12}>
                <ProfileLargeBoldText>Comments</ProfileLargeBoldText>
                <ManageUserTextArea onChange={(e) => {
                    dispatch(setManageUsersUser({
                        ...user,
                        Comments: e.target.value
                    }))
                }}>{user.Comments}</ManageUserTextArea>
            </Grid>
        </Grid>
      </div>
  )
}

export default EditUser;
