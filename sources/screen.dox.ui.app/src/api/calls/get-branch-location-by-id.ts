import axios from  '../axios';
import { ICreateUpdateBranchLocationResponse } from '../../actions/branch-locations';

const getBranchLocationById = async (id: number): Promise<ICreateUpdateBranchLocationResponse> => await axios.instance.get(`branchlocation/${id}`);

export default getBranchLocationById;