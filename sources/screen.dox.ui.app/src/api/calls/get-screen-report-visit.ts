import axios from  '../axios';

const getScreeningReportVisit = async (reportId: number): Promise<Number> => {
   return await axios.instance.post(`screen/${reportId}/visit`);
}

export default getScreeningReportVisit;