import { IErrorLogListResponseItem } from '../../actions/error-log';
import axios from  '../axios';

const getErrorLogItem = async (id: number): Promise<IErrorLogListResponseItem> => {
    return await axios.instance.get(`errorlog/${id}`);
}

export default getErrorLogItem;