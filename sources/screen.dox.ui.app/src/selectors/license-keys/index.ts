import { IRootState } from '../../states';

export const getLicenseKeysSelector = (state: IRootState) => state.licenseKeys.licenseKeys;
export const isLicenseKeysLoadingSelector = (state: IRootState) => state.licenseKeys.isLicenseKeysLoading;
export const getLicenseKeySelector = (state: IRootState) => state.licenseKeys.licenseKey;
export const getLicenseActivationKeySelector = (state: IRootState) => state.licenseKeys.licenseActivationKey;
export const isLicenseKeyCreateLoadingSelector = (state: IRootState) => state.licenseKeys.isLicenseKeyCreateLoading;
export const getLicenseKeysSystemSettingsSummary  = (state: IRootState) => state.licenseKeys.summary;