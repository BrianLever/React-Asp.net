import { combineReducers } from 'redux';
import dashboardReducer ,{ IDashboard } from './dashboard';
import screenReducer ,{ IScreenState } from './screen';
import screenDefinitionReducer ,{ IScreenDefinitionState } from './screen/report';
import profileReducer, { IProfileState } from './profile';
import settingsReducer, { ISettingsState } from './settings';
import visitReducer, { IVisitState } from './visit';
import visitReportReducer, { IVisitReportState } from'./visit/report';
import visitDemographicReportReducer, { IVisitDemographicReportState } from'./visit/demographic-report';
import followUpReducer, { IFollowUpState } from './follow-up';
import followUpReportReducer, { IFollowUpReportState } from './follow-up/report';
import managedDevicesReducer, { IManageDeviceState } from './managed-devices';
import branchLocationsReducer, { IBranchLocationsState } from './branch-locations';
import sharedReducer, { ISharedState } from './shared';
import reportsReducer, { IReportsState } from './reports';
import screenProfilesReducer, { IScreenProfilesState } from './screen-profiles'; 
import ageGroupsReducer, { IAgeGroupsState } from './age-groups';
import visitSettingsReducer, { IVisitSettingsState } from './visit-settings';
import errorLogReducer, { IErrorLogState } from './error-log';
import securityLogSettingsReducer, { ISecurityLogSettingsState } from './security-log-settings';
import licenseKeysReducer, { ILicenseKeysState } from './license-keys';
import loginReducer, { ILoginState } from './login';
import changeSecurityQuestionReducer, { IChangeSecurityQuestionState } from './change-security-question';
import autoExportLogsReducer, { IAutoExportLogsState } from './auto-export-logs';
import findPatientAddressReducer, { IFindPatientAddressState } from './find-patient-address';
import cssrsListReducer, { ICssrsListState } from './c-ssrs-list';
import resetPasswordReducer, { IResetPasswordState } from './reset-password';
import cssrsReportReducer, { ICssrsReportState } from './c-ssrs-list/c-ssrs-report';
import manageUsersReducer, { IManageUsersState } from './manage-users';
import ehrLoginReducer, { IEhrLoginState } from './ehr-login';


export interface IRootState {
    visit: IVisitState;
    screen: IScreenState;
    profile: IProfileState;
    followUp: IFollowUpState,
    settings: ISettingsState;
    dashboard: IDashboard;
    visitReport: IVisitReportState;
    manageDevice: IManageDeviceState;
    screenReport: IScreenDefinitionState;
    followUpReport: IFollowUpReportState;
    visitDemographicReport: IVisitDemographicReportState;
    branchLocations: IBranchLocationsState;
    shared: ISharedState,
    reports: IReportsState,
    screenProfiles: IScreenProfilesState,
    ageGroups: IAgeGroupsState,
    visitSettings: IVisitSettingsState,
    errorLogs: IErrorLogState,
    securityLogSettings: ISecurityLogSettingsState,
    licenseKeys: ILicenseKeysState,
    login: ILoginState,
    changeSecurityQuestion: IChangeSecurityQuestionState,
    autoExportLogs: IAutoExportLogsState,
    findPatientAddress: IFindPatientAddressState,
    cssrsList: ICssrsListState,
    resetPassword: IResetPasswordState,
    cssrsReport: ICssrsReportState,
    manageUsers: IManageUsersState,
    ehrLogin: IEhrLoginState,
}

const reducersCombineObject = {
    shared: sharedReducer,
    visit: visitReducer,
    screen: screenReducer,
    profile: profileReducer,
    followUp: followUpReducer,
    settings: settingsReducer,
    dashboard: dashboardReducer,
    visitReport: visitReportReducer,
    manageDevice: managedDevicesReducer,
    screenReport: screenDefinitionReducer,
    followUpReport: followUpReportReducer,
    branchLocations: branchLocationsReducer,
    visitDemographicReport: visitDemographicReportReducer,
    reports: reportsReducer,
    screenProfiles: screenProfilesReducer,
    ageGroups: ageGroupsReducer,
    visitSettings: visitSettingsReducer,
    errorLogs: errorLogReducer,
    securityLogSettings: securityLogSettingsReducer,
    licenseKeys:  licenseKeysReducer,
    login: loginReducer,
    changeSecurityQuestion: changeSecurityQuestionReducer,
    autoExportLogs: autoExportLogsReducer,
    findPatientAddress: findPatientAddressReducer,
    cssrsList: cssrsListReducer,
    resetPassword: resetPasswordReducer,
    cssrsReport: cssrsReportReducer,
    manageUsers: manageUsersReducer,
    ehrLogin: ehrLoginReducer,
}

export default combineReducers(reducersCombineObject);