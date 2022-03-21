import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { convertDate} from '../../../../helpers/dateHelper';
import { useEffect } from 'react';
import { postFilteredFollowupOutcomesRequest,reportAgeGroupByAgeRequest } from '../../../../actions/reports'
import { getListFollowupOutcomesSelector, isReportLoadingSelector }  from '../../../../selectors/reports';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Grid,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText, EMPTY_LOCALTION_LIST_VALUE }  from 'helpers/general';
import { 
  getReportLocationsSelector, getReportSelectedLocationIdSelector,getReportBsrReportTypeSelector,getReportGrpaPeriodKeySelector,getReportStartDateSelector,getReportEndDateSelector,
  getListReportAgeGroupByAgeSelector
} from '../../../../selectors/reports';
import customClasss from  '../../pages.module.scss';


const FollowupOutcomesLists = (): React.ReactElement => {

    const dispatch = useDispatch();      
    useEffect(() => {
        dispatch(postFilteredFollowupOutcomesRequest());
        dispatch(reportAgeGroupByAgeRequest());
    }, [])       
    const followupOutcomesList: any = useSelector(getListFollowupOutcomesSelector);      
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
    console.log(followupOutcomesList);
    if(displayRepotType!=null && displayRepotType==1){
      reportName="Total Reports"
    }  
    var gpraPeriodInit="10/1/2021-09/30/2022";   
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
               Follow-Up Outcomes
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
          {followupOutcomesList.PatientAttendedVisit &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.PatientAttendedVisit['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.PatientAttendedVisit['Items']) !== "undefined") && followupOutcomesList.PatientAttendedVisit['Items'].length!= 0 &&
                  followupOutcomesList.PatientAttendedVisit['Items'].map((item: any) => 
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


          {followupOutcomesList.FollowUpContactOutcome &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.FollowUpContactOutcome['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.FollowUpContactOutcome['Items']) !== "undefined") && followupOutcomesList.FollowUpContactOutcome['Items'].length != 0 &&
                  followupOutcomesList.FollowUpContactOutcome['Items'].map((item: any) => 
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



          {followupOutcomesList.NewVisitReferralRecommendation &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.NewVisitReferralRecommendation['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.NewVisitReferralRecommendation['Items']) !== "undefined") && followupOutcomesList.NewVisitReferralRecommendation['Items'].length != 0 &&
                  followupOutcomesList.NewVisitReferralRecommendation['Items'].map((item: any) => 
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
          {followupOutcomesList.NewVisitReferralRecommendationAccepted &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.NewVisitReferralRecommendationAccepted['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.NewVisitReferralRecommendationAccepted['Items']) !== "undefined") && followupOutcomesList.NewVisitReferralRecommendationAccepted['Items'].length != 0 &&
                  followupOutcomesList.NewVisitReferralRecommendationAccepted['Items'].map((item: any) => 
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
          {followupOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items']) !== "undefined") && followupOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items'].length != 0 &&
                  followupOutcomesList.ReasonNewVisitReferralRecommendationNotAccepted['Items'].map((item: any) => 
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
           {followupOutcomesList.Discharged &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.Discharged['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.Discharged['Items']) !== "undefined") && followupOutcomesList.Discharged['Items'].length != 0 &&
                  followupOutcomesList.Discharged['Items'].map((item: any) => 
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
            {followupOutcomesList.FollowUps &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{followupOutcomesList.FollowUps['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(followupOutcomesList.FollowUps['Items']) !== "undefined") && followupOutcomesList.FollowUps['Items'].length != 0 &&
                  followupOutcomesList.FollowUps['Items'].map((item: any) => 
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
  
  export default FollowupOutcomesLists;
  