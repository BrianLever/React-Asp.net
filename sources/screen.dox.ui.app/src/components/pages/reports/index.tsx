import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import { EReportsRouterKeys, ERouterKeys, ERouterUrls } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ReportsIndicateLists from './reports-indicate-list';
import ReportsIndicateTemplate from '../../UI/side-search-layout/templates/ReportsIndicateTemplate';
import { getListReportByProblemSelector } from '../../../selectors/reports';
import { IReportsInnerItem, postFilteredReportsRequest  } from '../../../actions/reports';

export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
    reports?: Array<IReportsInnerItem>;
    getReportsList:() => void;
}
export interface IReportState {}

class Report extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.INDICATOR_REPORTS, ERouterUrls.INDICATOR_REPORTS);
        if (!Array.isArray(this.props.reports) || !this.props.reports.length) {
            this.props.getReportsList();
        }
    }

    public render():React.ReactNode {
        const content = (<ReportsIndicateLists />);
        const bar = (<ReportsIndicateTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({
    reports: getListReportByProblemSelector(state)
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getReportsList: () => dispatch(postFilteredReportsRequest()),
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }   
    }
}


export default connect(mapStateToPtops, mapDispatchToProps)(Report);