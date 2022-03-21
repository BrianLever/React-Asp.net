import axios from  '../axios';
import { ageGroupsResponseItem } from '../../actions/age-groups';

const updateAgeGroups = async (props: any): Promise<ageGroupsResponseItem> => {
    const post = !!props ? props : {};
   return await axios.instance.post(`systemtools/agegroups`, post);
}

export default updateAgeGroups;