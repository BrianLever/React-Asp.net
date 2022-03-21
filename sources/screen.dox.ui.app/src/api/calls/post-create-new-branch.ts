import axios from  '../axios';
import { ICreateUpdateBranchLocationRequest, ICreateUpdateBranchLocationResponse } from '../../actions/branch-locations';

const postCreateNewBranchLocation = async (params: ICreateUpdateBranchLocationRequest)
: Promise<ICreateUpdateBranchLocationResponse> => await axios.instance.post(`branchlocation`, params);

export default postCreateNewBranchLocation;