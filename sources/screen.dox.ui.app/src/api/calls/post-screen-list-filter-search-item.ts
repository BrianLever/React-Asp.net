import { IScreenListInnerItem } from '../../actions/screen';
import axios from  '../axios';

export interface IGetFilteredScreenListItemsRequest {
    MaximumRows?: number;
    StartRowIndex?: number;
    Location?: string | null;
    StartDate?: string | null;
    EndDate?: string | null;
    FirstName?: string | null;
    LastName?: string | null;
    ScreeningResultID?: string;
    OrderBy?: string;
}

const postFilterScreenListItem = async (screeningResultID: number, props: IGetFilteredScreenListItemsRequest = {})
: Promise<IScreenListInnerItem[]> => {
    const currentDateStamp = new Date().toISOString();
    return await axios.instance.post(`screen/search/${screeningResultID ? screeningResultID : ''}`, {
      "Location": null,
      "StartDate": "2020-10-01",
      "EndDate": currentDateStamp,
      "FirstName": null,
      "LastName": null,
      "ScreeningResultID": null,
      ...props,
    });
}

export default postFilterScreenListItem;