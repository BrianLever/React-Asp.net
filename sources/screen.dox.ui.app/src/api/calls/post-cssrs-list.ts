import axios from  '../axios';
import {  ICssrsListResponse, ICssrsListRequest } from '../../actions/c-ssrs-list';

const postCssrsList = async (props: ICssrsListRequest): Promise<Array<ICssrsListResponse>> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('columbiasuicide/search', {     
      ...replace
   });
}

export default postCssrsList;
