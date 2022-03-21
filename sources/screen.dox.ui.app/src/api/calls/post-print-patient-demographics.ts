import axios, { IPdfFileDownload} from  '../axios';
const postPatientDemographicsPrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/patientdemographics/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postPatientDemographicsPrint;
