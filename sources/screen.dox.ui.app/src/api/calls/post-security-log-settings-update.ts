import axios from  '../axios';
import {  ISecurityLogSettingsItem } from '../../actions/security-log-settings';


const updateSecurityLogSettingsItems = async (props: { Items: Array<ISecurityLogSettingsItem>}): Promise<Array<ISecurityLogSettingsItem>> => {
    const replace = !!props ? props: {};
    return await axios.instance.post(`systemtools/securitylogsettings`, {
       ...replace
   });
}

export default updateSecurityLogSettingsItems;