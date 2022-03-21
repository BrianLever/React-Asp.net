import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import { 
    getVisitReferalRecomendationAcceptedOptionsSelector, getVisitReferalRecomendationNotAcceptedOptionsSelector, 
    getVisitReferralRecommendationAcceptedSelector, getVisitReferralRecommendationNotAcceptedSelector, getSaveChangesFlag
} from '../../../../../selectors/visit/report';
import { 
    getVisitNewReferalRecomendationAcceptedRequest, getVisitNewReferalRecomendationNotAcceptedRequest, 
    setVisitReferralRecommendationAccepted, setVisitReferralRecommendationNotAccepted
} from '../../../../../actions/visit/report';
import {EMPTY_LIST_VALUE} from 'helpers/general'
import ScreendoxSelect from 'components/UI/select';

export const ReferalRecomendationAcceptedContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

export const ReferalRecomendationAcceptedContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 14px;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const ReferalRecomendationAcceptedContainerHeaderTitle = styled.h1`
    font-size: 14px;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const ReferalRecomendationAcceptedContainerContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const ReferalRecomendationAcceptedComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const [isError, setError] = React.useState(false);

    const isSaveChanging: boolean = useSelector(getSaveChangesFlag);
    const acceptedOptions = useSelector(getVisitReferalRecomendationAcceptedOptionsSelector);
    const notAcceptedOptions = useSelector(getVisitReferalRecomendationNotAcceptedOptionsSelector);
    const referralAccepted = useSelector(getVisitReferralRecommendationAcceptedSelector);
    const referralNotAccepted = useSelector(getVisitReferralRecommendationNotAcceptedSelector);

    React.useEffect(() => {
        if (!acceptedOptions || !acceptedOptions.length) {
            dispatch(getVisitNewReferalRecomendationAcceptedRequest());
        }
        if (!notAcceptedOptions || !notAcceptedOptions.length) {
            dispatch(getVisitNewReferalRecomendationNotAcceptedRequest());
        }
        if (isSaveChanging && (!referralAccepted || !referralNotAccepted)) {
            setError(true);
        } else {
            setError(false);
        }
    }, [dispatch, acceptedOptions, acceptedOptions.length, notAcceptedOptions, notAcceptedOptions.length]);

    return (
        <ReferalRecomendationAcceptedContainer>
            <ReferalRecomendationAcceptedContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <ReferalRecomendationAcceptedContainerHeaderTitle>
                            New Visit/Referral Recommendation Accepted*
                        </ReferalRecomendationAcceptedContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <ReferalRecomendationAcceptedContainerHeaderTitle>
                            Reason Recommendation NOT Accepted*
                        </ReferalRecomendationAcceptedContainerHeaderTitle>
                    </Grid>
                </Grid>
            </ReferalRecomendationAcceptedContainerHeader>
            <ReferalRecomendationAcceptedContainerContent>
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
                                acceptedOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={referralAccepted ? referralAccepted : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: -1 }}
                            changeHandler={(value: any) => {
                                const v = parseInt(`${value}`);
                                dispatch(setVisitReferralRecommendationAccepted(v))
                            }}
                            isError={isError}
                        />
                    </Grid>
                    <Grid item xs={6}>
                        <ScreendoxSelect 
                            options={
                                notAcceptedOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={referralNotAccepted}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const v = parseInt(`${value}`);
                                dispatch(setVisitReferralRecommendationNotAccepted(v))
                            }}
                            isError={isError}
                            rootOptionDisabled
                        />
                    </Grid>
                </Grid>
            </ReferalRecomendationAcceptedContainerContent>
        </ReferalRecomendationAcceptedContainer>
    )
}

export default ReferalRecomendationAcceptedComponent;