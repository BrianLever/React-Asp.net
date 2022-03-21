import React from "react";
import styled from 'styled-components';
import { Grid } from '@material-ui/core';
import classes from './Report.module.scss';
import { useDispatch, useSelector } from "react-redux";
import { 
    geSelectedVisitDemographicMilitaryExperienceSelector, getVisitDemographicMilitaryExperienceSelector 
} from "../../../../../selectors/visit/demographic-report";
import { 
    changeVisitDemographicMilitaryExperience,
    getVisitDemographicMilitaryExperienceRequest, TVisitDemographicItem,  
} from "../../../../../actions/visit/demographic-report";


const LivingReservationContainer = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid rgb(237,237,242);
    margine-top: 20px;
    border-radius: 5px;
`;

const LivingReservationContainerHeader = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: rgb(237,237,242);
  border-radius: 5px 5px 0 0;
`;

const LivingReservationContainerHeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

const LivingReservationContent = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;

const MilitaryExperienceComponent = (): React.ReactElement => {

    const dispatch = useDispatch();
    const militaryOptions = useSelector(getVisitDemographicMilitaryExperienceSelector);
    const selectedMilitaryOptions = useSelector(geSelectedVisitDemographicMilitaryExperienceSelector);
    const selectedMilitaryOptionsMap: {[k: string]: boolean} = {};
    if (Array.isArray(selectedMilitaryOptions)) {
        selectedMilitaryOptions.forEach(d => {
            selectedMilitaryOptionsMap[d.Id] = true;
        });
    }

    React.useEffect(() => {
        dispatch(getVisitDemographicMilitaryExperienceRequest())
    }, [dispatch, militaryOptions.length])

    const chnageMilitaryExperience = (selectedItem: TVisitDemographicItem, checked: boolean) => {
        switch(selectedItem.Id) {
            case 1:
                dispatch(changeVisitDemographicMilitaryExperience([selectedItem]));
                break;
            case 2: 
                checked ? 
                dispatch(changeVisitDemographicMilitaryExperience([selectedItem])) : 
                dispatch(changeVisitDemographicMilitaryExperience(militaryOptions.filter(d=> d.Id === 1)));
                break;
            case 3:
                const isVeteranLast = ((selectedMilitaryOptions.length === 1) && (selectedMilitaryOptions.findIndex(d=> d.Id === selectedItem.Id) > -1));
                const isVeteranNo = selectedMilitaryOptions.length === 0;
                if (!checked && isVeteranLast) {
                    dispatch(changeVisitDemographicMilitaryExperience(militaryOptions.filter(d=> d.Id === 1)));
                } else if (!checked) {
                    dispatch(changeVisitDemographicMilitaryExperience(selectedMilitaryOptions.filter(d=> d.Id !== selectedItem.Id)));
                } else if (isVeteranNo) {
                    dispatch(changeVisitDemographicMilitaryExperience([selectedItem]));
                } else {
                    const newVeteranArr = selectedMilitaryOptions.filter(d => d.Id !== 1 && d.Id !== 2);
                    dispatch(changeVisitDemographicMilitaryExperience(newVeteranArr.concat(selectedItem)));
                }
                break;
            case 4: 
                const isDepCombatZone = ((selectedMilitaryOptions.length === 1) && (selectedMilitaryOptions.findIndex(d=> d.Id === selectedItem.Id) > -1));
                const isDepCombatNo = selectedMilitaryOptions.length === 0;
                if (!checked && isDepCombatZone) {
                    dispatch(changeVisitDemographicMilitaryExperience(militaryOptions.filter(d=> d.Id === 1)));
                } else if (!checked) {
                    dispatch(changeVisitDemographicMilitaryExperience(selectedMilitaryOptions.filter(d=> d.Id !== selectedItem.Id)));
                } else if (isDepCombatNo) { 
                    dispatch(changeVisitDemographicMilitaryExperience([selectedItem]));
                } else {
                    const newDepCombatZoneArr = selectedMilitaryOptions.filter(d => d.Id !== 1 && d.Id !== 2);
                    dispatch(changeVisitDemographicMilitaryExperience(newDepCombatZoneArr.concat(selectedItem)));
                }
                break;
        }
    }

    return (        
    <LivingReservationContainer>
        <LivingReservationContainerHeader>
            <Grid container spacing={1}>
                <Grid item xs={12}>
                    <LivingReservationContainerHeaderTitle>
                        Military Experience
                    </LivingReservationContainerHeaderTitle>
                </Grid>
            </Grid>
        </LivingReservationContainerHeader>
        <LivingReservationContent>
            <Grid container spacing={1}>
                {
                    militaryOptions.map(d => {
                        const isSelected = !!selectedMilitaryOptionsMap[d.Id];
                        return (
                            <Grid 
                                key={d.Id}
                                item
                                xs={3}
                                style={{
                                    display: 'flex',
                                    alignItems: 'center',
                                    justifyContent: 'center',
                                }}
                            >
                                <label 
                                    className={classes.container}
                                >
                                    <span className={classes.checkboxText} style={{ color : true ? '' : '#ededf2' }}>
                                        {d.Name}
                                    </span>
                                    <input 
                                        type="checkbox" 
                                        checked={isSelected}
                                        onChange={e => {
                                            e.stopPropagation();
                                            chnageMilitaryExperience(d, e.target.checked);
                                        }}
                                    />
                                    <span className={classes.checkmark}></span>
                                </label>
                            </Grid>
                        )
                    })
                }
            </Grid>
        </LivingReservationContent>
    </LivingReservationContainer>)
}

export default MilitaryExperienceComponent;