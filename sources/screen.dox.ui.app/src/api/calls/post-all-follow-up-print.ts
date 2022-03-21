import axios, { IPdfFileDownload } from  '../axios';
const postAllFollowUpPrint = async (): Promise<IPdfFileDownload> => {
    const currentDateStamp = new Date().toISOString();
    return await axios.instance.post(`followup/search/print`, {
        "Location": 2,
        "StartDate": "2019-10-01",
        "EndDate": currentDateStamp,
        "FirstName": null,
        "LastName": null,
        "ScreeningResultID": null,
        "OrderBy": "FullName DESC",
        "StartRowIndex": 0,
        "MaximumRows": 100,
        "ReportType": 2
      },{
        responseType: 'blob',
     });
}

export default postAllFollowUpPrint;