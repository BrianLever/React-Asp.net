import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import FollowUpList from '../follow-up/follow-up-list';
import FollowUpTemplate from '../../UI/side-search-layout/templates/FollowUpTemplate';
import { IRootState } from '../../../states';
import { IFollowUpResponseItem, postFilteredFollowUpsRequest } from '../../../actions/follow-up';
import { isFollowUpLoadingSelector, getListFollowUpSelector } from '../../../selectors/follow-up';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';

export interface IFollowUpProps {
    getFollowUpList: () => void;
    setCurrentPage?: (k: string, p: string) => void;
    followUpList?: Array<IFollowUpResponseItem>;
    isFollowUpListLoading: boolean;
}
export interface IFollowUpState {}

class FollowUpPage extends React.Component<IFollowUpProps, IFollowUpState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.FOLLOW_UP, ERouterUrls.FOLLOW_UP);
        if (!Array.isArray(this.props.followUpList) || !this.props.followUpList.length) {
            this.props.getFollowUpList();
        }
    }

    public render(): React.ReactNode {
        const content = (<FollowUpList />);
        const bar = (<FollowUpTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({
    followUpList: getListFollowUpSelector(state),
    isFollowUpListLoading: isFollowUpLoadingSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getFollowUpList: () => dispatch(postFilteredFollowUpsRequest()),
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }   
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(FollowUpPage);
