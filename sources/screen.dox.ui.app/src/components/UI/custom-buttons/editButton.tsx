import React from 'react';
import styled from 'styled-components';
import backgroundImage from 'assets/pencil.svg';
import hoverBackgroundImage from 'assets/pencil-hover.svg';

const ContainerInner = styled.div`
    flex-direction: row;
    justify-content: center;
    align-items: center;
`;


export const EditButton = styled.i`
    display: flex;
    width: 54px;
    height: 45px;
    color: #2e2e42;
    font-size: 14px;
    position: relative;
    background-size: 42px;
    cursor: pointer;
    background-repeat: no-repeat, repeat;
    background-image: url(${backgroundImage});
    &:hover {
        background-image: url(${hoverBackgroundImage});
    }
`; 


export type TScreendoxEditButton = {
    children?: any;
    onClickHandler?: (e: React.MouseEvent<HTMLElement>) => void;
}

const ScreendoxEditButton = (props: TScreendoxEditButton): React.ReactElement => {
    const { children, onClickHandler } = props;
    return (
        <ContainerInner>
            <EditButton
                onClick={onClickHandler}
            >
                { children }
            </EditButton>
        </ContainerInner>
    )
}

export default ScreendoxEditButton;