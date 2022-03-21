
import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { IRootState } from '../../../states';
import { 
    isManageListLoadingSelector, getManageDevicesListSelector, 
} from '../../../selectors/manage-devices';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import { 
    IManegeDevicesListResponse, getManageDevicesListRequest 
} from '../../../actions/manage-devices';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
import ManageDevicesList from './manage-devices-list';
import ManageDeviceTemplate from '../../UI/side-search-layout/templates/ManageDeviceTemplate';

export interface IManageDevicePageProps {
    isListLoading?: boolean;
    list: Array<IManegeDevicesListResponse>;
    getManageDevicesList?: () => void;
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IManageDevicePageState {}

class ManageDevicePage extends React.Component<IManageDevicePageProps, IManageDevicePageState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.MANAGE_DEVICES, ERouterUrls.MANAGE_DEVICES);
        if (!this.props.isListLoading && (!Array.isArray(this.props.list) || !this.props.list.length)) {
            this.props.getManageDevicesList && this.props.getManageDevicesList();
        }
    }

    render(): React.ReactElement {
        const content = (<ManageDevicesList />);
        const bar = (<ManageDeviceTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }
}


const mapStateToPtops = (state: IRootState) => ({
    isListLoading: isManageListLoadingSelector(state),
    list: getManageDevicesListSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getManageDevicesList: () => dispatch(getManageDevicesListRequest()),
        setCurrentPage: (key: string, path: string) => dispatch(setCurrentPage(key, path))
    }
}


export default connect(mapStateToPtops, mapDispatchToProps)(ManageDevicePage);