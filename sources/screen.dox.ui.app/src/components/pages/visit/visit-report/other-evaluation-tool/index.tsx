import React from 'react';
import { Grid, TextField, Button } from '@material-ui/core';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { getOtherScreeningToolsSelector } from '../../../../../selectors/visit/report';
import { initVisitReportOtherScreeningToolsItem, TOtherScreeningToolsItem } from '../../../../../actions/visit/report';
import DeleteForeverIcon from '@material-ui/icons/DeleteForever';
import { VisitReportHeaderTitleText } from '../../styledComponents';
import { ButtonTextStyle } from 'components/pages/styledComponents';

export const OETContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const OETTitleContainer = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const OETBosyContainer = styled.div`
  padding: 16px; 16px; 16px; 16px;;
`;


const emptyScreening: TOtherScreeningToolsItem = {
    ScoreOrResult: '', ToolName: ''
}

const OtherEvaluationTools = (): React.ReactElement => {

    const dispatch = useDispatch();
    const otherScreeningsArray = useSelector(getOtherScreeningToolsSelector);
    const isOtherScreeningsArray = !!otherScreeningsArray.length;
    const isOtherScreeningsLimit = otherScreeningsArray.length === 4;

    const changeEvaluationHandler = (value: string, index: number): void => {
        const copyOtherScreeningsArray = (
            Array.isArray(otherScreeningsArray) && otherScreeningsArray.length
        ) ? [...otherScreeningsArray] : [emptyScreening];
        copyOtherScreeningsArray[index].ToolName = value;
        dispatch(initVisitReportOtherScreeningToolsItem(copyOtherScreeningsArray))
    }

    const changeEvaluationScoreHandler = (value: string, index: number): void => {
        const copyOtherScreeningsArray = (
            Array.isArray(otherScreeningsArray) && otherScreeningsArray.length
        )  ? [...otherScreeningsArray] : [emptyScreening];
        copyOtherScreeningsArray[index].ScoreOrResult = value;
        dispatch(initVisitReportOtherScreeningToolsItem(copyOtherScreeningsArray))
    }

    return (
        <OETContainer>
            <OETTitleContainer>
                <Grid container>
                    <Grid item xs={6}><VisitReportHeaderTitleText>Other Evaluation Tool(s)</VisitReportHeaderTitleText></Grid>    
                    <Grid item xs={6}><VisitReportHeaderTitleText>Score or Result</VisitReportHeaderTitleText></Grid>    
                </Grid>    
            </OETTitleContainer>
            <OETBosyContainer>
                {
                    isOtherScreeningsArray ? (
                        otherScreeningsArray.map((d: TOtherScreeningToolsItem, ind: number) => {
                            const isNotFirst = (ind !== 0);
                            const fieldWidth = isNotFirst ? 5 : 6;
                            return (
                                <Grid 
                                    container key={ind} 
                                    spacing={1}
                                    justifyContent="center"
                                    alignItems="center"
                                >
                                    <Grid item xs={6}>
                                        <TextField 
                                            id={`first-field-${ind}`} 
                                            variant="outlined"
                                            margin="dense"
                                            fullWidth
                                            value={d.ToolName}
                                            onChange={e => {
                                                e.stopPropagation();
                                                changeEvaluationHandler(e.target.value, ind);
                                            }}
                                        />
                                    </Grid>    
                                    <Grid item xs={fieldWidth}>
                                        <TextField 
                                            id={`second-field-${ind}`}
                                            variant="outlined" 
                                            margin="dense"
                                            fullWidth
                                            value={d.ScoreOrResult}
                                            onChange={e => {
                                                e.stopPropagation();
                                                changeEvaluationScoreHandler(e.target.value, ind);
                                            }}
                                        />
                                    </Grid>
                                    {
                                        isNotFirst ? (
                                            <Grid item xs={1}>
                                                <Button 
                                                    size="small" 
                                                    variant="contained" 
                                                    color="primary"
                                                    style={{ 
                                                        backgroundColor: '#2e2e42', 
                                                        border: '1px solid #2e2e42' 
                                                    }}
                                                    onClick={event => {
                                                        event.stopPropagation();
                                                        dispatch(initVisitReportOtherScreeningToolsItem(otherScreeningsArray.filter((d, i) => i !== ind )));
                                                    }}
                                                >
                                                    <DeleteForeverIcon fontSize="medium"/>
                                                </Button>
                                            </Grid>    
                                        ) : null
                                    } 
                                </Grid>
                            )
                        })
                    ) : null
                }
                {
                    isOtherScreeningsLimit ? null : (
                        <Grid container style={{ marginTop: '20px' }}>
                            <Grid item >
                                <Button 
                                    size="medium" 
                                    fullWidth
                                    disabled={isOtherScreeningsLimit}
                                    variant="contained" 
                                    color="default" 
                                    style={{ 
                                        backgroundColor: '#2e2e42', 
                                        border: '1px solid #2e2e42' 
                                    }}
                                    onClick={e => {
                                        e.stopPropagation();
                                        if (Array.isArray(otherScreeningsArray) && otherScreeningsArray.length) {
                                            otherScreeningsArray.push({ ...emptyScreening })
                                            dispatch(initVisitReportOtherScreeningToolsItem([...otherScreeningsArray]));
                                        } else {
                                            dispatch(initVisitReportOtherScreeningToolsItem([{ ...emptyScreening }, { ...emptyScreening }]));
                                        }
                                    }}
                                >
                                    <ButtonTextStyle>Add Tool</ButtonTextStyle>
                                </Button>    
                            </Grid>    
                        </Grid>
                    )
                }
            </OETBosyContainer>
        </OETContainer>
    )
}

export default OtherEvaluationTools;

