import { IProfileResponse } from '../../actions/profile';
import axios from  '../axios';

const getProfile = async (clientCode: number): Promise<IProfileResponse> => {
   return await axios.instance.get('profile');
}

export default getProfile;