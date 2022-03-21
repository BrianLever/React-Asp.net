import axios from  '../axios';

const DeleteUser = async (id: number): Promise<string> => {
    return await axios.instance.delete(`user/${id}`);
 }
 
 export default DeleteUser;