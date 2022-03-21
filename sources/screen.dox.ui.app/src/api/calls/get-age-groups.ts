import axios from  '../axios';
import { ageGroupsResponseItem } from '../../actions/age-groups';

const getAgeGroups = async (): Promise<ageGroupsResponseItem> => {
   return await axios.instance.get(`systemtools/agegroups`);
}

export default getAgeGroups;