import { lazy } from 'react';
import { RouteProps } from 'react-router-dom';

const DashBoardPage = lazy(() => import('./components/pages/dashboard'));
const ScreenPage = lazy(() => import('./components/pages/screen'));
const VisitPage = lazy(() => import('./components/pages/visit'));
const VisitReportPage = lazy(() => import('./components/pages/visit/visit-report'));
const VisitDemographicReport = lazy(() => import('./components/pages/visit/visit-demigraphic-report'));
const FollowUpPage = lazy(() => import('./components/pages/follow-up'));
const FollowUpReportPage = lazy(() => import('./components/pages/follow-up/follow-up-report'));
const ReportPage = lazy(() => import('./components/pages/reports'));
const ScreeningReportPage = lazy(() => import('./components/pages/screen/screening-report'));
const ManageDevicePage =  lazy(() => import('./components/pages/manage-devices'));
const BranchLocationsPage = lazy(() => import('./components/pages/branch-locations'));
const ReportByAge= lazy(() => import('./components/pages/reports/reportByAge'));
const DrugByAge= lazy(() => import('./components/pages/reports/drugByAge'));
const PatientDemographics= lazy(() => import('./components/pages/reports/patientDemographics'));
const FollowupOutcomes= lazy(() => import('./components/pages/reports/followupOutcomes'));
const VisitsOutcomes= lazy(() => import('./components/pages/reports/visitsOutcomes'));
const ScreenTimeLog= lazy(() => import('./components/pages/reports/screenTimeLog'));
const ExportToExcel= lazy(() => import('./components/pages/reports/exportToExcel'));
const ScreenProfilesPage = lazy(() => import('./components/pages/screen-profiles'));
const AgeGroups  = lazy(() => import('./components/pages/age-groups'));
const VisitSettings = lazy(() => import('./components/pages/visit-settings'));
const ReportsBySort = lazy(() => import('./components/pages/reports/reportBySort'));
const ErrorLog = lazy(() => import('./components/pages/error-log'));
const AutoExportLog = lazy(() => import('./components/pages/auto-export-log'));
const SecurityLogSettings = lazy(() => import('./components/pages/security-log-settings'));
const LicenseKeys = lazy(() => import('./components/pages/license-keys'));
const Login = lazy(() => import('./components/pages/login'));
const ChangePassword = lazy(() => import('./components/pages/change-password'));
const ChangeSecurityQuestion = lazy(() => import('./components/pages/change-security-question'));
const FindPatientAddress = lazy(() => import('./components/pages/find-patient-address'));
const CSSRSListPage = lazy(() => import('./components/pages/c-ssrs-list'));
const CssrsAddReport = lazy(() => import('./components/pages/c-ssrs-list/c-ssrs-new-report'));
const Profile = lazy(() => import('./components/pages/profile'));
const CssrsRecentReport = lazy(() => import('./components/pages/c-ssrs-list/c-ssrs-recent-report'));
const ManageUsers  = lazy(() => import('./components/pages/manage-users'));
const EhrLogin = lazy(() => import('./components/pages/ehr-login'));



export enum ERouterKeys {
    DEFAULT = 'DEFAULT_PAGE',
    DASHBOARD = 'Dashboard',
    SCREEN = 'Screen',
    ASSESSMENT = 'Assessment',
    VISIT = 'Visits',
    FOLLOW_UP = 'Follow-Up',
    REPORTS = 'Reports',
    MANAGE_DEVICES= 'Manage Devices',
    BRANCH_LOCATION = 'Branch Location',
    SCREEN_PROFILES = 'Screen Profiles',
    AGE_GROUPS = 'Age Groups',
    VISIT_SETTINGS = 'Visit Settings',
    ERROR_LOG = 'Error Log',
    SECURITY_LOG_SETTINGS = 'Security Log Settings',
    LICENSE_KEYS = 'License Keys',
    LOG_IN = 'Login',
    CHANGE_PASSWORD = 'Change Password',
    CHANGE_SECURITY_QUESTION = 'Change Security Question',
    AUTO_EXPORT_DASHBOARD='Auto-Export Dashboard',
    FIND_PATIENT_ADDRESS = 'Select Patient Address',
    PROFILE = 'Profile',
    RESET_PASSWORD = 'Reset Password',
    MANAGE_USERS =  'Manage Users',
    EHR_LOGIN = 'EHR Login',
}

