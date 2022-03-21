import { Action } from 'redux';
import { EDashboardActionType, IActionPayload } from "../";

export interface ISystemSettingsLicense {
    LicenseString: string;
    MaxKiosk: number;
    MaxBranchLocations: number;
    ExpirationDate: Date;
}

export interface ISystemSettingsSummary {
    CheckInRecordCount: number;
    BranchLocationCount: number;
    KioskCount: number;
  }

export interface ISystemSettings {
    AppVersion: string;
    HasActiveLicense: boolean;
    License: ISystemSettingsLicense | undefined;
    Summary: ISystemSettingsSummary | undefined;
    IsEhrCredentialsExpirationAlert: boolean;
    EhrCredentialsExpirationAlertMessage: string;
    CentralLoggingUrl: string;

}

export const getSystemSettingsRequest = (): Action => ({
    type: EDashboardActionType.getSystemSettingsRequest,
});

export const getSystemSettingsStart = (): Action => ({
    type: EDashboardActionType.getSystemSettingsRequestStart,
});

export const getSystemSettingsError = (): Action => ({
    type: EDashboardActionType.getSystemSettingsRequestError,
});

export const getSystemSettingsSuccess = (systemSettings: ISystemSettings): IActionPayload => {
    return {
        type: EDashboardActionType.getSystemSettingsRequestSuccess,
        payload: { systemSettings },
    }
}