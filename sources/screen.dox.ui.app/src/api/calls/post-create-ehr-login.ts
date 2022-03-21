import axios from  '../axios';
import { IEhrLoginRequest } from '../../actions/ehr-login';


const postEhrLoginCreate = async (props: IEhrLoginRequest): Promise<string> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`systemtools/rpmscredentials`, {
        ...replace,
    });
}

export default postEhrLoginCreate;