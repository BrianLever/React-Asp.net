import { IChangeSecurityQuestionRequest } from '../../actions/change-security-question';
import axios from  '../axios';


const putChangeSecurityQuestion = async (props: IChangeSecurityQuestionRequest): Promise<string> => {
    return await axios.instance.put(`profile/securityquestion`, props);
}
 
export default putChangeSecurityQuestion;