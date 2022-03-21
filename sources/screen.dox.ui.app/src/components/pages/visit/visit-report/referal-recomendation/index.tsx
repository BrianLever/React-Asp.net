import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { Grid, FormControl, Select, TextField } from '@material-ui/core';
import { 
    getVisitReferalRecomendationOptionsSelector, getVisitReferralRecommendationDescriptionSelector, 
    getVisitReferralRecommendationIdSelector, getSaveChangesFlag
} from '../../../../../selectors/visit/report';
import { 
    getVisitNewReferalRecomendationRequest, setVisitReferralRecommendationDescription, 
    setVisitReferralRecommendationId 
} from '../../../../../actions/visit/report';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';

export const ReferalRecomendationComponentContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const ReferalRecomendationComponentContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const ReferalRecomendationComponentContainerHeaderTitle = styled.h1`
    font-size: 14px;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const ReferalRecomendationComponentContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const ReferalRecomendationComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const [isError, setError] = React.useState(false);

    const isSaveChanging: boolean = useSelector(getSaveChangesFlag);
    const options = useSelector(getVisitReferalRecomendationOptionsSelector);
    const referalDiscription = useSelector(getVisitReferralRecommendationDescriptionSelector);
    const referalId = useSelector(getVisitReferralRecommendationIdSelector);

    React.useEffect(() => {
        if (!options || !options.length) {
            dispatch(getVisitNewReferalRecomendationRequest());
        }
        if (isSaveChanging && (!referalId || !referalDiscription)) {
            setError(true);
        } else {
            setError(false);
        }
    }, [options, options.length, dispatch, isSaveChanging, referalId, referalDiscription]);

    return (
        <ReferalRecomendationComponentContainer>
            <ReferalRecomendationComponentContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <ReferalRecomendationComponentContainerHeaderTitle>
                            Primary Reason for New Visit/Referral Recommendation*
                        </ReferalRecomendationComponentContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <ReferalRecomendationComponentContainerHeaderTitle>
                            Description
                        </ReferalRecomendationComponentContainerHeaderTitle>
                    </Grid>
                </Grid>
            </ReferalRecomendationComponentContainerHeader>
            <ReferalRecomendationComponentContainerContent>
                <Grid container spacing={1}>
                    <Grid 
                        item 
                        xs={6} 
                        style={{
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                        }}
                    >
                        <ScreendoxSelect 
                            options={
                                options.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={referalId ? referalId : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                dispatch(setVisitReferralRecommendationId(parseInt(`${value}`)));
                            }}
                            isError={isError}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <TextField 
                            id="ReferalRecomendationComponentDescription"
                            variant="outlined" 
                            margin="dense"
                            error={isError}
                            fullWidth
                            value={referalDiscription}
                            onChange={e => {
                                e.stopPropagation();
                                dispatch(setVisitReferralRecommendationDescription(`${e.target.value}`));
                            }}
                        />
                    </Grid>
                </Grid>
            </ReferalRecomendationComponentContainerContent>
        </ReferalRecomendationComponentContainer>
    )
}

export default ReferalRecomendationComponent;