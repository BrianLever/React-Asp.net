import axios from  '../axios';
import { IReportsRequest } from '../../actions/reports';

const postFollowupOutcomes = async (props: IReportsRequest): Promise<Array<any>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('bhireport/followupOutcomes', {     
      ...replace
   });
}

export default postFollowupOutcomes;