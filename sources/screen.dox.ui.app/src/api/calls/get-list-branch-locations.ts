import axios from  '../axios';
import { TBranchLocationsItemResponse } from '../../actions/shared';

const getListBranchLocations = async (): Promise<Array<TBranchLocationsItemResponse>> => {
   return await axios.instance.get(`branchlocation/list`)
}

export default getListBranchLocations;