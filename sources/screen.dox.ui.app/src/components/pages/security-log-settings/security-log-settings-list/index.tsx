import { getSecurityLogSettingsRequest, getSecurityLogSettingsRequestSuccess, ISecurityLogSettingsCategory, ISecurityLogSettingsItem, updateSecurityLogSettingsItemsRequest } from 'actions/security-log-settings';
import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { getSecurityLogSettingsItemsSelector, getSecurityLogSettingsCategorySelector, isSecurityLogSettingsLoadingSelector } from 'selectors/security-log-settings';
import { ContentContainer, useStyles, SecurityLogSettingsButtonText } from '../../styledComponents';
import {
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Switch, Button
} from '@material-ui/core';
import customClass from '../../pages.module.scss';
import { TitleText } from 'components/UI/table/styledComponents';
import CustomAlert from 'components/UI/alert';

const SecurityLogSettingsList = (): React.ReactElement => {
    const dispatch = useDispatch();
    const classes = useStyles();
    const securityLogSettingsItems: Array<ISecurityLogSettingsItem> = useSelector(getSecurityLogSettingsItemsSelector);
    const securityLogSettingsCategory: Array<ISecurityLogSettingsCategory> = useSelector(getSecurityLogSettingsCategorySelector);
    const isLoading: boolean = useSelector(isSecurityLogSettingsLoadingSelector);
    
    const handleChange = (value: boolean, index: number) => { 
        const arrayData = securityLogSettingsItems.map(item => {
            if(item.ID === index) {
                item.IsEnabled = value;
            }
            return item;
        })
        dispatch(getSecurityLogSettingsRequestSuccess(arrayData, securityLogSettingsCategory));
    }
    
    useEffect(() => {
        dispatch(getSecurityLogSettingsRequest());
    }, [dispatch])
    
    return (
        <ContentContainer>
            <CustomAlert />
            <Grid container spacing={1} style={{ marginBottom: 30 }}>
                <Grid item sm={6}>
                    <TitleText>Security Log Settings</TitleText>
                </Grid>
                <Grid item sm={6} className={customClass.rightTh}>
                    <Button 
                        size="large"  
                        disabled={false}
                        className={customClass.removeBoxShadow}
                        variant="contained" 
                        color="primary" 
                        style={{ backgroundColor: '#2e2e42' }}
                        onClick={() => {
                           dispatch(updateSecurityLogSettingsItemsRequest())
                        }}
                    >
                        <SecurityLogSettingsButtonText>Save Changes</SecurityLogSettingsButtonText>
                            
                    </Button>
                </Grid>
            </Grid>
            {isLoading?<CircularProgress disableShrink={false} className={customClass.circularLoadingStyle}/>:
            <TableContainer>
                {securityLogSettingsCategory && securityLogSettingsCategory.map((category: ISecurityLogSettingsCategory) => {
                    const filteredSecurityItems = securityLogSettingsItems.filter(d => d.CategoryID == category.ID);
                    return (
                        <Table key={category.ID} style={{ marginBottom: 30 , fontSize: 14 }}>
                            <TableHead className={customClass.tableHead}>
                                <TableRow>
                                    <TableCell style={{ fontSize: '1em' }}>{category.Description}</TableCell>
                                    <TableCell style={{ fontSize: '1em' }} className={customClass.rightTh}>Turn OFF/ON</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                            {filteredSecurityItems && filteredSecurityItems.map((item, index) => (
                                <TableRow key={index}>
                                    <TableCell style={{ fontSize: '1em' }}>{item.Description}</TableCell>
                                    <TableCell style={{ fontSize: '1em' }}className={customClass.rightTh}>
                                        <Switch  
                                            checked={item.IsEnabled} 
                                            onChange={(e) => handleChange(e.target.checked, item.ID)}
                                            classes={{
                                                root: classes.element,
                                                switchBase: classes.switchBase,
                                                thumb: classes.thumb,
                                                track: classes.track,
                                                checked: classes.checked
                                            }}
                                            focusVisibleClassName={classes.focusVisible}
                                        />
                                    </TableCell>
                                </TableRow>
                            ))}
                            </TableBody>
                        </Table>
                    )
                })}
            </TableContainer>}
        </ContentContainer>
    )   
}

export default SecurityLogSettingsList;