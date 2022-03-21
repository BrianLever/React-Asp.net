
import React from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { IRootState } from '../../../states';
import ScreenProfilesList from './screen-profiles-list';
import SideSearchLayout from '../../../components/UI/side-search-layout';
import ScreenProfilesTemplate from '../../../components/UI/side-search-layout/templates/ScreenProfilesTemplate';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';

export interface IScreenProfilePageProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface IScreenProfilePageState {}

class ScreenProfilesPage extends React.Component<IScreenProfilePageProps, IScreenProfilePageState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.SCREEN_PROFILES, ERouterUrls.SCREEN_PROFILES);
    }

    render(): React.ReactElement {
        const content = (<ScreenProfilesList />);
        const bar = (<ScreenProfilesTemplate />);
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

export default connect(mapStateToPtops, mapDispatchToProps)(ScreenProfilesPage);