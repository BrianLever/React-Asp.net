import AppBar from '@material-ui/core/AppBar';
import React, { useEffect, useState, ChangeEvent } from 'react';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import { 
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, 
    Grid, TableSortLabel, Box, CircularProgress, Input, Switch } 
from '@material-ui/core';
import PropTypes from 'prop-types';
import { useStyles } from 'components/pages/styledComponents';
import { TabsListContainer, TabsLists, TabsList, TabsButton, TabsText  } from './styledComponents';

export function TabPanel(props: any) {
    const { children, value, index, ...other } = props;

    return (
        <div
            role="tabpanel"
            hidden={value !== index}
            id={`screendox-tabpanel-${index}`}
            aria-labelledby={`screendox-tab-${index}`}
            {...other}
        >
        {value === index && (
            <Box p={3} style={{ padding: 0 }}>
                <Typography>{children}</Typography>
            </Box>
        )}
        </div>
    );
}
  
TabPanel.propTypes = {
    children: PropTypes.node,
    index: PropTypes.any.isRequired,
    value: PropTypes.any.isRequired,
};
  
export function a11yProps(index: number) {
    return {
        id: `simple-tab-${index}`,
        'aria-controls': `simple-tabpanel-${index}`,
    };
}

export interface ITabBarProps {
    tabArray?: Array<string>;
    handleChange?: (event: ChangeEvent<{}>, newValue: number) => void;
    selectedTab?: number;
}


export const TabBar = (props: ITabBarProps) => {
    const { selectedTab, handleChange, tabArray } = props; 
    return (
        <TabsListContainer>
            <TabsLists>
                {tabArray && tabArray.map((tab, index) => {
                    return (<TabsList  key={index}>
                        <TabsButton type="button" selected={selectedTab===index} 
                            onClick={(e) => {
                                handleChange && handleChange(e, index)
                            }}
                        >
                            <TabsText>
                                {tab}
                            </TabsText>
                        </TabsButton>
                    </TabsList>)
                })}
            </TabsLists>
        </TabsListContainer>
    )
}