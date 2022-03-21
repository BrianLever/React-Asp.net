import { IErrorLogListResponseItem } from '../../actions/error-log';
import axios from  '../axios';

const postErrorLog = async (props: any): Promise<{
    Items: Array<IErrorLogListResponseItem>;
    TotalCount: number;
}> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`errorlog`, {
        ...replace,
    });
}

export default postErrorLog;