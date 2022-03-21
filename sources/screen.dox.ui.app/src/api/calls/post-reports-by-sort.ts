import axios from  '../axios';
import {  IScreeningReportResultsBySortItem } from '../../actions/reports';
const postScreeningResultReportsBySort = async (props: any): Promise<Array<IScreeningReportResultsBySortItem>> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`/bhireport/bysort`,{       
        ...replace 
    });
}

export default postScreeningResultReportsBySort;

