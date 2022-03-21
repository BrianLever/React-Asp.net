import styled, { css }  from 'styled-components';
import { MenuItem, Select } from '@material-ui/core';

export const CustomMenuItem = styled(MenuItem)`
    &.MuiMenuItem-root{        
        background: transparent;
        border: 0px solid var(--main-dark-color);
        color: #2e2e42;
        font-family: 'Hero New',sans-serif;
        font-size: 1rem;
        font-weight: normal;
        display: block;
        white-space: nowrap;        
        padding: 0px 10px;       
    }    
`
export const CustomSelect = styled(Select)`       
    .MuiSelect-outlined.MuiSelect-outlined {
        padding-right: 60px !important
      }     
    }
  
`
export const CustomSelectTwo = styled(Select)`       
    .MuiSelect-outlined.MuiSelect-outlined {
        padding-right: 150px !important
      }   
  
`