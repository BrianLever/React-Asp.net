import axios from  '../axios';
import { IFollowUpReportResponse } from '../../actions/follow-up/report';

const getFollowUpReportById = async (id: number): Promise<IFollowUpReportResponse> => {
   return await axios.instance.get(`followup/${id}`);
}

export default getFollowUpReportById;