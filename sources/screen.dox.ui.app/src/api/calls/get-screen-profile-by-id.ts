import axios from  '../axios';
import { IScreenProfilesResponseItem } from '../../actions/screen-profiles';

const getScreenProfileById = async (id: number): Promise<IScreenProfilesResponseItem> => {
   return await axios.instance.get(`screeningprofile/${id}`);
}

export default getScreenProfileById;