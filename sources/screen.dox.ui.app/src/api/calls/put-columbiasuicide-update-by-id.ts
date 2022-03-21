import axios from  '../axios';
import { ICssrsEditableReportItem } from '../../actions/c-ssrs-list/c-ssrs-report';

const putCssrsReportUpdate = async (id: number, props: ICssrsEditableReportItem): Promise<string> => {
    return await axios.instance.put(`columbiasuicide/${id}`, props);
}
 
export default putCssrsReportUpdate;