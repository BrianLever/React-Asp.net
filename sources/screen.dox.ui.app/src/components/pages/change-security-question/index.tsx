import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import ChangeSecurityQuestionPage from './change-security-question-page';
import { getSecurityQuestionListRequest } from 'actions/change-security-question';

export interface IChangeSecurityQuestionProps {
    setCurrentPage?: (k: string, p: string) => void;
    getSecurityQuestionListRequest?:() => void;
}
export interface IChangeSecurityQuestionState {}

class ChangeSecurityQuestion extends React.Component<IChangeSecurityQuestionProps, IChangeSecurityQuestionState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.CHANGE_PASSWORD, ERouterUrls.CHANGE_PASSWORD);
        this.props.getSecurityQuestionListRequest && this.props.getSecurityQuestionListRequest();
    }

    public render():React.ReactNode {
        return <ChangeSecurityQuestionPage />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        getSecurityQuestionListRequest:() => {
            dispatch(getSecurityQuestionListRequest())
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(ChangeSecurityQuestion);