import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import {  CssrsPlusButton, CssrsTextArea, CssrsTextAreaWrapper,CustomMenuItem,CustomSelect } from '../../styledComponents';
import {  TableCell, TableRow, FormControl, Select, TextField, MenuItem, InputLabel } from '@material-ui/core';
import styled from "styled-components";
import classes from  '../../pages.module.scss';
import AddIcon from '@material-ui/icons/Add';
import Autocomplete from '@material-ui/lab/Autocomplete';
import { BorderAll } from '@material-ui/icons';
import ScreendoxSelect from 'components/UI/select';
import { cssrsReportDetailSelector } from 'selectors/c-ssrs-list/c-ssrs-report';

interface SuicidalIdeationRowProps {
    titleText: string;
    descriptionText: string;
    questionText: string;
    LifetimeMostSucidal?: boolean | null;
    PastLastMonth?: boolean | null;
    Description?: string | null;
    handleChangeDescription?: (value: string) => void;
    handleChangeLifeTimeMostSucidal?: (value: number | null) => void;
    handleChangePastLastMonth?: (value: number) => void;
}

const SuicidalIdeationRow = (props: SuicidalIdeationRowProps): React.ReactElement => {
    const dispatch = useDispatch();
    const [enable, setEnable] = React.useState(!!props.Description);
    const cssrsReport = useSelector(cssrsReportDetailSelector);
    
    const optionArray=[       
        {name:"Yes",value:1},
        {name:"No",value:2}
    ]
    const valueToReturn = (value: boolean | null | undefined) => { console.log(value)
        if(typeof value === 'boolean') {
            if(value) {
                return 1;
            } else {
               
                return 2
            }
        } else {
            return 3;
        }
    }
    console.log(props.LifetimeMostSucidal)
    React.useEffect(() => {
        setEnable(!!props.Description)
    }, [props])    

    return (    
        <TableRow >
            <TableCell width={'60%'} style={{ fontSize: 14,borderRight: "1px solid #e0e0e0",borderLeft:"1px solid #e0e0e0",borderBottom:"1px solid #e0e0e0"}}>
                <p className={classes.boldText}>{props.titleText}</p>
                <p>{props.descriptionText}</p>
                <p className={classes.boldText} style={{ fontStyle: 'italic', marginLeft: 10, marginTop: 10 }}>{props.questionText}</p>
                <p style={{ cursor: 'pointer', display: 'flex', marginTop: 10, marginBottom: 10 }} onClick={() => setEnable(!enable)}>
                    <CssrsPlusButton style={{ transform: enable?'rotate(45deg)':'rotate(90deg)' }}/>
                    <span>if yes describe</span>
                </p>
                <CssrsTextAreaWrapper style={{ height: enable?'180px': '0px' }}>
                    <CssrsTextArea 
                        style={{  display:enable?'unset': 'none' }} 
                        onChange={(e) => {
                            e.stopPropagation();
                            props.handleChangeDescription && props.handleChangeDescription(`${e.target.value}`);
                        }}
                        value={props.Description || ""} 
                    >
                    </CssrsTextArea>
                </CssrsTextAreaWrapper>
            </TableCell> 
            <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center',borderRight: "1px solid #e0e0e0"}}>               
                <FormControl variant="outlined" style={{minWidth:120}}>                     
                    <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            disabled={valueToReturn(props.LifetimeMostSucidal) !== null && valueToReturn(props.LifetimeMostSucidal)!=3 && valueToReturn(props.LifetimeMostSucidal)!=2 }
                            defaultValue={valueToReturn(props.LifetimeMostSucidal)}
                            rootOption={{name:'Select',value:3}}
                            changeHandler={(value: any) => {                               
                                const v = parseInt(`${value}`)
                                props.handleChangeLifeTimeMostSucidal && props.handleChangeLifeTimeMostSucidal(v)                                
                                                              
                            }}
                    />                                  
                </FormControl>                            
            </TableCell>
            <TableCell width={'20%'} className={classes.boldText} style={{ textAlign: 'center',borderRight: "1px solid #e0e0e0"}}>                
                <FormControl variant="outlined" style={{minWidth:120}}>                                   
                    <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`.slice(0, 20), value: option.value}
                            ))}                          
                            defaultValue={valueToReturn(props.PastLastMonth)}
                            rootOption={{name:'Select',value:3}}
                            changeHandler={(value: any) => {
                                const v = parseInt(`${value}`);
                                props.handleChangePastLastMonth && props.handleChangePastLastMonth(v);                               
                            }}
                    />  
                </FormControl>  
            </TableCell>
        </TableRow>
    )
}

export default SuicidalIdeationRow;