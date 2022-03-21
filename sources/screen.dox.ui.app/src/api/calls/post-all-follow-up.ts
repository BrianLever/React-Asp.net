import axios from  '../axios';
import { IFollowUpRequest, IFollowUpResponseItem } from '../../actions/follow-up';

const postAllFollowUp = async (props: IFollowUpRequest): Promise<{
    Items: Array<IFollowUpResponseItem>;
    TotalCount: number;
}> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`followup/search`, {
        "Location": 2,
        "StartDate": "2019-10-01",
        "EndDate": "2020-09-30",
        "FirstName": null,
        "LastName": null,
        "ScreeningResultID": null,
        "OrderBy": "LastFollowUpDate DESC",
        "StartRowIndex": 0,
        "MaximumRows": 10,
        "ReportType": 0,
        ...replace,
      });
}

export default postAllFollowUp;