export enum EVisitRouterKeys {
    VISIT_REPORTS = 'Visit Reports',
    VISIT_DEMOGRAPHIC_REPORTS = 'Visit Demographic Reports'
}

export enum EScreenRouterKeys {
    SCREEN_LIST = 'Screen Lists',
    SCREEN_HISTORY = 'Screen History',
    CREATE_SCREEN = 'Create Screen',
    SCREENING_REPORTS = 'Screening Report',
}

export enum EFollowUpRouterKeys {
    FOLLOW_UP_REPORT = 'Follow Up Report',
}


export enum EAssessmentRouterKeys {
    CSSRS_LIST = 'C-SSRS',
    CSAT_GPRA_LIST = 'CSAT GPRA',
    VISTIM_SERVICE_LIST = 'Victim Service',
    CSSRS_LIFETIME_RECENT_REPORT = 'C-SSRS LIFETIME RECENT REPORT',
    CSSRS_LIFETIME_ADD_REPORT = 'C-SSRS LIFETIME NEW REPORT',
}

export enum EReportsRouterKeys {
    INDICATOR_REPORTS = 'Indicator Reports',
    EXPORT_TO_EXCEL = 'Export to Excel',
    REPORTS_BY_AGE =  'Reports By Age',
    DRUGS_BY_AGE =   'Drugs By Age',
    PATIENT_DEMOGRAPHICS= 'Patient Demographics',
    FOLLOW_UP_OUTCOMES= 'FollowUp Outcomes',
    VISITS_OUTCOMES= 'Visits Outcomes',
    SCREEN_TIME_LOG = 'Screen Time Log',
    REPORTS_BY_SORT = 'Reports By Sort',
}


export enum ERouterUrls {
    MANAGE_DEVICES= '/manage-devices',
    BRANCH_LOCATION= '/branch-locations',
    DASHBOARD = '/dashboard',
    SCREEN = '/screen',
    SCREENING_REPORTS = '/screening-report/:reportId',
    ASSESSMENT = '/c-ssrs-list',
    VISIT = '/visit',
    VISIT_REPORTS = '/visit-report/:reportId',
    VISIT_DEMOGRAPHIC_REPORT = '/patient-demographics-report/:reportId',
    FOLLOW_UP = '/follow-up',
    FOLLOW_UP_REPORT = '/follow-up-report/:reportId',
    REPORTS = '/reports',
    SCREEN_LIST = '/screen-list',
    SCREEN_HISTORY = '/screen-history',
    CREATE_SCREEN ='/create-screen',
    CSSRS_LIST = '/c-ssrs-list',
    CSAT_GPRA_LIST = '/csat-gpra-list',
    VISTIM_SERVICE_LIST = '/victim-service-list',
    INDICATOR_REPORTS = '/reports-by-problem',
    REPORTS_BY_AGE = '/reports-by-age',
    REPORTS_BY_SORT = '/reports-by-sort',
    EXPORT_TO_EXCEL = '/export-to-excel',
    DRUGS_BY_AGE ='/drugs-by-age',
    PATIENT_DEMOGRAPHICS ='/patient-demographics',
    FOLLOW_UP_OUTCOMES = '/follow-up-outcomes',
    VISITS_OUTCOMES = '/visits-outcomes',
    SCREEN_TIME_LOG = '/screen-time-log',
    DEFAULT_PAGE = '/',
    SCREEN_PROFILES = '/screen-profiles',
    AGE_GROUPS = '/age-groups',
    VISIT_SETTINGS = '/visit-settings',
    ERROR_LOG = '/error-log',
    SECURITY_LOG_SETTINGS = '/security-log-settings',
    LICENSE_KEYS = '/license-keys',
    LOGIN = '/login',
    CHANGE_PASSWORD = '/change-password',
    CHANGE_SECURITY_QUESTION = '/change-security-question',
    AUTO_EXPORT_DASHBOARD='/auto-export-log',
    FIND_PATIENT_ADDRESS = '/find-patient-address/:screeningResultId',
    CSSRS_LIFETIME_ADD_REPORT = '/c-ssrs-lifetime-add-report',
    CSSRS_LIFETIME_RECENT_REPORT = '/c-ssrs-lifetime-recent-report/:reportId',
    PROFILE = '/profile',
    RESET_PASSWORD = '/reset-password',
    MANAGE_USERS    = '/users',
    EHR_LOGIN = '/ehr-login',
}

