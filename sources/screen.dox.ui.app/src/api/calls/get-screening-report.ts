import { IScreeningReport } from '../../actions/screen/report';
import axios from  '../axios';

const getScreeningReport = async (reportId: number): Promise<IScreeningReport> => {
   return await axios.instance.get(`screen/${reportId}`);
}

export default getScreeningReport;