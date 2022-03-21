import axios from  '../axios';
import { IEhrLoginRequest } from '../../actions/ehr-login';


const deleteEhrLogin = async (id: string): Promise<string> => {
    return await axios.instance.delete(`systemtools/rpmscredentials/`+id);
}

export default deleteEhrLogin;