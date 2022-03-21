import axios from  '../axios';
import { IFindPatientAddressRequestItem } from '../../actions/find-patient-address';

const postFindPatientAddress = async (id: number, props: any ): Promise<string> => {
    return await axios.instance.post(`visit/patientaddress/${id}`, {
        ...props
    });
}

export default postFindPatientAddress;