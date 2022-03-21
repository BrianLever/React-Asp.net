import axios from  '../axios';

const deleteKioskById = async (id: number): Promise<string> => {

    return await axios.instance.delete(`kiosk/${id}`);
}

export default deleteKioskById;