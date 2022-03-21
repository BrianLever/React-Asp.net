import axios from  '../axios';
import { IScreeningProfileMinimunAgeResponseItem } from '../../actions/screen-profiles';

const getScreeningMinumumList = async (id: number): Promise<Array<IScreeningProfileMinimunAgeResponseItem>> => {
   return await axios.instance.get(`screeningprofile/${id}/age`);
}

export default getScreeningMinumumList;