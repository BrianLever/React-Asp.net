import axios,{IPdfFileDownload}  from  '../axios';
const postExportToExcelPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/export`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postExportToExcelPrint;
