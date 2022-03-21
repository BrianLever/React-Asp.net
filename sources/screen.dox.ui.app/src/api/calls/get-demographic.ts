import axios from  '../axios';
import { IVisitDemographicReportResponse } from '../../actions/visit/demographic-report';

const getDemographicById = async (id: number): Promise<IVisitDemographicReportResponse> => {
   return await axios.instance.get(`demographics/${id}`);
}

export default getDemographicById;