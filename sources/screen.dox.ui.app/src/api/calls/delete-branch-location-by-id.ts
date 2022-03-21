import axios from  '../axios';

const deleteBranchLocationId = async (id: number): Promise<string> => await axios.instance.delete(`branchlocation/${id}`);

export default deleteBranchLocationId;