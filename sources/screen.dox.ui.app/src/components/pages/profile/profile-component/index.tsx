import React, { useEffect, useState, ChangeEvent } from 'react';
import { Grid, TextField, Button } from '@material-ui/core';
import { useSelector, useDispatch  } from 'react-redux';
import { ContentContainer } from '../../styledComponents';
import { ProfileTitle, ProfileLargeBoldText, ProfileLargeText, ProfileSmallText } from '../styledComponent';
import { getProfileSelector } from 'selectors/profile';
import { getListBranchLocationsSelector } from 'selectors/shared';
import { TabBar, TabPanel } from 'components/UI/tab';
import TermsOfUse from '../terms-of-use';
import ContactInfo from '../contact-info';
import ChangePassword from '../change-password';
import SecurityQuestion from '../security-question';
import { useStyles } from '../styledComponent';
import CustomAlert from 'components/UI/alert';


const ProfileComponent = (): React.ReactElement => {

    const profileInfo = useSelector(getProfileSelector);
    const branchLocations = useSelector(getListBranchLocationsSelector);
    const branchLocation = branchLocations.filter(location => location.ID === Number(profileInfo?.BranchLocationID));
    const [value, setValue] = React.useState(0);
    const classes = useStyles();
    const handleChange = (event: ChangeEvent<{}>, newValue: number) => {
        setValue(newValue);
    } 
    return (
        <ContentContainer>
            <CustomAlert />
            <Grid container style={{ fontSize: 14 }}>
                <Grid item sm={3} xs={12} className={classes.profileContainer}>
                    <Grid item sm={12}>
                        <ProfileTitle>My Profile</ProfileTitle>
                    </Grid>
                    <Grid container>
                        <Grid item sm={12} xs={4}>
                            <ProfileLargeBoldText style={{ marginTop: 40 }}>Name</ProfileLargeBoldText>
                            <ProfileLargeText>{profileInfo?.FullName}</ProfileLargeText>
                        </Grid>
                        <Grid item sm={12} xs={4}>
                            <ProfileLargeBoldText style={{ marginTop: 40 }}>Group</ProfileLargeBoldText>
                            <ProfileLargeText>{profileInfo?.RoleName}</ProfileLargeText>
                        </Grid>
                        <Grid item sm={12} xs={4}>
                            <ProfileLargeBoldText style={{ marginTop: 40 }}>Branch Location</ProfileLargeBoldText>
                            <ProfileLargeText>{(branchLocations.length && profileInfo && branchLocation.length)?branchLocation[0].Name:''}</ProfileLargeText>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid item sm={9} xs={12} className={classes.profileContainer}>
                    <TabBar 
                        tabArray={['CONTACT INFO', 'PASSWORD', 'SECURITY QUESTION']}
                        handleChange={handleChange}
                        selectedTab={value}
                    />

                    <TabPanel value={value} index={0}>
                        <ContactInfo />
                    </TabPanel>
                    <TabPanel value={value} index={1}>
                        <ChangePassword />
                    </TabPanel>
                    <TabPanel value={value} index={2}>
                         <SecurityQuestion />
                    </TabPanel>
                </Grid>
            </Grid>
        </ContentContainer>
    )
}

export default ProfileComponent;