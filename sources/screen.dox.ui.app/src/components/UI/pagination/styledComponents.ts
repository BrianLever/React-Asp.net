import styled from 'styled-components';
import { Button } from '@material-ui/core';


export const NUMBER_TO_SHOW = 10;
export const FIRST_TEXT = 'First';
export const LAST_TEXT = 'Last';
export const PREVIOUS_TEXT = 'Previous';
export const NEXT_TEXT = 'Next';

export const List = styled('ul')({
    listStyle: 'none',
    padding: 0,
    margin: 0,
    display: 'flex',
});

type IEllipsisButtonTextStyle = {
    selected: boolean;
}


type IPageButtonTextStyle = {
    selected: boolean;
}

export const   EllipsisButtonTextStyle = styled.p`
    text-transform: none;
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    font-weight: 700;
    line-height: 1;
    text-decoration: underline;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: 5px;
    ${({ selected }: IEllipsisButtonTextStyle) => `${ selected ? `color: rgba(46,55,66,0.5)`:`color: rgb(46,46,66)`}`};
`

export const   PageButtonTextStyle = styled.p`
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    font-weight: 700;
    line-height: 1;
    color: rgba(46,55,66,0.5);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    flex-direction: row;
    justify-content: center;
    align-items: center;
    ${({ selected }: IPageButtonTextStyle) => `${ selected ? `color: rgb(46,46,66)`:`color: rgba(46,55,66,0.5)`}`};
    &:hover {
        color: rgb(46,46,66);
    }
`