import axios from  '../axios';
import { IVisitDemographicReportUpdateRequast } from '../../actions/visit/demographic-report';

const updateDemographicReportById = async (id: number, data: IVisitDemographicReportUpdateRequast)
: Promise<string> => {
   return await axios.instance.put(`demographics/${id}`, data);
}

export default updateDemographicReportById;