import { IManageUsersUser } from '../../actions/manage-users';
import axios from  '../axios';

const postCreateUser = async (props: IManageUsersUser): Promise<string> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`user`, {
        ...replace,
    });
}

export default postCreateUser;