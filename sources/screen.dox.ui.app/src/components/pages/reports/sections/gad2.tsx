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

const Gad2Section = (): React.ReactElement => {

    const dispatch = useDispatch();
    const reportListByAge: any = useSelector(getListReportByAgeSelector);
    const reportAgeGroupListByAge:any=useSelector(getListReportAgeGroupByAgeSelector);
    if(!reportListByAge['AnxietySectionGad2']) {
      return <></>;
    }

    if((!reportListByAge['AnxietySectionGad2']['Items'].length && !reportListByAge['AnxietySectionGad2']['MainQuestions'].length)) {
        return <></>;
    }

    return (
      <TableContainer>
        <Table>
        <TableHead className={customClasss.tableHead}>
          <TableRow>
            <TableCell width={'50%'}><ReportHeaderText>{reportListByAge['AnxietySectionGad2']['Header']}</ReportHeaderText></TableCell>
            {reportAgeGroupListByAge.Labels && reportAgeGroupListByAge.Labels.map((item: any, i: number) =>
                (<TableCell  key={i}><ReportHeaderText>{ item }</ReportHeaderText></TableCell>)
            )}
            <TableCell ><ReportHeaderText>Total</ReportHeaderText></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
        {(typeof(reportListByAge['AnxietySectionGad2']['MainQuestions']) !== "undefined") && reportListByAge['AnxietySectionGad2']['MainQuestions'].length != 0 &&
          reportListByAge['AnxietySectionGad2']['MainQuestions'].map((item: any) => 
            (<TableRow>
              <TableCell>{item['ScreeningSectionQuestion']}</TableCell>
              {item['PositiveScreensByAge'] && Object.keys(item['PositiveScreensByAge']).map((subItem: any, key: number) => (
                  <TableCell>{item['PositiveScreensByAge'][subItem]}</TableCell>
              ))}
              <TableCell>{item['Total']}</TableCell>
            </TableRow>)
          )
        }

        {(typeof(reportListByAge['AnxietySectionGad2']['Items']) !== 'undefined') && reportListByAge['AnxietySectionGad2']['Items'].length !== 0 &&
          reportListByAge['AnxietySectionGad2']['Items'].map((item: any) => 
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
        <p style={{ marginTop: 10, marginBottom: 30 }}>{reportListByAge['AnxietySectionGad2']['Copyrights']}</p>
      </TableContainer>
    )
}

export default Gad2Section;