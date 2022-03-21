
import axios from  '../axios';

const getAgeGroup = async (): Promise<any> => {
   return await axios.instance.get('systemtools/agegroups');
}

export default getAgeGroup;