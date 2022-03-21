import axios from  '../axios';
import { TScreeningProfileItemResponse } from '../../actions/shared';

const getScreeningProfileList = async (): Promise<Array<TScreeningProfileItemResponse>> => {
   return await axios.instance.get(`screeningprofile/list`);
}

export default getScreeningProfileList;