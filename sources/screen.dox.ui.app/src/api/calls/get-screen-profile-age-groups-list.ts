import axios from  '../axios';
import { IScreenProfileAgeGroupsItem } from '../../actions/screen-profiles';

const getScreenProfileAgeGroupsList = async (): Promise<Array<IScreenProfileAgeGroupsItem>> => {
   return await axios.instance.get(`screeningprofile/age/groups`);
}

export default getScreenProfileAgeGroupsList;