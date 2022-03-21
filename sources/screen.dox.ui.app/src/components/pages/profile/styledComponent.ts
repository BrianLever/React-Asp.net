import styled, { css }  from 'styled-components';
import { makeStyles } from '@material-ui/core/styles';

export const ProfileTitle = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 2em;
    font-style: normal;
    font-weight: 700;
    line-height: 1.2;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: rgb(46,46,66);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const ProfileLargeBoldText = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 1.2em;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: rgb(46,46,66);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const ProfileLargeText = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 1.2em;
    font-style: normal;
    font-weight: 500;
    line-height: 1.4;
    letter-spacing: 0em;
    text-transform: none;
    color: rgb(46,46,66);
    background-color: transparent;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const ProfileSmallText = styled.span`
    font-family: inherit;
    font-size: 14px;
    font-style: normal;
    font-weight: 400;
    line-height: 1.6;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: none;
    color: rgb(46,46,66);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    display: block;
    margin-left: 50px;
`

export const TextArea = styled.textarea`
    font-size: 1.2em !important;
    border: 1px solid rgb(46, 46, 46);
    width: 100%;
    min-height: 120px;
    background: transparent;
    z-index: 999;
    color: rgb(46, 46, 46);
`

export  const useStyles = makeStyles((theme) => ({
    profileContainer: {
        zIndex: 1,
        borderWidth: "0px 1px 0px 0px",
        borderStyle: 'none solid none none',
        borderColor: "transparent rgb(237,237,242) transparent transparent",
        borderRadius: '10px 0px 0px 10px',
        padding: '50px 60px 60px 60px',
        fontSize: "1em",
        backgroundColor: "rgb(255,255,255)"
    },
    buttonStyle: {
        minWidth: '10px',
        borderRadius: '0.35em',
        fontSize: '14px',
        backgroundColor: 'rgb(46,46,66)',
        width: 161,
        height: 40,
        boxShadow: 'none',
        '&:hover': {
            boxShadow: 'none',
        },
        '&:focus': {
            boxShadow: 'none',
        },
    },
    buttonTextStyle: {
        fontSize: '14px',
        fontStyle: 'normal',
        fontWeight: 700,
        lineHeight: 1,
        color: 'rgb(255,255,255)',
        textTransform: 'none'
    }
}))