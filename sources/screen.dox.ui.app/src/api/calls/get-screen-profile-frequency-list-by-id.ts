import axios from  '../axios';
import { IScreenProfileFrequencyResponseItem } from '../../actions/screen-profiles';

const getScreenProfileFrequencyListById = async (id: number): Promise<Array<IScreenProfileFrequencyResponseItem>> => {
   return await axios.instance.get(`screeningprofile/${id}/frequency`);
}

export default getScreenProfileFrequencyListById;