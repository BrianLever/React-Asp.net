import styled from 'styled-components';
import { makeStyles } from '@material-ui/core/styles';

export const ColumdText = styled.a`
    text-decoration: none;
    cursor: pointer;
    border-bottom-width: 3px;
    color: #272727;
    font-family: 'hero-new', sans-serif;
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    &:after {
      content: "";
      display: block;
      width: 0%;
      padding-top: 3px;
      border-bottom: 3px solid #2e2e42;
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
`;

export const ColumdPaginationText = styled.a`
    text-decoration: none;
    cursor: pointer;
    border-bottom-width: 3px;
    color: #272727;
    font-family: 'hero-new', sans-serif;
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
`;

export const TitleText = styled.h1`
  font-family: 'hero-new', sans-serif;
  font-size: 25px;
  font-style: normal;
  font-weight: 700;
  line-height: 1.4;
  letter-spacing: 0em;
  margin-right: calc(0em * -1);
  text-transform: capitalize;
  color: #2e2e42;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;


export const ReportIcon = styled.i`
    display: flex;
    position: relative;
    background-size: 30px;
    width: 30px;
    height: 30px;
    background-image: url(../assets/report.svg);
    // &:hover {
    //     background-image: url(../assets/report-hover.svg);
    // }
`; 

export const ReportBlackIcon =  styled.i`
  display: flex;
  position: relative;
  background-size: 30px;
  width: 30px;
  height: 30px;
  background-image: url(../assets/report-hover.svg);
`

export const ReportText = styled.span`
    font-size: 14px;
    font-style: normal;
    font-weight: 500;
    line-height: 1;
    color: #2e2e42;
  `; 


export const useRowStyles = makeStyles({
  selectedInner: {
    transitionTimingFunction: 'cubic-bezier(0.400,0.000,0.200,1.000)',
    transform: 'translate3d(0, 0, 0)',
    borderWidth: '1px 1px 1px 1px',
    borderStyle: 'solid solid solid solid',
    borderColor: 'rgba(16, 16, 16, 0.95)',
    borderRadius: '0px 0px 5px 5px',
    borderRightWidth: 2,
    '& > td:first-child': {
      borderTopLeftRadius: '10px'
    },
    '& > td:last-child': {
      borderTopRightRadius: '10px'
    },
  },
  innerRoot: {
    '& > *': {
      borderBottom: 'unset',
    },
    borderRadius: '5px 5px 5px 5px',
  },
  root: {
    transitionTimingFunction: 'cubic-bezier(0.400,0.000,0.200,1.000)',
    transform: 'translate3d(0, 0, 0)',
    cursor: 'pointer',
    '& > *': {
      borderBottom: 'unset',
    },
    '&:hover': {
      backgroundColor: '#f5f6f8',
      borderColor: 'transparent transparent rgb(237,237,242) #2e2e42',
      borderLeftStyle: 'solid',
      borderLeftWidth: '10px',
    },
    borderRadius: '5px',
    borderLeftWidth: '10px',
    borderLeftStyle: 'solid',
    borderColor: 'transparent',
    borderBottom: '1px solid #ededf2',
  },
  rootSelected: {
    transitionTimingFunction: 'cubic-bezier(0.400,0.000,0.200,1.000)',
    transform: 'translate3d(0, 0, 0)',
    backgroundColor: '#f5f6f8',
    borderWidth: '1px 1px 1px 1px',
    borderStyle: 'solid solid solid solid',
    borderColor: '#2e2e42',
    borderLeftWidth: '10px',
    borderTopWidth: 1.5,
    borderRightWidth: 2,

    '& > td:first-child': {
      borderTopLeftRadius: '10px'
    },
    '& > td:last-child': {
      borderTopRightRadius: '10px'
    },
    // borderBottomColor: '#c8d3d3',
    '&:hover': {
      transform: 'translate3d(0, 0, 0)',
      transitionTimingFunction: 'cubic-bezier(0.400,0.000,0.200,1.000)',
      backgroundColor: '#f5f6f8',
      borderWidth: '1px 1px 1px 1px',
      borderStyle: 'solid solid solid solid',
      borderColor: '#2e2e42',
      borderLeftWidth: '10px',
      borderTopWidth: 1.5,
      borderRightWidth: 2,
      '& > td:first-child': {
        borderTopLeftRadius: '10px'
      },
      '& > td:last-child': {
        borderTopRightRadius: '10px'
      },
      // borderBottomColor: '#c8d3d3',
    },
  }
});



export const TableHeaderContainer = styled.div`
  z-index: 1;
  margin: 0px auto 0px auto;
  border-radius: 5px;
  padding: 20px 15px 15px 45px;
  font-size: 1em;
  background-color: rgb(237,237,242);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  display: flex;
  position: relative;
  flex-flow: row nowrap;
  justify-content: center;
  align-items: stretch;
  width: auto;
  min-width: 0;
  max-width: none;
  height: auto;
  min-height: 0;
  max-height: none;
