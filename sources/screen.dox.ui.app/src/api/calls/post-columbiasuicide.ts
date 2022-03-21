import axios from  '../axios';
import { ICssrsReportRequest  } from '../../actions/c-ssrs-list/c-ssrs-report';


const postColumbiasuicide = async (props: ICssrsReportRequest): Promise<number> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('columbiasuicide', {     
      ...replace
   });
}

export default postColumbiasuicide;
