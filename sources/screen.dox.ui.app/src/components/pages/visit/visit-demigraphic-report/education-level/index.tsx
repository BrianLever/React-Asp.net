import React from 'react';
import styled from 'styled-components';
import { Grid, FormControl, Select } from '@material-ui/core';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getVisitDemographicEducationLevelOptionsSelector, getVisitDemographicMaritalStatusOptionsSelector,
    getSelectedVisitDemographicEducationLevelSelector, getSelectedVisitDemographicMaritalStatusSelector
} from '../../../../../selectors/visit/demographic-report';
import { 
    getVisitDemographicEducationLevelRequest, getVisitDemographicMeritalStatusRequest, 
    setSelectedVisitDemographicEducationLevel, setSelectedVisitDemographicMeritalStatus
} from '../../../../../actions/visit/demographic-report';
import {EMPTY_LIST_VALUE} from 'helpers/general';
import ScreendoxSelect from 'components/UI/select';


const EducationLevelContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

const EducationLevelContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

const EducationLevelContainerHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

const EducationLevelContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const EducationLevelComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const educationOptions = useSelector(getVisitDemographicEducationLevelOptionsSelector);
    const maritalOptions = useSelector(getVisitDemographicMaritalStatusOptionsSelector);
    const selectedEducation = useSelector(getSelectedVisitDemographicEducationLevelSelector);
    const selectedMarital = useSelector(getSelectedVisitDemographicMaritalStatusSelector);


    React.useEffect(() => {
        if (!Array.isArray(educationOptions) || !educationOptions.length) {
            dispatch(getVisitDemographicEducationLevelRequest());
        }
        if (!Array.isArray(maritalOptions) || !maritalOptions.length) {
            dispatch(getVisitDemographicMeritalStatusRequest());
        }
    }, [dispatch, maritalOptions, maritalOptions.length, educationOptions, educationOptions.length])

    return (
        <EducationLevelContainer>
            <EducationLevelContainerHeader>
                <Grid container spacing={1}>
                    <Grid item xs={6}>
                        <EducationLevelContainerHeaderTitle>
                            Education Level
                        </EducationLevelContainerHeaderTitle>
                    </Grid>
                    <Grid item xs={6}>
                        <EducationLevelContainerHeaderTitle>
                            Marital Status
                        </EducationLevelContainerHeaderTitle>
                    </Grid>
                </Grid>
            </EducationLevelContainerHeader>
            <EducationLevelContent>
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
                            options={educationOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedEducation ? selectedEducation.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const edu = educationOptions.find(d => `${d.Id}` === value);
                                if (edu) {
                                    dispatch(setSelectedVisitDemographicEducationLevel(edu));
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
                            options={maritalOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={selectedMarital ? selectedMarital.Id : 0}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const marital = maritalOptions.find(d => `${d.Id}` === value);
                                if (marital) {
                                    dispatch(setSelectedVisitDemographicMeritalStatus(marital));
                                }
                            }}
                        />
                    </Grid>
                </Grid>
            </EducationLevelContent>
        </EducationLevelContainer>
    );
}

export default EducationLevelComponent;