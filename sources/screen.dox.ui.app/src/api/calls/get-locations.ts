import { ILocationItemResponse } from '../../actions';
import axios from  '../axios';

const getLocationsListCall = async (): Promise<ILocationItemResponse> => {
   return await axios.instance.get('branchlocation/list');
}

export default getLocationsListCall;