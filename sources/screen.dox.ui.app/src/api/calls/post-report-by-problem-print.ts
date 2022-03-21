import axios, { IPdfFileDownload } from  '../axios';
const postReportByProblemPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/byproblem/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postReportByProblemPrint;