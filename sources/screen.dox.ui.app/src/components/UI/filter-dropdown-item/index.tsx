import React, { useEffect } from 'react';
import { 
    Button, Checkbox, FormControl, Grid, InputLabel, Select, TextField, 
} from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Collapse from '@material-ui/core/Collapse';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import {  FilterBySort, FilterBySortItem } from '../../../actions/reports';
import RectangleCheckbox from 'components/UI/checkbox/RectangleCheckbox';



export type TFilterDropdownItemProps = {
    data: Array<{ 
        title: string,
        name: string,
        value: number,
        checked: boolean,
    }>,
    name: string;
    open: boolean,
    index: number,
    changeHandler?: (item: any, value: boolean, index: number) => void;
    clickHandler?: () => void;
    unHandleCheck?:(index: number, value: boolean) => void;
}


const useStyles = makeStyles((theme) => ({
    filterItem: {
        paddingLeft: 0
    }
}));
  

const FilterDropdownItem = (props: TFilterDropdownItemProps): React.ReactElement => {
    const list_classes = useStyles();
    const unCheck = props.data.filter(b => b.checked === true);
    return (
        <Grid item sm={12} style={{ border: '1px solid #272727', borderRadius: 8, marginBottom: 5 }}>
            <ListItem 
                button 
                onClick={() => props.clickHandler && props.clickHandler() }
            >
                <ListItemText primary={props.name} />
                {props.open ? <ExpandLess /> : <ExpandMore />}
            </ListItem>                   
            <Collapse in={props.open} timeout="auto" unmountOnExit>
                <List component="div">
                    <ListItem button className={list_classes.filterItem}>
                        <Grid container>
                            <Grid item sm={2}>
                                <RectangleCheckbox 
                                    name={''}
                                    id={''}
                                    style={{
                                        marginBottom: 10,
                                        marginLeft: 10
                                    }}
                                    changeHandler={(e) => props.unHandleCheck && props.unHandleCheck(props.index, e.target.checked)}
                                    isChecked={!unCheck.length}
                                />
                            </Grid>
                            <Grid item sm={10}>
                            <ListItemText primary={"None"} style={{ fontSize: '1em', paddingLeft: 8 }} onClick={(e) => props.unHandleCheck && props.unHandleCheck(props.index, !!unCheck.length)}/> 
                            </Grid>
                        </Grid>
                    </ListItem>
                    {props.data && props.data.map((item: FilterBySortItem, i) => (
                    <ListItem button key={i} className={list_classes.filterItem}>
                        <Grid container>
                            <Grid item sm={2}>
                                <RectangleCheckbox 
                                    name={''}
                                    id={''}
                                    style={{
                                        marginBottom: 10,
                                        marginLeft: 10
                                    }}
                                    changeHandler={(event) => props.changeHandler && props.changeHandler(item, event.target?.checked, props.index)}
                                    isChecked={item.checked}
                                />
                            </Grid>
                            <Grid item sm={10}>
                            <ListItemText primary={item?.title} style={{ wordBreak: 'break-all', fontSize: '1em', paddingLeft: 8 }} onClick={(event) => props.changeHandler && props.changeHandler(item, !item.checked, props.index)}/> 
                            </Grid>
                        </Grid>                        
                    </ListItem>
                    ))}
                </List>
            </Collapse> 
        </Grid>
    )
}

export default FilterDropdownItem;