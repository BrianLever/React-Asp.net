import axios, { IPdfFileDownload } from  '../axios';
const postVisitsOutcomesPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/visitsOutcomes/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postVisitsOutcomesPrint;