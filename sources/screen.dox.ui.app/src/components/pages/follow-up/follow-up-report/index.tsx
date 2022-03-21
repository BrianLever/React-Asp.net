import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import styled from 'styled-components';
import ReportHeader from '../../../../components/pages/common/reportHeader';
import { ContentContainer, TitleText } from '../../styledComponents';
import { 
    getFollowUpReportPatientInfoLastName,getFollowUpReportPatientInfoFirstName, getCurrentlySelectedNewVisitReferralRecommendationNotAcceptedOptionSelector,
    getFollowUpReportPatientInfoMiddleName, getFollowUpReportPatientInfoBirthday, getFollowUpReportPatientInfoStreetAddress, getFollowUpReportPatientInfoCity,
    getFollowUpReportPatientInfoStateName, getFollowUpReportPatientInfoZipCode, getFollowUpReportPatientInfoPhone, getFollowUpReportLocationSelector,
    getNewVisitDateSelector, getFollowUpReportStaffCompleted,
    getFollowUpReportPatientAttendedVisitOptionsSelector, getCurrentSelectedPatientAttendedVisitDateSelector,
    getPatientAttendedVisitAllOptionsSelector, getCurrentSelectedPatientAttendedVisitOptionSelector, getDischargedOptionsSelector,
    getFollowUpReportFollowUpContactOutcomeOptionsSelector, getFollowUpReportNewVisitReferralRecommendationOptionsSelector, 
    getCurrentSelectedFollowUpContactOutcomeOption, getcurrentSelectedNewVisitReferralRecommendationOption, 
    getNewVisitReferralRecommendationSelector, getNewVisitReferralRecommendationAcceptedOptionsSelector, 
    getNewVisitReferralRecommendationNotAcceptedOptionsSelector, getCurrentlySelectedNewVisitReferralRecommendationAcceptedOptionSelector, 
    getCurrentlySelectedDischargedOptionSelector,
    getFollowUpReportCompleteDate, getFollowUpReporScreeningResultId
} from '../../../../selectors/follow-up/report';
import { 
    getFollowUpReponewVisitReferralRecommendationNotAcceptedRequest, getFollowUpReporNewVisitReferralRecommendationAcceptedRequest,
    getFollowUpReportDischargedRequest, getFollowUpReportFollowUpContactOutcomeRequest, getFollowUpReportNewVisitReferralRecommendationRequest,
    getFollowUpReportPatientAttendedVisitOptionsRequest, postFollowUpsRequest, setNewVisitReferralRecommendation,
    setCurrentSelectedFollowUpReportPatientAttendedVisitDate, setCurrentSelectedFollowUpReportPatientAttendedVisitOption, 
    setFollowUpReponewVisitReferralRecommendationNotAcceptedOption, setFollowUpReporNewVisitReferralRecommendationAcceptedOption, 
    setFollowUpReportSelectedContactOutcomeOption, setFollowUpReportSelectedNewVisitReferralRecommendationOption,
    setFollowUpReportDischargedOption, setFollowUpReportNewVisitDate 
} from '../../../../actions/follow-up/report';
import { IPage } from '../../../../common/types/page';
import ScreendoxFieldDatePicker, { ETScreendoxFieldDatePicker } from '../../../../components/UI/custom-components/field-date-picker';
import ScreendoxSelectorsSet from '../../../../components/UI/custom-components/selectros-set';
import SideSearchLayout from '../../../../components/UI/side-search-layout';
import FollowUpReportTemplate from '../../../../components/UI/side-search-layout/templates/FollowUpReportTemplate';
import FullTextarea from '../../../../components/UI/custom-components/full-textarea';
import FollowUpToolComponent from './follow-up-tool';
import { setCurrentPage } from '../../../../actions/settings';
import { EFollowUpRouterKeys, ERouterUrls } from '../../../../router';
import { useParams } from "react-router-dom";

export const Container = styled.div`
    font-size: 1em;;
`;

export const HeaderContainer = styled.div`
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`
export const Header = styled.div`
  padding: 16px; 16px; 16px; 16px;;
  font-size: 1em;;
  background-color: #ededf2;
  border-radius: 5px 5px 0 0;
`;

export const HeaderTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 700;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const ContentTitle = styled.h1`
    font-size: 1em;;
    font-style: normal;
    font-weight: 400;
    line-height: 1.4;
    letter-spacing: 0em;
    color: #2e2e42;
`;

