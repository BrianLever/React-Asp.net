import {  IAutoExportLogsStatisticsItem } from '../../actions/auto-export-logs';
import axios from  '../axios';

const postAutoExportLogsStatistics = async (props: any): Promise<IAutoExportLogsStatisticsItem> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`statistics`, {
        ...replace,
    });
}

export default postAutoExportLogsStatistics;