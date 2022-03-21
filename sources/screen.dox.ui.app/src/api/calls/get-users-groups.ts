import axios from  '../axios';

const getUserGroups = async (): Promise<Array<string>> => {

    return await axios.instance.get(`user/roles`);
}

export default getUserGroups;