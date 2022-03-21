import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getListReportByAgeSelector, getListReportAgeGroupByAgeSelector } from 'selectors/reports';
import { ContentContainer, TitleText, TitleTextModal } from '../../styledComponents';
import {
  TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box,CircularProgress
} from '@material-ui/core';
import { ReportHeaderText } from 'helpers/general';
import { convertDate } from 'helpers/dateHelper';
import customClasss from  '../../pages.module.scss';
import { ReportQuestionText, ReportIndicateText } from '../../styledComponents';

const BbgsSection = (): React.ReactElement => {

    const reportListByAge: any = useSelector(getListReportByAgeSelector);
    const reportAgeGroupListByAge:any=useSelector(getListReportAgeGroupByAgeSelector);
    if(!reportListByAge['ProblemGamblingSection']) {
      return <></>;
    }

    if((!reportListByAge['ProblemGamblingSection']['Items'].length && !reportListByAge['ProblemGamblingSection']['MainQuestions'].length)) {
        return <></>;
    }

    return (
      <TableContainer>
        <Table>
        <TableHead className={customClasss.tableHead}>
          <TableRow>
            <TableCell width={'50%'}><ReportHeaderText>{reportListByAge['ProblemGamblingSection']['Header']}</ReportHeaderText></TableCell>
            {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
            )}
            <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
        {(typeof(reportListByAge['ProblemGamblingSection']['MainQuestions']) !== "undefined") && reportListByAge['ProblemGamblingSection']['MainQuestions'].length != 0 &&
          reportListByAge['ProblemGamblingSection']['MainQuestions'].map((item: any) => 
            (<TableRow>
              <TableCell>{item['ScreeningSectionQuestion']}</TableCell>
              {item['PositiveScreensByAge'] && Object.keys(item['PositiveScreensByAge']).map((subItem: any, key: number) => (
                  <TableCell>{item['PositiveScreensByAge'][subItem]}</TableCell>
              ))}
              <TableCell>{item['Total']}</TableCell>
            </TableRow>)
          )
        }

        {(typeof(reportListByAge['ProblemGamblingSection']['Items']) !== 'undefined') && reportListByAge['ProblemGamblingSection']['Items'].length !== 0 &&
          reportListByAge['ProblemGamblingSection']['Items'].map((item: any) => 
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
        <p style={{ marginTop: 10, marginBottom: 30 }}>{reportListByAge['ProblemGamblingSection']['Copyrights']}</p>
      </TableContainer>
    )
}

export default BbgsSection;