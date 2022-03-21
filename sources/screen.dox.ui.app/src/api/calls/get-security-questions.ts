import axios from  '../axios';

const getSecurityQuestions = async (): Promise<Array<string>> => {
   return await axios.instance.get('profile/list/securityquestion');
}

export default getSecurityQuestions;