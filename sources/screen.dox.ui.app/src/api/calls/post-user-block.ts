import axios from  '../axios';

const BlockUser = async (id: number): Promise<string> => {
    return await axios.instance.post(`user/block/${id}`);
 }
 
 export default BlockUser;