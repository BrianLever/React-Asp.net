import axios from  '../axios';
import { IEhrLoginResponseItem } from '../../actions/ehr-login';


const getEhrLoginList = async (): Promise<IEhrLoginResponseItem[]> => {
   return await axios.instance.get('systemtools/rpmscredentials');
}

export default getEhrLoginList;