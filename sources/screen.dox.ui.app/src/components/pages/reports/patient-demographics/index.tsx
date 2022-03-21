import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { convertDate} from '../../../../helpers/dateHelper';
import { useEffect } from 'react';
import { postFilteredPatientDemographicsRequest,reportAgeGroupByAgeRequest } from '../../../../actions/reports'
import { getListPatientDemographicsSelector, isReportLoadingSelector }  from '../../../../selectors/reports';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Grid,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText, EMPTY_LOCALTION_LIST_VALUE } from 'helpers/general';
import { 
  getReportLocationsSelector, getReportSelectedLocationIdSelector,getReportBsrReportTypeSelector,getReportGrpaPeriodKeySelector,getReportStartDateSelector,getReportEndDateSelector,
  getListReportAgeGroupByAgeSelector
} from '../../../../selectors/reports';
import customClasss from  '../../pages.module.scss';

const PatientDemographicsLists = (): React.ReactElement => {

    const dispatch = useDispatch();      
    useEffect(() => {
        dispatch(postFilteredPatientDemographicsRequest());
        dispatch(reportAgeGroupByAgeRequest());
    }, [])       
    const patientDemographicsList: any = useSelector(getListPatientDemographicsSelector);      
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
               Patient Demographics
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
          {patientDemographicsList.RaceSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.RaceSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.RaceSection['Items']) !== "undefined") && patientDemographicsList.RaceSection['Items'].length!= 0 &&
                  patientDemographicsList.RaceSection['Items'].map((item: any) => 
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


          {patientDemographicsList.GenderSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.GenderSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.GenderSection['Items']) !== "undefined") && patientDemographicsList.GenderSection['Items'].length != 0 &&
                  patientDemographicsList.GenderSection['Items'].map((item: any) => 
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



          {patientDemographicsList.SexualOrientationSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.SexualOrientationSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.SexualOrientationSection['Items']) !== "undefined") && patientDemographicsList.SexualOrientationSection['Items'].length != 0 &&
                  patientDemographicsList.SexualOrientationSection['Items'].map((item: any) => 
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
          {patientDemographicsList.MaritalStatusSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.MaritalStatusSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.MaritalStatusSection['Items']) !== "undefined") && patientDemographicsList.MaritalStatusSection['Items'].length != 0 &&
                  patientDemographicsList.MaritalStatusSection['Items'].map((item: any) => 
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
          {patientDemographicsList.EducationLevelSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.EducationLevelSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.EducationLevelSection['Items']) !== "undefined") && patientDemographicsList.EducationLevelSection['Items'].length != 0 &&
                  patientDemographicsList.EducationLevelSection['Items'].map((item: any) => 
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
           {patientDemographicsList.LeavingOnOrOffReservationSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.LeavingOnOrOffReservationSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.LeavingOnOrOffReservationSection['Items']) !== "undefined") && patientDemographicsList.LeavingOnOrOffReservationSection['Items'].length != 0 &&
                  patientDemographicsList.LeavingOnOrOffReservationSection['Items'].map((item: any) => 
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
            {patientDemographicsList.MilitaryExperienceSection &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead className={customClasss.tableHead}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>{patientDemographicsList.MilitaryExperienceSection['Header']}</ReportHeaderText></TableCell>
                    {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                        (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
                    )}
                   <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(patientDemographicsList.MilitaryExperienceSection['Items']) !== "undefined") && patientDemographicsList.MilitaryExperienceSection['Items'].length != 0 &&
                  patientDemographicsList.MilitaryExperienceSection['Items'].map((item: any) => 
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
  
  export default PatientDemographicsLists;
  