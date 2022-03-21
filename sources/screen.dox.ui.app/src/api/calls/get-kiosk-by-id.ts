import axios from  '../axios';
import { IKioskDetailsResponse } from '../../actions/manage-devices';

const getKioskById = async (id: number): Promise<IKioskDetailsResponse> => await axios.instance.get(`kiosk/${id}`);

export default getKioskById;