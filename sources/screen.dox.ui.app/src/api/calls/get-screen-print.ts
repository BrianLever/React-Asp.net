import axios, {IPdfFileDownload} from  '../axios';

const getScreenPrint = async (screenReportID: number): Promise<IPdfFileDownload> => {
   return await axios.instance.get(`screen/${screenReportID}/print`, {
      responseType: 'blob',
   })
}

export default getScreenPrint;