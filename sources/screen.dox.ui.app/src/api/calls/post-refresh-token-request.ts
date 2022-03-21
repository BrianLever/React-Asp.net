import axios from  '../axios';
import { IloginResponse } from '../../actions/login';


const postRefreshTokenRequest = async (props: { RefreshToken: string }): Promise<IloginResponse> => {
    return await axios.instance.post(`auth/refreshtoken`, props);
}

export default postRefreshTokenRequest;