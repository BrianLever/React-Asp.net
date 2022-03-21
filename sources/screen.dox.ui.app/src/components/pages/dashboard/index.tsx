import React from 'react';
import { connect } from 'react-redux';
import { IRootState } from '../../../states';
import { getSystemSettingsRequest, ISystemSettings } from '../../../actions/dashboard';
import { Grid } from '@material-ui/core';
import { BoxLink } from '../../UI/custom-link/';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { IPage } from '../../../common/types/page';
import { Dispatch } from 'redux';
import { getProfileRequest } from '../../../actions/profile';
import * as dashboardComponents from './styledComponents';
import { getFullNameSelector, getFirstNameSelector } from '../../../selectors/profile';
import ScreendoxHeading from '../../UI/typography';
import { setCurrentPage } from '../../../actions/settings';

export interface IDashboardProps extends IPage {
    systemSettingsRequest?: () => void;
    userProfileRequest?: () => void;
    setCurrentPage?: (k: string, p: string) => void;
    isLoadingSystemSettings?: boolean;
    isSystemSettingsError?: boolean;
    systemSettings?: ISystemSettings;
    fullName?: string;
    firstName?: string;
}
export interface IDashboardState {}

class Dashboard extends React.Component<IDashboardProps, IDashboardState> {

    state = {
        isLoadingSystemSettings: false,
        systemSettings: undefined,
    }

    componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.DASHBOARD, ERouterUrls.DASHBOARD);
        if ((!this.props.isLoadingSystemSettings && !this.props.systemSettings?.AppVersion)) {
            this.props.systemSettingsRequest && this.props.systemSettingsRequest();
            this.props.userProfileRequest && this.props.userProfileRequest();
        }
    }

    public render():React.ReactNode {

        const { CheckInRecordCount = 0, BranchLocationCount = 0, KioskCount = 0 } = this.props.systemSettings?.Summary || {};
        return (
            <dashboardComponents.XMainFull>
                <dashboardComponents.DashboardEntryContainer>
                    <dashboardComponents.DashboardCSContent>
                        <dashboardComponents.DashboardSection>
                            <dashboardComponents.DashboardRowTwo>
                                <dashboardComponents.DashboardXRowInner>
                                    <dashboardComponents.DashboardRowCole23XCol>
                                        <dashboardComponents.DashboardRowCole24XCol>
                                            <dashboardComponents.DashboardXRowInner2>
                                                <dashboardComponents.DashboardRowCole25XCol>
                                                    <dashboardComponents.DashboardRowCole26XColXTextHandler>
                                                        <dashboardComponents.DashboardXTextContent>
                                                            <dashboardComponents.DashboardXTextContentText>
                                                                <dashboardComponents.XTextContentTextPrimary>
                                                                    Welcome back,
                                                                </dashboardComponents.XTextContentTextPrimary>
                                                                <dashboardComponents.XTextContentTextPrimary>
                                                                    { this.props.firstName }! 
                                                                </dashboardComponents.XTextContentTextPrimary>
                                                            </dashboardComponents.DashboardXTextContentText>
                                                        </dashboardComponents.DashboardXTextContent>
                                                    </dashboardComponents.DashboardRowCole26XColXTextHandler>
                                                </dashboardComponents.DashboardRowCole25XCol>
                                                <dashboardComponents.DashboardRowCole27XCol>
                                                    <dashboardComponents.E28XImage>
                                                        <dashboardComponents.E28XImageImg>
                                                            <img src="../assets/ani-trans-full-new.gif" width="250" height="216 " alt="ani-trans-full-new" loading="lazy" />
                                                        </dashboardComponents.E28XImageImg>
                                                    </dashboardComponents.E28XImage>
                                                </dashboardComponents.DashboardRowCole27XCol>
                                            </dashboardComponents.DashboardXRowInner2>
                                        </dashboardComponents.DashboardRowCole24XCol>
                                    </dashboardComponents.DashboardRowCole23XCol>
                                    <dashboardComponents.DashboardRowCole29XCol>
                                        <dashboardComponents.DashboardE210XRow>
                                            <dashboardComponents.DashboardXRowInner>
                                                <dashboardComponents.DashboardE211XCol>
                                                    <dashboardComponents.Dashboarde212XTextXTextHeadline>
                                                        <div style={{ display: 'felx' }}>
                                                            <div style={{ display: 'block', flexGrow: 1, minWidth: '1px', maxWidth: '100%' }}>
                                                                <dashboardComponents.DashboardeXTextContentTextPrimary>
                                                                    RSBCIHI
                                                                </dashboardComponents.DashboardeXTextContentTextPrimary>
                                                            </div>
                                                        </div>
                                                    </dashboardComponents.Dashboarde212XTextXTextHeadline>
                                                    <dashboardComponents.Dashboardee213XImage>
                                                        <dashboardComponents.E28XImageImg>
                                                            <img src="../assets/custom_logo.svg" width="0" height="0" alt="RSBCIHI_logo" loading="lazy" />
                                                        </dashboardComponents.E28XImageImg>
                                                    </dashboardComponents.Dashboardee213XImage>
                                                </dashboardComponents.DashboardE211XCol>
                                            </dashboardComponents.DashboardXRowInner>
                                            <dashboardComponents.DashboardE214XCol>
                                                <dashboardComponents.DashboardE215XTextXTextHeadline>
                                                    <div style={{ display: 'block', flexGrow: 1, minWidth: '1px', maxWidth: '100%' }}>
                                                        <dashboardComponents.DashboardXTextContentTextPrimary>
                                                            Here is a quick overview:
                                                        </dashboardComponents.DashboardXTextContentTextPrimary>
                                                    </div>
                                                </dashboardComponents.DashboardE215XTextXTextHeadline>
                                                <dashboardComponents.DashboardText style={{ marginTop: '21px' }}>Check-In record count: <b>{CheckInRecordCount}</b></dashboardComponents.DashboardText>
                                                <dashboardComponents.DashboardText style={{ marginTop: '7px', marginBottom: '7px' }}>Branch Location count: <b>{BranchLocationCount}</b></dashboardComponents.DashboardText>
                                                <dashboardComponents.DashboardText style={{ marginBottom: '7px' }}>Device count:  <b>{ KioskCount }</b></dashboardComponents.DashboardText>
                                            </dashboardComponents.DashboardE214XCol>
                                        </dashboardComponents.DashboardE210XRow>
                                    </dashboardComponents.DashboardRowCole29XCol>
                                </dashboardComponents.DashboardXRowInner>
                            </dashboardComponents.DashboardRowTwo>
                            <Grid container justifyContent="flex-start" alignItems="center" spacing={2} style={{ marginBottom: '15px', marginTop: '15px' }}>
                                <Grid item md={8} lg={4}>
                                    <ScreendoxHeading size="1.3em" style={{ marginTop: '29px' }} bold={'700'}>
                                        What do you want to do?
                                    </ScreendoxHeading>
                                </Grid>
                            </Grid>
                            <Grid container justifyContent="space-between" alignItems="center" spacing={2}>
                                <Grid item xs={6} sm={6} md={6} lg={3} style={{ textAlign: 'center' }}>
                                    <BoxLink link={ERouterUrls.SCREEN} src={'../assets/screen.svg'}/>
                                    <ScreendoxHeading size="16px;" style={{ marginTop: '14px' }} bold={'700'}>
                                        Screen
                                    </ScreendoxHeading>
                                </Grid>
                                <Grid item xs={6} sm={6} md={6} lg={3} style={{ textAlign: 'center' }}>
                                    <BoxLink link={ERouterUrls.ASSESSMENT} src={'../assets/assess-w.svg'}/>
                                    <ScreendoxHeading size="16px;" style={{ marginTop: '14px' }} bold={'700'}>
                                        Assessment
                                    </ScreendoxHeading>
                                </Grid>
                                <Grid item xs={6} sm={6} md={6} lg={3} style={{ textAlign: 'center' }}>
                                     <BoxLink link={ERouterUrls.VISIT} src={'../assets/visit.svg'}/>
                                     <ScreendoxHeading size="16px;" style={{ marginTop: '14px' }} bold={'700'}>
                                        Visit
                                    </ScreendoxHeading>
                                </Grid>
                                <Grid item xs={6} sm={6} md={6} lg={3} style={{ textAlign: 'center' }}>
                                    <BoxLink link={ERouterUrls.REPORTS} src={'../assets/follow.svg'}/>
                                    <ScreendoxHeading size="16px;" style={{ marginTop: '14px' }} bold={'700'}>
                                        Follow-Up
                                    </ScreendoxHeading>
                                </Grid>
                            </Grid>
                        </dashboardComponents.DashboardSection>
                    </dashboardComponents.DashboardCSContent>
                </dashboardComponents.DashboardEntryContainer>
            </dashboardComponents.XMainFull>
        );
    }

}

const mapStateToPtops = (state: IRootState) => ({
    isLoadingSystemSettings: state.dashboard.isLoadingSystemSettings,
    isSystemSettingsError: state.dashboard.isSystemSettingsError,
    systemSettings: state.dashboard.systemSettings,
    fullName: getFullNameSelector(state),
    firstName: getFirstNameSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        systemSettingsRequest: () => {
            dispatch(getSystemSettingsRequest());
        },
        userProfileRequest: () => {
            dispatch(getProfileRequest(1));
        },
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(Dashboard);