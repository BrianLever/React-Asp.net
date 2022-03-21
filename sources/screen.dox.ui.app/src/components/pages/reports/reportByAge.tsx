import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import {  ERouterUrls, EReportsRouterKeys } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ReportsByAgeLists from './reports-by-age-list';
import ReportByAgeTemplate from '../../UI/side-search-layout/templates/ReportByAgeTemplate';


export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IReportState {}

class ReportByAge extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.REPORTS_BY_AGE, ERouterUrls.REPORTS_BY_AGE);
    }

    public render():React.ReactNode {
        const content = (<ReportsByAgeLists />);
        const bar = (<ReportByAgeTemplate />);
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


export default connect(mapStateToPtops, mapDispatchToProps)(ReportByAge);