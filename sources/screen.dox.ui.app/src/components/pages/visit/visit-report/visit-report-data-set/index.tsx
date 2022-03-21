import React from 'react';
import { Grid, FormControl, Select } from '@material-ui/core';
import { 
    AnswerSection, TopSectionText, AnswerLabelSection, TopSectionLabel, AnswerSectionMiddle
} from '../../../styledComponents';
import classes from './VisitReportDataSet.module.scss'
import { useDispatch, useSelector } from 'react-redux';
import { 
    getVisitReportTobacoExposureSmokerInHomeFlag, getVisitReportTobacoExposureSmokingFlag, 
    getVisitReportTobacoExposureSmoklessFlag, getVisitReportAlcoholUseScoreLevelLabelSelector,
    getVisitReportAlcoholUseScoreLevelSelector, getVisitReportSubstanceAbuseScoreLevel,
    getVisitReportSubstanceAbuseScoreLevelLabel, getVisitDrugChoiceOptionSelector,
    getVisitDepressionFlagScoreLevelLabelSelector, getVisitDepressionFlagScoreLevelSelector,
    getVisitDepressionThinkOfDeathAnswerSelector, getVisitAnxietyFlagScoreLevelLabelSelector,
    getVisitAnxietyFlagScoreLevelSelector, getVisitPartnerViolenceFlagScoreLevelLabelSelector,
    getVisitPartnerViolenceFlagScoreLevelSelector, getVisitProblemGamblingFlagScoreLevelLabelSelector,
    getVisitProblemGamblingFlagScoreLevelSelector, getVisitDrugPrimaryItemSelector,
    getVisitDrugSecondaryItemSelector, getVisitDrugTertiaryItemSelector
} from '../../../../../selectors/visit/report';
import { 
    getVisitDrugChoiceRequest, getVisitTritmentOptionsRequest, setVisitDrugPrimaryItem, 
    setVisitDrugSecondaryItem, setVisitDrugTertiaryItem, TChoiceItem
} from '../../../../../actions/visit/report';
import OtherEvaluationTools from '../other-evaluation-tool';
import ReferalRecomendationComponent from '../referal-recomendation';
import ReferalRecomendationAcceptedComponent from '../referal-recomendation-accepted';
import VisitDateComponent from '../visit-date';
import TritmentActionsTools from '../tritment-actions-tool';
import FollowUpToolComponent from '../follow-up-tool';
import ScreendoxSelect from 'components/UI/select';
import { EMPTY_LIST_VALUE } from 'helpers/general';


