import { IScreenListResponseItem } from '../../actions/screen';
import axios from  '../axios';

export interface IGetFilteredScreenListItemsRequest {
    MaximumRows?: number;
    StartRowIndex?: number;
    Location?: string | null;
    StartDate?: string | null;
    EndDate?: string | null;
    FirstName?: string | null;
    LastName?: string | null;
    ScreeningResultID?: number | null;
    OrderBy?: string;
}

const postFilterScreenList = async (
    props: IGetFilteredScreenListItemsRequest,
    screeningResultID?: number
): Promise<{ Items: IScreenListResponseItem[], TotalCount: number }> => {
    const currentDateStamp = new Date().toISOString();
    return await axios.instance.post(`screen/search${screeningResultID ? screeningResultID : ''}`, {
        "Location": null,
        "StartDate": "2020-10-01",
        "EndDate": currentDateStamp,
        "FirstName": null,
        "LastName": null,
        "ScreeningResultID": null,
        ...props,
        });
}

export default postFilterScreenList;