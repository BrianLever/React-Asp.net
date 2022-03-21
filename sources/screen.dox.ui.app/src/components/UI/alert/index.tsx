import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {  Box, Collapse } from '@material-ui/core';
import CloseIcon from '@material-ui/icons/Close';
import styled from 'styled-components';
import { isEhrSystemExpiredAlertSelector, getEhrSystemExpiredAlertMessageSelector  } from '../../../selectors/settings';


const AlertContainer = styled.div`
    margin: 0em 0em 2em 0em;
    border-width: 1px;
    border-style: solid;
    border-color: #db9d47;
    border-radius: 3px;
    padding: 1em 2.75em 1em 1.15em;
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    font-weight: 400;
    line-height: 1.5;
    text-shadow: none;
    color: rgb(46,46,66);
    background-color: rgba(219,157,71,0.25);
    box-shadow: none;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    position: relative;
    transition: opacity .3s ease;
`

const CustomAlert = (): React.ReactElement => {
    const [open, setOpen] = React.useState(true);
    const isEhrSystemAlert: boolean = useSelector(isEhrSystemExpiredAlertSelector);
    const alertMessage: string = useSelector(getEhrSystemExpiredAlertMessageSelector);

    return (
        <Box style={{ fontSize: 14 }}>
            {isEhrSystemAlert?
            <Collapse in={open}>
                <AlertContainer>
                    <p>{alertMessage}</p>
                    <CloseIcon onClick={() => setOpen(false)} 
                        style={{ position: 'absolute', top: 4, right: 4, cursor: 'pointer' }}
                    />
                </AlertContainer>
            </Collapse>:null}
        </Box>
    )
    
}


export default CustomAlert;