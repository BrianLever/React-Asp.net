import axios from  '../axios';
import { IAddNewKioskResponse, IAddNewKioskRequest } from '../../actions/manage-devices';

const postCreateNewKiosk = async (params: IAddNewKioskRequest): Promise<Array<IAddNewKioskResponse>> => {
    return await axios.instance.post(`kiosk`, params);
}

export default postCreateNewKiosk;