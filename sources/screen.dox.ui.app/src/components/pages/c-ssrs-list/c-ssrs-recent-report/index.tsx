import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls, EAssessmentRouterKeys } from '../../../../router';
import { setCurrentPage } from '../../../../actions/settings';
import { IRootState } from '../../../../states';
import SideSearchLayout from '../../../UI/side-search-layout';
import CssrsReportComponent from '../c-ssrs-report-component';
import CssrsRecentReportTemplate from 'components/UI/side-search-layout/templates/CssrsRecentReportTemplate';
import { cssrsReportDetailRequest } from 'actions/c-ssrs-list/c-ssrs-report';


export interface ICssrsRecentReportProps {
    setCurrentPage?: (k: string, p: string) => void;
    cssrsReportDetailRequest?: (id: number) => void;
}
export interface ICssrsRecentReportState {}

class CssrsRecentReport extends React.Component<ICssrsRecentReportProps, ICssrsRecentReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EAssessmentRouterKeys.CSSRS_LIFETIME_ADD_REPORT, ERouterUrls.CSSRS_LIFETIME_ADD_REPORT);
    }

    public render():React.ReactNode {
        const content = (<CssrsReportComponent />);
        const bar = (<CssrsRecentReportTemplate/>);
        return <SideSearchLayout content={content} bar={bar} isFixed />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        cssrsReportDetailRequest: (reportId: number) => {
            dispatch(cssrsReportDetailRequest(reportId))
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(CssrsRecentReport);