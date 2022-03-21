import axios from  '../axios';
import {  ISecurityLogSettingsItem } from '../../actions/security-log-settings';


const getSecurityLogSettingsItems = async (): Promise<Array<ISecurityLogSettingsItem>> => {
   return await axios.instance.get(`systemtools/securitylogsettings`);
}

export default getSecurityLogSettingsItems;