import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import AutoExportLogList from './auto-export-log-list';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import AutoExportLogTemplate from '../../UI/side-search-layout/templates/AutoExportLogTemplate';

export interface IAutoExportLogProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IAutoExportLogState {}

class AutoExportLog extends React.Component<IAutoExportLogProps, IAutoExportLogState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.AUTO_EXPORT_DASHBOARD, ERouterUrls.AUTO_EXPORT_DASHBOARD);
    }

    public render():React.ReactNode {
        const content = (<AutoExportLogList />);
        const bar = (<AutoExportLogTemplate />);
        return (<SideSearchLayout content={content} bar={bar} />)
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

export default connect(mapStateToPtops, mapDispatchToProps)(AutoExportLog);