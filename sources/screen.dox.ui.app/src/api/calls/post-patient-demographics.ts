import axios from  '../axios';
import { IReportsRequest } from '../../actions/reports';

const postPatientDemographics = async (props: IReportsRequest): Promise<Array<any>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('bhireport/patientdemographics', {     
      ...replace
   });
}

export default postPatientDemographics;