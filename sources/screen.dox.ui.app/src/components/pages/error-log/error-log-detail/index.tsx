import React, { useEffect } from 'react';
import { Grid, CircularProgress } from '@material-ui/core';
import { TitleTextModal , DescriptionText } from '../../styledComponents';
import { useSelector, useDispatch  } from 'react-redux';
import { IErrorLogListResponseItem } from 'actions/error-log';
import { getErrorLogItemSelector, isErrorLogDetailLoadingSelector } from 'selectors/error-log';
import customClasss from  '../../pages.module.scss';

const ErrorLogDetail = (): React.ReactElement => {
    const dispatch = useDispatch();
    const errorLogDetail: IErrorLogListResponseItem = useSelector(getErrorLogItemSelector);
    const isLoading: boolean = useSelector(isErrorLogDetailLoadingSelector);

    return (
        <div>
            {isLoading?<CircularProgress disableShrink={false} className={customClasss.circularLoadingStyle}/>: 
            <Grid container spacing={1}>    
                <Grid item sm={12}>
                    <TitleTextModal>ID</TitleTextModal>
                </Grid>
                <Grid item sm={12}>
                    <DescriptionText>{errorLogDetail.ErrorLogID}</DescriptionText>
                </Grid>
                <Grid item sm={12}>
                    <TitleTextModal>Created Date/Time</TitleTextModal>
                </Grid>
                <Grid item sm={12}>
                    <DescriptionText>{errorLogDetail.CreatedDateFormatted}</DescriptionText>
                </Grid>
                <Grid item sm={12}>
                    <TitleTextModal>Device</TitleTextModal>
                </Grid>
                <Grid item sm={12}>
                    <DescriptionText>{errorLogDetail.KioskID}</DescriptionText>
                </Grid>
                <Grid item sm={12}>
                    <TitleTextModal>Message</TitleTextModal>
                </Grid>
                <Grid item sm={12}>
                    <DescriptionText>{errorLogDetail.ErrorMessage}</DescriptionText>
                </Grid>
                <Grid item sm={12}>
                    <TitleTextModal>Stack Trace</TitleTextModal>
                </Grid>
                <Grid item sm={12} style={{ overflowY: 'scroll' }}>
                    <DescriptionText style={{ height: 200 }}>{errorLogDetail.ErrorTraceLog}</DescriptionText>
                </Grid>
            </Grid>}
        </div>
    )
}

export default ErrorLogDetail;