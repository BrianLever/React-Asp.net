import axios from  '../axios';
import { IEhrRecordPatientsItem } from '../../actions/find-patient-address';

const postEhrRecordPatients = async (screeningResultId: number, props: {
    StartRowIndex: number;
    MaximumRows: number;
}): Promise<Array<IEhrRecordPatientsItem>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('ehrexport/patient/'+screeningResultId, {     
      ...replace
   });
}

export default postEhrRecordPatients;
