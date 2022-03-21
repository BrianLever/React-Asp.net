import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import styled from 'styled-components';
import { 
    ContentContainer, TitleText 
} from '../../styledComponents';
import SideSearchLayout from '../../../../components/UI/side-search-layout';
import VisitDemographicReportTemplate from '../../../../components/UI/side-search-layout/templates/VisitDemographicReportTemplate';
import { 
    getVisitDemographicReportPatientFirstName, getVisitDemographicReportLocation, getVisitDemographicReportCreatedDate,
    getVisitDemographicReportPatientLastName, getVisitDemographicReportPatientFullName, getVisitDemographicReportPatientPhone,
    getVisitDemographicReportPatientBirthDate, getVisitDemographicReportPatientMiddleName, getVisitDemographicReportPatientStreetAddress,
    getVisitDemographicReportPatientZipCode, getVisitDemographicReportPatientStateName, getVisitDemographicReportPatientCity,
    getVisitDemographicReportExportedToHRN, getVisitReportScreeningResultId, getVisitDemoGraphicStaffNameCompltedSelector, getVisitDemoGraphicCompletedDateSelector
} from '../../../../selectors/visit/demographic-report';
import { 
    getVisitDemographicReportRequest, getVisitDemoGraphicStaffNameComplted 
} from '../../../../actions/visit/demographic-report';
import { IPage } from '../../../../common/types/page';
import ReportHeader from '../../../../components/pages/common/reportHeader';
import RaceComponent from './race';
import GenderComponent from './gender';
import EducationLevelComponent from './education-level';
import LivingReservationComponent from './living-reservation';
import MilitaryExperienceComponent from './military-experience';
import { setCurrentPage } from '../../../../actions/settings';
import { ERouterUrls, EVisitRouterKeys } from '../../../../router';
import { convertDateToStringFormat } from 'helpers/dateHelper';
import { useParams } from "react-router-dom";


export const Container = styled.div`
    margin-top: 20px;
    font-size: 1em;;
    border: 1px solid #f5f6f8;
    border-radius: 5px;
`;

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

export const Content = styled.div`
    display: flex;
    padding: 15px 20px 15px 20px;
`;


export interface IVisitDemographicReportReport extends IPage {}

const VisitDemographicReport = (props: IVisitDemographicReportReport): React.ReactElement => {

    const dispatch = useDispatch();
    const city = useSelector(getVisitDemographicReportPatientCity);
    const state = useSelector(getVisitDemographicReportPatientStateName);
    const patientHRN = useSelector(getVisitDemographicReportExportedToHRN);
    const lastName = useSelector(getVisitDemographicReportPatientLastName);
    const birthDate = useSelector(getVisitDemographicReportPatientBirthDate);
    const firstName = useSelector(getVisitDemographicReportPatientFirstName);
    const middleName = useSelector(getVisitDemographicReportPatientMiddleName);
    const mailingAddress = useSelector(getVisitDemographicReportPatientStreetAddress);
    const zip = useSelector(getVisitDemographicReportPatientZipCode);
    const phone = useSelector(getVisitDemographicReportPatientPhone);
    const location = useSelector(getVisitDemographicReportLocation);
    const createdDate = useSelector(getVisitDemographicReportCreatedDate);
    const createdBy = useSelector(getVisitDemographicReportPatientFullName);
    const srn = useSelector(getVisitReportScreeningResultId);
    const completeData = useSelector(getVisitDemoGraphicCompletedDateSelector);
    const staffName = useSelector(getVisitDemoGraphicStaffNameCompltedSelector);
    const { reportId } = useParams<{ reportId: string }>();



    React.useEffect(() => {
        dispatch(getVisitDemographicReportRequest(Number(reportId)));
        dispatch(setCurrentPage(EVisitRouterKeys.VISIT_DEMOGRAPHIC_REPORTS, ERouterUrls.VISIT_DEMOGRAPHIC_REPORT));
    }, [dispatch, reportId])

    const content = ( 
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={4} style={{ textAlign: 'center' }}>
                    <TitleText>
                        Demographics Report
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
            <RaceComponent />
            <GenderComponent />
            <EducationLevelComponent />
            <LivingReservationComponent />
            <MilitaryExperienceComponent />
            <Container>
                <Header>
                    <Grid container spacing={1}>
                        <Grid item xs={6}>
                            <HeaderTitle>
                                Staff Name
                            </HeaderTitle>
                        </Grid>
                        <Grid item xs={6}>
                            <HeaderTitle>
                                Complete Date
                            </HeaderTitle>
                        </Grid>
                    </Grid>
                </Header>
                <Content>
                    <Grid container spacing={1}>
                        <Grid item xs={6}>
                            <p>
                                {staffName}
                            </p>
                        </Grid>
                        <Grid item xs={6}>
                            <p>
                               {convertDateToStringFormat(completeData)}
                            </p>
                        </Grid>
                    </Grid>
                </Content>
            </Container>
        </ContentContainer>
    );

    return (
        <SideSearchLayout content={content} bar={<VisitDemographicReportTemplate />} isFixed/>
    )
}

export default VisitDemographicReport;