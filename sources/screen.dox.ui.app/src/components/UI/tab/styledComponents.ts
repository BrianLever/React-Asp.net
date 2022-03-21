import styled, { css }  from 'styled-components';
import { makeStyles  } from '@material-ui/core/styles';

type ITabsButton  = {
    selected: boolean;
}

export const TabsListContainer = styled.div`
    margin: 0px 0px -1px 0px;
    display: flex;
    flex-flow: row nowrap;
    justify-content: space-between;
    align-items: stretch;
    position: relative;
    z-index: 2;
    overflow-x: auto;
    overflow-y: hidden;
    font-size: 1em;
    &:before: {
        content: "";
        display: block;
        width: 0;
        height: 0;
        visibility: hidden;
    }
`

export const TabsLists = styled.ul`
    justify-content: flex-start;
    display: flex;
    flex-flow: inherit;
    box-sizing: border-box;
    font-size: 1em;
    align-items: stretch;
    flex: 1 0 0%;
    margin: 0;
    padding: 0;
    list-style: none;
    &:after {
        content: "";
        display: block;
        width: 0;
        height: 0;
        visibility: hidden;
    }
`
export const TabsList = styled.li`
    display: flex;
    font-size: 1em;
    justify-content: flex-start;
    align-items: stretch;
`

export const TabsButton = styled.button`
    margin: 0px 40px 0px 0px;
    padding: 0.75rem 0rem 0.75rem 0rem;
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    font-weight: 700;
    line-height: 1;
    text-transform: uppercase;
    color: rgba(46,55,66,0.3);
    background-color: transparent;
    flex: 1 0 0%;
    display: block;
    border: 0;
    border-radius: 0;
    white-space: nowrap;
    cursor: pointer;
    color: rgb(46,46,66);
    ${({ selected }: ITabsButton) => `${ selected ? `color: rgb(46,46,66)`:`color: rgba(46,55,66,0.3)`}`};
    &:after {
        content: "";
        display: block;
        ${({ selected }: ITabsButton) => `${ selected ? `width: 100%;`:`width: 0%;`}`};
        box-sizing: border-box;
        padding-top: 5px;
        border-bottom: 3px solid rgb(46,46,66);
        transition: .35s;
    }
    &:hover {
        &:after {
          content: "";
          display: block;
          width: 100%;
          padding-top: 3px;
          border-bottom: 3px solid #2e2e42;
          transition: .35s;
    }
`

export const TabsText = styled.span`
    font-size: 1em;
    color: inherit;
    font-weight: inherit;
`