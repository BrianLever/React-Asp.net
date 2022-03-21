import axios from  '../axios';
import { IProifleUpdateRequest } from '../../actions/profile';


const userProfileUpdate = async (props: any): Promise<string> => {
    return await axios.instance.put(`profile`, props);
}
 
export default userProfileUpdate;