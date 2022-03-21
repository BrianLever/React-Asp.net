import React, { ChangeEvent } from 'react';
import styled from 'styled-components';
import { withStyles } from '@material-ui/core/styles';
import { useDispatch, useSelector } from 'react-redux';
import { FormControl, Select, TextField } from '@material-ui/core';
import { TabBar, TabPanel } from 'components/UI/tab';
import { DescriptionText, TitleTextModal, useStyles, ScreendoxTextInput, ScreendoxTextArea  } from '../../styledComponents';
import {  
    screenProfileCreateScreenProfileDescriptionSelector, 
    isNewScreenProfileLoadingSelector, 
    screenProfileCreateScreenProfileNameSelector,
    isScreenProfilesListRequestLoadingSelector,
    screeningMinimumAgeListSelector,
    setScreenProfileSelectedOptionSelector,
    screenProfileFrequencyListSelector,
    isScreenProfileFrequencyListLoadingSelector,
    screenProfileDefaultFrequencyListSelector,
    screenProfileAgeGroupsListSelector } 
from '../../../../selectors/screen-profiles';
import { 
    setCreateScreenProfileDescription, setCreateScreenProfileName, 
    screenProfileMinimumAgeListRequest, IScreeningProfileMinimunAgeResponseItem, 
    screenProfileMinimumAgeListRequestSuccess, IScreenProfileFrequencyResponseItem,
    setScreenProfileSelectedEditOption, screenProfileFrequencyListRequest, 
    IScreenProfileFrequencyListItem, screenProfileFrequencyListRequestSuccess, IScreenProfileAgeGroupsItem } 
from '../../../../actions/screen-profiles';
import { 
    TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, 
    Grid, TableSortLabel, Box, CircularProgress, Input, Switch, } 
from '@material-ui/core';
import customClasss from  '../../pages.module.scss';



