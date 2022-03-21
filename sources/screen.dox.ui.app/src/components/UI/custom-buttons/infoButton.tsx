import React from 'react';
import styled from 'styled-components';
import backgroundImage from 'assets/info.png';

const ContainerInner = styled.div`
    flex-direction: row;
    justify-content: center;
    align-items: center;
`;


export const DeleteButton = styled.i`
    display: flex;
    width: 30px;
    height: 30px;
    color: #2e2e42;
    font-size: 14px;
    position: relative;
    background-size: 30px;
    cursor: pointer;
    background-repeat: no-repeat, repeat;
    background-image: url(${backgroundImage});
    margin-top: 2px;
    margin-left: 31px;
`; 


export type TScreendoxInfoButton = {
    children?: any;
    onClickHandler?: (e: React.MouseEvent<HTMLElement>) => void;
}

const ScreendoxInfoButton = (props: TScreendoxInfoButton): React.ReactElement => {
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

export default ScreendoxInfoButton;