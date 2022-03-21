import axios from  '../axios';
import { IScreenProfileFrequencyListItem } from '../../actions/screen-profiles';

const getScreenProfileFrequencyList = async (): Promise<Array<IScreenProfileFrequencyListItem>> => {
   return await axios.instance.get(`screeningprofile/frequency/list`);
}

export default getScreenProfileFrequencyList;