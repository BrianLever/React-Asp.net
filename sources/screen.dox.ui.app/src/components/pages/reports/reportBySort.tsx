import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import {  ERouterUrls, EReportsRouterKeys } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ReportsBySortLists from './reports-by-sort-list';
import ReportBySortTemplate from '../../UI/side-search-layout/templates/ReportBySortTemplate';


export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IReportState {}

class ReportBySort extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.REPORTS_BY_SORT, ERouterUrls.REPORTS_BY_SORT);
    }

    public render():React.ReactNode {
        const content = (<ReportsBySortLists />);
        const bar = (<ReportBySortTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }   
    }
}


export default connect(mapStateToPtops, mapDispatchToProps)(ReportBySort);