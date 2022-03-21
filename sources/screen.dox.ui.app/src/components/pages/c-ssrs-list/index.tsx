import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls, EAssessmentRouterKeys } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import CSSRSListComponent from './c-ssrs-list-component';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import CssrsListTemplate from '../../../components/UI/side-search-layout/templates/CssrsListTemplate';
import { getCssrsListRequest } from 'actions/c-ssrs-list';
import { getListBranchLocationsRequest } from 'actions/shared';

export interface ICSSRSListProps {
    setCurrentPage?: (k: string, p: string) => void;
    cssrsListRequest?: () => void;
    getLocationsRequest?: () => void;
}
export interface ICSSRSListState {}

class CSSRSListPage extends React.Component<ICSSRSListProps, ICSSRSListState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EAssessmentRouterKeys.CSSRS_LIST, ERouterUrls.CSSRS_LIST);
        this.props.cssrsListRequest && this.props.cssrsListRequest();
        this.props.getLocationsRequest && this.props.getLocationsRequest();
    }

    public render():React.ReactNode {
        const content = (<CSSRSListComponent />);
        const bar = (<CssrsListTemplate />);
        return <SideSearchLayout content={content} bar={bar} />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        cssrsListRequest: () => {
            dispatch(getCssrsListRequest());
        },
        getLocationsRequest: () => {
            dispatch(getListBranchLocationsRequest())
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(CSSRSListPage);