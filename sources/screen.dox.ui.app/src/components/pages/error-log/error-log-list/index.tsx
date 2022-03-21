import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AddIcon from '@material-ui/icons/Add';
import { Button, Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ContentContainer, TitleText } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import ScreendoxModal from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import ScreendoxEditButton from '../../../UI/custom-buttons/editButton';
import { useHistory } from 'react-router-dom';
import { getErrorLogCurrentPageSelector, getErrorLogsSelector, getErrorLogTotalCountSelector, isErrorLogLoadingSelector } from 'selectors/error-log';
import { changeErrorLogCurrentPageRequest, getErrorLogByIdRequest, getErrorLogRequest, IErrorLogListResponseItem } from 'actions/error-log';
import ErrorLogDetail from '../error-log-detail';
import { ButtonTextStyle } from '../../styledComponents';
import { ReportIcon } from '../../styledComponents';
import CustomAlert from 'components/UI/alert';



const ErrorLogList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const history = useHistory();
  const errorLogs = useSelector(getErrorLogsSelector);
  const totalErrorLogs = useSelector(getErrorLogTotalCountSelector);
  const isLoading = useSelector(isErrorLogLoadingSelector);
  const currentPage = useSelector(getErrorLogCurrentPageSelector);
  console.log(errorLogs);

  React.useEffect(() => {
    dispatch(getErrorLogRequest());
  }, [dispatch]);

  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container justifyContent="flex-start" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={6} style={{ textAlign: 'start' }}>
            <TitleText>
                Error Log
            </TitleText>
          </Grid>
          <Grid item xs={6} style={{ textAlign: 'end' }}>
            <Button 
                size="large" 
                className={classes.removeBoxShadow}
                variant="contained" 
                color="primary" 
                style={{ backgroundColor: '#2e2e42', border: '1px solid #2e2e42' }}
                onClick={() => {
                   
                }}
            >
                 <ButtonTextStyle>Link To Central Logging</ButtonTextStyle> 
            </Button>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalErrorLogs}
          isCollapsable="NO"
          onPageClick={(page: number) => {
            dispatch(changeErrorLogCurrentPageRequest(page));
          }}
          currentPage={currentPage}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={ () => console.log('Screendox.')}
          onTriggerAutoUpdate={() => {
            
          }}
          headers={
            [
              { 
                name: 'Date/Time',
                key: 'CREATED DATE',
              },
              { 
                name: 'Device',
                key: 'Device',
              },
              { 
                name: 'Error Message',
                key: 'ERROR MESSAGE',
              },
              { 
                name:  'View',
                key: 'View',
              },
            ]
          }
          rows={errorLogs.map((r: IErrorLogListResponseItem) => {
            return { 
                id: r.ErrorLogID,
                Create_date: r.CreatedDateFormatted,
                Kiosk: r.KioskID,
                Error_message: r.ErrorMessage,
                Action: (
                  <ReportIcon
                    onClick={e => {
                      e.stopPropagation();
                      if (r.ErrorLogID) {
                        dispatch(getErrorLogByIdRequest(r.ErrorLogID));
                      }
                    }}
                  />
                ),
              onSelectItem: () => { console.log('Screendox.') },
              customStyleObject: {
                
              },
              subItem: [],
              }
          })}
        />
        <ScreendoxModal
          uniqueKey={EModalWindowKeys.errorLogDetail}
          content={<ErrorLogDetail />}
          actions={<></>}
          title="Error Log Item Details"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.errorLogDetail));
          }}
        />
      </ContentContainer>
  )
}

export default ErrorLogList;
