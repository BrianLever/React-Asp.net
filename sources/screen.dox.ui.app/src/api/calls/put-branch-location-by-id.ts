import axios from  '../axios';
import { ICreateUpdateBranchLocationRequest } from '../../actions/branch-locations';

const putUpdateBranchLocationById = async (id: number, params: ICreateUpdateBranchLocationRequest): Promise<string> => {
    return await axios.instance.put(`branchlocation/${id}`, params);
}
 
export default putUpdateBranchLocationById;