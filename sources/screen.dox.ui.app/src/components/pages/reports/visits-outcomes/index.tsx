import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { convertDate} from '../../../../helpers/dateHelper';
import { useEffect } from 'react';
import {postFilteredVisitsOutcomesRequest,reportAgeGroupByAgeRequest } from '../../../../actions/reports'
import {getListVisitsOutcomesSelector, isReportLoadingSelector }  from '../../../../selectors/reports';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Grid,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText, EMPTY_LOCALTION_LIST_VALUE }  from 'helpers/general';
import { 
  getReportLocationsSelector, getReportSelectedLocationIdSelector,getReportBsrReportTypeSelector,getReportGrpaPeriodKeySelector,getReportStartDateSelector,getReportEndDateSelector,
  getListReportAgeGroupByAgeSelector
} from '../../../../selectors/reports';
import customClasss from  '../../pages.module.scss';


const VisitsOutcomesLists = (): React.ReactElement => {

    const dispatch = useDispatch();      
    useEffect(() => {
        dispatch(postFilteredVisitsOutcomesRequest());
        dispatch(reportAgeGroupByAgeRequest());
    }, [])       
    const visitsOutcomesList: any = useSelector(getListVisitsOutcomesSelector);      
    const reportAgeGroupListByAge:any=useSelector(getListReportAgeGroupByAgeSelector);
    const locations = useSelector(getReportLocationsSelector);
    const locationId = useSelector(getReportSelectedLocationIdSelector);    
    const startDate = useSelector(getReportStartDateSelector);
    const endDate = useSelector(getReportEndDateSelector);
    const isLoading = useSelector(isReportLoadingSelector);    
    const reportType = useSelector(getReportBsrReportTypeSelector);
    const currentDateStamp = new Date().toLocaleString().split(',')[0];
    const gpraPeriod = useSelector(getReportGrpaPeriodKeySelector); 
    const [displayRepotType, setDisplayReportType] = React.useState(reportType);
    const [displayLocationId, setDisplayLocationId] = React.useState(locationId);
    const [displayStartDate, setDisplayStartDate] = React.useState(startDate);
    const [displayEndDate, setDisplayEndDate] = React.useState(endDate);
    const [displayGPRA, setDisplayGPRA] = React.useState(gpraPeriod);

    var locationName = EMPTY_LOCALTION_LIST_VALUE;
    var reportName="Unique Patients"
    if(locations.length !== 0 && displayLocationId !== 0) {
      var data: Array<any> =  locations.filter(item => item.ID == displayLocationId);
      locationName = data[0]['Name'];
    }
    console.log(visitsOutcomesList);
    if(displayRepotType!=null && displayRepotType==1){
      reportName="Total Reports"
    }  
    var gpraPeriodInit="10/1/2020-09/30/2021";    
    React.useEffect(() => {
      setDisplayReportType(reportType);
      setDisplayLocationId(locationId);
      setDisplayStartDate(startDate);
      setDisplayEndDate(endDate);
      setDisplayGPRA(gpraPeriod);
    },[isLoading])         
   
    return (
        <ContentContainer>
          <Grid container justify="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
            <Grid item xs={4} style={{ textAlign: 'center' }}>
              <TitleText>
                Health Indicator Report
              </TitleText>
              <TitleTextModal>
               Visit Outcomes
              </TitleTextModal>
            </Grid>
          </Grid>
          <Table aria-label="simple  table" style={{ marginBottom: 50 }}>
            <TableHead className={customClasss.tableHead}>
              <TableRow>
                <TableCell>
                  <p><ReportHeaderText>Created Date</ReportHeaderText></p>
                  <p>{currentDateStamp}</p>
                </TableCell>
                <TableCell>
                  <p><ReportHeaderText>Report Period</ReportHeaderText></p>
                  <p>{displayStartDate && displayEndDate? `${convertDate(displayStartDate)}-${convertDate(displayEndDate)}`: !displayGPRA?gpraPeriodInit:displayGPRA}</p>
                </TableCell>
                <TableCell>
                  <p><ReportHeaderText>Branch Locations</ReportHeaderText></p>
                  <p>{locationName}</p>
                </TableCell>
                <TableCell>
                  <p><ReportHeaderText>Report Type</ReportHeaderText></p>
                  <p>{reportName}</p>
                </TableCell>
              </TableRow>
            </TableHead>
          </Table>
          {isLoading?<CircularProgress disableShrink={false} style={{ color: '#2e2e42', margin: '0 auto' }}/>:
          <>
          {visitsOutcomesList.TreatmentAction1 &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.TreatmentAction1['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.TreatmentAction1['Items']) !== "undefined") && visitsOutcomesList.TreatmentAction1['Items'].length!= 0 &&
                  visitsOutcomesList.TreatmentAction1['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }                        
                </TableBody>
              </Table>              
              </TableContainer>      
         }


          {visitsOutcomesList.TreatmentAction2 &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.TreatmentAction2['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.TreatmentAction2['Items']) !== "undefined") && visitsOutcomesList.TreatmentAction2['Items'].length != 0 &&
                  visitsOutcomesList.TreatmentAction2['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }                   
                </TableBody>
              </Table>              
              </TableContainer>
          }



          {visitsOutcomesList.TreatmentAction3 &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.TreatmentAction3['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.TreatmentAction3['Items']) !== "undefined") && visitsOutcomesList.TreatmentAction3['Items'].length != 0 &&
                  visitsOutcomesList.TreatmentAction3['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
          {visitsOutcomesList.TreatmentAction4 &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.TreatmentAction4['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.TreatmentAction4['Items']) !== "undefined") && visitsOutcomesList.TreatmentAction4['Items'].length != 0 &&
                  visitsOutcomesList.TreatmentAction4['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
          {visitsOutcomesList.TreatmentAction5 &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.TreatmentAction5['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.TreatmentAction5['Items']) !== "undefined") && visitsOutcomesList.TreatmentAction5['Items'].length != 0 &&
                  visitsOutcomesList.TreatmentAction5['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
           {visitsOutcomesList.NewVisitReferralRecommendation &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.NewVisitReferralRecommendation['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.NewVisitReferralRecommendation['Items']) !== "undefined") && visitsOutcomesList.NewVisitReferralRecommendation['Items'].length != 0 &&
                  visitsOutcomesList.NewVisitReferralRecommendation['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }

          {visitsOutcomesList.NewVisitReferralRecommendationAccepted &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.NewVisitReferralRecommendationAccepted['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.NewVisitReferralRecommendationAccepted['Items']) !== "undefined") && visitsOutcomesList.NewVisitReferralRecommendationAccepted['Items'].length != 0 &&
                  visitsOutcomesList.NewVisitReferralRecommendationAccepted['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
          {visitsOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items']) !== "undefined") && visitsOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items'].length != 0 &&
                  visitsOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
           {visitsOutcomesList.Discharged &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.Discharged['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.Discharged['Items']) !== "undefined") && visitsOutcomesList.Discharged['Items'].length != 0 &&
                  visitsOutcomesList.Discharged['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
            {visitsOutcomesList.FollowUps &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{visitsOutcomesList.FollowUps['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(visitsOutcomesList.FollowUps['Items']) !== "undefined") && visitsOutcomesList.FollowUps['Items'].length != 0 &&
                  visitsOutcomesList.FollowUps['Items'].map((item: any) => 
                    (<TableRow>
                      <TableCell>{item['Indicator']}</TableCell>
                      {item['TotalByAge'] && Object.keys(item['TotalByAge']).map((subItem: any, key: number) => (
                          <TableCell>{item['TotalByAge'][subItem]}</TableCell>
                      ))}
                      <TableCell>{item['Total']}</TableCell>
                    </TableRow>)
                  )
                }             
                </TableBody>
              </Table>
            
              </TableContainer>
          }
          </>}  
        </ContentContainer>
    )
  }
  
  export default VisitsOutcomesLists;
  