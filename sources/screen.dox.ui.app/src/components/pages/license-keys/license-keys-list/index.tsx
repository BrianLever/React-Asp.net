import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { ContentContainer, useStyles, LicenseKeysSummaryTitle, LicenseKeysSummaryDescription  } from '../../styledComponents';
import {
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Switch, Button
} from '@material-ui/core';
import customClass from '../../pages.module.scss';
import { TitleText } from 'components/UI/table/styledComponents';
import { DescriptionText, ButtonTextStyle } from '../../styledComponents';
import { getLicenseKeyDetailRequest, getLicenseKeysRequest, ILicenseKeysResponseItem, setLicenseActivationKey, setLicenseKey } from 'actions/license-keys';
import { getLicenseKeysSelector, getLicenseKeysSystemSettingsSummary, isLicenseKeysLoadingSelector } from 'selectors/license-keys';
import { convertDate } from 'helpers/dateHelper';
import ScreendoxDeleteButton from 'components/UI/custom-buttons/deleteButton';
import ScreendoxModal, { LicenseKeysMiniModal } from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import DeleteLicenseKeys from '../license-keys-delete';
import CreateLicenseKey from '../license-keys-create';
import ActivateLicenseKey from '../license-keys-activate';
import CustomAlert from 'components/UI/alert';

const LicenseKeysList = (): React.ReactElement => {
    const dispatch = useDispatch();
    const classes = useStyles();
    const licenseKeys: Array<ILicenseKeysResponseItem> = useSelector(getLicenseKeysSelector);
    const isLoading: boolean = useSelector(isLicenseKeysLoadingSelector);
    const summary = useSelector(getLicenseKeysSystemSettingsSummary);
    const deleteConfirmModal = (value: string) => {
        dispatch(setLicenseKey(value));
        dispatch(openModalWindow(EModalWindowKeys.licenseKeysDelete));
    }


    useEffect(() => {
        dispatch(getLicenseKeysRequest());
    }, [dispatch]);

    return (
        <ContentContainer>
           <CustomAlert />
           <Grid container spacing={1}>
            <Grid item sm={8}>
                <TitleText>License Keys</TitleText>
                <DescriptionText>If you have more than one activated license, only the license shown on top of the list can be used. Other licenses, even if they are not expired, will be ignored.</DescriptionText>
            </Grid>
            <Grid item sm={4} className={customClass.rightTh}>  
                <Button 
                    size="large"  
                    disabled={false}
                    className={customClass.removeBoxShadow}
                    variant="contained" 
                    color="primary" 
                    style={{ backgroundColor: 'rgb(46,46,66)' }}
                    onClick={() => {
                        dispatch(setLicenseKey(''));
                        dispatch(openModalWindow(EModalWindowKeys.licenseKeysCreate));
                    }}
                >
                    <ButtonTextStyle>
                        + Enter New License Key
                    </ButtonTextStyle>
                </Button>
            </Grid>
           </Grid>
            <Grid container spacing={1}>
                <Grid item sm={12}>
                {isLoading?<CircularProgress disableShrink={false} className={customClass.circularLoadingStyle}/>:
                    <TableContainer>
                        <Table>
                            <TableHead className={customClass.tableHead}>
                                <TableRow>
                                    <TableCell>License Key</TableCell>
                                    <TableCell>Registered Date</TableCell>
                                    <TableCell>MAX Locations</TableCell>
                                    <TableCell>MAX Devices</TableCell>
                                    <TableCell>Duration (Years)</TableCell>
                                    <TableCell>Activated On</TableCell>
                                    <TableCell>Expires On</TableCell>
                                    <TableCell>Action</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {licenseKeys && licenseKeys.map((licenseKey, index) => (
                                    <TableRow key={index}>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{licenseKey.LicenseString}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{convertDate(licenseKey.RegisteredDate)}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{licenseKey.MaxBranchLocations}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{licenseKey.MaxKiosks}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{licenseKey.DurationInYears}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>
                                            {!!licenseKey.ActivatedDateLabel?licenseKey.ActivatedDateLabel:
                                                <Button 
                                                    size="small"  
                                                    disabled={false}
                                                    className={customClass.removeBoxShadow}
                                                    variant="contained" 
                                                    color="primary" 
                                                    style={{ backgroundColor: 'rgb(46,46,66)' }}
                                                    onClick={() => {
                                                        if(licenseKey.LicenseString) {
                                                            dispatch(getLicenseKeyDetailRequest(licenseKey.LicenseString))
                                                        }
                                                    }}
                                                >
                                                Activate
                                                </Button>
                                            }
                                        </TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}>{licenseKey.ExpirationDateLabel}</TableCell>
                                        <TableCell className={licenseKey.IsLicenseExpired?customClass.disabled:''}><ScreendoxDeleteButton onClickHandler={() => deleteConfirmModal(licenseKey.LicenseString)}></ScreendoxDeleteButton></TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>   
                        </Table>
                    </TableContainer>}
                </Grid>
                <Grid item sm={12}>
                   <Grid item sm={12}>
                        <LicenseKeysSummaryTitle>Total System Summary</LicenseKeysSummaryTitle>
                   </Grid>
                   <Grid item sm={12}>
                        <LicenseKeysSummaryDescription>Check-In record count: <b>{summary?.CheckInRecordCount}</b></LicenseKeysSummaryDescription>
                   </Grid>    
                   <Grid item sm={12}>
                        <LicenseKeysSummaryDescription>Branch Location count: <b>{summary?.BranchLocationCount}</b></LicenseKeysSummaryDescription>
                   </Grid>    
                   <Grid item sm={12}>
                        <LicenseKeysSummaryDescription>Device count: <b>{summary?.KioskCount}</b></LicenseKeysSummaryDescription>
                   </Grid>                             
                </Grid>
            </Grid>
            <LicenseKeysMiniModal
                uniqueKey={EModalWindowKeys.licenseKeysDelete}
                content={<DeleteLicenseKeys />}
                actions={<></>}
                title=""
                onConfirm={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.licenseKeysDelete));
                }}
            />
            <LicenseKeysMiniModal
                uniqueKey={EModalWindowKeys.licenseKeysCreate}
                content={<CreateLicenseKey />}
                actions={<></>}
                title=""
                onConfirm={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.licenseKeysCreate));
                }}
            /> 
            <ScreendoxModal
                uniqueKey={EModalWindowKeys.licenseKeysActivate}
                content={<ActivateLicenseKey />}
                actions={<></>}
                title=""
                onConfirm={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.licenseKeysActivate));
                }}
            />                                   
        </ContentContainer>
    )   
}

export default LicenseKeysList;