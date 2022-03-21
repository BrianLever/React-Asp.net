import React, { CSSProperties } from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { useSelector } from 'react-redux';
import DropDownTreeComponent from './tree';
import { IPage } from '../../../../common/types/page';
import { getCurrentPageKeySelector, getCurrentPagePathSelector } from '../../../../selectors/settings';
import { EAssessmentRouterKeys, EFollowUpRouterKeys, EReportsRouterKeys, ERouterKeys, EScreenRouterKeys, EVisitRouterKeys } from 'router';

const TreeCustomUL = styled.ul`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    font-size: 16px;
    transition-property: opacity,filter,transform;
    min-width: 1px;
    margin: 0;
    padding: 0;
    list-style: none;
`;

const TreeCustomLI = styled.li`
    width: inherit;
    position: relative;
    box-sizing: border-box;
    font-size: 16px;
    margin: 0;
    padding: 0;
    list-style: none;
    font-style: normal;
    font-weight: 400;
    color: #999999;
    cursor: pointer;
`;

const TreeCustomA = styled.div`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: 0em 1.75em 1.25em 1.75em;
    border-radius: 0.5em;
    font-size: 16px;
    background-color: #ffffff00;
    // display: flex;
`;

const TreeCustomXAnchor = styled.div`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    flex-direction: row;
    justify-content: center;
    align-items: center;
    padding: 0.25em 12px 0.25em 0.25em;
    overflow: hidden;
    display: flex;
    flex: 1 0 auto;
    position: relative;
    z-index: 2;
    height: 100%;
    border-radius: inherit;
    transform: translate3d(0, 0, 0);
    font-size: 16px;
    box-sizing: border-box;
    &:hover {
        color: #2e2e42;
    }
    i::hover {
        color: #2e2e42;
    }
    div::hover {
        color: #2e2e42;
    }
`;

type TTreeCustomAnchorTextProps = {
    url?: string;
}

const TreeCustomAnchorText = styled.div`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: 0px auto 0px 0px;
    display: flex;
    align-items: center;
    flex-shrink: 1;
    min-width: 1px;
    max-width: 100%;
    box-sizing: border-box;
    &:before {
        font-style: normal;
        font-variant: normal;
        text-rendering: auto;
        font-size: 16px;
        opacity: 1;
        ${({ url }: TTreeCustomAnchorTextProps) => `content: ${ url ? `url(${url});` : `` }`};
        font-weight: 900;
        background-color: rgba(255,255,255,1);
        border-radius: 5px;
        vertical-align: middle;
        padding: 2px;
        width: 40px;
        height: 40px;
        margin-right: 10px;
        box-sizing: border-box;
    }
`;

type TTreeCustomIconColapsabel = {
    collapsed?: string;
    released?: string;
    selected?: boolean;
}

const TreeCustomIconColapsabel = styled.i`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    font-size: 16px;
    color: rgba(46,55,66,0.25);
    font-family: "FontAwesome" !important;
    font-weight: 900;
    display: inline-block;
    font-style: normal;
    text-decoration: inherit;
    text-rendering: auto;
    transition-property: color,text-shadow;
    position: relative;
    width: auto;
    height: auto;
    letter-spacing: 0;
    line-height: 1;
    text-align: center;
    z-index: 2;
    box-sizing: border-box;
    list-style: none;
    width: 19px;
    height: 14px;
    &:before {
        transform: rotate(0deg);
        transition: all 0.3s ease-in-out;
        line-height: 1;
        ${({ collapsed, released }: TTreeCustomIconColapsabel) => `content: ${ collapsed ? collapsed : released }`};
        display: inherit;
        width: inherit;
        height: inherit;
        text-align: inherit;
        box-sizing: border-box;
    }
    &:after {
        box-sizing: border-box;
    }
    &:hover {
        color: #2e2e42;
    }
    ${({ selected }: TTreeCustomIconColapsabel) => `color: ${ selected ? '#2e2e42': '#2e374280' }`};
}
`;

type TTreeCustomSpanProps = {
    selected: boolean;
}

// ${({ selected }: TTreeCustomSpanProps) => `font-weight: ${ selected ? 700: 600 }`};
const TreeCustomSpan = styled.span`
    font-family: hero-new,sans-serif;
    font-size: 12.8px;
    font-style: normal;
    line-height: 0;
    color: #2e374280;
    ${({ selected }: TTreeCustomSpanProps) => `color: ${ selected ? '#2e2e42': '#2e374280' }`};
    ${({ selected }: TTreeCustomSpanProps) => `font-weight: ${ selected ? '800': '700' }`};
    transition: all 0.2s ease-out;
    &:hover {
        color: #2e2e42;
    }
`;

const TreeCustomInnerUL = styled.ul`
    background-color: #ffffff00;  
    margin: 0 2.4em;
    margin-top: -30px;
    padding-bottom: 2em;
    margin-bottom: 10px;
    border-radius: 0px 0px 16px 16px;
    transition-duration: 300ms;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    padding: 0;
    list-style: none;
    transition-property: height;
    overflow: hidden;
`;



export interface NavigationTreeItems {
    name: string;
    link?: string;
    iconUrl?: string;
    collapsable?: boolean;
    childrenItems?: NavigationTreeItems[],
}

export interface IDropDownNavigationTreeProsp extends IPage {
    tree: NavigationTreeItems[],
    style?: CSSProperties;
}

