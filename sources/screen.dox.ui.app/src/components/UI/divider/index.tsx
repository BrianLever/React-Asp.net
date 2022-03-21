import React from 'react';
import styled from 'styled-components';

const ScreendoxStick =  styled.hr`
    height: 20px;
    max-height: none;
    border-width: 0 0 0 1px;
    border-style: solid;
    border-color: #2e2e42;
    font-size: 16px;;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    `;
    
    const ScreendoxStickContainer =  styled.div`
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    flex: 0 1 auto;
    z-index: 1;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin-left: 10px;
    margin-right: 10px;
    padding-top: 8px;
`;

const ScreendoxDivider = (): React.ReactElement => (
    <ScreendoxStickContainer>
        <ScreendoxStick />
    </ScreendoxStickContainer>
);

export default ScreendoxDivider;
