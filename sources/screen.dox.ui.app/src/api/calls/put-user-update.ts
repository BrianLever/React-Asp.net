import axios from  '../axios';
import { IManageUsersUser } from "../../actions/manage-users";

const putUser = async (id: number, props: IManageUsersUser): Promise<string> => {
    return await axios.instance.put(`user/${id}`, props);
 }
 
 export default putUser;