const VisitReportDataSet = (): React.ReactElement => {

    const dispatch = useDispatch();

    const drugChoiceOptions = useSelector(getVisitDrugChoiceOptionSelector);
    const useScoreLevel = useSelector(getVisitReportAlcoholUseScoreLevelSelector);
    const exposureSmokingFlag = useSelector(getVisitReportTobacoExposureSmokingFlag);
    const exposureSmoklessFlag = useSelector(getVisitReportTobacoExposureSmoklessFlag);
    const scoreLevelLabel = useSelector(getVisitReportAlcoholUseScoreLevelLabelSelector);
    const substanceAbuseScoreLevel = useSelector(getVisitReportSubstanceAbuseScoreLevel);
    const depressionFlagScoreLevel = useSelector(getVisitDepressionFlagScoreLevelSelector);
    const exposureSmokerInHomeFlag = useSelector(getVisitReportTobacoExposureSmokerInHomeFlag);
    const depressionThinkOfDeathAnswer = useSelector(getVisitDepressionThinkOfDeathAnswerSelector);
    const substanceAbuseScoreLevelLabel = useSelector(getVisitReportSubstanceAbuseScoreLevelLabel);
    const depressionFlagScoreLevelLabel = useSelector(getVisitDepressionFlagScoreLevelLabelSelector);
    const anxietyFlagScoreLevelLabel = useSelector(getVisitAnxietyFlagScoreLevelLabelSelector);
    const anxietyFlagScoreLevel = useSelector(getVisitAnxietyFlagScoreLevelSelector);
    const partnerViolenceFlagScoreLevelLabel = useSelector(getVisitPartnerViolenceFlagScoreLevelLabelSelector);
    const partnerViolenceFlagScoreLevel = useSelector(getVisitPartnerViolenceFlagScoreLevelSelector);
    const problemGamblingFlagScoreLevelLabel = useSelector(getVisitProblemGamblingFlagScoreLevelLabelSelector);
    const problemGamblingFlagScoreLevel = useSelector(getVisitProblemGamblingFlagScoreLevelSelector);
    const drugPrimaryItem = useSelector(getVisitDrugPrimaryItemSelector);
    const drugSecondaryItem = useSelector(getVisitDrugSecondaryItemSelector);
    const drugTertiaryItem = useSelector(getVisitDrugTertiaryItemSelector);

    const isSecondPriorityDisabled = (!drugPrimaryItem || drugPrimaryItem.Id === 0);
    const isTertiaryPriorityDisabled = (!drugPrimaryItem || drugPrimaryItem.Id === 0) || (!drugSecondaryItem || drugSecondaryItem.Id === 0);
    const noneDrugOption: TChoiceItem = { Id: 0, Name: '(None) Don’t Use Any Other Drugs', OrderIndex: 1 };

    const changeDrugPriorityHandler = (priorityNumber: number, item: TChoiceItem) => {
        switch(priorityNumber) {
            case 1:
                dispatch(setVisitDrugPrimaryItem(item));
                if (item.Id === 0) {
                    dispatch(setVisitDrugSecondaryItem(noneDrugOption));
                    dispatch(setVisitDrugTertiaryItem(noneDrugOption));
                }
                break;
            case 2:
                dispatch(setVisitDrugSecondaryItem(item));
                if (item.Id === 0) {
                    dispatch(setVisitDrugTertiaryItem(noneDrugOption));
                }
                break;
            case 3:
                dispatch(setVisitDrugTertiaryItem(item));
                break;
        }
    }

    React.useEffect(() => {
        if (!drugChoiceOptions || !drugChoiceOptions.length) {
            dispatch(getVisitDrugChoiceRequest());
            dispatch(getVisitTritmentOptionsRequest());
        }
    }, [drugChoiceOptions.length, dispatch, drugChoiceOptions]);

    const onChangeDumpHundler= () => console.log('Screendox.');

    return (
        <>
            <Grid container>
                <Grid item xs={6}>
                    <AnswerLabelSection>
                        <TopSectionLabel>
                            Tool
                        </TopSectionLabel>
                    </AnswerLabelSection>
                </Grid>
                <Grid item xs={6}>
                    <AnswerLabelSection>
                        <TopSectionLabel>
                            Score or Result
                        </TopSectionLabel>
                    </AnswerLabelSection>
                </Grid>
            </Grid>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <TopSectionText>
                            Tobacco Exposure (Smoker in the Home)
                        </TopSectionText>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : exposureSmokingFlag ? '' : '#ededf2' }}>Yes</span>
                            <input type="checkbox" checked={exposureSmokerInHomeFlag} onChange={onChangeDumpHundler} />
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : !exposureSmokerInHomeFlag ? '' : '#ededf2' }}>No</span>
                            <input type="checkbox" checked={!exposureSmokerInHomeFlag}  onChange={onChangeDumpHundler} />
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                </Grid>
            </AnswerSection>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <TopSectionText>
                            Tobacco Use (Smoking)
                        </TopSectionText>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : exposureSmokingFlag ? '' : '#ededf2' }}>Yes</span>
                            <input type="checkbox" checked={exposureSmokingFlag} onChange={onChangeDumpHundler}/>
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : !exposureSmokingFlag ? '' : '#ededf2' }}>No</span>
                            <input type="checkbox" checked={!exposureSmokingFlag} onChange={onChangeDumpHundler}/>
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                </Grid>
            </AnswerSection>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <TopSectionText>
                            Tobacco Use (Smokeless)
                        </TopSectionText>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : exposureSmoklessFlag ? '' : '#ededf2' }}>Yes</span>
                            <input type="checkbox" checked={exposureSmoklessFlag} onChange={onChangeDumpHundler}/>
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                    <Grid item xs={3}>
                        <label className={classes.container} style={{ marginTop: '6px' }}>
                            <span className={classes.checkboxText} style={{ color : !exposureSmoklessFlag ? '' : '#ededf2' }}>No</span>
                            <input type="checkbox" checked={!exposureSmoklessFlag} onChange={onChangeDumpHundler}/>
                            <span className={classes.checkmark}></span>
                        </label>
                    </Grid>
                </Grid>
            </AnswerSection>
            <AnswerSectionMiddle>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Alcohol Use (CAGE)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (scoreLevelLabel || (typeof scoreLevelLabel === 'number')) ? (
                                `${useScoreLevel} - ${scoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
            </AnswerSectionMiddle>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Non-Medical Drug Use (DAST-10)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (substanceAbuseScoreLevelLabel  || (typeof substanceAbuseScoreLevelLabel === 'number')) ? (
                                `${substanceAbuseScoreLevel} - ${substanceAbuseScoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
                <Grid container style={{ marginTop: '20px' }}>
                    <Grid item xs={6} className={classes.alignedFlex} style={{ paddingLeft: '20px' }}>
                        Primary Drug Use Type
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <ScreendoxSelect 
                            options={drugChoiceOptions.map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={drugPrimaryItem?.Id}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const selectedValue = value;
                                const item = drugChoiceOptions.find(d => `${d.Id}` === selectedValue);
                                if (item) {
                                    changeDrugPriorityHandler(1, item);
                                }
                            }}
                            disabled={!substanceAbuseScoreLevel}
                        />
                    </Grid>
                </Grid>
                <Grid container style={{ marginTop: '20px' }}>
                    <Grid item xs={6} className={classes.alignedFlex} style={{ paddingLeft: '20px' }}>
                        Secondary Drug Use Type
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <ScreendoxSelect 
                            options={drugChoiceOptions
                                .filter(d => (isSecondPriorityDisabled ? true : drugPrimaryItem?.Id !== d.Id)).map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={drugSecondaryItem?.Id}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const selectedValue = value;
                                const item = drugChoiceOptions.find(d => `${d.Id}` === selectedValue);
                                if (item) {
                                    changeDrugPriorityHandler(2, item);
                                }
                            }}
                            disabled={isSecondPriorityDisabled}
                        />
                    </Grid>
                </Grid>
                <Grid container style={{ marginTop: '20px' }}>
                    <Grid item xs={6} className={classes.alignedFlex} style={{ paddingLeft: '20px' }}>
                        Tertiary Drug Use Type
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        <ScreendoxSelect 
                            options={
                            drugChoiceOptions.filter(d =>  isTertiaryPriorityDisabled ? true : ((drugPrimaryItem?.Id !== d.Id) && (drugSecondaryItem?.Id !== d.Id)))
                                .map((l) => (
                                { name: `${l.Name}`, value: l.Id}
                            ))}
                            defaultValue={drugTertiaryItem?.Id}
                            rootOption={{ name: EMPTY_LIST_VALUE, value: 0 }}
                            changeHandler={(value: any) => {
                                const selectedValue = value;
                                const item = drugChoiceOptions.find(d => `${d.Id}` === selectedValue);
                                if (item) {
                                    changeDrugPriorityHandler(3, item);
                                }
                            }}
                            disabled={isTertiaryPriorityDisabled}
                        />
                    </Grid>
                </Grid>
            </AnswerSection>
            <AnswerSectionMiddle>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Depression (PHQ-9)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (depressionFlagScoreLevelLabel  || (typeof depressionFlagScoreLevelLabel === 'number')) ? (
                                `${depressionFlagScoreLevel} - ${depressionFlagScoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
                <Grid container style={{ marginTop: '20px' }}>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Suicide Ideation (PHQ-9)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        { depressionThinkOfDeathAnswer }
                    </Grid>
                </Grid>
            </AnswerSectionMiddle>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Anxiety (GAD-7)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (anxietyFlagScoreLevelLabel  || (typeof anxietyFlagScoreLevelLabel === 'number')) ? (
                                `${anxietyFlagScoreLevel} - ${anxietyFlagScoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
            </AnswerSection>
            <AnswerSectionMiddle>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Domestic/Intimate Partner Violence (HITS)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (partnerViolenceFlagScoreLevelLabel  || (typeof partnerViolenceFlagScoreLevelLabel === 'number')) ? (
                                `${partnerViolenceFlagScoreLevel} - ${partnerViolenceFlagScoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
                {/* <Grid container style={{ marginTop: '20px' }}>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Physically Hurt (HITS)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Fairly often
                    </Grid>
                </Grid> */}
            </AnswerSectionMiddle>
            <AnswerSection>
                <Grid container>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        Problem Gambling (BBGS)
                    </Grid>
                    <Grid item xs={6} className={classes.alignedFlex}>
                        {
                            (problemGamblingFlagScoreLevelLabel  || (typeof problemGamblingFlagScoreLevelLabel === 'number')) ? (
                                `${problemGamblingFlagScoreLevel} - ${problemGamblingFlagScoreLevelLabel}`
                            ) : null
                        }
                    </Grid>
                </Grid>
            </AnswerSection>
            <OtherEvaluationTools />
            <TritmentActionsTools />
            <ReferalRecomendationComponent />
            <ReferalRecomendationAcceptedComponent />
            <VisitDateComponent />
            <FollowUpToolComponent />
        </>
    );
}

export default VisitReportDataSet;