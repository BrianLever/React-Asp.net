import axios, { IPdfFileDownload } from  '../axios';

const postPrintAllVisit = async (id?: number | string): Promise<IPdfFileDownload> => {
   const currentDateStamp = new Date().toISOString();
   return await axios.instance.post(`visit/${id ? id : 'search'}/print`, 
      {
         "Location": 1,
         "StartDate": "2020-10-01",
         "EndDate": currentDateStamp,
         "FirstName": null,
         "LastName": null,
         "ScreeningResultID": null,
         "OrderBy": "FullName DESC",
         "StartRowIndex": 0,
         "MaximumRows": 100,
         "ReportType": 2
      },
      {
         responseType: 'blob',
      }
   )
}

export default postPrintAllVisit;