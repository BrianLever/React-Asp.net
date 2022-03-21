import React from 'react';
import { ContentContainer, TitleText } from '../../styledComponents';
import { Grid } from '@material-ui/core';
import { useDispatch, useSelector } from 'react-redux';
import { 
    getCurrentScreeningReportSelector, getReportCreatedDateFormatted, getReportCreatedByFullName, 
    getReportLocation, getReportPatientCity, getReportPatientDateOfBirth, getReportPatientFirstName, 
    getReportPatientHRN, getReportPatientLastName, getReportPatientMailingAddress, getReportPatientMiddleName, 
    getReportPatientPhone, getReportPatientState, getReportPatientZIP, getScreenDefinition, getReportSRN, getReportPatientStateId} from '../../../../selectors/screen/report';
import { 
    getScreeningReportRequest, getScreenReportDefinition} from '../../../../actions/screen/report';
import { IPage } from '../../../../common/types/page';
import ReportHeader from '../../common/reportHeader';
import SideSearchLayout from '../../../../components/UI/side-search-layout';
import ScreenReportTemplate from '../../../UI/side-search-layout/templates/ScreenReportTemplate';
import TobaccoSection from './tobacco';
import CageSection from './cage';
import PHQ9Section from './phq9';
import DastSection from './dast';
import DochSection from './doch';
import GAD7Section from './gad7';
import HitsSection from './hits';
import ProblemGamblingSection from './bbgs';
import { useParams } from "react-router-dom";
import { setCurrentPage } from '../../../../actions/settings';
import { ERouterUrls, EScreenRouterKeys } from '../../../../router';



export interface IScreeningReport extends IPage {}



const ScreeningReport = (props: IScreeningReport): React.ReactElement => {
    
    const dispatch = useDispatch();
    const { reportId } = useParams<{ reportId: string }>();
    const currentScreeeningReport = useSelector(getCurrentScreeningReportSelector);
    const screenDefinition = useSelector(getScreenDefinition);
    const lastName = useSelector(getReportPatientLastName);
    const firstName = useSelector(getReportPatientFirstName);
    const middleName = useSelector(getReportPatientMiddleName);
    const birthDate = useSelector(getReportPatientDateOfBirth);
    const patientHRN = useSelector(getReportPatientHRN);
    const mailingAddress = useSelector(getReportPatientMailingAddress);
    const city = useSelector(getReportPatientCity);
    const state = useSelector(getReportPatientState);
    const stateId = useSelector(getReportPatientStateId);
    const zip = useSelector(getReportPatientZIP);
    const phone = useSelector(getReportPatientPhone);
    const location = useSelector(getReportLocation);
    const createdDate = useSelector(getReportCreatedDateFormatted);
    const createdBy = useSelector(getReportCreatedByFullName);
    const srn = useSelector(getReportSRN);
  
    
    React.useEffect(() => {
        dispatch(setCurrentPage(EScreenRouterKeys.SCREENING_REPORTS, ERouterUrls.SCREENING_REPORTS));
        dispatch(getScreeningReportRequest(Number(reportId)));
        if (!screenDefinition) {
            dispatch(getScreenReportDefinition());
        }
    }, [dispatch, screenDefinition, reportId])

    const content = (
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={4} style={{ textAlign: 'center' }}>
                    <TitleText>
                        Screen Report
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
                state={stateId}
                zip={zip}
                phone={phone}
                location={location}
                createdDate={createdDate}
                createdBy={createdBy}
                srn={srn}
                showSrnAsLink={false}
            />
            <TobaccoSection />
            <CageSection />
            <DastSection />
            <DochSection />
            <PHQ9Section />
            <GAD7Section />
            <HitsSection />
            <ProblemGamblingSection />
        </ContentContainer>
    )

    return (
        <SideSearchLayout content={content}  isFixed bar={<ScreenReportTemplate visitID={currentScreeeningReport?.BhsVisitID} visitStatus={currentScreeeningReport?.BhsVisitStatus == "Open Visit"? true: false } screenReportId={Number(reportId)} />} />
    )
}


export default ScreeningReport;
