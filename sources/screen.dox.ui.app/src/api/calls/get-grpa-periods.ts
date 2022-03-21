import { TGPRAPeriodResponseItem } from '../../actions';
import axios from  '../axios';

const getGPRAPeriods = async (): Promise<Array<TGPRAPeriodResponseItem>> => {
   return await axios.instance.get('screen/gpra');
}

export default getGPRAPeriods;