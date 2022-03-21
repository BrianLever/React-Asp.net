import React from 'react';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getVisitDemographicGenderOptionsSelector, getVisitDemographicSexualOrientationOptionsSelector,
    getSelectedVisitDemographicGenderSelector, getSelectedVisitDemographicSexualOrientationSelector,
} from '../../../../../selectors/visit/demographic-report';
import { 
    getVisitDemographicGenderRequest, getVisitDemographicSexualOrientationRequest, 
    setSelectedVisitDemographicGender, setSelectedVisitDemographicSexualOrientation
} from '../../../../../actions/visit/demographic-report';
import {EMPTY_LIST_VALUE} from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';

export const GenderComponentContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    margine-top: 20px;
    border-radius: 5px;
`;

export const GenderComponentContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

export const GenderComponentContainerHeaderTitle = styled.h1`
    font-size: 1em;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const GenderComponentContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const GenderComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const genderOptions = useSelector(getVisitDemographicGenderOptionsSelector);
    const sexualOptions = useSelector(getVisitDemographicSexualOrientationOptionsSelector);
    const selectedGender = useSelector(getSelectedVisitDemographicGenderSelector);
    const selectedOrientation = useSelector(getSelectedVisitDemographicSexualOrientationSelector);

    React.useEffect(() => {
        if (!Array.isArray(genderOptions) || !genderOptions.length) {
            dispatch(getVisitDemographicGenderRequest());
        }
        if (!Array.isArray(sexualOptions) || !sexualOptions.length) {
            dispatch(getVisitDemographicSexualOrientationRequest());
        }
    }, [dispatch, genderOptions, genderOptions.length, sexualOptions, sexualOptions.length]);

    return (
        <GenderComponentContainer>
            <GenderComponentContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <GenderComponentContainerHeaderTitle>
                            Gender
                        </GenderComponentContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <GenderComponentContainerHeaderTitle>
                            Sexual Orientation
                        </GenderComponentContainerHeaderTitle>
                    </Grid>
                </Grid>
            </GenderComponentContainerHeader>
            <GenderComponentContent>
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
                            options={genderOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedGender ? selectedGender.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const gender = genderOptions.find(d => `${d.Id}` === value);
                                if (gender) {
                                    dispatch(setSelectedVisitDemographicGender(gender));
                                }
                            }}
                        />
                    </Grid>
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
                            options={sexualOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedOrientation ? selectedOrientation.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const orientation = sexualOptions.find(d => `${d.Id}` === value);
                                if (orientation) {
                                    dispatch(setSelectedVisitDemographicSexualOrientation(orientation));
                                }
                            }}
                        />
                    </Grid>
                </Grid>
            </GenderComponentContent>
        </GenderComponentContainer>
    );
}

export default GenderComponent;