import React from 'react';
import { Grid } from '@material-ui/core';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { getCurrentPagePathSelector } from '../../../../selectors/settings';
import { Logout } from 'helpers/auth';
import { useHistory, Redirect } from 'react-router';
import { logoutRequest } from 'actions/login';
import config from 'config/app.json';
import { ERouterUrls } from 'router';
import { EModalWindowKeys, openModalWindow } from 'actions/settings';
import { getSystemSettingsRequest } from 'actions/dashboard';


export const TitleContentContainer = styled.div`
    padding: 0em 0em 1em 1em;
    font-size: 1em;
    background-color: transparent;
    border-left: 5px solid transparent;
`;

export const TitleText = styled.h1`
    font-family: "hero-new",sans-serif;
    font-size: 16px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    text-transform: uppercase;
    color: rgb(46,46,66);
`;

export const LogoutContent = styled.div`
    font-family: "hero-new",sans-serif;
    font-size: 16px;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    margin-right: calc(0em * -1);
    color: white;
    position: absolute;
    width: 100%;
    background: rgb(46,46,66);
    bottom: 0;
    height: 43px;
    left: 0;
    text-align: left;
    padding-left: 45px;
    padding-top: 10px;
    cursor: pointer;

`
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

export const ScreendoxContentItemContainer = styled.div`
    margin: 0em 0em 1em 0em;
    cursor: pointer;
    border-left: 5px solid transparent;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
`;

export const ScreendoxItemContainer = styled.div`
    align-items: center;
    padding: 0.7em 0.7em 0.7em 1em;
`;

export const ColumdText = styled.span`
    font-size: 16px;
    font-style: normal;
    font-weight: 400;
    line-height: 1;
    color: #2e2e42;
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
        width: 35%;
        padding-top: 3px;
        border-bottom: 3px solid #2e2e42;
        transition: .35s;
    }
`;


const ProfileMenuContent = (props: { onClick?: () => void; }) => {

    const path  = useSelector(getCurrentPagePathSelector);
    const history = useHistory();
    const dispatch = useDispatch();
    
    return <div style={{ width: '250px', padding: '25px 20px 25px 25px' }}>
                <Grid container alignItems="flex-start">
                    <TitleContentContainer>
                        <TitleText>
                            USER
                        </TitleText>
                    </TitleContentContainer>
                </Grid>
                <Grid item>
                    <Link 
                        to={ERouterUrls.PROFILE || '#'} 
                        style={{ color: 'inherit', textDecoration: 'inherit' }}
                        onClick={() => {
                            props.onClick && props.onClick();
                        }}
                    >
                        <ContentItemContainer selected={path === ERouterUrls.PROFILE}>
                            <ItemContainer selected={path === ERouterUrls.PROFILE}>
                                <ItemText>
                                   My Profile
                                </ItemText>
                            </ItemContainer>
                        </ContentItemContainer>
                    </Link>
                </Grid>
                <Grid container alignItems="flex-start" style={{ marginTop: 40 }}>
                    <TitleContentContainer>
                        <TitleText>
                            SCREENDOX
                        </TitleText>
                    </TitleContentContainer>
                </Grid>
               
                <Grid item>
                    <a 
                        href={config.SUPPORT_LINK}
                        style={{ color: 'inherit', textDecoration: 'inherit' }}
                        target="_blank"
                        onClick={() => {
                            props.onClick && props.onClick();
                        }}
                    >
                        <ScreendoxContentItemContainer>
                            <ScreendoxItemContainer>
                                <ColumdText>
                                   Support
                                </ColumdText>
                            </ScreendoxItemContainer>
                        </ScreendoxContentItemContainer>
                    </a>
                </Grid>
                <Grid item style={{ marginBottom: 45 }}>
                    <Link 
                        to={'#'} 
                        style={{ color: 'inherit', textDecoration: 'inherit' }}
                        onClick={() => {
                            props.onClick && props.onClick();
                            dispatch(openModalWindow(EModalWindowKeys.screendoxAbout))
                        }}
                    >
                        <ScreendoxContentItemContainer>
                            <ScreendoxItemContainer>
                                <ColumdText>
                                    About
                                </ColumdText>
                            </ScreendoxItemContainer>
                        </ScreendoxContentItemContainer>
                    </Link>
                </Grid>
                <LogoutContent onClick={() => { 
                    props.onClick && props.onClick();
                    dispatch(logoutRequest());
                    setTimeout(() => {
                        Logout();history.push(ERouterUrls.LOGIN)
                    }, 1000)
                }}>
                    Logout
                </LogoutContent>
            </div>;
}

export default ProfileMenuContent;