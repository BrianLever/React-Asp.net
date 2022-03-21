import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import ProfileComponent from './profile-component';
import { getListBranchLocationsRequest } from 'actions/shared';
import { getSecurityQuestionListRequest } from 'actions/change-security-question';


export interface IProfileProps {
    setCurrentPage?: (k: string, p: string) => void;
    getBranchLocations?:() => void;
    getSecurityQuestionListRequest?:() => void;
}
export interface IProfileState {}

class Profile extends React.Component<IProfileProps, IProfileState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.PROFILE, ERouterUrls.PROFILE);
        this.props.getBranchLocations && this.props.getBranchLocations();
        this.props.getSecurityQuestionListRequest && this.props.getSecurityQuestionListRequest();
    }

    public render():React.ReactNode {
        return <ProfileComponent />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        getBranchLocations: () => {
            dispatch(getListBranchLocationsRequest())
        },
        getSecurityQuestionListRequest: () => {
            dispatch(getSecurityQuestionListRequest())
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(Profile);