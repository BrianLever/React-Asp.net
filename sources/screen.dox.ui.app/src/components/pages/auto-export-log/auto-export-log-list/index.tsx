import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Button, Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ContentContainer, TitleText, TopSectionLabel } from '../../styledComponents';
import classes from  '../../pages.module.scss';
import { useHistory } from 'react-router-dom';
import { changeAutoExportLogsCurrentPageRequest, getAutoExportLogsRequest, IAutoExportLogsResponseItem } from 'actions/auto-export-logs';
import { getAutoExportLogsCurrentPageSelector, getAutoExportLogsSelector, getAutoExportLogsStatisticsSelector, getAutoExportLogsTotalSelector, isAutoExportLogsLoadingSelector } from 'selectors/auto-export-logs';
import { convertDate, convertDateToStringFormat } from 'helpers/dateHelper';
import CustomAlert from 'components/UI/alert';


const AutoExportLogList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const history = useHistory();
  const autoExportLogs = useSelector(getAutoExportLogsSelector);
  const isLoading = useSelector(isAutoExportLogsLoadingSelector);
  const currentPage = useSelector(getAutoExportLogsCurrentPageSelector);
  const total = useSelector(getAutoExportLogsTotalSelector);
  const statistics = useSelector(getAutoExportLogsStatisticsSelector);

  React.useEffect(() => {
    dispatch(getAutoExportLogsRequest());
  }, [dispatch]);

  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container justifyContent="flex-start" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={8} style={{ textAlign: 'start' }}>
            <TitleText>
               Auto Export Log
            </TitleText>
          </Grid>
          <Grid item xs={4} style={{ textAlign: 'start' }}>
            <div className={classes.statistacsContainerStyle}>
              <Grid item xs={12} ><TopSectionLabel>Auto-Export Statistics</TopSectionLabel></Grid>
              <Grid container xs={12} className={classes.statistacsItemStyle}>
                <Grid item xs={8}>Succeed exports:</Grid>
                <Grid item xs={4}>{statistics.Succeed}</Grid>
              </Grid>
              <Grid container xs={12} className={classes.statistacsItemStyle}>
                <Grid item xs={8}>Failed exports:</Grid>
                <Grid item xs={4}>{statistics.Failed}</Grid>
              </Grid>
              <Grid container xs={12} className={classes.statistacsItemStyle}>
                <Grid item xs={8}>Total exports:</Grid>
                <Grid item xs={4}>{statistics.Total}</Grid>
              </Grid>
            </div>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={total}
          isCollapsable="NO"
          onPageClick={(page: number) => {
            dispatch(changeAutoExportLogsCurrentPageRequest(page));
          }}
          currentPage={currentPage}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={ () => console.log('Item selected.')}
          onTriggerAutoUpdate={() => {
            
          }}
          headers={
            [
              { 
                name: 'Date/Time',
                key: 'Date/Time',
              },
              { 
                name: 'EHR Name',
                key: 'EHR Name',
              },
              { 
                name: 'EHR Date Of Birth',
                key: 'EHR Date Of Birth',
              },
              { 
                name:  'Initial Name',
                key: 'Initial Name',
              },
          
              { 
                name:  'Initial Date Of Birth',
                key: 'Initial Date Of Birth',
              },
              {
                name: 'Comments',
                key: 'Comments',
              }
            ]
          }
          rows={autoExportLogs.map((r: IAutoExportLogsResponseItem) => {
            return { 
                Create_date: convertDateToStringFormat(r.CreatedDate),
                name: r.CorrectedPatientName,
                birthday: convertDate(r.CorrectedBirthday),
                origin_name: r.OriginalPatientName,
                origin_birthday: convertDate(r.OriginalBirthday),
                comments: r.Comments,
                onSelectItem: () => { console.log('Screendox.') },
                customStyleObject: {
                
                },
                subItem: [],
                }
          })}
        />       
      </ContentContainer>
  )
}

export default AutoExportLogList;
