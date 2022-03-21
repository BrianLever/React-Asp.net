import axios, { IPdfFileDownload } from  '../axios';
const postScreenTimeLogPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/ScreenTimeLog/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postScreenTimeLogPrint;