import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import { ERouterUrls, EReportsRouterKeys } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import VisitsOutcomesLists from './visits-outcomes';
import VisitsOutcomesTemplate from '../../UI/side-search-layout/templates/VisitsOutcomesTemplate';


export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IReportState {}

class VisitsOutcomes extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.VISITS_OUTCOMES, ERouterUrls.VISITS_OUTCOMES);
    }

    public render():React.ReactNode {
        const content = (<VisitsOutcomesLists />);
        const bar = (<VisitsOutcomesTemplate />);
        return (
            <SideSearchLayout content={content} bar={bar} />
        )
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }   
    }
}


export default connect(mapStateToPtops, mapDispatchToProps)(VisitsOutcomes);