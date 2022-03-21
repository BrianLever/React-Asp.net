import axios, { IPdfFileDownload } from  '../axios';

const getFollowUpReportPrint = async (id: string): Promise<IPdfFileDownload> => {
   return await axios.instance.get(`followup/${id}/print`, {
      responseType: 'blob',
   })
}

export default getFollowUpReportPrint;