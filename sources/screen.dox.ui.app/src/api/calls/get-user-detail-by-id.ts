import axios from  '../axios';

const getUserDetail = async (userId: number): Promise<any> => {

    return await axios.instance.get(`user/${userId}`);
}

export default getUserDetail;