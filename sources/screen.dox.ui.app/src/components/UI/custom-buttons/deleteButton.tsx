import React from 'react';
import styled from 'styled-components';
import backgroundImage from 'assets/delete.png';
import hoverBackgroundImage from 'assets/hover-delete.png';

const ContainerInner = styled.div`
    flex-direction: row;
    justify-content: center;
    align-items: center;
`;


export const DeleteButton = styled.i`
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


export type TScreendoxDeleteButton = {
    children?: any;
    onClickHandler?: (e: React.MouseEvent<HTMLElement>) => void;
}

const ScreendoxDeleteButton = (props: TScreendoxDeleteButton): React.ReactElement => {
    const { children, onClickHandler } = props;
    return (
        <ContainerInner>
            <DeleteButton
                onClick={onClickHandler}
            >
                { children }
            </DeleteButton>
        </ContainerInner>
    )
}

export default ScreendoxDeleteButton;