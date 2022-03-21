import axios from  '../axios';

const UnBlockUser = async (id: number): Promise<string> => {
    return await axios.instance.post(`user/unblock/${id}`);
 }
 
 export default UnBlockUser;