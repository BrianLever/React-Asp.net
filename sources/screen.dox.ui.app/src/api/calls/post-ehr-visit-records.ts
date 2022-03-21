import axios from  '../axios';
import { IScreenListEhrExportVisitRecord } from '../../actions/screen';

const postEhrVisitRecords = async (patientId: number, props: {
    StartRowIndex: number;
    MaximumRows: number;
}): Promise<{ Items: Array<IScreenListEhrExportVisitRecord>, TotalCount: number }> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('ehrexport/visit/'+patientId, {     
      ...replace
   });
}

export default postEhrVisitRecords;
