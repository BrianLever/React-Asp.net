import axios from  '../axios';
import { IUpdateKioskRequestParamaters } from '../../actions/manage-devices';

const putUpdateKioskById = async (id: number, params: IUpdateKioskRequestParamaters): Promise<string> => {
    return await axios.instance.put(`kiosk/${id}`, params);
}
 
export default putUpdateKioskById;