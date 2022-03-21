import { IVisitRequest, IVisitResponseItem } from '../../actions/visit';
import axios from  '../axios';

const postAllVisit = async (props: IVisitRequest): Promise<{
    Items: Array<IVisitResponseItem>;
    TotalCount: number;
}> => {
    const currentDateStamp = new Date().toISOString();
    const replace = !!props ? props : {};
    return await axios.instance.post(`visit/search`, {
        "Location": 1,
        "StartDate": "2020-10-01",
        "EndDate": currentDateStamp,
        "FirstName": null,
        "LastName": null,
        "ScreeningResultID": null,
        "OrderBy": "LastCreatedDate desc",
        "StartRowIndex": 0,
        "MaximumRows": 10,
        "ReportType": 0,
        ...replace,
    });
}

export default postAllVisit;