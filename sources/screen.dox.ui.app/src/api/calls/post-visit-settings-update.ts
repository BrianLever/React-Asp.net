import { VisitSettingsResponseItem } from '../../actions/visit-settings';
import axios from  '../axios';

const updateVisitSettings = async (props: { Items: Array<VisitSettingsResponseItem> }): Promise<Array<VisitSettingsResponseItem>> => {
    const data = !!props ? props : {};
   return await axios.instance.post('systemtools/visitSettings', data);
}

export default updateVisitSettings;