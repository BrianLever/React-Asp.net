import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import ScreendoxCustomTable from 'components/UI/table/custom-table';
import { ContentContainer, TitleText } from '../styledComponents';
import { 
  getTotalVisitsSelector, getVisitItemsSelector, getVisitListActiveSortKeySelector, isVisitLoadingSelector, 
  visitListCurrentPageSelector, getVisitListActiveSortDirectionSelector, getVisitDescriptiveReport, isVisitListErrorSelector
} from '../../../../selectors/visit';
import { 
  IVisitReportsResponse, IVisitResponseItem, requestVisitCurrentPage, requestVisitListSort, changeAutoUpdateStatusRequest,
  getRelatedByIdVisitRequest
} from '../../../../actions/visit';
import { getVisitReportRequestSuccess } from '../../../../actions/visit/report';
import { getVisitDemographicReportRequestSuccess } from '../../../../actions/visit/demographic-report';
import { ERouterUrls } from '../../../../router';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';

const VisitListComponent = (): React.ReactElement => {

  const [selectedId, setSelectedId] = React.useState(0);

  const dispatch =            useDispatch();
  const sortKey =             useSelector(getVisitListActiveSortKeySelector);
  const isLoading =           useSelector(isVisitLoadingSelector);
  const visitList =           useSelector(getVisitItemsSelector);
  const totalItems =          useSelector(getTotalVisitsSelector);
  const currentPage =         useSelector(visitListCurrentPageSelector);
  const sortDirection =       useSelector(getVisitListActiveSortDirectionSelector);
  const descriptReportArray = useSelector(getVisitDescriptiveReport);
  const isVisitListError =    useSelector(isVisitListErrorSelector);
console.log(totalItems, 'visit total items')
  React.useEffect(() => {
    dispatch(requestVisitCurrentPage(1));
    dispatch(getVisitReportRequestSuccess(null));
    dispatch(getVisitDemographicReportRequestSuccess(null));
    dispatch(changeAutoUpdateStatusRequest())
  }, [dispatch]);

  return (
      <ContentContainer>
        <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={4} style={{ textAlign: 'center' }}>
            <TitleText>
              Visit List
            </TitleText>
          </Grid>
        </Grid>
        <ScreendoxCustomTable
          isLoading={isLoading}
          isFaild={isVisitListError}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          onPageClick={(page: number) => {
            dispatch(requestVisitCurrentPage(page))
          }}
          onTriggerAutoUpdate={() => {
            dispatch(changeAutoUpdateStatusRequest())
          }}
          currentPage={currentPage}
          isAutoRefresh={true}
          isPagination={true}
          selectedItemValue={selectedId}
          onSelectHandler={(v: number) => {
            if (v === selectedId) {
              setSelectedId(0);
            } else {
              setSelectedId(v);
              dispatch(getRelatedByIdVisitRequest(v));
            }
          }}
          headers={
            [
              { 
                name: 'Patient Name',
                key: 'FullName',
                action: requestVisitListSort,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Date of Birth',
                key: 'Birthday',
                action: requestVisitListSort,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Report Date',
                key: 'LastCompleteDate',
                action: requestVisitListSort,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Location',
                key: 'LocationName',
                action: requestVisitListSort,
                active: sortKey,
                direction: sortDirection,
              },
            ]
          }
          columnWidths={[
            '45%',
            '20%',
            '20%',
            '15%',
          ]}
          rows={visitList.map((r: IVisitResponseItem) => {
            return { 
              PatientName: r.PatientName,
              Birthday: r.Birthday ? convertDateToStringFormat(r.Birthday, "MM/DD/YYYY") : '',
              LastCreatedDate : r.LastCreatedDateLabel,
              LocationName: r.LocationName ?? "",
              id: r.ScreeningResultID,
              onSelectItem: () => {},
              customStyleObject: {
                PatientName: {
                  flexBasis: 'calc(45% - 0rem)'
                },
                Birthday: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                LastCreatedDate: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                LocationName: {
                  flexBasis: 'calc(15% - 0rem)'
                },
              },
              subItem: (descriptReportArray.length && !isVisitListError) ? descriptReportArray.map((report: IVisitReportsResponse, index: number) => ({
                route: {
                  name: report.ReportName,
                  link: (index === 0) ? 
                    ERouterUrls.VISIT_DEMOGRAPHIC_REPORT.replace(':reportId', `${report.ID}`) : 
                    ERouterUrls.VISIT_REPORTS.replace(':reportId', `${report.ID}`),
                },
                CreatedDate: report.CreatedDateLabel,
                LocationName: report.LocationName ? report.LocationName : '',
                customStyleObject: {
                  route: {
                    flexBasis: 'calc(65% - 0rem)'
                  },
                  CreatedDate: {
                    flexBasis: 'calc(20% - 0rem)'
                  },
                  LocationName: {
                    flexBasis: 'calc(15% - 0rem)'
                  },
                },
              })) : [],
              }
          })}
        />
      </ContentContainer>
  )
}

export default VisitListComponent;