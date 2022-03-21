import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import ErrorLogList from './error-log-list';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ErrorLogTemplate from '../../UI/side-search-layout/templates/ErrorLogTemplate';

export interface IErrorLogProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IErrorLogState {}

class ErrorLog extends React.Component<IErrorLogProps, IErrorLogState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.ERROR_LOG, ERouterUrls.ERROR_LOG);
    }

    public render():React.ReactNode {
        const content = (<ErrorLogList />);
        const bar = (<ErrorLogTemplate />);
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

export default connect(mapStateToPtops, mapDispatchToProps)(ErrorLog);