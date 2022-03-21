import { Action } from 'redux';
import { ESecurityLogSettingsActions, IActionPayload,  } from '..';


export interface ISecurityLogSettingsItem {
    ID: number;
    Description: string;
    CategoryID: number;
    IsEnabled: boolean;
}

export interface ISecurityLogSettingsCategory {
    ID: number;
    Description: string;
}


export const getSecurityLogSettingsRequest = (): Action => ({ type: ESecurityLogSettingsActions.getSecurityLogSettingsRequest });
export const getSecurityLogSettingsRequestStart = (): Action => ({ type: ESecurityLogSettingsActions.getSecurityLogSettingsRequestStart });
export const getSecurityLogSettingsRequestSuccess = (securityLogSettingsItems: Array<ISecurityLogSettingsItem>,securityLogSettingsCategory: Array<ISecurityLogSettingsCategory> ): IActionPayload => (
    { type: ESecurityLogSettingsActions.getSecurityLogSettingsRequestSuccess, 
      payload: {
        securityLogSettingsItems, 
        securityLogSettingsCategory
      } 
    }
);
export const getSecurityLogSettingsRequestError = (): Action => ({ type: ESecurityLogSettingsActions.getSecurityLogSettingsRequestError });
export const updateSecurityLogSettingsItemsRequest = ():Action => ({ type: ESecurityLogSettingsActions.updateSecurityLogSettingsItemsRequest });