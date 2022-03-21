import React, { CSSProperties } from 'react';
import styled from 'styled-components';
import { useHistory } from 'react-router-dom';

type TTreeCustomInnerLIProps = {
    selected?: boolean;
}

const TreeCustomInnerLI = styled.li`
    position: relative;
    min-width: 1px;
    box-sizing: border-box;
    &:before {
        position: absolute;
        left: 13px;
        top: 30px;
        content: '';
        display: block;
        border-left: 2px solid #ededf2;
        height: auto;
        border-bottom: 2px solid #ededf2;
        width: 34px;
        z-index: 2;
        box-sizing: border-box;
    }
    &:after {
        position: absolute;
        left: 13px;
        bottom: 13px;
        content: '';
        display: block;
        border-left: 2px solid #ededf2;
        height: 100%;
        min-width: 255px;
        ${({ selected }: TTreeCustomInnerLIProps) => `z-index: ${ selected ? 1 : 2 }`};
        box-sizing: border-box;
        clear: both;
    }
    &:hover div div span {
        color: #2e2e42;
    }
`;

const TreeCustomInnerA = styled.div`
    background-color: #ffffff00 !important;
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    font-size: 16px;
    display: flex;
    transition-property: border-color, background-color, box-shadow, opacity, filter, transform;
    overflow: hidden;
    flex-flow: column nowrap;
    justify-content: stretch;
    position: relative;
    min-width: 1px;
    cursor: pointer;
    text-decoration: none;
    list-style: none;
`;

const TreeCustomInnerContent = styled.div`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    flex-direction: row;
    justify-content: center;
    align-items: center;
    padding: 16px 12px 5.6px 44.8px;
    overflow: hidden;
    display: flex;
    flex: 1 0 auto;
    position: relative;
    z-index: 2;
    height: 100%;
    border-radius: inherit;
    transform: translate3d(0, 0, 0);
    box-sizing: border-box;
    cursor: pointer;
    list-style: none;
`;

const TreeCustomInnerText = styled.div`
    transition-timing-function: cubic-bezier(0.400,0.000,0.200,1.000);
    margin: 9px auto 0px 0px;
    display: flex;
    align-items: center;
    flex-shrink: 1;
    min-width: 1px;
    max-width: 100%;
    box-sizing: border-box;
`;

type TTreeCustomInnerSpanProps = {
    selected?: boolean;
}
//  ${({ selected }: TTreeCustomInnerSpanProps) => `font-weight: ${ selected ? 600: 500 }`};
const TreeCustomInnerSpan = styled.span`
    color: #2e3742b3;
    transition: all 0.2s ease-out;
    font-family: 'hero-new', sans-serif;
    font-size: 12.8px;
    font-style: normal;
    line-height: 1;
    position: relative;
    display: block;
    z-index: 3;
    ${({ selected }: TTreeCustomInnerSpanProps) => `color: ${ selected ? '#2e2e42': '#2e3742b3' }`};
    ${({ selected }: TTreeCustomInnerSpanProps) => `font-weight: ${ selected ? 800: 500 }`};
    &:hover {
        color: #2e2e42;
    }
`;

export interface IDropDownTreeComponentProsp {
    keyIndex: number;
    name: string;
    link: string;
    selected?: boolean;
    style?: CSSProperties;
    onItemSelected: (n: string) => void;
}

export const DropDownTreeComponent = (props: IDropDownTreeComponentProsp): React.ReactElement => {

    const history = useHistory();

    return (
        <TreeCustomInnerLI 
            key={props.keyIndex}
            selected={props.selected}
            onClick={e =>
                { 
                    e.stopPropagation();
                    props.onItemSelected(props.name);
                    history.push(props.link || '');
                }
            }
        >
                <TreeCustomInnerA>
                    <TreeCustomInnerContent>
                        <TreeCustomInnerText>
                            <TreeCustomInnerSpan selected={props.selected}>
                                { props.name }
                            </TreeCustomInnerSpan>
                        </TreeCustomInnerText>
                    </TreeCustomInnerContent>
                </TreeCustomInnerA>
        </TreeCustomInnerLI>
    )
}

export default DropDownTreeComponent