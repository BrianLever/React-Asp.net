import axios, { IPdfFileDownload } from  '../axios';
import { IReportsRequest, IReportsInnerItem } from '../../actions/reports';
const postDrugByAgePrint = async (props: any): Promise<IPdfFileDownload> => {    
   const replace = !!props ? props : {};
    return await axios.instance.post(`bhireport/byproblem/print`, {        
         ...replace
      },{
        responseType: 'blob',
     });
}

export default postDrugByAgePrint;



