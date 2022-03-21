import React from 'react';
import { Grid, TextField, Button, Select, FormControl } from '@material-ui/core';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import { getTritmentActionToolsSelector, getTritmenOptionsSelector, getSaveChangesFlag } from '../../../../../selectors/visit/report';
import { initVisitReportTritmentActionToolsItem, TTreatmentActionsItem } from '../../../../../actions/visit/report';
import DeleteForeverIcon from '@material-ui/icons/DeleteForever';
import { VisitReportHeaderTitleText } from '../../styledComponents';
import { ButtonTextStyle } from 'components/pages/styledComponents';
import ScreendoxSelect from 'components/UI/select';
import { EMPTY_LIST_VALUE } from 'helpers/general';

export const OETContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const OETTitleContainer = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const OETBosyContainer = styled.div`
  padding: 16px; 16px; 16px; 16px;;
`;


const emptyTritments: TTreatmentActionsItem = {
    Id: 1, Name: '', OrderIndex: 0
}

const TritmentActionsTools = (): React.ReactElement => {

    const dispatch = useDispatch();

    const [isError, setError] = React.useState(false);

    const isSaveChanging: boolean = useSelector(getSaveChangesFlag);
    const tritmentActionsArray = useSelector(getTritmentActionToolsSelector);
    const tritmentOptions = useSelector(getTritmenOptionsSelector);

    const isTritmentActionsArray = !!tritmentActionsArray.length;
    const isTritmentActionsLimit = tritmentActionsArray.length === 5;

    const changeTritmentDelivery = (d: string, i: number) => {
        const choosedItem = tritmentOptions.find(opt => `${opt.Id}` === d);
        if (!!choosedItem) {
            if (Array.isArray(tritmentActionsArray) && !isTritmentActionsArray) {
                tritmentActionsArray.push({...emptyTritments});
            }
            tritmentActionsArray[i].Id = choosedItem.Id;
            tritmentActionsArray[i].Name = choosedItem.Name;
            tritmentActionsArray[i].OrderIndex = choosedItem.OrderIndex;
            dispatch(initVisitReportTritmentActionToolsItem([...tritmentActionsArray]));
        }
    }
    const changeTritmentDescription = (d: string, i: number) => {
        if (Array.isArray(tritmentActionsArray) && !isTritmentActionsArray) {
            tritmentActionsArray.push({...emptyTritments});
        }
        if (!!tritmentActionsArray[i]) {
            tritmentActionsArray[i].Description = d;
            dispatch(initVisitReportTritmentActionToolsItem([...tritmentActionsArray]));
        }
    }

    React.useEffect(() => {
        if (isSaveChanging && (!tritmentActionsArray[0] || !tritmentActionsArray[0].Description)) {
            setError(false);
        } else {
            setError(false);
        }
    }, [isSaveChanging, tritmentActionsArray.length]);

    return (
        <OETContainer>
            <OETTitleContainer>
                <Grid container>
                    <Grid item xs={6}><VisitReportHeaderTitleText>Treatment Action(s) Delivered*</VisitReportHeaderTitleText></Grid>    
                    <Grid item xs={6}><VisitReportHeaderTitleText>Description</VisitReportHeaderTitleText></Grid>    
                </Grid>    
            </OETTitleContainer>
            <OETBosyContainer>
                {
                    isTritmentActionsArray ? (
                        tritmentActionsArray.map((d: TTreatmentActionsItem, ind: number) => {
                            const isNotFirst = (ind !== 0);
                            const fieldWidth = isNotFirst ? 5 : 6;
                            return (
                                <Grid 
                                    container 
                                    key={ind} 
                                    spacing={1}
                                    justifyContent="center"
                                    alignItems="center"
                                >
                                    <Grid item xs={6}>
                                        <ScreendoxSelect 
                                            options={
                                            tritmentOptions.map((l) => (
                                                { name: `${l.Name}`, value: l.Id}
                                            ))}
                                            defaultValue={d?.Id}
                                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                                            changeHandler={(value: any) => {
                                                changeTritmentDelivery(value, ind);
                                            }}
                                            rootOptionDisabled
                                        />
                                    </Grid>    
                                    <Grid item xs={fieldWidth}>
                                        <TextField 
                                            id={`second-field-${ind}`}
                                            variant="outlined" 
                                            margin="dense"
                                            value={d.Description}
                                            error={isError}
                                            fullWidth
                                            onChange={(e: any) => {
                                                e.stopPropagation()
                                                const value: string = `${e.target.value}`;
                                                changeTritmentDescription(value, ind);
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
                                                    onClick={(event: any) => {
                                                        event.stopPropagation();
                                                        dispatch(initVisitReportTritmentActionToolsItem(tritmentActionsArray.filter((d, i) => i !== ind )));
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
                <Grid container style={{ marginTop: '20px' }}>
                    {
                        isTritmentActionsLimit ? null : (
                            <Grid item >
                                <Button 
                                    size="medium" 
                                    fullWidth
                                    disabled={isTritmentActionsLimit}
                                    variant="contained" 
                                    color="default" 
                                    style={{ 
                                        backgroundColor: '#2e2e42', 
                                        border: '1px solid #2e2e42' 
                                    }}
                                    onClick={e => {
                                        e.stopPropagation();
                                        if (Array.isArray(tritmentActionsArray) && isTritmentActionsArray) {
                                            tritmentActionsArray.push({...emptyTritments})
                                            dispatch(initVisitReportTritmentActionToolsItem([...tritmentActionsArray]));
                                        } else {
                                            dispatch(initVisitReportTritmentActionToolsItem([{...emptyTritments}, {...emptyTritments}]));
                                        }
                                    }}
                                >
                                    <ButtonTextStyle>Add Treatment Action</ButtonTextStyle>
                                </Button>    
                            </Grid>
                        )
                    }
                </Grid>
            </OETBosyContainer>
        </OETContainer>
    )
}

export default TritmentActionsTools;

