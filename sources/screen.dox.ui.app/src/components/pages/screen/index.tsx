import React from 'react';
import { connect } from 'react-redux';
import { IPage } from '../../../common/types/page';
import ScreenList from './screen-list';
import { IRootState } from '../../../states';
import { Dispatch } from 'redux';
import { 
    isScreenListErrorSelector, 
    isScreenListLoadingSelector, 
    screenListSelector, 
} from '../../../selectors/screen'
import { IScreenListResponseItem, postScreenListFilterRequest, postScreenListFilterRequestAutoUpdate, postScreenListRequest } from '../../../actions/screen';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ScreenSearchTemplate from '../../UI/side-search-layout/templates/ScreenSearchTemplate';
import { setCurrentPage } from 'actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { resetScreeningReportRequest } from 'actions/screen/report';
  
export interface IScreenProps extends IPage {
    screenList?: IScreenListResponseItem[],
    isScreenListLoading?: boolean,
    isScreenListError?: boolean,
    getScreenList: () => void;
    setCurrentPage?: (k: string, p: string) => void;
    postScreenListFilterRequest?:() => void;
    postScreenListFilterRequestAutoUpdate?:() => void;
}
export interface IScreenState {}

class ScreenPage extends React.Component<IScreenProps, IScreenState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.SCREEN, ERouterUrls.SCREEN);
        if (!Array.isArray(this.props.screenList) || !this.props.screenList.length) {
            this.props.getScreenList();
        }
        // this.props.postScreenListFilterRequestAutoUpdate && this.props.postScreenListFilterRequestAutoUpdate();
        this.props.postScreenListFilterRequest && this.props.postScreenListFilterRequest();
    }

    public render(): React.ReactNode {
        const content = (<ScreenList />);
        const bar = (<ScreenSearchTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({
    screenList: screenListSelector(state),
    isScreenListLoading: isScreenListLoadingSelector(state),
    isScreenListError: isScreenListErrorSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getScreenList: () => dispatch(postScreenListRequest()),
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        postScreenListFilterRequest: () => dispatch(postScreenListFilterRequest({ page: 1 })),
        postScreenListFilterRequestAutoUpdate: () => dispatch(postScreenListFilterRequestAutoUpdate()),
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(ScreenPage);