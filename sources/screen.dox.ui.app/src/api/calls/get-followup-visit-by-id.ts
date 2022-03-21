import axios from  '../axios';
import { IFollowUpInnerInnerItem } from '../../actions/follow-up';

const getFollowUpVisitById = async (id: number): Promise<IFollowUpInnerInnerItem> => await axios.instance.get(`followup/visit/${id}`);

export default getFollowUpVisitById;