import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import { useEffect } from 'react';
import { postFilteredReportsByAgeRequest,reportAgeGroupByAgeRequest} from '../../../../actions/reports'
import { getListReportByProblemSelector,getReportLocationsSelector,getReportSelectedLocationIdSelector,getReportBsrReportTypeSelector,getReportStartDateSelector,getReportEndDateSelector,
          getReportGrpaPeriodKeySelector, 
          getListReportAgeGroupByAgeSelector,
          isReportLoadingSelector} from '../../../../selectors/reports';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText, EMPTY_LOCALTION_LIST_VALUE } from 'helpers/general';
import { convertDate } from 'helpers/dateHelper';
import customClasss from  '../../pages.module.scss';
import { ReportQuestionText, ReportIndicateText } from '../../styledComponents';
import TobacoSection from '../sections/tobaco';
import Cagesection from '../sections/cage';
import DastSection from '../sections/dast';
import Gad2Section from '../sections/gad2';
import Gad7Section from '../sections/gad7';
import Phq2Section from '../sections/phq2';
import Phq9Section from '../sections/phq9';
import HitsSection from '../sections/hits';
import BbgsSection from '../sections/bbgs';
import DrugPrimarySection  from '../sections/drugPrimary';
import DrugSecondarySection  from '../sections/drugSecondary';
import DrugTertiarySection from '../sections/drugTertiary';


const ReportsByAgeLists = (): React.ReactElement => {
      
    const dispatch = useDispatch();
    useEffect(() => {
        dispatch(postFilteredReportsByAgeRequest()) 
        dispatch(reportAgeGroupByAgeRequest());
    }, [])    

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
                Screening Results By Age
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
          <TobacoSection />
          <Cagesection />
          <DastSection />
          <Gad2Section />
          <Gad7Section />
          <Phq2Section />
          <Phq9Section />
          <HitsSection />
          <BbgsSection />
          <DrugPrimarySection />
          <DrugSecondarySection />
          <DrugTertiarySection />
        </>}
        </ContentContainer>
    )
  }
  
  export default ReportsByAgeLists;
  