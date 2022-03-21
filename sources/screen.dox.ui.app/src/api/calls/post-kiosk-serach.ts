import axios from  '../axios';
import { IManegeDevicesListResponse, IManegeDevicesListRequest } from '../../actions/manage-devices';

const postManageDevicesSearch = async (porps: IManegeDevicesListRequest): Promise<{
    Items: Array<IManegeDevicesListResponse>;
    TotalCount: number;
}> => {
    if (!porps) {
        porps = {};
    }
    return await axios.instance.post(`kiosk/search`, 
    {
        "OrderBy": "KioskID ASC",
        "StartRowIndex": 0,
        "MaximumRows": 100,
        "NameOrKey": "",
        "BranchLocationId": null,
        "ScreeningProfileId": null,
        "ShowDisabled": false,
        ...porps,
      }
    );
}

export default postManageDevicesSearch;