import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import SecurityLogSettingsList from './security-log-settings-list';



export interface ISecurityLogSettingsProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface ISecurityLogSettingsState {}

class SecurityLogSettings extends React.Component<ISecurityLogSettingsProps, ISecurityLogSettingsState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.SECURITY_LOG_SETTINGS, ERouterUrls.SECURITY_LOG_SETTINGS);
    }

    public render():React.ReactNode {
        return <SecurityLogSettingsList />
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

export default connect(mapStateToPtops, mapDispatchToProps)(SecurityLogSettings);