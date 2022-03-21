import axios, { IExcelFileDownload } from  '../axios';
const printErrorLogs = async (props: any): Promise<IExcelFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`errorlog/export`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default printErrorLogs;