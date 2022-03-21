import React from 'react';
import styled from 'styled-components';
import { Grid } from '@material-ui/core';
import { IRootState } from '../../../states';
import { IActionPayload } from '../../../actions';
import { useDispatch, useSelector } from 'react-redux';

export const FullTextareaContainer = styled.div`
    margin-top: 10px;
    font-size: 1em;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const FullTextareaHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: #ededf2;
  border-radius: 5px 5px 0 0;
`;

export const FullTextareaHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const FullTextareaContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

export const FullTextareaTextarea = styled.textarea`
    font-size: 1.2em !important;
    border: 1px solid #2e2e42 !important;
    width: 100%;
    min-height: 120px;
    background: transparent;
    padding: 10px;
    color: #2e2e42 !important;
    z-index: 999;
    margin-bottom: 0px;
    padding: 0 .65em;
    padding-top: 6px;
    padding-bottom: 6px;
    transition: border linear 0.2s, box-shadow linear 0.2s;
    height: auto;
    line-height: 1.3;
    border-radius: 4px;
    box-shadow: inset 0 1px 1px rgb(0 0 0 / 8%);
    display: inline-block;
    overflow: auto;
    vertical-align: top;
`;

export type TFullTextarea = {
    title: string;
    textSelector: (state: IRootState) => string;
    onTextChangeAction: (i: string) => IActionPayload;
}

const FullTextarea = (props: TFullTextarea): React.ReactElement => {

    const { title, onTextChangeAction, textSelector } = props;

    const dispatch = useDispatch();
    const text = useSelector(textSelector);

    return (
    <FullTextareaContainer>
        <FullTextareaHeader>
            <Grid container spacing={1}>
                <Grid item xs={6}>
                    <FullTextareaHeaderTitle>
                        { title }
                    </FullTextareaHeaderTitle>
                </Grid>
            </Grid>
        </FullTextareaHeader>
        <FullTextareaContainerContent>
            <FullTextareaTextarea 
                rows={10}
                onChange={e => {
                    e.stopPropagation();
                    dispatch(onTextChangeAction(e.target.value));
                }}
            >
                {text}
            </FullTextareaTextarea>
        </FullTextareaContainerContent>
    </FullTextareaContainer>)
}

export default FullTextarea;