`

export const TableBodyContainer = styled.div`
  z-index: 1;
  margin: 0px auto 0px auto;
  padding: 1px;
  font-size: 1em;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const TableHeaderRowInner = styled.div`
  flex-direction: row;
  justify-content: center;
  align-items: stretch;
  align-content: stretch;
  margin: calc((0rem / 2) * -1) calc((0rem / 2) * -1);
  display: flex;
  flex-wrap: wrap;
  flex-grow: 1;
  flex-shrink: 1;
  flex-basis: auto;
  min-width: 0;
  min-height: 0;
`

export const TableHeaderColInner = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  flex-wrap: wrap;
  align-content: flex-start;
  z-index: 1;
  font-size: 1em;
  background-color: transparent;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  flex-grow: 1;
  margin: calc(0rem / 2) calc(0rem / 2);
`

export const TablebodyList = styled.div`
  width: 100%;
  font-size: 1em;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`

export const TableBodyRow = styled.div`
  overflow: hidden;
  background-color: rgba(255,255,255,0);
  display: flex;
  flex-flow: column nowrap;
  justify-content: flex-start;
  align-items: stretch;
  position: relative;
  z-index: 1;
  transform: translate3d(0, 0, 0);

`


type TTableBodyRowButton = {
  selected: boolean;
}


export const TableBodyRowButton = styled.button`
  outline: none;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  border-width: 0px 0px 1px 5px;
  border-style: none none solid solid;
  border-color: transparent transparent rgb(237,237,242) transparent;
  padding: 25px 15px 25px 15px;
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 400;
  line-height: 1.3;
  cursor: pointer;
  text-align: left;
  display: flex;
  color: rgb(46,46,66);
  transition-duration: 300ms;
  ${({ selected }: TTableBodyRowButton) => `${ selected ? `border-width: 1px 1px 0px 5px !important`:``}`};
  ${({ selected }: TTableBodyRowButton) =>  `${ selected ? `border-style: solid !important`:``}`};
  ${({ selected }: TTableBodyRowButton) =>  `${ selected ? `border-color: rgb(46,46,66) !important`:``}`};
  ${({ selected }: TTableBodyRowButton) =>  `${ selected ? `border-radius: 5px 5px 0px 0px !important`:``}`};
  ${({ selected }: TTableBodyRowButton) =>  `${ selected ? `background-color: #f5f6f8`:`background-color: rgba(255,255,255,0)`}`};
  &:hover {
    background-color: #f5f6f8;
    padding: 25px 15px 25px 15px;
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    display: flex;
    font-weight: 400;
    line-height: 1.3;
    cursor: pointer;
    text-align: left;
    color: rgb(46,46,66);
    border-left: 5px solid rgb(46,46,66);
  }
`

export const TableBodyRowButtonSpan = styled.span`
  pointer-events: none;
  display: flex;
  flex-flow: row nowrap;
  justify-content: flex-start;
  align-items: center;
  box-sizing: border-box;

`

export const IndicatorSpan = styled.span`
  transform: translate3d(0,0,0) rotate(45deg);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  width: auto;
  height: 1em;
  font-size: 1em;
  color: rgb(46,46,66);
`
export const TableBodyColSpan = styled.span`
  margin-left: 1em;
  flex: 1 0 0%;
  pointer-events: none;
  display: flex;
  transition: all 0.3s ease-in-out;
  margin: 0px;
  padding: 0px;
  background-color: transparent;
  z-index: 1;
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`
type TTableBodyTabPanel = {
  selected: boolean;
}


export const TableBodyTabPanel = styled.div`
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
  transition-duration: 300ms;
  transition-property: height;
  ${({ selected }: TTableBodyTabPanel) => `${ selected ? `display: block`:`display: none`}`};
`
export const TableBodyTabPanelContent = styled.div`
  border-width: 1px 1px 1px 1px;
  border-style: solid solid solid solid;
  border-radius: 0px 0px 5px 5px;
  padding: 20px 10px 20px 35px;
  font-family: "hero-new",sans-serif;
  font-size: 1em;
  font-style: normal;
  font-weight: 400;
  line-height: 1.6;
  color: rgb(46,46,66);
`

export const TableBodyAnchor = styled.div`
  border-radius: 0.35em;
  font-size: 1em;
  background-color: rgba(0,0,0,0);
  box-shadow: 0em 0.15em 0.65em 0em rgb(0 0 0 / 0%);
  transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`
export const TableBodyAnchorContent = styled.div`
  flex-direction: row;
  justify-content: left;
  align-items: center;
  display: flex;
`
export const TableBodyAnchorGraphic = styled.span`
    margin: 5px 10px 5px 0px;
    display: inline-flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: flex-start;
    flex-shrink: 0;
    position: relative;
    letter-spacing: 0;
    line-height: 1;
    z-index: 2;
    flex-flow: row nowrap;
    -webkit-box-pack: start;
`
export const TableBodyAnchorText = styled.span`
    display: flex;
    align-items: center;
    flex-shrink: 1;
    min-width: 1px;
    max-width: 100%;
`

export const TableBodyCol = styled.div`
    z-index: 1;
    font-size: 1em;
    background-color: transparent;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    flex-grow: 1;
    margin: calc(1rem / 2) calc(0rem / 2);
`


export const LoadingContainer = styled.div`
  height: 120px;
  border: 1px solid rgb(46,46,66);
`