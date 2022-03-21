import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import EhrLoginList from './ehr-login-list';
import { getEhrLoginListRequest } from 'actions/ehr-login';


export interface IEhrLoginProps {
    setCurrentPage?: (k: string, p: string) => void;
    getEhrLoginListRequest?: () => void;
}
export interface IEhrLoginState {}

class EhrLogin extends React.Component<IEhrLoginProps, IEhrLoginState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.EHR_LOGIN, ERouterUrls.EHR_LOGIN);
        this.props.getEhrLoginListRequest && this.props.getEhrLoginListRequest();
    }

    public render():React.ReactNode {
        return <EhrLoginList />
    }

}

const mapStateToProps = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        getEhrLoginListRequest: () => {
            dispatch(getEhrLoginListRequest())
        }
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(EhrLogin);