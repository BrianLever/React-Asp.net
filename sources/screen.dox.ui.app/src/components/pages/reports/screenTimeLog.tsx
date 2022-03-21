import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import { ERouterUrls, EReportsRouterKeys } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ScreenTimeLogLists from './screen-time-log';
import ScreenTimeLogTemplate from '../../UI/side-search-layout/templates/ScreenTimeLogTemplate';


export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IReportState {}

class ScreenTimeLog extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.SCREEN_TIME_LOG, ERouterUrls.SCREEN_TIME_LOG);
    }

    public render():React.ReactNode {
        const content = (<ScreenTimeLogLists />);
        const bar = (<ScreenTimeLogTemplate />);
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


export default connect(mapStateToPtops, mapDispatchToProps)(ScreenTimeLog);