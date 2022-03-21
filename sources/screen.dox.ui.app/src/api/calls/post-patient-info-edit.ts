import axios from  '../axios';
import { IScreeningReportPatientInfo } from '../../actions/screen/report';

const postPatientInfoEdit = async (props: IScreeningReportPatientInfo, id: number): Promise<string> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('patient/'+id, {     
      ...replace
   });
}

export default postPatientInfoEdit;