export interface ICustomRouterProps extends RouteProps {
    key: string;
    path: ERouterUrls;
    public?: boolean;
}


const ROUTES: ICustomRouterProps[] = [
    {
        key: ERouterKeys.SCREEN,
        path: ERouterUrls.SCREEN,
        component: ScreenPage,
        exact: true,
    },
    {
        key: ERouterKeys.MANAGE_DEVICES,
        path: ERouterUrls.MANAGE_DEVICES,
        component: ManageDevicePage,
        exact: true,
    },
    {
        key: ERouterKeys.BRANCH_LOCATION,
        path: ERouterUrls.BRANCH_LOCATION,
        component: BranchLocationsPage,
        exact: true,
    },
    {
        key: ERouterKeys.VISIT,
        path: ERouterUrls.VISIT,
        component: VisitPage,
        exact: true,
    },
    {
        key: EVisitRouterKeys.VISIT_REPORTS,
        path: ERouterUrls.VISIT_REPORTS,
        component: VisitReportPage,
        exact: true,
    },
    {
        key: EVisitRouterKeys.VISIT_DEMOGRAPHIC_REPORTS,
        path: ERouterUrls.VISIT_DEMOGRAPHIC_REPORT,
        component: VisitDemographicReport,
        exact: true,
    },
    {
        key: ERouterKeys.FOLLOW_UP,
        path: ERouterUrls.FOLLOW_UP,
        component: FollowUpPage,
        exact: true,
    },
    {
        key: EFollowUpRouterKeys.FOLLOW_UP_REPORT,
        path: ERouterUrls.FOLLOW_UP_REPORT,
        component: FollowUpReportPage,
        exact: true,
    },
    {
        key: ERouterKeys.REPORTS,
        path: ERouterUrls.REPORTS,
        component: ReportPage,
        exact: true,
    },  
    {
        key: EScreenRouterKeys.SCREENING_REPORTS,
        path: ERouterUrls.SCREENING_REPORTS,
        component: ScreeningReportPage,
        exact: true,
    },
    {
        key: EScreenRouterKeys.SCREEN_LIST,
        path: ERouterUrls.SCREEN_LIST,
        component: ScreenPage,
        exact: true,
    },  
    {
        key: EAssessmentRouterKeys.CSSRS_LIST,
        path: ERouterUrls.CSSRS_LIST,
        component: CSSRSListPage,
        exact: true,
    },    
    {
        key: EReportsRouterKeys.INDICATOR_REPORTS,
        path: ERouterUrls.INDICATOR_REPORTS,
        component: ReportPage,
        exact: true,
    },  
    {
        key: EReportsRouterKeys.REPORTS_BY_AGE,
        path: ERouterUrls.REPORTS_BY_AGE,
        component: ReportByAge,
        exact: true,
    },  
    {
        key: EReportsRouterKeys.REPORTS_BY_SORT,
        path: ERouterUrls.REPORTS_BY_SORT,
        component: ReportsBySort,
        exact: true,
    }, 
    {
        key: EReportsRouterKeys.DRUGS_BY_AGE,
        path: ERouterUrls.DRUGS_BY_AGE,
        component: DrugByAge,
        exact: true,
    }, 
    {
        key: EReportsRouterKeys.PATIENT_DEMOGRAPHICS,
        path: ERouterUrls.PATIENT_DEMOGRAPHICS,
        component: PatientDemographics,
        exact: true,
    }, 
    {
        key: EReportsRouterKeys.FOLLOW_UP_OUTCOMES,
        path: ERouterUrls.FOLLOW_UP_OUTCOMES,
        component: FollowupOutcomes,
        exact: true,
    },
    {
        key: EReportsRouterKeys.VISITS_OUTCOMES,
        path: ERouterUrls.VISITS_OUTCOMES,
        component: VisitsOutcomes,
        exact: true,
    },  
    {
        key: EReportsRouterKeys.SCREEN_TIME_LOG,
        path: ERouterUrls.SCREEN_TIME_LOG,
        component: ScreenTimeLog,
        exact: true,
    },
    {
        key: EReportsRouterKeys.EXPORT_TO_EXCEL,
        path: ERouterUrls.EXPORT_TO_EXCEL,
        component: ExportToExcel,
        exact: true,
    },
    {
        key: ERouterKeys.DASHBOARD,
        path: ERouterUrls.DASHBOARD,
        component: DashBoardPage,
        exact: true,
    },
    {
        key: ERouterKeys.DEFAULT,
        path: ERouterUrls.DEFAULT_PAGE,
        component: DashBoardPage,
        exact: true,
    },  
    {
        key: ERouterKeys.SCREEN_PROFILES,
        path: ERouterUrls.SCREEN_PROFILES,
        component: ScreenProfilesPage,
        exact: true,
    },
    {
        key:ERouterKeys.AGE_GROUPS,
        path: ERouterUrls.AGE_GROUPS,
        component: AgeGroups,
        exact: true,
    },
    {
        key:ERouterKeys.VISIT_SETTINGS,
        path: ERouterUrls.VISIT_SETTINGS,
        component: VisitSettings,
        exact: true,
    },
    {
        key:ERouterKeys.ERROR_LOG,
        path: ERouterUrls.ERROR_LOG,
        component: ErrorLog,
        exact: true,
    },
    {
        key:ERouterKeys.SECURITY_LOG_SETTINGS,
        path: ERouterUrls.SECURITY_LOG_SETTINGS,
        component: SecurityLogSettings,
        exact: true
    },
    {
        key:ERouterKeys.LICENSE_KEYS,
        path: ERouterUrls.LICENSE_KEYS,
        component: LicenseKeys,
        exact: true
    },
    {
        key: ERouterKeys.PROFILE,
        path: ERouterUrls.PROFILE,
        component: Profile,
        exact: true,
    },
    {
        key: ERouterKeys.CHANGE_PASSWORD,
        path: ERouterUrls.CHANGE_PASSWORD,
        component: ChangePassword,
        exact: true
    },
    {
        key: ERouterKeys.CHANGE_SECURITY_QUESTION,
        path: ERouterUrls.CHANGE_SECURITY_QUESTION,
        component: ChangeSecurityQuestion,
        exact: true,
    },
    {
        key:ERouterKeys.AUTO_EXPORT_DASHBOARD,
        path: ERouterUrls.AUTO_EXPORT_DASHBOARD,
        component: AutoExportLog,
        exact: true,
    },
    {
        key: ERouterKeys.FIND_PATIENT_ADDRESS,
        path:ERouterUrls.FIND_PATIENT_ADDRESS,
        component: FindPatientAddress,
        exact: true
    },
    {
        key: EAssessmentRouterKeys.CSSRS_LIFETIME_ADD_REPORT,
        path: ERouterUrls.CSSRS_LIFETIME_ADD_REPORT,
        component: CssrsAddReport,
        exact: true
    },
    {
        key: EAssessmentRouterKeys.CSSRS_LIFETIME_RECENT_REPORT,
        path: ERouterUrls.CSSRS_LIFETIME_RECENT_REPORT,
        component: CssrsRecentReport,
        exact: true,
    },
    {
        key: ERouterKeys.MANAGE_USERS,
        path: ERouterUrls.MANAGE_USERS,
        component: ManageUsers,
        exact: true
    },
    {
        key: ERouterKeys.EHR_LOGIN,
        path: ERouterUrls.EHR_LOGIN,
        component: EhrLogin,
        exact: true,
    }
] 



export default ROUTES;