const COLLAPS_PREFIX = 'COLLAPSE_LIST_ITEM_'

const DropDownNavigationTree = (props: IDropDownNavigationTreeProsp): React.ReactElement => {
    
    const { tree } = props;
    const key  = useSelector(getCurrentPageKeySelector);
    const path  = useSelector(getCurrentPagePathSelector);
    const initState: {[k: string]: boolean } = {};
    const [collapseMap, setCollapseMap] = React.useState(initState);
    const [selectedMap, setSelectedMap] = React.useState(initState);
    const [selectedInnerMap, setSelectedInnerMap] = React.useState('');

    const changeCollapseMap = (index: number, value: boolean) => {
        setCollapseMap({ [`${COLLAPS_PREFIX}${index}`]: !value })
    }

    const changeSelectedMap = (index: number, value: boolean) => {
        const k = `${COLLAPS_PREFIX}${index}`;
        if (!selectedMap[k]) {
            setSelectedMap({ [k]: !value });
            setCollapseMap({ [`${COLLAPS_PREFIX}${index}`]: !value });
        }
    }

    const isSelectedRout = (parentItem: NavigationTreeItems, index: number): boolean => {
        if (key === parentItem.name && path === parentItem.link) {
            return true;
        } else {
            switch(key) {
                case EVisitRouterKeys.VISIT_DEMOGRAPHIC_REPORTS:
                case EVisitRouterKeys.VISIT_REPORTS:
                    return parentItem.name === ERouterKeys.VISIT;
                case EScreenRouterKeys.SCREENING_REPORTS:
                    return parentItem.name === ERouterKeys.SCREEN;
                case EFollowUpRouterKeys.FOLLOW_UP_REPORT:
                    return parentItem.name === ERouterKeys.FOLLOW_UP;
                case EAssessmentRouterKeys.CSSRS_LIST:
                    return parentItem.name === ERouterKeys.ASSESSMENT;
                case EReportsRouterKeys.INDICATOR_REPORTS:
                    return parentItem.name === ERouterKeys.REPORTS;
                case EReportsRouterKeys.EXPORT_TO_EXCEL:
                    return parentItem.name === ERouterKeys.REPORTS;
                default: return false;
            }
        }
    }

    const isChildSelectedRout = (parentItem: NavigationTreeItems, index: number): boolean => {
        if (key === parentItem.name && path === parentItem.link) {
            return true;
        } else {
            return false;
        }
    }

    return (
        <TreeCustomUL>
            {
                tree.map((parentItem: NavigationTreeItems, pIndex: number) => {
                    const isCollapsed: boolean = collapseMap[`${COLLAPS_PREFIX}${pIndex}`];
                    const isSelected: boolean =  isSelectedRout(parentItem, pIndex);
                    const { childrenItems } = parentItem;
                    const isChildren = Array.isArray(childrenItems) && childrenItems.length;
                    const item = (
                        <TreeCustomLI 
                            key={`${parentItem.name}-${pIndex}`}
                            onClick={event => {
                                changeSelectedMap(pIndex, isSelected)
                                if (isChildren) {
                                    event.stopPropagation();
                                    changeCollapseMap(pIndex, isCollapsed)
                                }
                            }}
                        >
                                <TreeCustomA style={ isSelected ? { backgroundColor: 'rgb(237,237,242)' } : {}}>
                                    <TreeCustomXAnchor>
                                        <TreeCustomAnchorText url={parentItem.iconUrl || ''}>
                                            <TreeCustomSpan selected={isSelected}>
                                                { parentItem.name }
                                            </TreeCustomSpan>
                                        </TreeCustomAnchorText>
                                        {
                                            parentItem.collapsable ? (
                                            <TreeCustomIconColapsabel  
                                                collapsed={isCollapsed ?`"▲"` :  ''} 
                                                released={(!isCollapsed) ? `"▼"` : ''} 
                                                selected={isSelected}
                                                {...{
                                                    history: props.history,
                                                    location: props.location,
                                                    match: props.match,
                                                }}/>
                                            ) : null
                                        }
                                    </TreeCustomXAnchor>
                                </TreeCustomA>
                                {
                                    (isCollapsed && isChildren) ? (
                                        <TreeCustomInnerUL>
                                            {
                                                childrenItems?.map((inner: NavigationTreeItems, iInx: number) => {
                                                const isChildSelected = isChildSelectedRout(inner, iInx);
                                                return (
                                                    <DropDownTreeComponent
                                                        key={iInx}
                                                        keyIndex={iInx} 
                                                        link={inner.link || ''} 
                                                        name={inner.name}
                                                        selected={isChildSelected}
                                                        onItemSelected={(name: string) => setSelectedInnerMap(name)} 
                                                    />
                                                )})
                                            }
                                        </TreeCustomInnerUL>
                                    ) : null
                                }
                        </TreeCustomLI>
                    )
                    return (
                        <React.Fragment  key={`Link-${pIndex}`}>
                            { isChildren ? item : (
                                <Link 
                                    key={`Link-${pIndex}`}
                                    style={{ color: 'inherit', textDecoration: 'inherit'}} 
                                    to={parentItem.link || ''}
                                >
                                    { item }
                                </Link>
                            )}
                        </React.Fragment>
                    )
                })
            }
        </TreeCustomUL>
    )
}

export default DropDownNavigationTree;