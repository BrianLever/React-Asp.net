import axios from  '../axios';
import { IVisitReportsResponse, IVisitRequest } from '../../actions/visit';

const postRelatedVisitById = async (id: number, props: IVisitRequest = {}): Promise<Array<IVisitReportsResponse>> => {
    const currentDateStamp = new Date().toISOString();
    return await axios.instance.post(`visit/search/${id}`, 
        {
            "Location": null,
            "StartDate": "2020-10-01",
            "EndDate": currentDateStamp,
            "FirstName": null,
            "LastName": null,
            "ScreeningResultID": null,
            "OrderBy": "LastCreatedDate DESC",
            "StartRowIndex": 0,
            "MaximumRows": 100,
            "ReportType": 2,
            ...props
        }
    );
}

export default postRelatedVisitById;