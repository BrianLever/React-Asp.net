import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { ERouterUrls } from '../../../../router';
import { ContentContainer, TitleText } from '../../styledComponents';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';
import { 
  getFollowUpActiveDirectionSelector, getFollowUpActivePageSelector, getFollowUpActiveSortKeySelector, 
  getFollowUpListTotalSelector, getFollowUpScreeningResultIdSelector, getListFollowUpSelector, isFollowUpLoadingSelector,
  getFollowUpRelatedReportObjectSelector,
} from '../../../../selectors/follow-up';
import { 
  IFollowUpResponseItem, changeFollowUpActiveSortObject, postFollowUpPageChangeRequest, getFollowUpRelatedReportRequest, setFollowUpScreeningID,
} from '../../../../actions/follow-up';

const FollowUpList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const followUpList = useSelector(getListFollowUpSelector);
  const isLoading = useSelector(isFollowUpLoadingSelector);
  const sortKey = useSelector(getFollowUpActiveSortKeySelector);
  const totalItems = useSelector(getFollowUpListTotalSelector);
  const currentPage = useSelector(getFollowUpActivePageSelector);
  const reportsObject = useSelector(getFollowUpRelatedReportObjectSelector);
  const sortDirection = useSelector(getFollowUpActiveDirectionSelector);
  const selectedResultId = useSelector(getFollowUpScreeningResultIdSelector);
  

  return (
      <ContentContainer>
        <Grid container justify="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={4} style={{ textAlign: 'center' }}>
            <TitleText>
              Follow-Up List
            </TitleText>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          onPageClick={(page: number) => dispatch(postFollowUpPageChangeRequest(page))}
          currentPage={currentPage}
          isAutoRefresh={true}
          isPagination={true}
          selectedItemValue={selectedResultId}
          onSelectHandler={(v: number) => {
            try {
              const value = selectedResultId === v ? null : v;
              dispatch(setFollowUpScreeningID(parseInt(`${value}`)))
            }catch(e) {}
          }}
          onTriggerAutoUpdate={() => {
            // dispatch(postScreenListFilterRequestAutoUpdate());
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
                name:  'VisitDate',
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
          rows={followUpList.map((r: IFollowUpResponseItem) => {
            const innerObject = reportsObject && reportsObject[r.ScreeningResultID];
            return { 
              PatientName: r.PatientName,
              Birthday: r.Birthday ? convertDateToStringFormat(r.Birthday,  "MM/DD/YYYY") : '',
              LastFollowUpDate: r.LastFollowUpDate ? convertDateToStringFormat(r.LastFollowUpDate) : '',
              LastVisitDate: r.LastVisitDate ? convertDateToStringFormat(r.LastVisitDate) : '',
              LastCompleteDate: r.LastCompleteDate ? convertDateToStringFormat(r.LastCompleteDate) : '',
              id: r.ScreeningResultID,
              onSelectItem: () => {
                // if (r.ScreeningResultID && selectedResultId !== r.ScreeningResultID) {
                //   const action = getFollowUpRelatedReportRequest(r.ScreeningResultID);
                //   dispatch(action);
                // }
              },
              subItem: !!innerObject ? innerObject.map(d => {
                return { 
                  route: {
                    name: 'Follow-Up Report',
                    link: ERouterUrls.FOLLOW_UP_REPORT.replace(':reportId', `${d.ID}`),
                  },
                  LastFollowUpDate: r.LastFollowUpDate ? convertDateToStringFormat(r.LastFollowUpDate) : '',
                  LastVisitDate: r.LastVisitDate ? convertDateToStringFormat(r.LastVisitDate) : '',
                  LastCompleteDate: r.LastCompleteDate ? convertDateToStringFormat(r.LastCompleteDate) : '',
                }
              }): []
              }
          })}
        />
      </ContentContainer>
  )
}

export default FollowUpList;
