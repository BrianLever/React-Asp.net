import axios from  '../axios';

const getEarliestDate = async (): Promise<any> => {
   return await axios.instance.get('screen/mindate');
}

export default getEarliestDate;