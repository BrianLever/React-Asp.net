import React, { useEffect, ChangeEvent } from 'react';
import { useSelector, useDispatch  } from 'react-redux';
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, FormControl, Select, TextField, MenuItem } from '@material-ui/core';
import classes from  '../../pages.module.scss';
import AddIcon from '@material-ui/icons/Add';
import {  CssrsPlusButton, CssrsTextArea, CssrsTextAreaWrapper,CustomMenuItem,CustomSelectTwo, ScreendoxTextInput} from '../../styledComponents';
import ScreendoxSelect from 'components/UI/select';
import { validation } from 'helpers/general';
import { notifyError } from 'actions/settings';


interface SuicideBehaviorRowProps {
    titleText: string;
    descriptionText: string;
    questionText: string | Array<string>;
    totalActivityText:string;
    LifetimeLevel?: boolean | null;
    LifetimeCount?:number | null;
    PastThreeMonths?: boolean | null;
    PastThreeMonthsCount?: number | null;    
    Description?: string | null;
    handleChangeDescription?: (value: string) => void;
    handleChangeLifetimeLevel?: (value: number) => void;
    handleChangeLifetimeCount?: (value: number) => void;
    handleChangePastThreeMonths?: (value: number) => void;    
    handleChangePastThreeMonthsCount?: (value: number) => void;

}

const SuicideBehaviorRow = (props: SuicideBehaviorRowProps): React.ReactElement => {
    const dispatch = useDispatch();
    const [enable, setEnable] = React.useState(!!props.Description);          
    const optionArray=[       
        {name:"Yes",value:1},
        {name:"No",value:2}
    ]   
    const valueToReturn = (value: boolean | null | undefined) => {
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
    React.useEffect(() => {
        setEnable(!!props.Description)
    }, [props])
    return (    
        <TableRow>
            <TableCell colSpan={3} >
                <p className={classes.boldText}>{props.titleText}</p>
                <p>{props.descriptionText}</p>
                { !Array.isArray(props.questionText)?
                <p className={classes.boldText} style={{ fontStyle: 'italic', marginLeft: 10, marginTop: 10 }}>{props.questionText}</p>
                :
                <>
                    { props.questionText && props.questionText.map((text, index) => (
                        <p className={classes.boldText} style={{ fontStyle: 'italic', marginLeft: 10, marginTop: 10 }} key={ index }>{ text }</p>
                    ))}
                </>
                }
                <p style={{ cursor: 'pointer', display: 'flex', marginTop: 10, marginBottom: 10 }} onClick={() => setEnable(!enable)}>
                    <CssrsPlusButton style={{ transform: enable?'rotate(45deg)':'rotate(90deg)' }}/>
                    <span>if yes describe</span>
                </p>
                <CssrsTextAreaWrapper style={{ height: enable?'180px': '0px' }}>
                    <CssrsTextArea style={{  display:enable?'unset': 'none' }}                 
                        onChange={(e) => {
                            props.handleChangeDescription && props.handleChangeDescription(`${e.target.value}`);
                        }}
                        value={props.Description || ""} 
                    >                    
                    </CssrsTextArea>
                </CssrsTextAreaWrapper>
            </TableCell> 
            <TableCell width={'20%'} className={classes.boldText}>              
                <FormControl variant="outlined" fullWidth style={{paddingRight:'120px !important'}}>                                    
                    <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            disabled={valueToReturn(props.LifetimeLevel) !== null && valueToReturn(props.LifetimeLevel)!=3 && valueToReturn(props.LifetimeLevel)!=2}
                            defaultValue={valueToReturn(props.LifetimeLevel)}
                            rootOption={{name:'Select',value:3}}
                            changeHandler={(value: any) => {                               
                                const v = parseInt(`${value}`)
                                props.handleChangeLifetimeLevel && props.handleChangeLifetimeLevel(v)                               
                                                              
                            }}
                    />  
                </FormControl>  
                {props.totalActivityText !== ''?
                    <>
                        <p className={classes.boldText} style={{ textAlign: 'center', marginTop: 50 }}>
                                {props.totalActivityText}
                        </p>
                        <TextField
                                fullWidth
                                margin="dense"                            
                                id="outlined-margin-none"
                                variant="outlined"
                                type="text"  
                                autoComplete='off'                                                                                                                                                                                               
                                value={props.LifetimeCount}  
                                inputProps={{ inputMode: 'numeric', pattern: '[0-9]*' }}                            
                                onChange={(e: any) => {                                                                                                                                       
                                    const value =`${e.target.value}`;  
                                    if(e.target.validity.valid) {                                      
                                        if (!validation(value)){                                      
                                            return false;
                                        }
                                        else{
                                            props.handleChangeLifetimeCount && props.handleChangeLifetimeCount(Number(value));
                                        } 
                                    }  
                                    else{                                        
                                        dispatch(notifyError("Char input is prohibited"));                                     
                                            return;
                                        }                                                                   
                                }
                            }
                        /> 
                    </>:null
                }
            </TableCell>
            <TableCell width={'20%'} className={classes.boldText}>               
                <FormControl variant="outlined" fullWidth>                                     
                    <ScreendoxSelect
                            options={optionArray.map((option: any) => (
                                { name: `${option.name}`, value: option.value}
                            ))}
                            defaultValue={valueToReturn(props.PastThreeMonths)}
                            rootOption={{name:'Select',value:3}}
                            changeHandler={(value: any) => {                               
                                const v = parseInt(`${value}`)
                                props.handleChangePastThreeMonths && props.handleChangePastThreeMonths(v)                                                                                        
                            }}
                    />  
                </FormControl> 
                {props.totalActivityText !== ''?
                    <>
                        <p className={classes.boldText} style={{ textAlign: 'center', marginTop: 50 }}>
                                {props.totalActivityText}
                        </p>
                        <TextField
                                fullWidth
                                margin="dense"                            
                                id="outlined-margin-none"
                                variant="outlined"  
                                type="text"
                                inputProps={{ inputMode: 'numeric', pattern: '[0-9]*' }} 
                                autoComplete='off'                                                                                                            
                                value={props.PastThreeMonthsCount}                             
                                onChange={(e: any) => {                                  
                                    const value = `${e.target.value}`;   
                                    if(e.target.validity.valid) {                                      
                                        if (!validation(value)){                                      
                                            return false;
                                        }
                                        else{
                                            props.handleChangePastThreeMonthsCount && props.handleChangePastThreeMonthsCount(Number(value)) 
                                        } 
                                    }  
                                    else{                                        
                                        dispatch(notifyError("Char input is prohibited"));                                       
                                            return;
                                        }                                                                                                                                                                                                                                                                    
                                }}
                        /> 
                       
                    </>:null
                } 
            </TableCell>
        </TableRow>
    )
}

export default SuicideBehaviorRow;