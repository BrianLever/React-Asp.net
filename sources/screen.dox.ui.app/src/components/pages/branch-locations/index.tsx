
import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { IRootState } from '../../../states';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
import BranchLocationsList from './branch-location-list';
import BranchLocationsTemplate from '../../UI/side-search-layout/templates/BranchLocationsTemplate';
import { getBranchLocationsRequest, IBranchLocationItemResponse } from '../../../actions/branch-locations';
import { getBranchLocationArraySelector, isBranchLocationsLoadingSelector } from '../../../selectors/branch-locations';

export interface IManageDevicePageProps {
    isListLoading?: boolean;
    list:Array<IBranchLocationItemResponse>;
    getBranchLocations?: () => void;
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IManageDevicePageState {}

class BranchLocationsPage extends React.Component<IManageDevicePageProps, IManageDevicePageState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.BRANCH_LOCATION, ERouterUrls.BRANCH_LOCATION);
        if (!this.props.isListLoading && (!Array.isArray(this.props.list) || !this.props.list.length)) {
            this.props.getBranchLocations && this.props.getBranchLocations();
        }
    }

    render(): React.ReactElement {
        const content = (<BranchLocationsList />);
        const bar = (<BranchLocationsTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }
}


const mapStateToPtops = (state: IRootState) => ({
    isListLoading: isBranchLocationsLoadingSelector(state),
    list: getBranchLocationArraySelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getBranchLocations: () => dispatch(getBranchLocationsRequest()),
        setCurrentPage: (key: string, path: string) => dispatch(setCurrentPage(key, path))
    }
}


export default connect(mapStateToPtops, mapDispatchToProps)(BranchLocationsPage);