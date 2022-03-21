import { ERouterKeys, ERouterUrls, EReportsRouterKeys, EAssessmentRouterKeys } from '../../../router';
import { TSidebarPoperContentItems } from './sidebar-poper-content';

export const TOP_NAVIGATION_ITEMS: Array<Array<TSidebarPoperContentItems>> = [
  [
    {
      name: 'Manage Users',
      url: ERouterUrls.MANAGE_USERS
    },
    {
      name: 'EHR Login',
      url: ERouterUrls.EHR_LOGIN
    },
    // {
    //   name: 'SPARS Login',
    //   url: ''
    // },
    {
      name: 'License Keys',
      url: ERouterUrls.LICENSE_KEYS
    }
  ], 
  [
    {
      name: 'Screen Profile',
      url: ERouterUrls.SCREEN_PROFILES
    },
    // {
    //   name: 'SAMHSA GRPA Profiles',
    //   url: ''
    // },
    {
      name: 'Visit Settings',
      url: ERouterUrls.VISIT_SETTINGS
    },
    {
      name: 'Manage Devices',
      url: ERouterUrls.MANAGE_DEVICES,
    },
    {
      name: 'Branch Locations',
      url: ERouterUrls.BRANCH_LOCATION,
    },
    // {
    //   name: 'Security Log',
    //   url: ''
    // },
    {
      name: 'Security Log Settings',
      url: ERouterUrls.SECURITY_LOG_SETTINGS
    },
    {
      name: 'Error Log',
      url: ERouterUrls.ERROR_LOG,
    },
    {
      name: 'Auto-Export Log',
      url: ERouterUrls.AUTO_EXPORT_DASHBOARD
    },
    {
      name: 'Age Groups',
      url: ERouterUrls.AGE_GROUPS
    }
  ]
]

export const DROPDOWN_ELEMENTS = [
    {
      name: ERouterKeys.DASHBOARD,
      iconUrl:'../assets/home.svg',
      collapsable: false,
      link: ERouterUrls.DASHBOARD
    },
    {
      name: ERouterKeys.SCREEN,
      iconUrl: '../assets/screen.svg',
      collapsable: false,
      link: ERouterUrls.SCREEN,
      // childrenItems: [
      //   {
      //     name: EScreenRouterKeys.SCREEN_LIST,
      //     link: ERouterUrls.SCREEN_LIST,
      //   },
      //   {
      //     name: EScreenRouterKeys.SCREEN_HISTORY,
      //     link: ERouterUrls.SCREEN_HISTORY,
      //   },
      //   {
      //     name: EScreenRouterKeys.CREATE_SCREEN,
      //     link: ERouterUrls.CREATE_SCREEN,
      //   }
      // ]
    },
    {
      name: ERouterKeys.ASSESSMENT,
      iconUrl: '../assets/assess-w.svg',
      collapsable: true,
      childrenItems: [
        {
          name: EAssessmentRouterKeys.CSSRS_LIST,
          link: ERouterUrls.CSSRS_LIST,
        },
        // {
        //   name: EAssessmentRouterKeys.CSAT_GPRA_LIST,
        //   link: ERouterUrls.CSAT_GPRA_LIST,
        // },
        // {
        //   name: EAssessmentRouterKeys.VISTIM_SERVICE_LIST,
        //   link: ERouterUrls.VISTIM_SERVICE_LIST,
        // }
      ]
    },
    {
      name: ERouterKeys.VISIT,
      iconUrl: '../assets/visit.svg',
      collapsable: false,
      link: ERouterUrls.VISIT
    },
    {
      name: ERouterKeys.FOLLOW_UP,
      iconUrl: '../assets/follow.svg',
      collapsable: false,
      link: ERouterUrls.FOLLOW_UP
    },
    {
      name: ERouterKeys.REPORTS,
      iconUrl: '../assets/reports2.svg',
      collapsable: true,
      link: ERouterUrls.REPORTS,
      childrenItems: [
        {
          name: EReportsRouterKeys.INDICATOR_REPORTS,
          link: ERouterUrls.INDICATOR_REPORTS,
        },
        {
          name: EReportsRouterKeys.EXPORT_TO_EXCEL,
          link: ERouterUrls.EXPORT_TO_EXCEL,
        }
      ]
    }
  ]