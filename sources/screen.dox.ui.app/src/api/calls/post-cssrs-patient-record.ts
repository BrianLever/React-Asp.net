import axios from  '../axios';
import { ICssrsReportPatientRecordsResponse, ICssrsReportPatientRecordsRequest } from '../../actions/c-ssrs-list/c-ssrs-report';


const postPatientRecords = async (props: ICssrsReportPatientRecordsRequest): Promise<Array<ICssrsReportPatientRecordsResponse>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('patient/search', {     
      ...replace
   });
}

export default postPatientRecords;
