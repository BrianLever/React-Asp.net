import { IManageUsersRequestItem, IManageUsersResponseItem } from '../../actions/manage-users';
import axios from  '../axios';

const postUserList = async (props: IManageUsersRequestItem): Promise<{ Items: IManageUsersResponseItem[], TotalCount: number }> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('user/search', {     
      ...replace
   });
}

export default postUserList;