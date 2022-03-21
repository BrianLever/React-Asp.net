import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getListReportByAgeSelector, getListReportAgeGroupByAgeSelector } from 'selectors/reports';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText } from 'helpers/general';
import customClasss from  '../../pages.module.scss';
import { ReportQuestionText, ReportIndicateText } from '../../styledComponents';

const Phq9Section = (): React.ReactElement => {

    const reportListByAge: any = useSelector(getListReportByAgeSelector);
    const reportAgeGroupListByAge:any=useSelector(getListReportAgeGroupByAgeSelector);
    if(!reportListByAge['DepressionSectionPhq9']) {
      return <></>;
    }

    if((!reportListByAge['DepressionSectionPhq9']['Items'].length && !reportListByAge['DepressionSectionPhq9']['MainQuestions'].length)) {
        return <></>;
    }

    return (
      <TableContainer>
        <Table>
        <TableHead className={customClasss.tableHead}>
          <TableRow>
            <TableCell width={'50%'}><ReportHeaderText>{reportListByAge['DepressionSectionPhq9']['Header']}</ReportHeaderText></TableCell>
            {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
            )}
            <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
        {(typeof(reportListByAge['DepressionSectionPhq9']['MainQuestions']) !== "undefined") && reportListByAge['DepressionSectionPhq9']['MainQuestions'].length != 0 &&
          reportListByAge['DepressionSectionPhq9']['MainQuestions'].map((item: any) => 
            (<TableRow>
              <TableCell>{item['ScreeningSectionQuestion']}</TableCell>
              {item['PositiveScreensByAge'] && Object.keys(item['PositiveScreensByAge']).map((subItem: any, key: number) => (
                  <TableCell>{item['PositiveScreensByAge'][subItem]}</TableCell>
              ))}
              <TableCell>{item['Total']}</TableCell>
            </TableRow>)
          )
        }

        {(typeof(reportListByAge['DepressionSectionPhq9']['Items']) !== 'undefined') && reportListByAge['DepressionSectionPhq9']['Items'].length !== 0 &&
          reportListByAge['DepressionSectionPhq9']['Items'].map((item: any) => 
            (<TableRow>
              <TableCell>
                <ReportQuestionText>{item['ScreeningSectionQuestion']}</ReportQuestionText> 
                <ReportIndicateText>{item['ScreeningSectionIndicates']}</ReportIndicateText>
              </TableCell>
              {item['PositiveScreensByAge'] && Object.keys(item['PositiveScreensByAge']).map((subItem: any, key: number) => (
                  <TableCell>{item['PositiveScreensByAge'][subItem]}</TableCell>
              ))}
              <TableCell>{item['Total']}</TableCell>
            </TableRow>)
          )
        }

        </TableBody>
        </Table>
        <p style={{ marginTop: 10, marginBottom: 30 }}>{reportListByAge['DepressionSectionPhq9']['Copyrights']}</p>
      </TableContainer>
    )
}

export default Phq9Section;