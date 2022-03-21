import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { 
  screenListSelector, screenListTotalItemsSelector, screenListCurrentPageSelector, 
  isScreenListActiveSortSelector, isScreenListActiveSortDirectionSelector,
  getScreenListReportObjectSelector, isScreenListLoadingSelector, getScreeningID,
  isScreenListAutoUpdateSelector,
  getScreenListEhrExportPatientInfoSelector
} from '../../../../selectors/screen';
import { Button, Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { 
  changeActiveSortObject, getScreenListEhrExportPatientInfoRequest, IScreenListResponseItem, postScreenListFilterRequest, 
  postScreenListFilterRequestAutoUpdate, postScreenListItemRequest, setEhrExportPatientRecordSelectedId, setEhrExportSelectedTab, setEhrExportVisitRecordSelectedId, setRequestScreendoxId 
} from '../../../../actions/screen';
import { ERouterUrls } from '../../../../router';
import { ButtonText } from '../styledComponents';
import { ContentContainer, TitleText } from '../../styledComponents';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';
import { commonTextFontStyle } from 'components/UI/typography';
import { EhrExportModal, EhrExportScreenDoxInformationModal } from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import EhrExport from '../ehr-export';
import ScreendoxCustomTable from 'components/UI/table/custom-table';

const ScreenList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const screenList = useSelector(screenListSelector);
  const isLoading = useSelector(isScreenListLoadingSelector);
  const sortKey = useSelector(isScreenListActiveSortSelector);
  const totalItems = useSelector(screenListTotalItemsSelector);
  const currentPage = useSelector(screenListCurrentPageSelector);
  const reportsObject = useSelector(getScreenListReportObjectSelector);
  const sortDirection = useSelector(isScreenListActiveSortDirectionSelector);
  const screeningId: number | null = useSelector(getScreeningID);
  const isAutoUpdate: boolean = useSelector(isScreenListAutoUpdateSelector);

  React.useEffect(() => {
    // dispatch(postScreenListFilterRequest({ page: 1 }));
    // dispatch(postScreenListFilterRequestAutoUpdate());
  }, []);

  const cellComponent = (text: string, isPositive: boolean = false): any => {
    
    return isPositive ? <span style={{ ...commonTextFontStyle, color: '#dc004e'  }}> { text }</span> : (<span style={commonTextFontStyle}> { text }</span>);
  }

  return (
      <ContentContainer>
        <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={4} style={{ textAlign: 'center' }}>
            <TitleText>
              Screen List
            </TitleText>
          </Grid>
        </Grid>
        <ScreendoxCustomTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          onPageClick={(page: number) => dispatch(postScreenListFilterRequest({ page }))}
          currentPage={currentPage}
          isAutoRefresh={isAutoUpdate}
          isPagination={true}
          selectedItemValue={screeningId}
          onSelectHandler={(v: number) => {
            const value = screeningId === v ? null : v;
            const action = setRequestScreendoxId(value);
            dispatch(action);
          }}
          onTriggerAutoUpdate={() => {
            dispatch(postScreenListFilterRequestAutoUpdate());
          }}
          headers={
            [
              { 
                name: 'Patient Name',
                key: 'FullName',
                action: changeActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Date of Birth',
                key: 'Birthday',
                action: changeActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Export Date/Time/HRN',
                key: 'ExportDate',
                action: changeActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Screen Date/Time',
                key: 'LastCheckinDate',
                action: changeActiveSortObject,
                active: sortKey,
                direction: sortDirection,
              },
            ]
          }
          columnWidths={[
            '45%', '15%', '20%', '20%'
          ]}
          rows={ screenList.map((r: IScreenListResponseItem) => {
            const innerObject = reportsObject && reportsObject[r.ScreeningResultID];
            return { 
              PatientName: r.PatientName,
              Birthday: convertDateToStringFormat(r.Birthday,  "MM/DD/YYYY"),
              ExportDate: r.ExportDate ? r.ExportDateLabel : (
                <Button
                  variant="contained"
                  style={{ backgroundColor: '#2e2e42' }}
                  size="medium"
                  startIcon={<img width="17px" height="20px" src="../assets/export.svg" alt="export.svg"/>}
              >
                <ButtonText>
                  Export
                </ButtonText>
              </Button>),
              LastCheckinDate: r.LastCheckinDateLabel,
              id: r.ScreeningResultID,
              customStyleObject: {
                PatientName: {
                  flexBasis: 'calc(45% - 0rem)'
                },
                Birthday: {
                  flexBasis: 'calc(15% - 0rem)'
                },
                ExportDate: {
                  flexBasis: 'calc(20% - 0rem)'
                },
                LastCheckinDate: {
                  flexBasis: 'calc(20% - 0rem)'
                },
              },
              onSelectItem: () => {
                if (r.ScreeningResultID && screeningId !== r.ScreeningResultID) {
                  const action = postScreenListItemRequest(r.ScreeningResultID);
                  dispatch(action);
                }
              },
              subItem: !!innerObject ? innerObject.map(d => {
                return { 
                  route: {
                    name: cellComponent('Screen Report', d.IsPositive),
                    link: ERouterUrls.SCREENING_REPORTS.replace(':reportId', `${d.ScreeningResultID}`),
                  },
                  ExportDateLabel: d.ExportDateLabel ? cellComponent(d.ExportDateLabel+', '+d.ExportedToHRN, d.IsPositive) : (
                    <Button
                      variant="contained"
                      style={{ backgroundColor: '#2e2e42' }}
                      size="medium"
                      startIcon={<img width="17px" height="20px" src="../assets/export.svg" alt="export.svg"/>}
                      onClick={() => { 
                        dispatch(getScreenListEhrExportPatientInfoRequest(d.ScreeningResultID));
                        dispatch(openModalWindow(EModalWindowKeys.screenListSelectEHRRecord));
                      }}
                    >
                      <ButtonText>
                        Export
                      </ButtonText>
                    </Button>),
                  CreatedDateLabel: d.CreatedDateLabel ? cellComponent(d.CreatedDateLabel, d.IsPositive) : '',
                  customStyleObject: {
                    route: {
                      flexBasis: 'calc(60% - 0rem)'
                    },
                    ExportDateLabel: {
                      flexBasis: 'calc(20% - 0rem)'
                    },
                    CreatedDateLabel: {
                      flexBasis: 'calc(20% - 0rem)'
                    },
                  },
                }
              }): []
              }
            })}
        />
        <EhrExportModal
          uniqueKey={EModalWindowKeys.screenListSelectEHRRecord}
          content={<EhrExport />}
          actions={<div />}
          title="Select EHR Record"
          onConfirm={() => {
            dispatch(closeModalWindow(EModalWindowKeys.screenListSelectEHRRecord));
            dispatch(setEhrExportVisitRecordSelectedId(null));
            dispatch(setEhrExportPatientRecordSelectedId(null));
            dispatch(setEhrExportSelectedTab(0));
          }}
        />
      </ContentContainer>
  )
}

export default ScreenList;
