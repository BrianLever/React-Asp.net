import React from 'react';
import styled from 'styled-components';

export const ButtonContainer = styled.div`
    border-width: 1px;
    border-style: solid;
    border-color: #2e2e42;
    border-radius: 5px 5px 5px 5px;
    font-size: 16px;
    background-color: #ffffff00;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ButtonContainerInnerBlock = styled.div`
    flex-direction: row;
    justify-content: center;
    align-items: center;
    padding: 0.5em 1em 0.5em 1em;
    height: 100%;
    border-radius: inherit;
    transform: translate3d(0, 0, 0);
    z-index: 2;
    overflow: hidden;
    display: flex;
    flex: 1 0 auto;
    position: relative;
`;


const EditIconButton = (): React.ReactElement => {
    return (
        <div>
            EditIconButton
        </div>
    )
}

export default EditIconButton;