import { IloginRequest, IloginResponse } from '../../actions/login';
import axios from  '../axios';

const postLogin = async (props: IloginRequest): Promise<IloginResponse> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('auth/authorize', replace);
}

export default postLogin;