import { IResetPasswordGetSecurityQuestionResponse } from '../../actions/reset-password';
import axios from  '../axios';

const getSecurityQuestion = async (username: string): Promise<IResetPasswordGetSecurityQuestionResponse> => {
   return await axios.instance.get('auth/resetpassword/'+username);
}

export default getSecurityQuestion;