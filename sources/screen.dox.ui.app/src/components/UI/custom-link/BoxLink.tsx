import React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';


const BoxLinkContainer =  styled.div`
    animation-duration: 1000ms;
    overflow: hidden;
    display: inline-flex;
    flex-flow: column nowrap;
    justify-content: stretch;
    position: relative;
    min-width: 1px;
    cursor: pointer;
    transform: scale3d(1.05,1.05,1.05);
    outline: none;
    width: 100%;
    min-height: 270px;
    border-radius: 16px;;
    font-size: 16px;;
    background-color: transparent;
    border-radius;
    border: 2px solid #2e2e42;
    transform: scale(1);
    transform-style: preserve-3d;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    &:hover {
        transform: scale3d(1.05,1.05,1.05);
        background-color: #fff;
    }
`;

const BoxContainerItem = styled.div`
    overflow: hidden;
    display: flex;
    flex: 1 0 auto;
    position: relative;
    z-index: 2;
    height: 100%;
    border-radius: inherit;
    transform: translate3d(0, 0, 0);
    flex-direction: row;
    justify-content: center;
    align-items: center;
    padding: 0.575em 0.85em 0.575em 0.85em;
`;

const BoxImage = styled.img`
    font-size: 16px;;
    cursor: pointer;
    display: block;
    max-width: 100%;
    height: auto;
    vertical-align: bottom;
    border: 0;
    width: 120px;
`;

export type TBoxLinkProps = {
    link: string;
    src: string;
}

const BoxLink = (props: TBoxLinkProps): React.ReactElement => {
    return (
        <Link to={props.link}  style={{ color: 'inherit', textDecoration: 'inherit' }} >
            <div style={{ flexBasis: 'calc(25% - 3rem)' }}>
                <BoxLinkContainer>
                    <BoxContainerItem>
                        <BoxImage src={props.src} width="0" height="0"/>
                    </BoxContainerItem>
                </BoxLinkContainer>
            </div>
        </Link>
    )
}

export default BoxLink;