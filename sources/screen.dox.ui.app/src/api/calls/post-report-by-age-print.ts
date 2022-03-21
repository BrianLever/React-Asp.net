import axios, { IPdfFileDownload } from  '../axios';
const postReportByagePrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/byage/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postReportByagePrint;