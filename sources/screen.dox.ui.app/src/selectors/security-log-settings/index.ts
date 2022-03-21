import { ISecurityLogSettingsState } from "../../states/security-log-settings";
import { IRootState } from '../../states';

export const getSecurityLogSettingsItemsSelector = (state: IRootState) => state.securityLogSettings.securityLogSettingsItems;
export const getSecurityLogSettingsCategorySelector = (state: IRootState) => state.securityLogSettings.securityLogSettingsCategory;
export const isSecurityLogSettingsLoadingSelector = (state: IRootState) => state.securityLogSettings.isSecurityLogSettingsLoading;