export const Content = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
    border-radius: 5px;
    border-radius: 0 0 5px 5px;
`;

export interface IFollowUpReportPage extends IPage {}

const FollowUpReportPage = (props: IFollowUpReportPage): React.ReactElement => {

    const dispatch =                     useDispatch();
    const zip =                          useSelector(getFollowUpReportPatientInfoZipCode);
    const srn =                          useSelector(getFollowUpReporScreeningResultId);;
    const city =                         useSelector(getFollowUpReportPatientInfoCity);
    const state =                        useSelector(getFollowUpReportPatientInfoStateName);
    const phone =                        useSelector(getFollowUpReportPatientInfoPhone);
    const location =                     useSelector(getFollowUpReportLocationSelector);
    const lastName =                     useSelector(getFollowUpReportPatientInfoLastName);
    const firstName =                    useSelector(getFollowUpReportPatientInfoFirstName);
    const birthDate =                    useSelector(getFollowUpReportPatientInfoBirthday);
    const createdBy =                    useSelector(getFollowUpReportStaffCompleted);
    const middleName =                   useSelector(getFollowUpReportPatientInfoMiddleName);
    const patientHRN =                   useSelector(getFollowUpReportPatientInfoMiddleName);;
    const createdDate =                  useSelector(getFollowUpReportCompleteDate);
    const nvrrOptions =                  useSelector(getFollowUpReportNewVisitReferralRecommendationOptionsSelector);
    const nvrraOptions=                  useSelector(getNewVisitReferralRecommendationAcceptedOptionsSelector);
    const nvrrnaOptions=                 useSelector(getNewVisitReferralRecommendationNotAcceptedOptionsSelector);
    const mailingAddress =               useSelector(getFollowUpReportPatientInfoStreetAddress);
    const contactOutcomeOptions =        useSelector(getFollowUpReportFollowUpContactOutcomeOptionsSelector);
    const patientAttendedVisitOptions =  useSelector(getFollowUpReportPatientAttendedVisitOptionsSelector);
    const dischargedOptions =            useSelector(getDischargedOptionsSelector);
    const { reportId } = useParams<{ reportId: string }>();
    React.useEffect(() => {
        dispatch(postFollowUpsRequest(reportId))
        if (!patientAttendedVisitOptions || !patientAttendedVisitOptions.length) {
            dispatch(getFollowUpReportPatientAttendedVisitOptionsRequest());
        }
        if (!contactOutcomeOptions || !contactOutcomeOptions.length) {
            dispatch(getFollowUpReportFollowUpContactOutcomeRequest());
        }
        if (!nvrrOptions || !nvrrOptions.length) {
            dispatch(getFollowUpReportNewVisitReferralRecommendationRequest());
        }
        if (!nvrraOptions || !nvrraOptions.length) {
            dispatch(getFollowUpReporNewVisitReferralRecommendationAcceptedRequest());
        }
        if (!nvrrnaOptions || !nvrrnaOptions.length) {
            dispatch(getFollowUpReponewVisitReferralRecommendationNotAcceptedRequest());
        }
        if (!dischargedOptions || !dischargedOptions.length) {
            dispatch(getFollowUpReportDischargedRequest());
        }
        dispatch(setCurrentPage(EFollowUpRouterKeys.FOLLOW_UP_REPORT, ERouterUrls.FOLLOW_UP_REPORT));
    }, [
        reportId, dispatch,
        patientAttendedVisitOptions, patientAttendedVisitOptions.length,
        contactOutcomeOptions, contactOutcomeOptions.length,
        nvrrOptions, nvrrOptions.length, nvrraOptions, nvrraOptions.length,
        nvrrnaOptions, nvrrnaOptions.length, dischargedOptions, dischargedOptions.length
    ]);

    const content = (
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={4} style={{ textAlign: 'center' }}>
                    <TitleText>
                        Follow-Up Report
                    </TitleText>
                </Grid>
            </Grid>
            <ReportHeader 
                lastName={lastName}
                firstName={firstName}
                middleName={middleName}
                birthDate={birthDate}
                patientHRN={patientHRN}
                mailingAddress={mailingAddress}
                city={city}
                state={state}
                zip={zip}
                phone={phone}
                location={location}
                createdDate={createdDate}
                createdBy={createdBy}
                srn={srn}
                showSrnAsLink={true}
            />
            <Container>
                <HeaderContainer>
                    <Header>
                        <Grid container spacing={1}>
                            <Grid item xs={6}>
                                <HeaderTitle>
                                    Visit/Referral Recommendation
                                </HeaderTitle>
                            </Grid>
                            <Grid item xs={3}>
                                <HeaderTitle>
                                    Scheduled Visit Date
                                </HeaderTitle>
                            </Grid>
                            <Grid item xs={3}>
                                <HeaderTitle>
                                    Scheduled Follow-Up Date
                                </HeaderTitle>
                            </Grid>
                        </Grid>
                    </Header>
                    <Content>
                        <Grid container spacing={1}>
                            <Grid item xs={6}>
                                <ContentTitle>
                                    Autofilled from Visit Report
                                </ContentTitle>
                            </Grid>
                            <Grid item xs={3}>
                                <ContentTitle>
                                    Autofilled from Visit Report
                                </ContentTitle>
                            </Grid>
                            <Grid item xs={3}>
                                <ContentTitle>
                                    Autofilled from Visit Report
                                </ContentTitle>
                            </Grid>
                        </Grid>
                    </Content>
                </HeaderContainer>
                <ScreendoxFieldDatePicker
                    type={ETScreendoxFieldDatePicker.selectorDate}
                    datePickerTitle="Visit Audit Date"
                    selectorTitle="Patient Attended Visit*"
                    selectedDatePickerValueSelector={getCurrentSelectedPatientAttendedVisitDateSelector}
                    optionsSelector={getPatientAttendedVisitAllOptionsSelector}
                    selectedOptionsValueSelector={getCurrentSelectedPatientAttendedVisitOptionSelector}
                    onOptionChangeAction={setCurrentSelectedFollowUpReportPatientAttendedVisitOption}
                    onDatePickerChangeAction={setCurrentSelectedFollowUpReportPatientAttendedVisitDate}
                />
                <ScreendoxSelectorsSet 
                    aSelectorTitle="Follow-Up Contact Outcome"
                    bSelectorTitle="New Visit/Referral Recommendation"
                    optionsASelector={getFollowUpReportFollowUpContactOutcomeOptionsSelector}
                    optionsBSelector={getFollowUpReportNewVisitReferralRecommendationOptionsSelector}
                    onAOptionChangeAction={setFollowUpReportSelectedContactOutcomeOption}
                    onBOptionChangeAction={setFollowUpReportSelectedNewVisitReferralRecommendationOption}
                    selectedAOptionsValueSelector={getCurrentSelectedFollowUpContactOutcomeOption}
                    selectedBOptionsValueSelector={getcurrentSelectedNewVisitReferralRecommendationOption}
                />
                <FullTextarea 
                    title="New Visit/Referral Recommendation Description"
                    onTextChangeAction={setNewVisitReferralRecommendation}
                    textSelector={getNewVisitReferralRecommendationSelector}
                />
                <ScreendoxSelectorsSet 
                    aSelectorTitle="New Visit/Referral Recommendation Accepted?"
                    bSelectorTitle="Reason Recommendation NOT Accepted"
                    optionsASelector={getNewVisitReferralRecommendationAcceptedOptionsSelector}
                    optionsBSelector={getNewVisitReferralRecommendationNotAcceptedOptionsSelector}
                    onAOptionChangeAction={setFollowUpReporNewVisitReferralRecommendationAcceptedOption}
                    onBOptionChangeAction={setFollowUpReponewVisitReferralRecommendationNotAcceptedOption}
                    selectedAOptionsValueSelector={getCurrentlySelectedNewVisitReferralRecommendationAcceptedOptionSelector}
                    selectedBOptionsValueSelector={getCurrentlySelectedNewVisitReferralRecommendationNotAcceptedOptionSelector}
                />
                <ScreendoxFieldDatePicker
                    type={ETScreendoxFieldDatePicker.dateSelector}
                    datePickerTitle="New Visit Date"
                    selectorTitle="Discharged?"
                    selectedDatePickerValueSelector={getNewVisitDateSelector}
                    optionsSelector={getDischargedOptionsSelector}
                    selectedOptionsValueSelector={getCurrentlySelectedDischargedOptionSelector}
                    onOptionChangeAction={setFollowUpReportDischargedOption}
                    onDatePickerChangeAction={setFollowUpReportNewVisitDate}
                />
                <FollowUpToolComponent />
            </Container>
        </ContentContainer>);

    return (
        <SideSearchLayout content={content} bar={<FollowUpReportTemplate />} isFixed/>
    )
}

export default FollowUpReportPage;