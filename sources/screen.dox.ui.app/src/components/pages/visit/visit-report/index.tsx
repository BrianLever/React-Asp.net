import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Grid } from '@material-ui/core';
import { 
    ContentContainer, TitleText 
} from '../../styledComponents';
import { IPage } from '../../../../common/types/page';
import { getFollowUPVisitArrayRequest, getVisitReportRequest } from '../../../../actions/visit/report';
import ReportHeader from '../../../../components/pages/common/reportHeader';
import { 
    getVisitReportPatientLastName, getVisitReportPatientFirstName,
    getVisitReportPatientMiddleName, getVisitReportPatientBirthDate, getVisitReportExportedToHRN,
    getVisitReportPatientStreetAddress, getVisitReportPatientCity, getVisitReportPatientStateName,
    getVisitReportPatientZipCode, getVisitReportPatientPhone, getVisitReportLocation, getVisitReportCompleteDate,
    getVisitReporStaffNameCompleted, getVisitReportScreeningResultIdSelector
} from '../../../../selectors/visit/report';


import SideSearchLayout from '../../../../components/UI/side-search-layout';
import VisitReportTemplate from '../../../../components/UI/side-search-layout/templates/VisitReportTemplate';
import VisitReportDataSet from './visit-report-data-set';
import { ERouterUrls, EVisitRouterKeys } from '../../../../router';
import { setCurrentPage } from '../../../../actions/settings';
import { useParams } from "react-router-dom";


export interface IVisitReportReport extends IPage {}

const VisitReportPage = (props: IVisitReportReport): React.ReactElement => {
    // TODO: changes
    const dispatch = useDispatch();
    const lastName = useSelector(getVisitReportPatientLastName);
    const firstName = useSelector(getVisitReportPatientFirstName);
    const middleName = useSelector(getVisitReportPatientMiddleName);
    const birthDate = useSelector(getVisitReportPatientBirthDate);
    const patientHRN = useSelector(getVisitReportExportedToHRN);
    const mailingAddress = useSelector(getVisitReportPatientStreetAddress);
    const city = useSelector(getVisitReportPatientCity);
    const state = useSelector(getVisitReportPatientStateName);
    const zip = useSelector(getVisitReportPatientZipCode);
    const phone = useSelector(getVisitReportPatientPhone);
    const location = useSelector(getVisitReportLocation);
    const createdDate = useSelector(getVisitReportCompleteDate);
    const createdBy = useSelector(getVisitReporStaffNameCompleted);
    const srn = useSelector(getVisitReportScreeningResultIdSelector);
    const { reportId } = useParams<{ reportId: string }>();


    React.useEffect(() => {
        dispatch(getVisitReportRequest(Number(reportId)));
        dispatch(getFollowUPVisitArrayRequest(Number(reportId)))
        dispatch(setCurrentPage(EVisitRouterKeys.VISIT_REPORTS, ERouterUrls.VISIT_REPORTS));
    }, [dispatch, reportId])

    const content = (
        <ContentContainer>
            <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
                <Grid item xs={4} style={{ textAlign: 'center' }}>
                    <TitleText>
                        Visit Report
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
            <VisitReportDataSet />
        </ContentContainer>
    );

    return (
        <SideSearchLayout content={content} bar={<VisitReportTemplate />} isFixed/>
    )
}

export default VisitReportPage;