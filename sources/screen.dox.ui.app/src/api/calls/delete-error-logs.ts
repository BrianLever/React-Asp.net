import axios from  '../axios';

const deleteErrorLogs = async (): Promise<any> => {
    return await axios.instance.delete(`errorlog/clear`);
}

export default deleteErrorLogs;