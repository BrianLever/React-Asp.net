import React from 'react';
import ScreendoxTable, { EScreendoxTableType } from './index';
import { ICssrsPatientRecordItem, ICssrsReportRequest } from '../../../actions/c-ssrs-list/c-ssrs-report';
import {
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Switch, Button
} from '@material-ui/core';
import classes from './table.module.scss';


export interface CssrsPatientRecordsTableProps {
    patientRecords: Array<ICssrsPatientRecordItem>;
    handleClick?: (params: ICssrsReportRequest) => void;
}

const CssrsPatientRecordsTable = (props: CssrsPatientRecordsTableProps) => {
    const {  patientRecords, handleClick } = props;
    return (
        <TableContainer>
            <Table>
            <TableHead className={classes.tableHead}>
                <TableRow>
                    <TableCell width={'20%'}>Patient Name</TableCell>
                    <TableCell width={'20%'}>Date of Birth</TableCell>
                    <TableCell width={'40%'}>Address</TableCell>
                    <TableCell width={'20%'}>Action</TableCell>
                </TableRow>
            </TableHead>
           
            <TableBody>
                {patientRecords && patientRecords.map((record, index) => (
                    <TableRow key={index}>
                        <TableCell>{record.FullName}</TableCell>
                        <TableCell>{record.BirthdayFormatted}</TableCell>
                        <TableCell>{record.StreetAddress}, {record.City}, {record.StateID}, {record.ZipCode}</TableCell>
                        <TableCell>
                            <Button 
                                size="large"  
                                fullWidth
                                disabled={false}
                                variant="contained" 
                                color="primary" 
                                style={{ backgroundColor: 'rgb(46,46,66)' }}
                                onClick={() => {
                                    handleClick && handleClick({
                                        ...record
                                    })
                                }}
                            >
                            Next
                            </Button> 
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>     
        </Table>
        </TableContainer>
    )
}

export default CssrsPatientRecordsTable;