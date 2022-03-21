import React from 'react';
import { connect } from 'react-redux';
import { IPage } from '../../../common/types/page';
import VisitListComponent from './visit-list';
import { IRootState } from '../../../states';
import { Dispatch } from 'redux';
import { 
    getVisitItemsSelector, getTotalVisitsSelector, isVisitLoadingSelector
} from '../../../selectors/visit'
import SideSearchLayout from '../../../components/UI/side-search-layout';
import { getAllVisitRequest, IVisitResponseItem } from '../../../actions/visit';
import VisitSearchTemplate from 'components/UI/side-search-layout/templates/VisitListTemplate';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';
  
export interface IVisitProps extends IPage {
    visitList?: Array<IVisitResponseItem>;
    totalVisits: number;
    isVisitLoading: boolean;
    getVisitList: () => void;
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IVisitState {}

class VisitPage extends React.Component<IVisitProps, IVisitState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.VISIT, ERouterUrls.VISIT);
        if (!Array.isArray(this.props.visitList) || !this.props.visitList.length) {
            // this.props.getVisitList();
        }
    }

    public render(): React.ReactNode {
        const content = (<VisitListComponent />);
        const bar = (<VisitSearchTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({
    visitList: getVisitItemsSelector(state),
    totalVisits: getTotalVisitsSelector(state),
    isVisitLoading: isVisitLoadingSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getVisitList: () => dispatch(getAllVisitRequest()),    
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }     
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(VisitPage);