import axios from  '../axios';
import { IDefenitionResponse } from '../../actions/screen/report';

const getScreenDefinition = async (): Promise<IDefenitionResponse> => {
   return await axios.instance.get('screen/definition');
}

export default getScreenDefinition;