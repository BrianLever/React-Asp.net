import axios from  '../axios';
import { IFollowUpRelatedReportResponse, IFollowUpRequest } from '../../actions/follow-up';

const postFollowUpRelatedReport = async (id?: number, props: IFollowUpRequest = {}): Promise<Array<IFollowUpRelatedReportResponse>> => {
    return await axios.instance.post(`followup/search/${id}`, {
        "Location": 2,
        "StartDate": "2019-10-01",
        "EndDate": "2020-09-30",
        "FirstName": null,
        "LastName": null,
        "ScreeningResultID": null,
        "OrderBy": "LastCreatedDate DESC",
        "StartRowIndex": 0,
        "MaximumRows": 100,
        "ReportType": 0,
        ...props
    });
}

export default postFollowUpRelatedReport;