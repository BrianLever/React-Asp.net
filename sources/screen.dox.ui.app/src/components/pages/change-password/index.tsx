import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import ChangePasswordPage from './change-password-page';


export interface IChangePasswordProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IChangePasswordState {}

class ChangePassword extends React.Component<IChangePasswordProps, IChangePasswordState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.CHANGE_PASSWORD, ERouterUrls.CHANGE_PASSWORD);
    }

    public render():React.ReactNode {
        return <ChangePasswordPage />
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

export default connect(mapStateToPtops, mapDispatchToProps)(ChangePassword);