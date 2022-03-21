import React from 'react';
import { Grid } from '@material-ui/core';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { getCurrentPagePathSelector } from '../../../../selectors/settings';

export const TitleContentContainer = styled.div`
    padding: 0em 0em 1em 1em;
    font-size: 1em;
    background-color: transparent;
    border-left: 5px solid transparent;
`;

export const TitleText = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 1em;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: uppercase;
    color: rgb(46,46,66);
`;

type TContentItemContainer = {
    selected: boolean;
}
export const ContentItemContainer = styled.div`
    margin: 0em 0em 1em 0em;
    cursor: pointer;
    border-left: 5px solid transparent;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    ${({ selected }: TContentItemContainer) => `background-color: ${ selected ? ` #ededf2` : `#fff` }`};
    ${({ selected }: TContentItemContainer) => `border-left: ${ selected ? ` 5px solid #2e2e42` : `5px solid transparent` }`};
    &:hover {
        background-color: #ededf2;
        border-left: 5px solid #2e2e42;
    } 
`;

type TItemContainer = {
    selected: boolean;
}
export const ItemContainer = styled.div`
    align-items: center;
    padding: 0.7em 0.7em 0.7em 1em;
    ${({ selected }: TItemContainer) => `background-color: ${ selected ? ` #ededf2` : `#fff` }`};
    &:hover {
        background-color: #ededf2;
    }
`;

export const ItemText = styled.span`
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 1;
    color: #2e2e42;
`;

export type TSidebarPoperLabelItems = {
    name: string;
}

export type TSidebarPoperContentItems = {
    name: string;
    url: string;
}

export type TSidebarPoperContentProps = {
    ref?: any;
    titlesarr: Array<TSidebarPoperLabelItems>;
    itemsarr: Array<Array<TSidebarPoperContentItems>>;
    onClick?: () => void;
}

const SidebarPoperContent = (props: TSidebarPoperContentProps) => {

    const { itemsarr, titlesarr, ref } = props;
    const path  = useSelector(getCurrentPagePathSelector);

    const createInnerLink = (index: number): Array<React.ReactElement> | null => (
        Array.isArray(itemsarr[index]) ? itemsarr[index]
        .map((item: TSidebarPoperContentItems, itemIndex: number) => (
            <Grid key={itemIndex} item>
                <Link 
                    to={item.url || '#'} 
                    style={{ color: 'inherit', textDecoration: 'inherit' }}
                    onClick={() => {
                        props.onClick && props.onClick();
                    }}
                >
                    <ContentItemContainer selected={path === item.url}>
                        <ItemContainer selected={path === item.url}>
                            <ItemText>
                                { item.name }
                            </ItemText>
                        </ItemContainer>
                    </ContentItemContainer>
                </Link>
            </Grid>
        )) : null
    );
    return <div style={{ width: '640px', padding: '35px 25px 35px 35px' }} {...props} ref={ref}>
                <Grid container justifyContent="center" alignItems="flex-start">
                    {
                        titlesarr.map((label: TSidebarPoperLabelItems, titleIndex: number) => (
                            <Grid key={titleIndex} item xs={6}>
                                <Grid item xs={12}>
                                    <TitleContentContainer>
                                        <TitleText>
                                            { label.name }
                                        </TitleText>
                                    </TitleContentContainer>
                                </Grid>
                                { createInnerLink(titleIndex) }
                            </Grid>
                        ))
                    }
                </Grid>
            </div>;
}

export default SidebarPoperContent;