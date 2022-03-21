import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { IRootState } from '../../../states';
import UsersList from './users-list';
import { getManageUsersListRequest } from 'actions/manage-users';
import ManageUsersTemplate from 'components/UI/side-search-layout/templates/ManageUsersTemplate';


export interface IManageUsersProps {
    setCurrentPage?: (k: string, p: string) => void;
    getUsersList?: () => void;
}
export interface IManageUsersState {}

class ManageUsers extends React.Component<IManageUsersProps, IManageUsersState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.MANAGE_USERS, ERouterUrls.MANAGE_USERS);
        this.props.getUsersList && this.props.getUsersList();
    }

    public render(): React.ReactNode {
        const content = (<UsersList />);
        const bar = (<ManageUsersTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({
    
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        getUsersList: () => {
            dispatch(getManageUsersListRequest())
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(ManageUsers);
