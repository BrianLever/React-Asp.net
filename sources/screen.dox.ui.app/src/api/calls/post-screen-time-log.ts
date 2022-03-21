import axios from  '../axios';
import { IReportsRequest } from '../../actions/reports';

const postScreenTimeLog = async (props: IReportsRequest): Promise<Array<any>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('bhireport/screentimelog', {  
      ...replace
   });
}

export default postScreenTimeLog;