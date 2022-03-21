import React, { CSSProperties } from 'react';
import styled from 'styled-components';


export type TScreendoxHeading = {
    size?: '16px;' | '16px;' | '1.2em' | '1.3em' | '1.4em' | '1.5em' | '1.6em'; 
    bold?: '700' | '400';
    children?: React.ReactElement | React.ReactElement[] | React.ReactChildren | React.ReactChild;
    [k: string] : any;
}

const ScreendoxHeadingComponent =  styled.h1`
    font-family: 'hero-new', sans-serif;
    ${({ size }: TScreendoxHeading) => `font-size: ${ size ? size: '1.4em' }`};
    ${({ bold }: TScreendoxHeading) => `font-weight: ${ bold ? bold: 700 }`};
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

const ScreendoxHeading = (props: TScreendoxHeading): React.ReactElement => {
    return (
        <ScreendoxHeadingComponent 
            size={props.size}
            bold={props.bold}
            {...props}
        >
            { props.children }
        </ScreendoxHeadingComponent>
    )
}

export const commonTextFontStyle: CSSProperties = {
   
    fontSize: '14px',
    fontStyle: 'normal',
    fontWeight: 400,
    lineHeight: '1.4',
    letterSpacing: '0em',
    textTransform: 'none',
    color: '#2e2e42',
  };


export default ScreendoxHeading;