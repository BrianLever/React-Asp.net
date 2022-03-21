import axios, { IPdfFileDownload } from  '../axios';
const postFollowupOutcomesPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/followupOutcomes/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postFollowupOutcomesPrint;