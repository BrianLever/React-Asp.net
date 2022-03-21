import { getScreeningResultReportsBySortRequest, IScreeningReportResultsBySortItem, changeReportsActiveSortObject, getScreeningResultReportsBySortAutoUpdateRequest, getInternalReportsListItemDataRequest } from 'actions/reports';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getInnerReportsBySortSelector, getReportsCurrentPageSelector, getReportSortDirectionSelector, getReportSortKeySelector, getScreeningResultReportsBySortSelector, isInnerReportsBySortLoadingErrorSelector, isInnerReportsBySortLoadingSelector, isReportAutoRefreshStatusSelector, isReportLoadingSelector } from 'selectors/reports';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { Grid } from '@material-ui/core';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { convertDateToStringFormat } from 'helpers/dateHelper';
import { ERouterUrls } from 'router';
import { commonTextFontStyle } from 'components/UI/typography';
import { getScreenListReportObjectSelector } from 'selectors/screen';
import { postScreenListItemRequest } from 'actions/screen';
import ScreendoxCustomTable from 'components/UI/table/custom-table';

const ReportsBySortLists = (): React.ReactElement => {
      
    const dispatch = useDispatch();
    const reports: Array<IScreeningReportResultsBySortItem> = useSelector(getScreeningResultReportsBySortSelector);
    const isLoading: boolean = useSelector(isReportLoadingSelector);
    const currentPage: number = useSelector(getReportsCurrentPageSelector);
    const sortKey: string = useSelector(getReportSortKeySelector);
    const sortDirection: string = useSelector(getReportSortDirectionSelector);
    const [selectedId, setSelectedId] = useState(0);
    const isAutoStatus: boolean = useSelector(isReportAutoRefreshStatusSelector);
    const reportsObject = useSelector(getInnerReportsBySortSelector);
    const isInnerReportsLoading: boolean = useSelector(isInnerReportsBySortLoadingSelector);
    const isInnerReportsLoadingError: boolean = useSelector(isInnerReportsBySortLoadingErrorSelector);

    useEffect(() => {
        dispatch(getScreeningResultReportsBySortRequest());
        dispatch(getScreeningResultReportsBySortAutoUpdateRequest());
    }, [dispatch])
    
    const cellComponent = (text: string, isPositive: boolean = false): any => {
        return isPositive ? <span style={{ ...commonTextFontStyle, color: '#dc004e'  }}> { text }</span> : (<span style={commonTextFontStyle}> { text }</span>);
    }
    
    return (
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={4} style={{ textAlign: 'center' }}>
                    <TitleText>
                        Health Indicator Report
                    </TitleText>
                    <TitleTextModal>
                        Screening Results By Sort
                    </TitleTextModal>
                </Grid>
            </Grid>
            <ScreendoxCustomTable
                isLoading={isLoading}
                type={EScreendoxTableType.screenList}
                total={reports.length}
                onPageClick={(page: number) => console.log('d')

                }
                currentPage={currentPage}
                isAutoRefresh={isAutoStatus}
                isPagination={true}
                selectedItemValue={selectedId}
                onSelectHandler={(v: number) => {
                    setSelectedId(v);
                }}
                onTriggerAutoUpdate={() => {
                    dispatch(getScreeningResultReportsBySortAutoUpdateRequest());
                }}
                headers={
                    [
                    { 
                        name: 'Patient Name',
                        key: 'FullName',
                        action: changeReportsActiveSortObject,
                        active: sortKey,
                        direction: sortDirection,
                    },
                    { 
                        name: 'Date of Birth',
                        key: 'Birthday',
                        action: changeReportsActiveSortObject,
                        active: sortKey,
                        direction: sortDirection,
                    },
                    { 
                        name: 'CHECK-IN TIME',
                        key: 'check_in_time',
                        action: changeReportsActiveSortObject,
                        active: sortKey,
                        direction: sortDirection,
                    },

                    ]
                }
                columnWidths={[' 35%', '30%', '35%']}
                rows={reports.map((r: IScreeningReportResultsBySortItem, index: number) => {
                    const innerObject = reportsObject && reportsObject[r.ReportItems[0].ScreeningResultID];
                    return { 
                    id: index,
                    PatientName: r.PatientName,
                    Birthday: r.Birthday ? convertDateToStringFormat(r.Birthday,  "MM/DD/YYYY") : '',
                    LastCreateDate: r.LastCreatedDate?convertDateToStringFormat(r.LastCreatedDate,  "MM/DD/YYYY"):'',
                    onSelectItem: () => {
                        if (r.ReportItems[0].ScreeningResultID) {
                            const action = getInternalReportsListItemDataRequest(r.ReportItems[0].ScreeningResultID);
                            dispatch(action);
                        }
                    },
                    customStyleObject: {
                        PatientName: {
                        width: '35%',
                        },
                        Birthday: {
                        width: '30%',
                        },
                        LastCreateDate: {
                        width: '35%',
                        },
                    },
                    subItem: !!innerObject ? innerObject.map(d => {
                        return { 
                            route: {
                                name: cellComponent('Screen Report', d.IsPositive),
                                link: ERouterUrls.SCREENING_REPORTS.replace(':reportId', `${d.ScreeningResultID}`),
                            },
                            Birthday: '',
                            LastCreateDate: d.CreatedDate ? cellComponent(convertDateToStringFormat(d.CreatedDate,  "MM/DD/YYYY"), d.IsPositive):'',
                            customStyleObject: {
                                route: {
                                    width: '65%',
                                },
                                LastCreateDate: {
                                    width: '35%'
                                }
                            }
                        }
                    }): []
                    }
                })}
            />
        </ContentContainer>
    )
  }
  
export default ReportsBySortLists;
  