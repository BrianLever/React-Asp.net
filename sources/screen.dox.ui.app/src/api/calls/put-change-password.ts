import axios from  '../axios';


const putChangePassword = async (props: { CurrentPassword: string, NewPassword: string }): Promise<string> => {
    return await axios.instance.put(`profile/password`, props);
}
 
export default putChangePassword;