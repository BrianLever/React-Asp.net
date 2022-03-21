import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { ContentContainer, useStyles  } from '../../styledComponents';
import {
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Switch, Button } from '@material-ui/core';
import customClass from '../../pages.module.scss';
import { TitleText } from 'components/UI/table/styledComponents';
import { DescriptionText, ButtonTextStyle } from '../../styledComponents';
import ScreendoxModal, { LicenseKeysMiniModal, ScreenProfileEditModal, EhrExportModal, ManageUsersModal } from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import { IEhrLoginResponseItem, setEhrLoginSelectedId } from 'actions/ehr-login';
import { getEhrLoginListSelector, isEhrLoginListLoadingSelector } from 'selectors/ehr-login';
import ScreendoxDeleteButton from 'components/UI/custom-buttons/deleteButton';
import EhrLoginCreate from '../ehr-login-create';
import EhrLoginCreateActions from '../ehr-login-create-actions';
import EhrLoginDelete from '../ehr-login-delete';
import CustomAlert from 'components/UI/alert';


const EhrLoginList = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const classes = useStyles();
    const ehrLoginList: IEhrLoginResponseItem[] = useSelector(getEhrLoginListSelector);
    const isLoading: boolean = useSelector(isEhrLoginListLoadingSelector);

    useEffect(() => {

    }, []);

    return (
        <ContentContainer>
            <CustomAlert />
            <Grid container spacing={4} style={{ marginBottom: 4 }}>
                <Grid item sm={8}>
                    <TitleText>EHR Credentials For Export</TitleText>
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
                            dispatch(openModalWindow(EModalWindowKeys.ehrLoginCreate));
                        }}
                    >
                        <ButtonTextStyle>
                            + Enter New EHR Credentials
                        </ButtonTextStyle>
                    </Button>
                </Grid>
            </Grid>
            <Grid container spacing={1}>
                <Grid item sm={12}>
                {isLoading?<CircularProgress disableShrink={false} className={customClass.circularLoadingStyle}/>:
                    <TableContainer>
                        <Table>
                            <TableHead className={customClass.tableHead} style={{ fontSize: 14 }}>
                                <TableRow>
                                    <TableCell>Access Code (User Name)</TableCell>
                                    <TableCell>Verify Code (Password)</TableCell>
                                    <TableCell>Expires On</TableCell>
                                    <TableCell>Action</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody className={customClass.tableBody} style={{ fontSize: 14 }}>
                                {ehrLoginList && ehrLoginList.map((ehrLogin, index) => (
                                    <TableRow key={index}>
                                        <TableCell className={!ehrLogin.IsActive?customClass.disabled:''}>{ehrLogin.AccessCode}</TableCell>
                                        <TableCell className={!ehrLogin.IsActive?customClass.disabled:''}>{ehrLogin.VerifyCode}</TableCell>
                                        <TableCell className={!ehrLogin.IsActive?customClass.disabled:''}>{ehrLogin.ExpireAtFormatted}</TableCell>
                                        <TableCell className={!ehrLogin.IsActive?customClass.disabled:''}>
                                            <ScreendoxDeleteButton 
                                            onClickHandler={() => {
                                                dispatch(setEhrLoginSelectedId(ehrLogin.Id))
                                                dispatch(openModalWindow(EModalWindowKeys.ehrLoginDelete));
                                            }}></ScreendoxDeleteButton>
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>   
                        </Table>
                    </TableContainer>}
                </Grid>
            </Grid>

            <LicenseKeysMiniModal
                uniqueKey={EModalWindowKeys.ehrLoginDelete}
                content={<EhrLoginDelete />}
                actions={<></>}
                title=""
                onConfirm={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.ehrLoginDelete));
                }}
            />

            <ScreendoxModal
                uniqueKey={EModalWindowKeys.ehrLoginCreate}
                content={<EhrLoginCreate />}
                actions={<EhrLoginCreateActions />}
                title="New EHR Credentials"
                onConfirm={() => {
                    dispatch(closeModalWindow(EModalWindowKeys.ehrLoginCreate));
                }}
            />                                   
        </ContentContainer>
    )   
}

export default EhrLoginList;