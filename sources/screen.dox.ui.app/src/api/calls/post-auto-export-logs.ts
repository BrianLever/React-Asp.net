import { IAutoExportRequestItem, IAutoExportLogsResponseItem } from '../../actions/auto-export-logs';
import axios from  '../axios';

const postAutoExportLogs = async (props: IAutoExportRequestItem): Promise<Array<IAutoExportLogsResponseItem>> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`patientname/autocorrectionlog`, {
        ...replace,
    });
}

export default postAutoExportLogs;