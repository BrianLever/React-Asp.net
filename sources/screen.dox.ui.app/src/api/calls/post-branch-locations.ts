import axios from  '../axios';
import { IBranchLocationItemRequest, IBranchLocationResponse } from '../../actions/branch-locations';

const postBranchLocations = async (props: IBranchLocationItemRequest): Promise<IBranchLocationResponse> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`branchlocation/search`, {
        "OrderBy": "Name ASC",
        "StartRowIndex": 0,
        "FilterByName": null,
        "ScreeningProfileId": 0,
        "MaximumRows": 10,
        "ShowDisabled": false,
        ...replace,
      });
}

export default postBranchLocations;