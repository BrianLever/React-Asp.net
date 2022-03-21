import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { convertDate} from '../../../../helpers/dateHelper';
import { useEffect } from 'react';
import { postFilteredScreenTimeLogRequest,reportAgeGroupByAgeRequest } from '../../../../actions/reports'
import { getListScreenTimeLogSelector, isReportLoadingSelector }  from '../../../../selectors/reports';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Grid,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText,EMPTY_LOCALTION_LIST_VALUE }  from 'helpers/general';
import { 
  getReportLocationsSelector, getReportSelectedLocationIdSelector,getReportBsrReportTypeSelector,getReportGrpaPeriodKeySelector,getReportStartDateSelector,getReportEndDateSelector,
  getListReportAgeGroupByAgeSelector
} from '../../../../selectors/reports';


const ScreenTimeLogLists = (): React.ReactElement => {

    const dispatch = useDispatch();      
    useEffect(() => {
        dispatch(postFilteredScreenTimeLogRequest());
        dispatch(reportAgeGroupByAgeRequest());
    }, [])       
    const screenTimeLogList: any = useSelector(getListScreenTimeLogSelector);      
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
    console.log(screenTimeLogList);
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
               Screen Time Log
              </TitleTextModal>
            </Grid>
          </Grid>
          <Table aria-label="simple  table" style={{ marginBottom: 50 }}>
            <TableHead style={{ backgroundColor: 'rgb(237,237,242)' }}>
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
                {/* <TableCell>
                  <p><ReportHeaderText>Report Type</ReportHeaderText></p>
                  <p>{reportName}</p>
                </TableCell> */}
              </TableRow>
            </TableHead>
          </Table>
          {isLoading?<CircularProgress disableShrink={false} style={{ color: '#2e2e42', margin: '0 auto' }}/>:
          <>
          {screenTimeLogList.SectionMeasures &&  
              <TableContainer>
                <Table aria-label="simple  table">
                <TableHead style={{ backgroundColor: 'rgb(237,237,242)' }}>
                  <TableRow>
                    <TableCell width={'50%'}><ReportHeaderText>Screening Measure</ReportHeaderText></TableCell>
                    <TableCell style={{ textAlign: "center" }}><ReportHeaderText>Number  Of  Reports</ReportHeaderText></TableCell>
                    <TableCell style={{ textAlign: "center" }}><ReportHeaderText>Total Time</ReportHeaderText></TableCell>
                    <TableCell style={{ textAlign: "center" }}><ReportHeaderText>Average Time</ReportHeaderText></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                {(typeof(screenTimeLogList.SectionMeasures)!== "undefined") && screenTimeLogList.SectionMeasures.length!= 0 &&
                  screenTimeLogList.SectionMeasures.map((item: any) => 
                    (<TableRow>
                      <TableCell >{item['ScreeningSectionName']}</TableCell>
                      <TableCell style={{ textAlign: "center" }}>{item['NumberOfReports']}</TableCell>
                      <TableCell style={{ textAlign: "center" }}>{item['TotalTime']}</TableCell>
                      <TableCell style={{ textAlign: "center" }}>{item['AverageTime']}</TableCell>
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
  
  export default ScreenTimeLogLists;
  