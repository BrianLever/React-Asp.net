import React, { useEffect } from 'react';
import { Grid, CircularProgress, TextField, Button } from '@material-ui/core';
import { useSelector, useDispatch  } from 'react-redux';
import { getCssrsListCurrentPageSelector, getCssrsListFirstNameSelector, getCssrsListLastNameSelector ,getCssrsListLocationIdSelector, getCssrsListSelector, getCssrsListSortDirectionSelector, getCssrsListSortKeySelector, getCssrsListTotalItem, getCssrsVisitReportSelector, isCssrsListLoadingSelector, isCssrsVisitReportLoadingSelector } from 'selectors/c-ssrs-list';
import { ContentContainer, TitleText } from '../../styledComponents';
import { cssrsListchangeAutoUpdateStatusRequest, cssrsListCurrentPageRequest, cssrsListSortRequest, ICssrsListResponse, getRelatedByIdCssrsRequest, ICssrsVisitResponseItem } from 'actions/c-ssrs-list';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';
import customClasss from  '../../pages.module.scss';
import { getBranchLocationsRequest } from 'actions/branch-locations';
import { ERouterUrls } from 'router';
import ScreendoxCustomTable from 'components/UI/table/custom-table';
import { EScreendoxTableType } from 'components/UI/table';

const CSSRSListComponent = (): React.ReactElement => {
    
    const dispatch = useDispatch();
    const isLoading =           useSelector(isCssrsListLoadingSelector);
    const cssrslist =           useSelector(getCssrsListSelector);
    const sortKey =             useSelector(getCssrsListSortKeySelector);
    const totalItems =          useSelector(getCssrsListTotalItem);
    const currentPage =         useSelector(getCssrsListCurrentPageSelector);
    const sortDirection =       useSelector(getCssrsListSortDirectionSelector);
    const isCssrsVisitLoading = useSelector(isCssrsVisitReportLoadingSelector);
    const visitReportsArray: Array<ICssrsVisitResponseItem> = useSelector(getCssrsVisitReportSelector);
    const [selectedId, setSelectedId] = React.useState(0);    
    
    return (     
      <ContentContainer>
        <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={12} style={{ textAlign: 'center' }}>
            <TitleText>
              Columbia-Suicide Serverity Rating Scale (C-SSRS) List
            </TitleText>
          </Grid>
        </Grid>       
        <ScreendoxCustomTable
          isLoading={isLoading}
          isFaild={isCssrsVisitLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          onPageClick={(page: number) => {
            dispatch(cssrsListCurrentPageRequest(page))
          }}
          onTriggerAutoUpdate={() => {
            dispatch(cssrsListchangeAutoUpdateStatusRequest())
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
              dispatch(getRelatedByIdCssrsRequest(v));
            }
          }}
          headers={
            [
              { 
                name: 'Patient Name',
                key: 'FullName',
                action: cssrsListSortRequest,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Date of Birth',
                key: 'Birthday',
                action: cssrsListSortRequest,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Report Date',
                key: 'LastCreatedDate',
                action: cssrsListSortRequest,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Location',
                key: 'LocationName',
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Staff Name',
                key: 'StaffName',
                // active: sortKey,
                // direction: sortDirection,
              },
            ]
          }      
          columnWidths={[
            '25%', '20%', '20%', '20%','15%'
          ]} 
          rows={cssrslist.map((r: ICssrsListResponse) => {                    
            return {               
              PatientName: r.PatientName,
              Birthday: r.Birthday ? convertDateToStringFormat(r.Birthday, "MM/DD/YYYY") : '',
              LastCreatedDate : r.LastCreatedDateLabel,
              LocationName: r.LocationName ?? "",
              StaffName: r.CompletedByName ?? "",
              id: r.ID,
              onSelectItem: () => {},
              customStyleObject: {
                PatientName: {
                  flexBasis: 'calc(25% - 0rem)'
                },
                Birthday: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                LastCreatedDate: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                LocationName: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                StaffName: {
                  flexBasis: 'calc(15% - 0rem)'
                },
              },
              subItem: (visitReportsArray.length && !isCssrsVisitLoading) ? visitReportsArray.map((report: ICssrsVisitResponseItem, index: number) => ({
                route: {
                  name: report.ReportName,
                  link: ERouterUrls.CSSRS_LIFETIME_RECENT_REPORT.replace(':reportId', `${report.ID}`)
                },
                CreatedDate: report.CreatedDateLabel,
                LocationName: report.LocationName ? report.LocationName : '',
                customStyleObject: {
                  route: {
                    width: '45%',
                  },
                  CreatedDate: {
                    width: '20%',
                  },
                  LocationName: {
                    width: '35%',
                  },                  
                },
              })) : [],             
              }              
          })} 
          cssrsTable      
        />       
      </ContentContainer>
    )
}

export default CSSRSListComponent;