import { ISystemSettings } from '../../actions/dashboard';
import axios from  '../axios';

const getSystemSettings = async (): Promise<ISystemSettings> => {
   return await axios.instance.get('systeminfo');
}

export default getSystemSettings;