import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import Collapse from '@material-ui/core/Collapse';
import ExpandLess from '@material-ui/icons/ExpandLess';
import ExpandMore from '@material-ui/icons/ExpandMore';
import { Link } from 'react-router-dom';
import { 
    Button, FormControl, Grid, InputLabel, Select, 
} from '@material-ui/core';
import { ERouterUrls } from 'router';

export interface IReportDropDownMenuProps {
    name?: string;
}


export const reportNameArray = [
    { name: 'Screening Results by Problem', url: ERouterUrls.INDICATOR_REPORTS },
    { name: 'Screening Results by Age', url: ERouterUrls.REPORTS_BY_AGE },
    { name: 'Screening Results by Sort', url: ERouterUrls.REPORTS_BY_SORT },
    { name: 'Drug Use Type', url: ERouterUrls.DRUGS_BY_AGE },
    { name: 'C-SSRS Report', url: '' },
    { name: 'Patient Demographics', url: ERouterUrls.PATIENT_DEMOGRAPHICS },
    { name: 'Visits Outcomes', url: ERouterUrls.VISITS_OUTCOMES },
    { name: 'Follow-Up Outcomes', url: ERouterUrls.FOLLOW_UP_OUTCOMES },
    { name: 'Screen Time Log', url: ERouterUrls.SCREEN_TIME_LOG },
]

const useStyles = makeStyles((theme) => ({
    root: {
      width: '100%',
      maxWidth: 360,
      backgroundColor: theme.palette.background.paper,
    },
    nested: {
      paddingLeft: theme.spacing(4),
    },
    textItem: {
        fontWeight: 'bold',
        color: 'black'
    }
}));

const ReportDropDownMenu = (props: IReportDropDownMenuProps): React.ReactElement => {
    
    const list_classes = useStyles();
    const [open, setOpen] = React.useState(false);
    const [hover, setHover] = React.useState(false);


    const handleClick = () => {
        setOpen(!open);
        if(open) {
            setHover(true);
        }
    };  

    return (
       
       <Grid item xs={12} style={{ textAlign: 'left', border: '1px solid', borderRadius: 8 }}>                     
        <ListItem button onClick={handleClick} style={{ background: '#fafafa', opacity: (hover || open)?0.5: 1 }}  onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}>
            <ListItemText primary={props.name} />
            {open ? <ExpandLess /> : <ExpandMore />}
        </ListItem>   
        <Collapse in={open} timeout="auto" unmountOnExit>
            <List component="div" disablePadding>
            {reportNameArray && reportNameArray.map((report, index) => {
                if(report.name !== props.name) {
                    return (
                        <ListItem button className={list_classes.nested} key={index}>
                            <Link to={report.url} style={{ textDecoration: 'none'}}> 
                                <ListItemText primary={report.name} className={list_classes.textItem}/> 
                            </Link>
                        </ListItem>
                    )
                } else {
                    return (
                        <ListItem button className={list_classes.nested} key={index}>                             
                            <Link to={'/visits-outcomes'} style={{ textDecoration: 'none'}} >
                                <p className={list_classes.textItem}>{report.name}</p>                          
                            </Link> 
                        </ListItem>
                    )
                }
            })}
            </List>
        </Collapse>   
    </Grid>
    )
}

export default ReportDropDownMenu;