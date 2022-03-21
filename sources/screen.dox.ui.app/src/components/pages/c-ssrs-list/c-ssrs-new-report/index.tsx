import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls, EAssessmentRouterKeys } from '../../../../router';
import { setCurrentPage } from '../../../../actions/settings';
import { IRootState } from '../../../../states';
import SideSearchLayout from '../../../UI/side-search-layout';
import CssrsNewReportTemplate from 'components/UI/side-search-layout/templates/CssrsReportTemplate';
import {  ContentContainer, TitleText } from '../../styledComponents';
import { Grid } from '@material-ui/core';
import CssrsReportComponent from '../c-ssrs-report-component';
import CssrsNewReportComponent from '../c-ssrs-new-report-component';
import { cssrsReportPatientRecordsRequest } from 'actions/c-ssrs-list/c-ssrs-report';


export interface ICssrsNewReportProps {
    setCurrentPage?: (k: string, p: string) => void;
    cssrsReportPatientRequest?: () => void;
}
export interface ICssrsNewReportState {}

class CssrsNewReportPage extends React.Component<ICssrsNewReportProps, ICssrsNewReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EAssessmentRouterKeys.CSSRS_LIFETIME_ADD_REPORT, ERouterUrls.CSSRS_LIFETIME_ADD_REPORT);
        this.props.cssrsReportPatientRequest && this.props.cssrsReportPatientRequest();
    }

    public render():React.ReactNode {
        const content = (<CssrsNewReportComponent />);
        const bar = (<CssrsNewReportTemplate />);
        return <SideSearchLayout content={content} bar={bar} />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        cssrsReportPatientRequest: () => {
            dispatch(cssrsReportPatientRecordsRequest());
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(CssrsNewReportPage);