import styled from 'styled-components';


export const CustomDateContainer = styled.div`
    display: flex;
    flex-flow: column nowrap;
    justify-content: flex-start;
    align-items: stretch;
    position: relative;
    z-index: 1;
    transform: translate3d(0, 0, 0);
    overflow: hidden;
    border-width: 1px;
    border-style: solid;
    border-color: #2e2e42;
    border-radius: 5px 5px 5px 5px;
    background-color: rgba(255,255,255,0);
`;

export const CustomDateCheckbox = styled.div`
    transform: translate3d(0, 0, 0);
    display: block;
    position: relative;
    z-index: 2;
    margin: 0;
    border: 0;
    border-radius: 0;
    transition-property: color,border-color,background-color,text-shadow,box-shadow;
    padding: 10px 15px 10px 10px;
    font-size: 16px;;
    font-style: normal;
    font-weight: 400;
    line-height: 1.3;
    text-align: left;
    color: #2e2e42;
    background-color: #f5f6f8;
    outline: none;
`;


export const CustomDateCheckboxSpan = styled.span`
    font-family: 'hero-new', sans-serif;
    font-size: 16px;;
    font-style: normal;
    font-weight: 400;
    line-height: 1.3;
    text-align: left;
    color: #2e2e42;
    box-sizing: border-box;
    display: flex;
    flex-flow: row nowrap;
    justify-content: flex-start;
    align-items: center;
    pointer-events: none;
    flex-direction: row-reverse;
`;

export const CustomDateCheckboxSpanIcon = styled.span`
    transform: translate3d(0,0,0) rotate(90deg);
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    width: auto;
    height: 16px;;
    font-size: 1.5em;
    color: #2e2e42;
`;

export const CustomDateCheckboxIcon = styled.i`
    font-family: "FontAwesomeRegular" !important;
    font-style: normal;
    font-weight: 400;
    text-decoration: inherit;
    text-rendering: auto;
    -webkit-font-smoothing: antialiased;
    &:before {
        font-family: "Font Awesome 5 Free";
        font-weight: 900;
        content: "\f192";
        color: 
    }
`;
