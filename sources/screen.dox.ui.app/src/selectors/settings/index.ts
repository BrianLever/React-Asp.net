import { IRootState } from '../../states';

export const isGlobalLoading = (state: IRootState): boolean => state.settings.isLoading;
export const getSideDrawerState = (state: IRootState): boolean => state.settings.sideDrawerState;
export const isNotificationSelector = (state: IRootState): boolean => state.settings.isNotification;
export const getNotificationMessageSelector = (state: IRootState): string => state.settings.notificationMessage;
export const getNotificatioStatusSelector = (state: IRootState): 'default' | 'error' | 'success' | 'warning' | 'info' => state.settings.notificationStatus;
export const getCurrentPageKeySelector = (state: IRootState): string => state.settings.currentPageKey;
export const getCurrentPagePathSelector = (state: IRootState): string => state.settings.currentPagePath;
export const getModalWindowObjectSelector = (state: IRootState): { [k: string]: boolean } => state.settings.modalWindowObject;
export const getAppVersion = (state: IRootState): string => state.dashboard.systemSettings ? state.dashboard.systemSettings.AppVersion : 'beta';
export const isEhrSystemExpiredAlertSelector = (state: IRootState): boolean => state.dashboard.systemSettings.IsEhrCredentialsExpirationAlert;
export const getEhrSystemExpiredAlertMessageSelector = (state: IRootState): string => state.dashboard.systemSettings.EhrCredentialsExpirationAlertMessage;