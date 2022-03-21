import axios from  '../axios';
import { IReportsRequest } from '../../actions/reports';

const postDrugByAge = async (props: IReportsRequest): Promise<Array<any>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('bhireport/drugusebyage', {     
      ...replace
   });
}

export default postDrugByAge;
