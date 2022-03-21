import axios from  '../axios';

import { IReportsRequest, IReportsInnerItem } from '../../actions/reports';
const postReportByAge = async (props: IReportsRequest): Promise<{
    Items: Array<IReportsInnerItem>;   
}> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`/bhireport/byage`,{       
        ...replace 
    });
}

export default postReportByAge;