const EditScreenProfile = (): React.ReactElement => {

    const dispatch = useDispatch();
    const createName: string = useSelector(screenProfileCreateScreenProfileNameSelector);
    const createDescription: string = useSelector(screenProfileCreateScreenProfileDescriptionSelector);
    const isNewScreenProfileLoading: boolean = useSelector(isNewScreenProfileLoadingSelector);
    const isScreeningMinimumAgeListLoading: boolean = useSelector(isScreenProfilesListRequestLoadingSelector);
    const screeningMinimumAgeList: Array<IScreeningProfileMinimunAgeResponseItem> = useSelector(screeningMinimumAgeListSelector);
    const selectedScreenProfileEditOption: number  = useSelector(setScreenProfileSelectedOptionSelector);
    const screenProfileFrequencyList: Array<IScreenProfileFrequencyResponseItem> = useSelector(screenProfileFrequencyListSelector);
    const isScreenProfileFrequencyListLoading: boolean = useSelector(isScreenProfileFrequencyListLoadingSelector);
    const frequencyList: Array<IScreenProfileFrequencyListItem> = useSelector(screenProfileDefaultFrequencyListSelector);
    const screenProfileAgeGroups: Array<IScreenProfileAgeGroupsItem> = useSelector(screenProfileAgeGroupsListSelector);
 
    const classes = useStyles();
    const [value, setValue] = React.useState(0);
    
    const handleChange = (event: ChangeEvent<{}>, newValue: number) => {
        setValue(newValue);
        dispatch(setScreenProfileSelectedEditOption(newValue));
        if(newValue == 1) {
            dispatch(screenProfileMinimumAgeListRequest());
        }
        if(newValue == 2) {
            dispatch(screenProfileFrequencyListRequest());
        }
    };

    const checkingHandle = (value: boolean, index: number, screeningSctionId: string) => {
        
        var primarySection = screenProfileAgeGroups.filter((section => section.PrimarySectionID == screeningSctionId));
        
       
        var alternativeSection = screenProfileAgeGroups.filter((section => section.AlternativeSectionID == screeningSctionId));
        
        
        var newList = [
            ...screeningMinimumAgeList,
        ];
        
        newList[index] = {
            ...screeningMinimumAgeList[index],
            IsEnabled: value
        };
        
        var updateList: any = newList;
        if(primarySection.length !== 0) {
            updateList = newList.map((list, i) => {
                if(list.ScreeningSectionID == primarySection[0].AlternativeSectionID && list.IsEnabled && value) {
                    list.IsEnabled = false;
                }
                return list;
            })
        }

        if(alternativeSection.length !== 0) {
            updateList = newList.map((list, i) => {
                if(list.ScreeningSectionID == alternativeSection[0].PrimarySectionID && list.IsEnabled && value) {
                    list.IsEnabled = false;
                }
                return list;
            })
        }
        
        dispatch(screenProfileMinimumAgeListRequestSuccess(updateList));

    }

    const minimumAgeChangeHandle = (value: number, index: number) => {
        var newList = [
            ...screeningMinimumAgeList,
        ];
        newList[index] = {
            ...screeningMinimumAgeList[index],
            MinimalAge: value
        }
        dispatch(screenProfileMinimumAgeListRequestSuccess(newList));
    }


    return (
        <div className={classes.root} style={{ fontSize: 16 }}>
            <TabBar 
                tabArray={['NAME/DESCRIPTION', 'AGE RANGE', 'FREQUENCY']}
                handleChange={handleChange}
                selectedTab={value}
            />
            <TabPanel 
                value={value} index={0}
            >
                <Grid  spacing={1}  style={{ marginTop: '3rem' }}>   
                    <Grid item xs={12} className={classes.grid}>
                        <TitleTextModal>
                        Screen Profile Name*
                        </TitleTextModal>
                    </Grid>
                    <Grid item xs={12} style={{ textAlign: 'center' }}>
                        <ScreendoxTextInput
                            type="text"
                            id="screenProfileName"
                            value={createName}
                            onChange={e => {
                                e.stopPropagation();
                                dispatch(setCreateScreenProfileName(e.target.value));
                            }}
                        />
                    </Grid>
                    <Grid item xs={12}  className={classes.grid}>
                        <TitleTextModal style={{ marginTop: 25 }}>
                            Description
                        </TitleTextModal>
                    </Grid>
                    <ScreendoxTextArea
                        value={createDescription}
                        onChange={e => {
                            dispatch(setCreateScreenProfileDescription(`${e.target.value}`));
                        }}
                    >
                        { createDescription }
                    </ScreendoxTextArea>
                </Grid> 
            </TabPanel>
            <TabPanel value={value} index={1} >
                <DescriptionText style={{ marginTop: '3rem' }}>
                    Enter the age range for each screening tool. Use 0 to include children under the age of 1 years old. 
                    <br/>
                    Turn ON the tools needed. Screendox will only screen patients who are in the age range selected with the tools that are turned on.
                </DescriptionText>
                <TableContainer>
                    <Table style={{ borderCollapse: 'revert', fontSize: 16 }}>
                        <TableHead className={customClasss.tableHead}>
                            <TableRow>
                                <TableCell>Screening Measure</TableCell>
                                <TableCell>Minimum Age</TableCell>
                                <TableCell className={customClasss.rightTh}>Turn ON/OFF</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {screeningMinimumAgeList && screeningMinimumAgeList.map((ageItem: IScreeningProfileMinimunAgeResponseItem, index: number) => (
                                <TableRow key={index} style={{ fontSize: 16 }}>
                                    <TableCell>{ageItem.Name}</TableCell>
                                    <TableCell>
                                        <TextField
                                            fullWidth
                                            margin="dense"
                                            variant="outlined"
                                            value={ageItem.MinimalAge}
                                            onChange={(e) => minimumAgeChangeHandle(parseInt(e.target.value), index)}
                                        />
                                    </TableCell>
                                    <TableCell className={customClasss.rightTh}>
                                        <Switch  
                                        checked={ageItem.IsEnabled} 
                                        onChange={(e) => checkingHandle(e.target.checked, index, ageItem.ScreeningSectionID)}
                                        classes={{
                                            root: classes.element,
                                            switchBase: classes.switchBase,
                                            thumb: classes.thumb,
                                            track: classes.track,
                                            checked: classes.checked
                                        }}
                                        focusVisibleClassName={classes.focusVisible}
                                        />
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>      
            </TabPanel>
            <TabPanel value={value} index={2}>
                <DescriptionText style={{ marginTop: '3rem' }}>
                    Set how frequenct each screening is conducted for the GPRA period.<br/>
                    Screendox  always  screens patient  on the first visit of each GPRA period.
                </DescriptionText>
                <TableContainer>
                <Table style={{ borderCollapse: 'revert', fontSize: 16  }}>
                    <TableHead className={customClasss.tableHead}>
                        <TableRow>
                            <TableCell style={{ fontSize: '16px' }}>Screening Tool</TableCell>
                            <TableCell style={{ fontSize: '16px' }}>Frequency Per GPRA Period</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {screenProfileFrequencyList && screenProfileFrequencyList.map((frequencyItem: IScreenProfileFrequencyResponseItem, index: number) => (
                            <TableRow key={index}>
                                <TableCell style={{ fontSize: '16px !important' }}>{frequencyItem.Name}</TableCell>
                                <TableCell style={{ fontSize: '16px  !important' }}>
                                    <FormControl fullWidth variant="outlined" className={classes.formControl} style={{ fontWeight: 400 }}>
                                        <Select
                                            margin="dense"
                                            native
                                            onChange={(event: ChangeEvent<{ name?: string; value: unknown }>) => {
                                                if(event.target.value) {
                                                    try {
                                                        const value = `${event.target.value}`;
                                                        var frequencyUpdatedList = [...screenProfileFrequencyList];
                                                        frequencyUpdatedList[index] = { ...frequencyUpdatedList[index], Frequency: parseInt(value) }
                                                        dispatch(screenProfileFrequencyListRequestSuccess(frequencyUpdatedList));
                                                    } catch (e) {
                                                        
                                                    }
                                                }
                                            }}
                                            value={frequencyItem.Frequency}
                                        >
                                            {frequencyList && frequencyList.map((item: IScreenProfileFrequencyListItem, id: number) => (
                                                <option key={id} value={item.ID}>{item.Name}</option>
                                            ))}
                                        </Select>
                                    </FormControl>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>      
        </TabPanel>
        </div>
        
    )
}

export default EditScreenProfile;
