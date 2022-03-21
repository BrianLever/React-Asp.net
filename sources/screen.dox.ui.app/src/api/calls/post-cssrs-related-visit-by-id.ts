import axios from  '../axios';
import { ICssrsListRequest, ICssrsVisitResponseItem } from '../../actions/c-ssrs-list';

const postCssrsRelatedVisitById = async (id: number, props: ICssrsListRequest = {}): Promise<Array<ICssrsVisitResponseItem>> => {
    return await axios.instance.post(`columbiasuicide/search/${id}`, 
        {
            ...props
        }
    );
}

export default postCssrsRelatedVisitById;