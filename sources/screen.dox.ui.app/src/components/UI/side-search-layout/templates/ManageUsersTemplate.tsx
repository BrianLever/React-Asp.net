import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {  Button, FormControl, Grid, Select } from '@material-ui/core';
import { TitleText, TitleTextH2, ContainerBlock } from './styledComponents';
import classes from  '../templates.module.scss';
import { getManageUsersListRequest, setManageUsersSelectedLocationId } from '../../../../actions/manage-users';
import { manageUsersLocationsSelector , manageUsersLocationIdSelector } from '../../../../selectors/manage-users';
import { TBranchLocationsItemResponse } from '../../../../actions/shared';
import { ButtonText } from '../styledComponents';
import { EMPTY_LIST_VALUE, EMPTY_LOCALTION_LIST_VALUE } from 'helpers/general'
import ScreenDoxSelect from '../../select';


const ManageUsersTemplate = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const locationId = useSelector(manageUsersLocationIdSelector);
    const locations = useSelector(manageUsersLocationsSelector);
    return (
        <ContainerBlock>
            <Grid container spacing={1} >
                <Grid item xs={12} style={{ textAlign: 'left', marginBottom: 25 }}>
                    <TitleText>
                        Search User
                    </TitleText>
                </Grid>
                <Grid item xs={12} style={{ textAlign: 'left', marginTop: '15px' }}>
                    <TitleTextH2>
                        Filter by Branch Location
                    </TitleTextH2>
                </Grid>
               
                <Grid item xs={12}>
                    <ScreenDoxSelect
                        options={locations.map((location: TBranchLocationsItemResponse) => (
                            { name: `${location.Name}`.slice(0, 20), value: location.ID}
                        ))}
                        defaultValue={locationId}
                        rootOption={{ name: EMPTY_LOCALTION_LIST_VALUE, value: 0 }}
                        changeHandler={(value: any) => {
                            const v = parseInt(`${value}`);
                            dispatch(setManageUsersSelectedLocationId(v))
                            dispatch(getManageUsersListRequest())
                        }}
                    />
                </Grid>
            </Grid>
        </ContainerBlock>
    )
}

export default ManageUsersTemplate;