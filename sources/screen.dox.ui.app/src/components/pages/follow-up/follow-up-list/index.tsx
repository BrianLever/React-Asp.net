import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ERouterUrls } from '../../../../router';
import { ContentContainer, TitleText } from '../../styledComponents';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';
import { 
  getFollowUpActiveDirectionSelector, getFollowUpActivePageSelector, getFollowUpActiveSortKeySelector, 
  getFollowUpListTotalSelector, getListFollowUpSelector, isFollowUpLoadingSelector, getFollowUpRelatedReportObjectSelector,
} from '../../../../selectors/follow-up';
import { 
  IFollowUpResponseItem, changeFollowUpActiveSortObject, postFollowUpPageChangeRequest, getFollowUpRelatedReportRequest, changeAutoUpdateStatus, changeAutoUpdateStatusRequest
} from '../../../../actions/follow-up';
import ScreendoxCustomTable from 'components/UI/table/custom-table';


const FollowUpList = (): React.ReactElement => {

  const [ selectedResultId, setSelectedResultId ] = React.useState(0);

  const dispatch = useDispatch();
  const followUpList = useSelector(getListFollowUpSelector);
  const isLoading = useSelector(isFollowUpLoadingSelector);
  const sortKey = useSelector(getFollowUpActiveSortKeySelector);
  const totalItems = useSelector(getFollowUpListTotalSelector);
  const currentPage = useSelector(getFollowUpActivePageSelector);
  const reportsObject = useSelector(getFollowUpRelatedReportObjectSelector);
  const sortDirection = useSelector(getFollowUpActiveDirectionSelector);
  
  React.useEffect(() => {
    dispatch(changeAutoUpdateStatusRequest());
  }, [])

  return (
      <ContentContainer>
        <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={4} style={{ textAlign: 'center' }}>
            <TitleText>
              Follow-Up List
            </TitleText>
          </Grid>
        </Grid>
        <ScreendoxCustomTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          onPageClick={(page: number) => dispatch(postFollowUpPageChangeRequest(page))}
          currentPage={currentPage}
          isAutoRefresh={true}
          isPagination={true}
          selectedItemValue={selectedResultId}
          onSelectHandler={(v: number) => {
            if (v === selectedResultId) {
              setSelectedResultId(0);
            } else {
              setSelectedResultId(v);
            }
          }}
          onTriggerAutoUpdate={() => {
            dispatch(changeAutoUpdateStatusRequest());
          }}
          headers={
            [
              { 
                name: 'Patient Name',
                key: 'FullName',
                action: changeFollowUpActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Date of Birth',
                key: 'Birthday',
                action: changeFollowUpActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Follow-Up Date',
                key: 'LastFollowUpDate',
                action: changeFollowUpActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Visit Date',
                key: 'LastVisitDate',
                action: changeFollowUpActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Completed',
                key: 'LastCompleteDate',
                action: changeFollowUpActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
            ]
          }
          columnWidths={[
            '35%', '15%', '15%', '15%', '20%'
          ]}
          rows={followUpList.map((r: IFollowUpResponseItem) => {
            const innerObject = reportsObject && reportsObject[r.ScreeningResultID];
            return { 
              PatientName: r.PatientName,
              Birthday: r.Birthday ? convertDateToStringFormat(r.Birthday,  "MM/DD/YYYY") : '',
              LastFollowUpDate: r.LastFollowUpDateLabel,
              LastVisitDate: r.LastVisitDateLabel,
              LastCompleteDate: r.LastCompleteDateLabel,
              id: r.ScreeningResultID,
              onSelectItem: () => {
                if (r.ScreeningResultID && selectedResultId !== r.ScreeningResultID) {
                  const action = getFollowUpRelatedReportRequest(r.ScreeningResultID);
                  dispatch(action);
                }
              },
              customStyleObject: {
                PatientName: {
                  width: '35%',
                },
                Birthday: {
                  width: '15%',
                },
                LastFollowUpDate: {
                  width: '15%',
                },
                LastVisitDate: {
                  width: '15%',
                },
                LastCompleteDate: {
                  width: '20%',
                },
              },
              subItem: !!innerObject ? innerObject.map(d => {
                return { 
                  route: {
                    name: 'Follow-Up Report',
                    link: ERouterUrls.FOLLOW_UP_REPORT.replace(':reportId', `${d.ID}`),
                  },
                  LastFollowUpDate: r.LastFollowUpDateLabel,
                  LastVisitDate: r.LastVisitDateLabel,
                  LastCompleteDate: r.LastCompleteDateLabel,
                  customStyleObject: {
                    route: {
                      width: '50%',
                    },
                  
                    LastFollowUpDate: {
                      width: '15%',
                    },
                    LastVisitDate: {
                      width: '15%',
                    },
                    LastCompleteDate: {
                      width: '20%',
                    },
                  }
                }
              }): []
              }
          })}
        />
      </ContentContainer>
  )
}

export default FollowUpList;
