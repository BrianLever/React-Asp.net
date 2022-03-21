import React from 'react';
import styled, { css }  from 'styled-components';
import { CustomSelect, CustomMenuItem } from './styledComponents';
import { EMPTY_LIST_VALUE } from '../../../helpers/general';
import {  TableCell, TableRow, FormControl, Select, TextField, MenuItem, InputLabel } from '@material-ui/core';


export type TScreendoxSelect = {
  options?: Array<{name: any, value: any}>;
  defaultValue?: any;
  rootOption?: { name: any, value: any };
  changeHandler?: (value: any, e?: React.ChangeEvent<{ name?: string; value: unknown }>) => void;
  rootOptionDisabled?: boolean;
  disabled?:boolean;
  isError?: boolean;
}


const ScreendoxSelect = (props: TScreendoxSelect): React.ReactElement => {
    const { options, defaultValue, rootOption, changeHandler, rootOptionDisabled = false, disabled = false, isError = false } = props;

    return (
        <FormControl variant="outlined" fullWidth style={{minWidth:100,minHeight:40}}>    
            <CustomSelect  
                margin="dense"
                MenuProps={{
                    anchorOrigin: {
                        vertical: "bottom",
                        horizontal: "left"
                    },
                    transformOrigin: {
                        vertical: "top",
                        horizontal: "left"
                    },
                    getContentAnchorEl: null,
                    PopoverClasses: {
                        paper: "My-Select-Menu",                                                        
                    }
                                                
                }}                                          
                value={defaultValue?defaultValue:rootOption?.value}
                onChange={(event: React.ChangeEvent<{ name?: string; value: unknown }>) => {                           
                        try {
                            const value = `${event.target.value}`;
                            changeHandler && changeHandler(value, event)
                        } catch(e) {}                            
                }}
                disabled={disabled}
                error={isError}

            >   
                {!rootOptionDisabled?
                    <CustomMenuItem value={rootOption?.value}>{rootOption?.name}</CustomMenuItem>:null
                }
                {options && (options?.length > 0) && options.map((option, index) => 
                    <CustomMenuItem value={option.value} key={index}>{option.name}</CustomMenuItem>
                )}
            </CustomSelect>                                  
        </FormControl>
    )
}

export default ScreendoxSelect;