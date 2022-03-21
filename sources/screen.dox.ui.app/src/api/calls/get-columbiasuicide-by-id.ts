import axios from  '../axios';
import { ICssrsEditableReportItem  } from '../../actions/c-ssrs-list/c-ssrs-report';


const getColumbiasuicide = async (reportId: number): Promise<ICssrsEditableReportItem> => {
   return await axios.instance.get('columbiasuicide/'+reportId);
}

export default getColumbiasuicide;
