import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import { ERouterUrls, EReportsRouterKeys } from '../../../router';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import DrugsByAgeLists from './drugs-by-age-list';
import DrugByAgeTemplate from '../../UI/side-search-layout/templates/DrugByAgeTemplate';


export interface IReportProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IReportState {}

class DrugByAge extends React.Component<IReportProps, IReportState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(EReportsRouterKeys.DRUGS_BY_AGE, ERouterUrls.DRUGS_BY_AGE);
    }

    public render():React.ReactNode {
        const content = (<DrugsByAgeLists />);
        const bar = (<DrugByAgeTemplate />);
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


export default connect(mapStateToPtops, mapDispatchToProps)(DrugByAge);