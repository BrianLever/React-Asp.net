import axios from  '../axios';
import { IReportsRequest, IReportsInnerItem } from '../../actions/reports';
const postReportByProblem = async (props: IReportsRequest): Promise<{
    Items: Array<IReportsInnerItem>;   
}> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`/bhireport/byproblem`,{       
        ...replace 
    });
}

export default postReportByProblem;

