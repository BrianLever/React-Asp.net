import { IVisitReportResponse } from '../../actions/visit/report';
import axios from  '../axios';

const getVisitReportByID = async (reportId: number): Promise<IVisitReportResponse> => {
   return await axios.instance.get(`visit/${reportId}`);
}

export default getVisitReportByID;