import axios from  '../axios';

const deleteScreeningReportById = async (id: number): Promise<string> => {

    return await axios.instance.delete(`screen/${id}`);
}

export default deleteScreeningReportById;