import { VisitSettingsResponseItem } from '../../actions/visit-settings';
import axios from  '../axios';

const getVisitSettings = async (): Promise<Array<VisitSettingsResponseItem>> => {
   return await axios.instance.get('systemtools/VisitSettings');
}

export default getVisitSettings;