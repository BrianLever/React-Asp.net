import axios from  '../axios';
import { ISecurityLogSettingsCategory } from '../../actions/security-log-settings';


const getSecurityLogSettingsCategory = async (): Promise<Array<ISecurityLogSettingsCategory>> => {
   return await axios.instance.get(`systemtools/securitylogsettings/category`);
}

export default getSecurityLogSettingsCategory;