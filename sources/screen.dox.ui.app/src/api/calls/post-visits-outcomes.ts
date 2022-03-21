import axios from  '../axios';
import { IReportsRequest } from '../../actions/reports';

const postVisitsOutcomes = async (props: IReportsRequest): Promise<Array<any>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('bhireport/visitsOutcomes', {     
      ...replace
   });
}

export default postVisitsOutcomes;